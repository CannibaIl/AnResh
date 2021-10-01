namespace Anresh.Application.Services.File.Interfaces
{
    public interface IFileService
    {
        string Load();
        void Save(string request);
    }
}
