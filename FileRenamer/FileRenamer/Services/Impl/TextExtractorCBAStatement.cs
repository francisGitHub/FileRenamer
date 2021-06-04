using System;
using System.Windows;
using FileRenamer.Model;
using iText.Kernel.Geom;
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
            
            if (!_fileService.Exists(filePath)) 
            {
                return bankStatementFields;

            }

            var rect = new Rectangle(350, 700, 595, 842);
            var regionFilter = new TextRegionEventFilter(rect);

            var strategy = new FilteredTextEventListener(new LocationTextExtractionStrategy(), regionFilter);
            var pdfDocument = new PdfDocument(new PdfReader(filePath));
            var page = pdfDocument.GetPage(1);
            string text = PdfTextExtractor.GetTextFromPage(page, strategy);

            bankStatementFields.AccountNumber = GetAccountNumber(text);
            bankStatementFields.DateRange = GetDateRange(text);

            pdfDocument.Close();

            return bankStatementFields;
        }

        public string GetAccountNumber(string extractedText)
        {
            try
            {
                var textArray = extractedText.Split('\n');

                // Separating it in spaces as sometimes the BSB is added into the account number
                // Getting the last item should be the account number
                var accountNumberArray = textArray[2].Split(' ');
                var accountNumber = accountNumberArray[accountNumberArray.Length - 1];
                return accountNumber;
            }
            catch (Exception e)
            {
                MessageBox.Show($"There was a problem trying to extract the Account Number: {e.Message}");
                return "";
            }
        }

        public string GetDateRange(string extractedText)
        {
            try
            {
                var textArray = extractedText.Split('\n');

                string textDateRange = null;

                if (textArray[4].StartsWith("Statement period"))
                {
                    textDateRange = textArray[4].TrimStart("Statement period ".ToCharArray());
                }
                else if (textArray[5].StartsWith("Period"))
                {
                    textDateRange = textArray[5].TrimStart("Period ".ToCharArray());
                }

                var textDateRangeArray = textDateRange?.Split('-');
                var textStartDate = textDateRangeArray[0].Trim();
                var textEndDate = textDateRangeArray[1].Trim();

                return ConvertToDateRangeFormat(textStartDate, textEndDate);
            }
            catch (Exception e)
            {
                MessageBox.Show($"There was a problem trying to extract the Date: {e.Message}");
                return "";
            }
        }

        private string ConvertToDateRangeFormat(string startDate, string endDate)
        {
            var parsedStartDate = DateTime.Parse(startDate);
            var parsedEndDate = DateTime.Parse(endDate);

            var formattedStartDate = $"{parsedStartDate:dd-MM-yy}";
            var formattedEndDate = $"{parsedEndDate:dd-MM-yy}";
            return $"{formattedStartDate}_{formattedEndDate}";
        }
    }
}
