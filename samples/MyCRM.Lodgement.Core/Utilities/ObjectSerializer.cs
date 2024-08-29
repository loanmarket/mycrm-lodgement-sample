using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using LMGTech.DotNetLixi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyCRM.Lodgement.Common.Utilities;

public class ObjectSerializer
{
    public static string Serialize(Package package, string mediaType)
    {
        if (package == null) throw new ArgumentNullException(nameof(package));

        return mediaType switch
        {
            "application/xml" => SerializeAsXml(package),
            "application/json" => JObject.FromObject(package).ToString(),
            _ => throw new NotImplementedException($"Media Type {mediaType} not supported.")
        };
    }

    public static string ObfuscateJson(JToken token,string[] propertiesToObfuscate = null)
    {
        var obfscuteCertainProperties = propertiesToObfuscate?.Length>0;
        if (token is JObject obj)
        {
            foreach (var property in obj.Properties())
            {
                Console.WriteLine(property.Name);
                // Check if the property name is in the list to be obfuscated
                if ((obfscuteCertainProperties && Array.Exists(propertiesToObfuscate, p => p.Equals(property?.Name, StringComparison.OrdinalIgnoreCase)))
                    ||!obfscuteCertainProperties)
                {
                    // Obfuscate string values
                    string originalString = property.Value.ToString();
                    string obfuscatedString = TransformString(originalString);
                    property.Value = obfuscatedString;
                }
                else
                {
                    // Recursively obfuscate nested objects or arrays

                    ObfuscateJson(property.Value,propertiesToObfuscate);
                }
            }
        }
        else if (token is JArray array)
        {
            foreach (var item in array)
            {
                ObfuscateJson(item,propertiesToObfuscate);
            }
        }

        return token.ToString(Formatting.Indented);
    }

    private static string SerializeAsXml(Package package)
    {
        using var writer = new StringWriter();
        var serializer = new XmlSerializer(package.GetType());
        serializer.Serialize(writer, package);
        return writer.ToString();
    }

    static string TransformString(string input)
    {
        StringBuilder result = new StringBuilder();
        Random random = new Random();
        foreach (char c in input)
        {
            if (char.IsDigit(c))
            {
                result.Append(random.Next(0, 9));
            }
            else if (char.IsLetter(c))
            {
                char randomLetter = (char)random.Next('a', 'z' + 1);
                result.Append(randomLetter);
            }
            else
            {
                result.Append(c); // Special characters remain the same
            }
        }

        return result.ToString();
    }
}