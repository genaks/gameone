namespace Core.Services.FileSystem
{
    public interface IFileSystemProvider
    {
        void WriteToFile<T>(T data, string fileName);
        T ReadFromFile<T>(string fileName) where T : class;
    }
}
