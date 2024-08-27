using System;
using System.IO;
using System.Xml.Serialization;
using MyCRMAPI.Lodgement.Models;
using Newtonsoft.Json.Linq;

namespace MyCRM.Lodgement.Common.Utilities;

public class ObjectSerializer 
{

    public static string Serialize(Package package ,string mediaType)
    {
        if (package == null) throw new ArgumentNullException(nameof(package));

        return mediaType switch
        {
            "application/xml" => SerializeAsXml(package),
            "application/json" => JObject.FromObject(package).ToString(),
            _ => throw new NotImplementedException($"Media Type {mediaType} not supported.")
        };
    }
    
    private static string SerializeAsXml(Package package)
    {
        using var writer = new StringWriter();
        var serializer = new XmlSerializer(package.GetType());
        serializer.Serialize(writer, package);
        return writer.ToString();
    }
}
