using System;
using System.Windows;
using FileRenamer.Services;
using Microsoft.WindowsAPICodePack.Shell.Interop;

namespace FileRenamer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly IFileService _fileService;
        private readonly IRenameFile _fileRenameService;
        private readonly IRenameFolderService _folderRenameService;
        private readonly IExtractInformation _textExtractorService;
        private readonly IDebugTextExtractionRegion _debugTextExtractionService;

        public MainWindow(
            IFileService fileService,
            IRenameFile fileRenameService,
            IRenameFolderService folderRenameService,
            IExtractInformation textExtractorService,
            IDebugTextExtractionRegion debugTextExtractionService)
        {
            _fileService = fileService;
            _fileRenameService = fileRenameService;
            _folderRenameService = folderRenameService;
            _debugTextExtractionService = debugTextExtractionService;
            _textExtractorService = textExtractorService;

            InitializeComponent();
        }

        private void SelectFile(object sender, RoutedEventArgs e)
        {
            DocumentPath.Text = _fileService.OpenFileDialog();
        }

        private void RenameFile(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DocumentPath.Text) || string.IsNullOrWhiteSpace(NewFileName.Text))
            {
                MessageBox.Show(
                    "Please ensure that you've selected a file, and pressed PREVIEW FILENAME to extract the information");
                return;
            }

            try
            {
                _fileRenameService.RenameFile(DocumentPath.Text, NewFileName.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong trying to rename file, make sure you haven't got that file opened");
            }
        }

        private void Extract(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DocumentPath.Text))
            {
                MessageBox.Show("Please select a file");
                return;
            }

            try
            {
                var extractedFields = _textExtractorService.ExtractText(DocumentPath.Text);

                NewFileName.Text = _fileRenameService.GetFileFormat(DocumentPath.Text, StatementType.Text, extractedFields);
            }
            catch (Exception exception)
            {
                MessageBox.Show($"{exception.Message}");
            }
        }

        private void DebugRectangleRegion(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DocumentPath.Text))
            {
                MessageBox.Show("Please select a file");
                return;
            }

            try
            {
                _debugTextExtractionService.DrawRectangleArea(new iText.Kernel.Geom.Rectangle(430, 752, 140, 16), DocumentPath.Text);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void SelectFolder(object sender, RoutedEventArgs e)
        {
            FolderPath.Text = _fileService.SelectFolderDialog();
        }

        private void TryAndRename(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FolderPath.Text))
            {
                MessageBox.Show("Please Select A Folder");
            }

            _folderRenameService.RenameFileService(FolderPath.Text);
        }
    }
}
