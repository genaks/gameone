using System;
using Core;
using MemoryMatch.DataModel;
using UnityEngine;
using Core.Config;
using Core.MessageBroker;
using Core.Services;
using Core.Services.FileSystem;
using ScriptableObjects;
using Scriptables;
using UnityEngine.EventSystems;

namespace UI
{
    public class LevelsManager : MonoBehaviour
    {
        [SerializeField] private LevelsScriptableObject levels;
        [SerializeField] private Transform levelsContainer;
        [SerializeField] private LevelButton levelButtonPrefab;
        [SerializeField] private GameObject continueButton;
        
        private IFileService _fileService;
        private MessageBroker _messageBroker;
        
        void Awake()
        {
            PopulateGrid();
        }
        
        private void Start()
        {
            if (ServiceLocator.Instance.TryGet(out MessageBroker messageBroker))
            {
                _messageBroker = messageBroker;
                _messageBroker.Subscribe<StartGameEvent>(StartGame);
            }
            
            if (ServiceLocator.Instance.TryGet(out FileService fileService))
            {
                _fileService = fileService;
                if (_fileService.FileExists(Constants.Filenames.CurrentLevel))
                {
                    continueButton.SetActive(true);
                }
            }
        }
        
        private void StartGame(StartGameEvent startGameEvent)
        {
            _fileService.WipeAll();
            LevelData level = new LevelData(startGameEvent.Level, false, 0, 0);
            _fileService.WriteToFile(level, Constants.Filenames.CurrentLevel);
        }
        
        public void ContinueGame()
        {
            _messageBroker.Publish(new ContinueGameEvent());
        }
        
        private void PopulateGrid()
        {
            // Clear existing children
            ClearGrid();

            // Create new elements
            foreach (LevelDesignerScriptableObject level in levels.Levels)
            {
                LevelButton cell = Instantiate(levelButtonPrefab, levelsContainer.transform);
                cell.SetLevel(level);
                cell.name = level.LevelName;
            }
        }
        
        private void ClearGrid()
        {
            for (int i = levelsContainer.transform.childCount - 1; i >= 0; i--)
            {
                if (Application.isPlaying)
                {
                    Destroy(levelsContainer.transform.GetChild(i).gameObject);
                }
                else
                {
                    DestroyImmediate(levelsContainer.transform.GetChild(i).gameObject);
                }
            }
        }

        private void OnDestroy()
        {
            _messageBroker.Unsubscribe<StartGameEvent>(StartGame);
        }
    }

}
