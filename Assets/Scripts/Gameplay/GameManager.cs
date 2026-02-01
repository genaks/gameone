using System;
using System.Collections;
using Core.MessageBroker;
using Core.Services;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private float endGameDelay = 5.0f;
        [SerializeField] private ScoreController scoreController;
        [SerializeField] private GameObject endGameView;
        [SerializeField] private TMP_Text finalScoreText;

        private MessageBroker _messageBroker;

        void Start()
        {
            if (ServiceLocator.Instance.TryGet(out MessageBroker messageBroker))
            {
                _messageBroker = messageBroker;
                _messageBroker.Subscribe<CardGridExhaustedEvent>(OnCardGridExhausted);
            }
        }
        
        public void GoToMainMenu()
        {
            StartCoroutine(EndGameWithDelay(0.0f));
        }

        private void OnCardGridExhausted(CardGridExhaustedEvent cardGridExhaustedEvent)
        {
            StartCoroutine(EndGameWithDelay(endGameDelay));
        }
        
        private IEnumerator EndGameWithDelay(float delay)
        {
            endGameView.SetActive(true);
            int finalScore = scoreController.GetFinalScore();
            finalScoreText.text = $"{finalScore}";
            yield return new WaitForSeconds(delay);
            _messageBroker.Publish(new EndGameEvent());
        }

        private void OnDestroy()
        {
            _messageBroker.Unsubscribe<CardGridExhaustedEvent>(OnCardGridExhausted);
        }
    }
}