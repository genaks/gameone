using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Services.FileSystem
{
    public class UnityFileSystemProvider : IFileSystemProvider
    {
        public UnityFileSystemProvider()
        {
            if (ServiceLocator.Instance.TryGet(out FileService fileService))
            {
                fileService.Register(this);
            }
        }
        
        public void WriteToFile<T>(T data, string fileName)
        {            
            var json = JsonConvert.SerializeObject(data, new JsonSerializerSettings{ 
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            File.WriteAllBytes(Path.Combine(Application.persistentDataPath, fileName), Encoding.UTF8.GetBytes(json));
        }

        public T ReadFromFile<T>(string fileName) where T : class
        {
            if (File.Exists(Path.Combine(Application.persistentDataPath, fileName)))
            {
                byte[] bytes = File.ReadAllBytes(Path.Combine(Application.persistentDataPath, fileName));
                string fileContents = Encoding.UTF8.GetString(bytes);
                var json = (T)JsonConvert.DeserializeObject(fileContents, typeof(T));
                return json;
            }

            return null;
        }

        public bool FileExists(string fileName)
        {
            return File.Exists(Path.Combine(Application.persistentDataPath, fileName));
        }
    }
}