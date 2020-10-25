using FileRenamer.Model;

namespace FileRenamer.Services
{
    public interface IRenameFile
    {
        string GetFileFormat(string path, string statementType, BankStatementFields bankStatementFields);
        void RenameFile(string path, string newFileName);
    }
}
