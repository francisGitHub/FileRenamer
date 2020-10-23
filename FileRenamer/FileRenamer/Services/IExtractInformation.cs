using FileRenamer.ViewModels;

namespace FileRenamer.Services
{
    public interface IExtractInformation
    {
        BankStatementFields ExtractText(string filePath);
        string GetAccountNumber(string extractedText);
        string GetDateRange(string extractedText);
    }
}
