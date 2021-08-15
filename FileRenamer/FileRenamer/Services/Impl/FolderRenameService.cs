using System;
using System.Linq;
using System.Windows;
using Common.Logging.Factory;

namespace FileRenamer.Services.Impl
{
    public class FolderRenameService : IRenameFolderService
    {
        private readonly IFileService _fileService;
        private readonly IRenameFile _fileRenameService;
        private readonly IExtractInformation _textExtractor;

        public FolderRenameService(
            IFileService fileService,
            IRenameFile fileRenameService,
            IExtractInformation textExtractor)
        {
            _fileService = fileService;
            _textExtractor = textExtractor;
            _fileRenameService = fileRenameService;
        }

        public void RenameFileService(string folderPath)
        {
            var pdfFiles = _fileService.GetFiles(folderPath, "*.pdf");

            if (!pdfFiles.Any())
            {
                MessageBox.Show($"Can't find any pdf files in this folder: {folderPath}");
            }

            foreach (var pdfFilePath in pdfFiles)
            {
                try
                {
                    var extractedFields = _textExtractor.ExtractText(pdfFilePath);
                    var newFilename = _fileRenameService.GetFileFormat(pdfFilePath, "TEST STATEMENT TYPE", extractedFields);
                    _fileRenameService.RenameFile(pdfFilePath, newFilename);
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}
