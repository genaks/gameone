using Core.Services;
using Core.Services.FileSystem;
using UnityEngine;

namespace Core.Boostrap
{
    public class BootstrapManager : MonoBehaviour
    {
        public void Start()
        {
            if (ServiceLocator.Instance.TryGet(out SceneLoadingService sceneLoadingService))
            {
                sceneLoadingService.GoToMainMenu();
            }
            
            UnityFileSystemProvider unityFileSystemProvider = new UnityFileSystemProvider();
        }
    }
}
