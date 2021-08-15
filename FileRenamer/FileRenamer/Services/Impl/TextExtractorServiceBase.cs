using System;
using System.IO;
using System.Linq;
using FileRenamer.Model;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using Rectangle = iText.Kernel.Geom.Rectangle;

namespace FileRenamer.Services.Impl
{
    public abstract class TextExtractorServiceBase : IExtractInformation, IDebugTextExtractionRegion
    {
        protected readonly IFileService FileService;

        protected TextExtractorServiceBase(IFileService fileService)
        {
            FileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        public BankStatementFields ExtractText(string filePath)
        {
            var bankStatementFields = new BankStatementFields();

            if (!FileService.Exists(filePath))
            {
                throw new FileNotFoundException($"Cannot find file in in filepath {filePath}");
            }

            var pdfDocument = new PdfDocument(new PdfReader(filePath).SetUnethicalReading(true));

            bankStatementFields.AccountNumber = GetAccountNumber(pdfDocument);
            bankStatementFields.DateRange = GetDateRange(pdfDocument);

            pdfDocument.Close();

            return bankStatementFields;
        }

        public void DrawRectangleArea(Rectangle rectangle, string inputFilePath)
        {
            var pdfReader = new PdfReader(inputFilePath);
            pdfReader.SetUnethicalReading(true);
            PdfDocument pdfDoc = new PdfDocument(pdfReader, new PdfWriter("C:\\temp\\output.pdf"));
            PdfCanvas canvas = new PdfCanvas(pdfDoc.GetPage(1));
            var colour = new DeviceCmyk(1, 1, 1, 1);
            canvas.SetFillColor(colour);
            canvas.Rectangle(rectangle);
            canvas.FillStroke();
            pdfDoc.Close();
        }

        public abstract string GetDateRange(PdfDocument pdfDocument);
        public abstract string GetAccountNumber(PdfDocument pdfDocument);


        protected string ConvertToDateRangeFormat(string startDate, string endDate)
        {
            var parsedStartDate = DateTime.Parse(startDate);
            var parsedEndDate = DateTime.Parse(endDate);

            var formattedStartDate = $"{parsedStartDate:dd-MM-yy}";
            var formattedEndDate = $"{parsedEndDate:dd-MM-yy}";

            return $"{formattedStartDate}_{formattedEndDate}";
        }

        protected string StripOutBsb(string accountNumberString)
        {
            if (accountNumberString.Contains(' '))
            {
                var accountNumberArray = accountNumberString.Trim().Replace(" ", "");

                return accountNumberArray.Substring(6);
            }

            return accountNumberString;
        }
    }
}
