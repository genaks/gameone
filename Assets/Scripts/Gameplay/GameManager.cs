using Core.MessageBroker;
using Core.Services;
using UnityEngine;

namespace Gameplay
{
    public class GameManager : MonoBehaviour
    {
        private MessageBroker _messageBroker;
        
        void Start()
        {
            if (ServiceLocator.Instance.TryGet(out MessageBroker messageBroker))
            {
                _messageBroker = messageBroker;
            }
        }
        
        public void GoToMainMenu()
        {
            _messageBroker.Publish(new EndGameEvent());
        }
    }
}