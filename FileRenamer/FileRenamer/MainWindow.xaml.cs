using System;
using System.Windows;
using FileRenamer.Services;

namespace FileRenamer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly IFileService _fileService;
        private readonly IRenameFile _fileRenameService;
        private readonly IExtractInformation _textExtractorService;

        public MainWindow(
            IFileService fileService,
            IRenameFile fileRenameService,
            IExtractInformation textExtractorService)
        {
            _fileService = fileService;
            _fileRenameService = fileRenameService;
            _textExtractorService = textExtractorService;

            InitializeComponent();
        }

        private void SelectFile(object sender, RoutedEventArgs e)
        {
            DocumentPath.Text = _fileService.OpenFileDialog();
        }

        private void RenameFile(object sender, RoutedEventArgs e)
        {
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

            var extractedFields = _textExtractorService.ExtractText(DocumentPath.Text);

            NewFileName.Text = _fileRenameService.GetFileFormat(DocumentPath.Text, StatementType.Text, extractedFields);
        }
    }
}
