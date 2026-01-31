using Core.Services;
using UnityEngine;

namespace Core
{
    public class BootstrapManager : MonoBehaviour
    {
        private SceneLoadingService _sceneLoadingService;
    
        public void Start()
        {
            if (ServiceLocator.Instance.TryGet(out SceneLoadingService sceneLoadingService))
            {
                _sceneLoadingService = sceneLoadingService;
                _sceneLoadingService.GoToMainMenu();
            }
        }
    }
}
