using System.Security.AccessControl;

namespace Notinot.Homework.Application;

public class FileConverter : IFileConverter
{
    private readonly string _workingPath;

    public FileConverter(IConfiguration configuration)
    {
        _workingPath = Path.Combine(Directory.GetCurrentDirectory(), configuration["FileConvertion:WorkingDirectoryName"]);
        PrepareWorkingFileSpace();
    }

    public async Task<string> Convert(FileType source, FileType target, string sourceFileContent, string targetFileName)
    {
        var strategy = ResolveConversionStrategy(source, target);
        var targetFilePath = Path.Combine(_workingPath, $"{targetFileName.GetRawFileName()}.{target.GetFileExtension()}");

        //var sourceFileContent = await GetFileContent(sourceFileName);
        var targetFileContent = await strategy.Convert(sourceFileContent);
        await WriteToTargetFile(targetFilePath, targetFileContent);

        return targetFilePath;
    }

    private IFileConversionStrategy ResolveConversionStrategy(FileType source, FileType target)
    {
        return (source, target) switch
        {
            (FileType.XML, FileType.JSON) => new XmlToJsonConvertionStrategy(),
            (FileType.JSON, FileType.XML) => new JsonToXmlConvertionStrategy(),
            _ => throw new NotImplementedException("No strategy for given source and target filetypes")
        };
    }

    private async Task<string> GetFileContent(string sourceFileName)
    {
        using (FileStream sourceStream = File.Open(sourceFileName, FileMode.Open))
        {
            using var reader = new StreamReader(sourceStream);
            return await reader.ReadToEndAsync();
        }
    }

    private async Task WriteToTargetFile(string targetFilePath, string fileContent)
    {
        using var targetStream = File.Open(targetFilePath, FileMode.Create, FileAccess.Write);
        using var sw = new StreamWriter(targetStream);
        await sw.WriteAsync(fileContent);
        await targetStream.FlushAsync();
    }

    private void PrepareWorkingFileSpace()
    {
        if (!Directory.Exists(_workingPath))
        {
            DirectoryInfo workingDirectory = Directory.CreateDirectory(_workingPath);
            var accessControl = workingDirectory.GetAccessControl();
            FileSystemAccessRule fsar = new FileSystemAccessRule("Users", FileSystemRights.FullControl, AccessControlType.Allow);
            accessControl.AddAccessRule(fsar);
            workingDirectory.SetAccessControl(accessControl);
        }
    }
}
