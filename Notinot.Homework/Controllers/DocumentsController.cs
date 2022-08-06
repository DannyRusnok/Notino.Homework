using Microsoft.AspNetCore.Mvc;
using Notinot.Homework.Application;

namespace Notinot.Homework.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly IFileConverter _fileConverter;
    private readonly IEmailSender _emailSender;

    public DocumentsController(IFileConverter fileConverter, IEmailSender emailSender)
    {
        _fileConverter = fileConverter;
        _emailSender = emailSender;
    }

    [HttpPost("convert-{source}-to-{target}")]
    public async Task<IActionResult> Convert([FromRoute] FileType source, [FromRoute] FileType target, IFormFile file, [FromQuery] string? emailTo)
    {
        if (file.Length == 0)
        {
            return BadRequest("File to convert is required.");
        }

        try
        {
            var fileContent = await FormFileHelper.GetFileContent(file);
            var targetFilePath = await _fileConverter.Convert(source, target, fileContent, FormFileHelper.GetFileName(file));

            if (!string.IsNullOrEmpty(emailTo))
            {
                _emailSender.SendConvertionEmail(emailTo, targetFilePath);
            }

            //no using (https://stackoverflow.com/a/42460443)
            var targetFileStream = System.IO.File.OpenRead(targetFilePath);
            return File(targetFileStream, "application/octet-stream");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex}");
        }
    }
}
