using Anresh.Application.Services.File.Interfaces;
using System.Text;


namespace Anresh.Application.Services.File.Implementations
{
    public sealed class FileService : IFileService
    {
        private static readonly object LockObject = new();
        private const string FilePath = @"Files\TextFile.txt";
        public string Load()
        {
            lock (LockObject)
            {
                return System.IO.File.ReadAllText(FilePath, Encoding.UTF8);
            }
        }

        public void Save(string request)
        {
            lock (LockObject)
            {
                System.IO.File.WriteAllTextAsync(FilePath, request, Encoding.UTF8);
            }
        }
    }
}
