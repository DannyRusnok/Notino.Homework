namespace Notinot.Homework.Application;

public interface IFileConversionStrategy
{
    Task<string> Convert(string fileContent);
}
