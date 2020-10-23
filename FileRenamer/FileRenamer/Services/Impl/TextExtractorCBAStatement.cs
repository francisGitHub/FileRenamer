using FileRenamer.ViewModels;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Filter;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace FileRenamer.Services.Impl
{
    public class TextExtractorCBAStatement : IExtractInformation
    {
        private readonly IFileService _fileService;

        public TextExtractorCBAStatement(IFileService fileService)
        {
            _fileService = fileService;
        }

        public BankStatementFields ExtractText(string filePath)
        {
            var bankStatementFields = new BankStatementFields();
            if (_fileService.Exists(filePath))
            {
                var rect = new iText.Kernel.Geom.Rectangle(350, 700, 595, 842);
                TextRegionEventFilter regionFilter = new TextRegionEventFilter(rect);

                var strategy = new FilteredTextEventListener(new LocationTextExtractionStrategy(), regionFilter);

                var pdfDocument = new PdfDocument(new PdfReader(filePath));

                var page = pdfDocument.GetPage(1);

                string text = PdfTextExtractor.GetTextFromPage(page, strategy);

                bankStatementFields.AccountNumber = GetAccountNumber(text);
                bankStatementFields.DateRange = GetDateRange(text);

                pdfDocument.Close();
            }

            return bankStatementFields;
        }

        public string GetAccountNumber(string extractedText)
        {
            var textArray = extractedText.Split('\n');
            var bsbAccountNumber = textArray[2].Split(' ');
            return bsbAccountNumber[2];
        }

        public string GetDateRange(string extractedText)
        {
            var textArray = extractedText.Split('\n');
            return textArray[5].TrimStart("Period ".ToCharArray());
        }
    }
}
