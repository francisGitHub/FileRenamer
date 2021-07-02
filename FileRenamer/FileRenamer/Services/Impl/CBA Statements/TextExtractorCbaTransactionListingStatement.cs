using System;
using System.Linq;
using System.Windows;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Filter;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace FileRenamer.Services.Impl.CBA_Statements
{
    public class TextExtractorCbaTransactionListingStatement : TextExtractorServiceBase
    {
        static readonly Rectangle AccountNumberLocation = new Rectangle(39, 695, 66, 10);
        static readonly Rectangle StartDateRegion = new Rectangle(28, 590, 66, 40);
        static readonly Rectangle EndDateRegion = new Rectangle(28, 0, 66, 1000);
        public TextExtractorCbaTransactionListingStatement(IFileService fileService) : base(fileService) { }

        public override string GetDateRange(PdfDocument pdfDocument)
        {
            var startDate = GetStartDate(pdfDocument);
            var endDate = GetEndDate(pdfDocument);

            return ConvertToDateRangeFormat(startDate, endDate);
        }

        public override string GetAccountNumber(PdfDocument pdfDocument)
        {
            try
            {
                var regionFilter = new TextRegionEventFilter(AccountNumberLocation);
                var strategy = new FilteredTextEventListener(new LocationTextExtractionStrategy(), regionFilter);
                string accountNumber = PdfTextExtractor.GetTextFromPage(pdfDocument.GetFirstPage(), strategy).Trim();

                return StripOutBsb(accountNumber);
            }
            catch (Exception e)
            {
                MessageBox.Show($"There was a problem trying to extract the Account Number: {e.Message}");
                return "";
            }
        }

        private string GetEndDate(PdfDocument pdfDocument)
        {
            var regionFilter = new TextRegionEventFilter(EndDateRegion);
            var strategy = new FilteredTextEventListener(new LocationTextExtractionStrategy(), regionFilter);
            string textFromRegion = PdfTextExtractor.GetTextFromPage(pdfDocument.GetLastPage(), strategy).Trim();

            var textFromRegionArray = textFromRegion.Split('\n').Reverse();

            foreach (string rowItem in textFromRegionArray)
            {
                if (DateTime.TryParse(rowItem.Trim(), out DateTime dateResult))
                {
                    return rowItem.Trim();
                }
            }

            return "";
        }

        private string GetStartDate(PdfDocument pdfDocument)
        {
            var regionFilter = new TextRegionEventFilter(StartDateRegion);
            var strategy = new FilteredTextEventListener(new LocationTextExtractionStrategy(), regionFilter);
            string textFromRegion = PdfTextExtractor.GetTextFromPage(pdfDocument.GetFirstPage(), strategy).Trim();

            /*We should always be getting 2 items here. The 'Date' Row, then the start date for transactions
              we need to do this to account for addresses in the pdf that slightly nudge the starting positions of these
              rows
            */

            var textArray = textFromRegion.Split('\n');

            if (textArray.Length == 2)
            {
                return textArray[1].Trim();
            }
            
            throw new IndexOutOfRangeException("Could not extract start date correctly");
        }
    }
}

