using System.Net;
using System.Net.Http.Headers;

namespace Notino.Homework.Tests;

public class IntegrationTests
{
    [Fact]
    public async Task ConversionJsonToXml_HappyPath_ShouldReturnStatusOK()
    {
		//Arrange
        var appFactory = new ApplicationFactory();
        var client = appFactory.CreateClient();
		
		var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "random.json");

		using (var multipartFormContent = new MultipartFormDataContent())
		{
			var fileStreamContent = new StreamContent(File.OpenRead(filePath));
			fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			multipartFormContent.Add(fileStreamContent, name: "file", fileName: "random.json");

			//Act
			var response = await client.PostAsync("documents/convert-json-to-xml", multipartFormContent);
			
			//Assert
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}
	}

	[Fact]
	public async Task ConvertionUnknwonToXml_ShouldReturnBadRequest()
	{
		//Arrange
		var appFactory = new ApplicationFactory();
		var client = appFactory.CreateClient();

		var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "random.json");

		using (var multipartFormContent = new MultipartFormDataContent())
		{
			var fileStreamContent = new StreamContent(File.OpenRead(filePath));
			fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			multipartFormContent.Add(fileStreamContent, name: "file", fileName: "random.json");

			//Act
			var response = await client.PostAsync("documents/convert-unknown-to-xml", multipartFormContent);

			//Assert
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}
	}
}
