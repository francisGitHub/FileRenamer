using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Filter;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace FileRenamer.Services.Impl.CBA_Statements
{
    public class TextExtractorCbaStatement : TextExtractorServiceBase
    {
        static readonly Rectangle AccountNumberLocation = new Rectangle(430, 748, 140, 16);
        static readonly Rectangle SingleLineStatementPeriodLocation = new Rectangle(430, 732, 140, 16);
        static readonly Rectangle MultiLineStatementPeriodLocation = new Rectangle(420, 715, 140, 16);
        static readonly List<Rectangle> StatementPeriodLocations = new List<Rectangle>
        {
            SingleLineStatementPeriodLocation,
            MultiLineStatementPeriodLocation
        };

        public TextExtractorCbaStatement(IFileService fileService) : base(fileService) { }

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

        public override string GetDateRange(PdfDocument pdfDocument)
        {
            var possibleDateRanges = new List<string>();

            foreach (var statementPeriodLocation in StatementPeriodLocations)
            {
                try
                {
                    var regionFilter = new TextRegionEventFilter(statementPeriodLocation);
                    var strategy = new FilteredTextEventListener(new LocationTextExtractionStrategy(), regionFilter);
                    string textDateRange = PdfTextExtractor.GetTextFromPage(pdfDocument.GetFirstPage(), strategy);

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
    }
}
