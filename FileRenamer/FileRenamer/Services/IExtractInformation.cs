using FileRenamer.Model;
using iText.Kernel.Pdf;

namespace FileRenamer.Services
{
    public interface IExtractInformation
    {
        BankStatementFields ExtractText(string filePath);
        string GetAccountNumber(PdfPage extractedText);
        string GetDateRange(PdfPage extractedText);
    }
}
