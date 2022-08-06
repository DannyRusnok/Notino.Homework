namespace Notinot.Homework.Application
{
    public interface IFileConverter
    {
        Task<string> Convert(FileType source, FileType target, string sourceFileName, string targetFileName);
    }
}