using Newtonsoft.Json;
using System.Xml;

namespace Notinot.Homework.Application;

public class JsonToXmlConvertionStrategy : IFileConversionStrategy
{
    public Task<string> Convert(string fileContent)
    {
        ArgumentNullException.ThrowIfNull(fileContent);

        XmlDocument? doc = JsonConvert.DeserializeXmlNode(fileContent, "root");

        if (doc is null)
        {
            return Task.FromResult(string.Empty);
        }

        return Task.FromResult(doc.OuterXml);
    }
}