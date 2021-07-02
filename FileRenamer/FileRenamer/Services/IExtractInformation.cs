using FileRenamer.Model;
using iText.Kernel.Pdf;

namespace FileRenamer.Services
{
    public interface IExtractInformation
    {
        BankStatementFields ExtractText(string filePath);
        string GetAccountNumber(PdfDocument pdfDocument);
        string GetDateRange(PdfDocument pdfDocument);
    }
}
