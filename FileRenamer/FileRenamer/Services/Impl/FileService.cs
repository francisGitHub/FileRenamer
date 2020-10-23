using System.IO;

namespace FileRenamer.Services.Impl
{

    public class FileService : IFileService
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        public DirectoryInfo GetParent(string path)
        {
            return Directory.GetParent(path);
        }
    }
}
