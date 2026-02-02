using Core;
using Core.MessageBroker;
using Core.MessageBroker.Events;
using Core.Services;
using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text buttonText;
        private LevelDesignerScriptableObject _level;
        private MessageBroker _messageBroker;
        
        private void Start()
        {
            if (ServiceLocator.Instance.TryGet(out MessageBroker messageBroker))
            {
                _messageBroker = messageBroker;
            }
        }

        public void SetLevel(LevelDesignerScriptableObject level)
        {
            _level = level;
            buttonText.text = level.LevelName;
        }

        public void SelectLevel()
        {
            _messageBroker.Publish(new StartGameEvent(_level));
        }
    }
}
