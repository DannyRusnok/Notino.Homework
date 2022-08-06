namespace Notinot.Homework.Application;

public enum FileType
{
    XML, JSON
}

public static class FileTypeExtensions
{
    public static string GetFileExtension(this FileType type)
    {
        switch (type)
        {
            case FileType.XML: return "xml";
            case FileType.JSON: return "json";
            default: throw new NotImplementedException();
        }
    }
}