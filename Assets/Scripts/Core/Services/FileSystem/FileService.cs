using System.IO;
using Core.Services.FileSystem;
using UnityEngine;

namespace Core.Services
{
    public class FileService : MonoBehaviour, IFileService
    {
        private IFileSystemProvider _provider;

        private void Awake()
        {
            ServiceLocator.Instance.Register(this);
        }

        public void WriteToFile<T>(T data, string fileName)
        {
            _provider.WriteToFile(data, fileName);
        }

        public T ReadFromFile<T>(string fileName) where T : class
        {
            var data = _provider.ReadFromFile<T>(fileName);
            return data;
        }

        public void Register(IFileSystemProvider provider)
        {
            if (null != _provider)
            {
                Debug.LogError("[FileManager::Register] FileManager already has a provider registered");
                return;
            }

            _provider = provider;
        }

        public void Unregister(IFileSystemProvider provider)
        {
            if (null == provider)
            {
                Debug.LogError("[FileManager::Unregister] No provider previously registered with FileManager.");
                return;
            }

            _provider = null;
        }
        
        public void WipeAll()
        {
            PlayerPrefs.DeleteAll();
            if (Directory.Exists(Application.persistentDataPath))
            {
                Directory.Delete(Application.persistentDataPath, true);
            }
            Directory.CreateDirectory(Application.persistentDataPath);
        }
        
        public void WrapUp(bool isAppExit)
        {
            
        }
    }
}

