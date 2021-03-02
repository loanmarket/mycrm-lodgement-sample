using System;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Extensions.Options;
using MyCRM.Lodgement.Sample.Services.Settings;
using MyCRMAPI.Lodgement.Models;
using Newtonsoft.Json.Linq;

namespace MyCRM.Lodgement.Sample.Services.Client
{
    internal class PackageSerializer : IPackageSerializer
    {
        private readonly LodgementSettings _settings;

        public PackageSerializer(IOptions<LodgementSettings> options)
        {
            _settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public string Serialize(Package package)
        {
            if (package == null) throw new ArgumentNullException(nameof(package));

            return _settings.MediaType switch
            {
                "application/xml" => SerializeAsXml(package),
                "application/json" => JObject.FromObject(package).ToString(),
                _ => throw new NotImplementedException($"Media Type {_settings.MediaType} not supported.")
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
}