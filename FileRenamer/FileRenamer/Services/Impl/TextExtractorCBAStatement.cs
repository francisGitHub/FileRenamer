using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using FileRenamer.Model;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Filter;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace FileRenamer.Services.Impl
{
    public class TextExtractorCBAStatement : IExtractInformation
    {
        private readonly IFileService _fileService;
        static readonly Rectangle AccountNumberLocation = new Rectangle(430, 748, 140, 16);
        static readonly Rectangle SingleLineStatementPeriodLocation = new Rectangle(430, 732, 140, 16);
        static readonly Rectangle MultiLineStatementPeriodLocation = new Rectangle(420, 715, 140, 16);
        static readonly List<Rectangle> StatementPeriodLocations = new List<Rectangle>
        {
            SingleLineStatementPeriodLocation,
            MultiLineStatementPeriodLocation
        };

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

            var pdfDocument = new PdfDocument(new PdfReader(filePath).SetUnethicalReading(true));
            var page = pdfDocument.GetPage(1);

            bankStatementFields.AccountNumber = GetAccountNumber(page);
            bankStatementFields.DateRange = GetDateRange(page);

            pdfDocument.Close();

            return bankStatementFields;
        }

        public string GetAccountNumber(PdfPage page)
        {
            try
            {
                var regionFilter = new TextRegionEventFilter(AccountNumberLocation);
                var strategy = new FilteredTextEventListener(new LocationTextExtractionStrategy(), regionFilter);
                string accountNumber = PdfTextExtractor.GetTextFromPage(page, strategy).Trim();

                return StripOutBsb(accountNumber);
            }
            catch (Exception e)
            {
                MessageBox.Show($"There was a problem trying to extract the Account Number: {e.Message}");
                return "";
            }
        }

        public string GetDateRange(PdfPage page)
        {
            var possibleDateRanges = new List<string>();

            foreach (var statementPeriodLocation in StatementPeriodLocations)
            {
                try
                {
                    var regionFilter = new TextRegionEventFilter(statementPeriodLocation);
                    var strategy = new FilteredTextEventListener(new LocationTextExtractionStrategy(), regionFilter);
                    string textDateRange = PdfTextExtractor.GetTextFromPage(page, strategy);

                    var textDateRangeArray = textDateRange?.Split('-');
                    var textStartDate = textDateRangeArray[0].Trim();
                    var textEndDate = textDateRangeArray[1].Trim();

                    possibleDateRanges.Add(ConvertToDateRangeFormat(textStartDate, textEndDate));
                }
                catch (Exception e)
                {
                    // Log here rather than throw
                }
            }

            return possibleDateRanges.FirstOrDefault();
        }

        private string ConvertToDateRangeFormat(string startDate, string endDate)
        {
            var parsedStartDate = DateTime.Parse(startDate);
            var parsedEndDate = DateTime.Parse(endDate);

            var formattedStartDate = $"{parsedStartDate:dd-MM-yy}";
            var formattedEndDate = $"{parsedEndDate:dd-MM-yy}";
            return $"{formattedStartDate}_{formattedEndDate}";
        }

        private string StripOutBsb(string accountNumberString)
        {
            var accountNumberArray = accountNumberString.Split(' ');

            return accountNumberArray.LastOrDefault();
        }
    }
}
