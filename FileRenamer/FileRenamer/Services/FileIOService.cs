using System.IO;

namespace FileRenamer.Services
{
    public interface IFileService
    {
        bool Exists(string path);
        string GetFileName(string path);
        DirectoryInfo GetParent(string path);
    }
}
