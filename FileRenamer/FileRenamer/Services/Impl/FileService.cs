using System.IO;
using Microsoft.Win32;

namespace FileRenamer.Services.Impl
{

    public class FileService : IFileService
    {
        public string OpenFileDialog()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            return fileDialog.ShowDialog() == true ? fileDialog.FileName : "";
        }

        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public string GetFileName(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        public DirectoryInfo GetParent(string path)
        {
            return Directory.GetParent(path);
        }

        public string GetFileExtension(string path)
        {
            return Path.GetExtension(path);
        }

        public void MoveFile(string sourceFile, string destinationFile)
        {
            File.Move(sourceFile, destinationFile);
        }

        public DirectoryInfo GetOriginalDocumentSubdirectory(DirectoryInfo directory)
        {
            return directory.CreateSubdirectory("Original Document");
        }

        public void CopyFile(string sourceFile, string destinationFile, bool overwriteFile)
        {
            File.Copy(sourceFile, destinationFile, true);
        }
    }
}
