using System;
using System.Collections;
using Core.Config;
using Core.MessageBroker;
using Core.MessageBroker.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Services
{
    public class SceneLoadingService : MonoBehaviour, IGameService
    {
        private MessageBroker _messageBroker;
        
        private void Awake()
        {
            ServiceLocator.Instance.Register(this);
        }

        private void Start()
        {
            if (ServiceLocator.Instance.TryGet(out MessageBroker messageBroker))
            {
                _messageBroker = messageBroker;
                _messageBroker.Subscribe<StartGameEvent>(GoToGame);
                _messageBroker.Subscribe<EndGameEvent>(GoBackToMainMenu);
                _messageBroker.Subscribe<ContinueGameEvent>(ContinueGame);
            }
        }

        public void LoadMainMenu()
        {
            StartCoroutine(StartAsyncSceneLoad(Constants.Scenes.MainMenu, LoadSceneMode.Additive, () =>
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(Constants.Scenes.MainMenu));
            }));
        }
         
        private void GoToGame(IGameEvent startGameEvent)
        {
            StartCoroutine(StartSceneUnload(Constants.Scenes.MainMenu, LoadGameScene));
        }
        
        private void ContinueGame(ContinueGameEvent continueGameEvent)
        {
            StartCoroutine(StartSceneUnload(Constants.Scenes.MainMenu, LoadGameScene));
        }

        private void GoBackToMainMenu(EndGameEvent endGameEvent)
        {
            StartCoroutine(StartSceneUnload(Constants.Scenes.GameScene, LoadMainMenu));
        }

        private void LoadGameScene()
        {
            StartCoroutine(StartAsyncSceneLoad(Constants.Scenes.GameScene, LoadSceneMode.Additive, () =>
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(Constants.Scenes.GameScene));
            }));
        }

        private static IEnumerator StartAsyncSceneLoad(string sceneName, LoadSceneMode loadSceneMode, Action onComplete)
        {
            var progress = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            progress.allowSceneActivation = false;
            
            progress.completed += operation =>
            {
                onComplete.Invoke();
            };

            while (!progress.isDone)
            {
                if (progress.progress >= 0.9f)
                {
                    progress.allowSceneActivation = true;
                }
                
                yield return null;
            }
        }

        private static IEnumerator StartSceneUnload(string sceneName, Action onComplete)
        {
            var progress = SceneManager.UnloadSceneAsync(sceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

            progress.completed += operation =>
            {
                onComplete?.Invoke();
            };
            
            while (!progress.isDone)
            {
                yield return null;
            }
        }

        public void WrapUp(bool isAppExit)
		{
		}

        private void OnDestroy()
        {
            _messageBroker.Unsubscribe<StartGameEvent>(GoToGame);
            _messageBroker.Unsubscribe<EndGameEvent>(GoBackToMainMenu);
        }
    }
}
