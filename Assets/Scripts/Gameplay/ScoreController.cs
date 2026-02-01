using Core.Services;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class ScoreController : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        
        private MessageBroker _messageBroker;
        private int _comboMultiplier = 1;
        private int _score = 0;
        
        private void Start()
        {
            if (ServiceLocator.Instance.TryGet(out MessageBroker messageBroker))
            {
                _messageBroker = messageBroker;
            }
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
    }
}
