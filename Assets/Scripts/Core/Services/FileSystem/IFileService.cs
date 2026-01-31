using Core.Services.FileSystem;

namespace Core.Services
{
    public interface IFileService : IGameService
    {
        void WriteToFile<T>(T data, string fileName);
        T ReadFromFile<T>(string fileName) where T : class;
        void Register(IFileSystemProvider provider);
        void Unregister(IFileSystemProvider provider);
    }
}