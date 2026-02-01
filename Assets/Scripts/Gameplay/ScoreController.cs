using Core.Services;
using UnityEngine;

namespace Gameplay
{
    public class ScoreController : MonoBehaviour
    {
        private MessageBroker _messageBroker;
        
        void Start()
        {
            if (ServiceLocator.Instance.TryGet(out MessageBroker messageBroker))
            {
                _messageBroker = messageBroker;
            }
        }
    }
}
