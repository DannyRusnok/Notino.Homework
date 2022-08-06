using System.Net.Http.Headers;

namespace Notinot.Homework.Controllers;

public class FormFileHelper
{
    public static string GetFileName(IFormFile file)
    {
        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;

        if (fileName == null)
        {
            throw new Exception("Empty filename.");
        }

        return fileName!.Trim('"');
    }

    public static async Task<string> GetFileContent(IFormFile file)
    {
        using var fileStream = file.OpenReadStream();
        using var fileStreamReader = new StreamReader(fileStream);
        return await fileStreamReader.ReadToEndAsync();
    }
}
