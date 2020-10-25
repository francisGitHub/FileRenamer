using System.IO;
using FileRenamer.Model;

namespace FileRenamer.Services.Impl
{
    public class FileRenameService : IRenameFile
    {
        private readonly IFileService _fileService;

        public FileRenameService(IFileService fileService)
        {
            _fileService = fileService;
        }

        public string GetFileFormat(string path, string statementType, BankStatementFields bankStatementFields)
        {
            var fileName = _fileService.GetFileName(path);
            var fileExtension = _fileService.GetFileExtension(path);

            var parentFolder = _fileService.GetParent(path);
            var originalDocumentSubdirectory = _fileService.GetOriginalDocumentSubdirectory(parentFolder);

            string originalDocumentSubdirectoryFileName =
                Path.Combine(originalDocumentSubdirectory.FullName, $"{fileName}{fileExtension}");

            _fileService.CopyFile(path, originalDocumentSubdirectoryFileName, true);

            var newFileName = $"{parentFolder.Name} {statementType} {bankStatementFields.AccountNumber} {bankStatementFields.DateRange}";

            return  Path.Combine(parentFolder.FullName, $"{newFileName}{fileExtension}");
        }

        public void RenameFile(string path, string newFileName)
        {
            _fileService.MoveFile(path, newFileName);
        }
    }
}
