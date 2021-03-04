using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyCRM.Lodgement.Common.Extensions
{
    public static class HttpContentExtensions
    {
        public static async Task<T> ReadResponse<T>(this HttpContent content) where T : class
        {
            if (content == null) throw new ArgumentNullException(nameof(content));
            var stream = await content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader);
            var ser = new JsonSerializer();
            return ser.Deserialize<T>(jsonReader);
        }
    }
}
