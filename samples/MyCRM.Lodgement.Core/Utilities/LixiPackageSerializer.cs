using System;
using System.Net.Mime;
using System.Text;
using LMGTech.DotNetLixi;
using LMGTech.DotNetLixi.Models;
using LMGTech.DotNetLixi.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyCRM.Lodgement.Common.Utilities;

public class LixiPackageSerializer
{
    public static string Serialize(Package package,LixiCountry country, string mediaType)
    {
        if (package == null) throw new ArgumentNullException(nameof(package));
        var json = new Json { Package = package };
        return mediaType switch
        {
            MediaTypeNames.Application.Xml => country==LixiCountry.Australia?  SerializeAsCalXml(package): SerializeAsCnzXml(package) ,
            MediaTypeNames.Application.Json =>country==LixiCountry.Australia?  SerializeAsCal(package): SerializeAsCnz(package),
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
    public static Json? DeserializeFromJson(string json, LixiCountry country, LixiVersion version) =>
        LixiSerializer.Deserialize(json, country, version);
    
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
    
    private static string SerializeAsCalXml(Package package, LixiVersion version = LixiVersion.Cal2635)
    {
        var json = new Json { Package = package };
        return LixiSerializer.SerializeToXml(json, LixiCountry.Australia, version);
    }

    private static string SerializeAsCnzXml(Package package, LixiVersion version = LixiVersion.Cnz218)
    {
        var json = new Json { Package = package };
        return LixiSerializer.SerializeToXml(json, LixiCountry.NewZealand, version);
    }

    private static string SerializeAsCal(Package package)
    {
        var json = new Json { Package = package };
        return LixiSerializer.Serialize(json, LixiCountry.Australia, LixiVersion.Cal2635);
    }

    private static string SerializeAsCnz(Package package)
    {
        var json = new Json { Package = package };
        return LixiSerializer.Serialize(json, LixiCountry.Australia, LixiVersion.Cnz218);
    }


}