using System;
using System.Collections;
using Core.MessageBroker;
using Core.Services;
using Core.Services.FileSystem;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private float endGameDelay = 5.0f;

        private MessageBroker _messageBroker;
        private IFileService _fileService;

        void Start()
        {
            if (ServiceLocator.Instance.TryGet(out MessageBroker messageBroker))
            {
                _messageBroker = messageBroker;
                _messageBroker.Subscribe<CardGridExhaustedEvent>(OnCardGridExhausted);
            }
            
            if (ServiceLocator.Instance.TryGet(out FileService fileService))
            {
                _fileService = fileService;
            }
        }
        
        public void GoToMainMenu()
        {
            StartCoroutine(EndGameWithDelay(0.0f));
        }

        private void OnCardGridExhausted(CardGridExhaustedEvent cardGridExhaustedEvent)
        {
            _fileService.WipeAll();
            StartCoroutine(EndGameWithDelay(endGameDelay));
        }
        
        private IEnumerator EndGameWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            _messageBroker.Publish(new EndGameEvent());
        }

        private void OnDestroy()
        {
            _messageBroker.Unsubscribe<CardGridExhaustedEvent>(OnCardGridExhausted);
        }
    }
}