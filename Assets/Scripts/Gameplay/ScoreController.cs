using System;
using Core.MessageBroker;
using Core.MessageBroker.Events;
using Core.Services;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class ScoreController : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text turnsText;
        [SerializeField] private GameObject endGameView;
        [SerializeField] private TMP_Text finalScoreText;

        private MessageBroker _messageBroker;
        private int _comboMultiplier = 1;
        private int _score = 0;
        private int _turns = 0;

        public int Score => _score;
        public int Turns => _turns;
        
        private void Start()
        {
            if (ServiceLocator.Instance.TryGet(out MessageBroker messageBroker))
            {
                _messageBroker = messageBroker;
                _messageBroker.Subscribe<CardGridExhaustedEvent>(OnCardGridExhausted);
            }
        }

        private void OnCardGridExhausted(CardGridExhaustedEvent cardGridExhaustedEvent)
        {
            ShowEndGameView();
        }

        public void OnCardSelected()
        {
            _turns++;
            turnsText.text = $"{_turns}";
        }

        public void OnMatchingCardsSelected()
        {
            _score += 2 * _comboMultiplier;
            scoreText.text = $"{_score}";
            _comboMultiplier++;
        }

        public void OnDifferentCardsSelected()
        {
            _comboMultiplier = 1;
        }

        private void ShowEndGameView()
        {
            endGameView.SetActive(true);
            finalScoreText.text = $"{_score}";
        }

        public void SetSavedData(int turns, int score)
        {
            _turns = turns;
            _score = score;
            turnsText.text = $"{_turns}";
            scoreText.text = $"{_score}";
        }

        private void OnDestroy()
        {
            _messageBroker.Unsubscribe<CardGridExhaustedEvent>(OnCardGridExhausted);
        }
    }
}
