using Anresh.Application.Services.File.Interfaces;
using Anresh.Domain.Shared;
using Microsoft.Extensions.Options;
using System;
using System.Text;


namespace Anresh.Application.Services.File.Implementations
{
    public sealed class FileService : IFileService
    {
        private static readonly object LockObject = new();
        private readonly Options _options;
        private readonly bool _fileExists;

        public FileService(IOptions<Options> options )
        {
            _options = options.Value;
            _fileExists = System.IO.File.Exists(_options.FilePath);
        }

        public string Load()
        {
            if (!_fileExists)
            {
                throw new Exception($"File not found");
            }
            lock (LockObject)
            {   
                return System.IO.File.ReadAllText(_options.FilePath, Encoding.UTF8);
            }
        }

        public void Save(string request)
        {
            if (!_fileExists)
            {
                throw new Exception($"File not found");
            }
            lock (LockObject)
            {
                System.IO.File.WriteAllTextAsync(_options.FilePath, request, Encoding.UTF8);
            }
        }
    }
}
