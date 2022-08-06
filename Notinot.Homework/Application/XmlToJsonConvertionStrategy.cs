using Newtonsoft.Json;
using System.Xml;

namespace Notinot.Homework.Application;

public class XmlToJsonConvertionStrategy : IFileConversionStrategy
{
    public Task<string> Convert(string fileContent)
    {
        ArgumentNullException.ThrowIfNull(fileContent);

        XmlDocument doc = new();
        doc.LoadXml(fileContent);
        return Task.FromResult(JsonConvert.SerializeXmlNode(doc));
    }
}
