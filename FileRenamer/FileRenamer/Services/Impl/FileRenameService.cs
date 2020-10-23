using FileRenamer.ViewModels;

namespace FileRenamer.Services.Impl
{
    public class FileRenameService : IRenameFile
    {
        private readonly IFileService _fileService;

        public FileRenameService(IFileService fileService)
        {
            _fileService = fileService;
        }

        public string GetFileFormat()
        {
            throw new System.NotImplementedException();
        }

        public void RenameFile(string filePath, BankStatementFields bankStatementFields)
        {
            var fileName = _fileService.GetFileName(filePath);
            var parentFolder = _fileService.GetParent(filePath);
        }
    }
}
