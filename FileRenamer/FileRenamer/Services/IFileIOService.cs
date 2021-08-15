using System.IO;

namespace FileRenamer.Services
{
    public interface IFileService
    {
        string SelectFolderDialog();
        string OpenFileDialog();
        bool Exists(string path);
        string GetFileName(string path);
        DirectoryInfo GetParent(string path);
        string GetFileExtension(string path);
        string[] GetFiles(string path, string searchPattern);
        void MoveFile(string sourceFileName, string destinationFileName);
        DirectoryInfo GetOriginalDocumentSubdirectory(DirectoryInfo directory);
        void CopyFile(string sourceFileName, string destinationFileName, bool overwriteFile);
    }
}
