using FileRenamer.ViewModels;

namespace FileRenamer.Services
{
    public interface IRenameFile
    {
        string GetFileFormat();
        void RenameFile(string filePath, BankStatementFields bankStatementFields);
    }
}
