namespace Notinot.Homework.Application;

public static class StringExtensions
{
    public static string GetRawFileName(this string fileName) => fileName.Split('.')[0];
}
