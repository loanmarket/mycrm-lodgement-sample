using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MyCRM.Lodgement.Sample.Models;
using MyCRM.Lodgement.Sample.Services.Settings;
using MyCRMAPI.Lodgement.Models;
using Newtonsoft.Json;

namespace MyCRM.Lodgement.Sample.Services.Client
{
    internal class LodgementClient : ILodgementClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IPackageSerializer _serializer;
        private readonly LodgementSettings _settings;

        public LodgementClient(IHttpClientFactory httpClientFactory, 
            IPackageSerializer serializer,
            IOptions<LodgementSettings> options)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<ValidationResult> Validate(Package package, CancellationToken token)
        {
            if (package == null) throw new ArgumentNullException(nameof(package));

            using var response = await Send(package, Routes.Validate, token);
            return response.StatusCode switch
            {
                HttpStatusCode.OK => await ReadResponse<ValidationResult>(response.Content),
                _ => throw new HttpRequestException($"Invalid response status code, {response.StatusCode} - {response.ReasonPhrase}")
            };
        }

        public async Task<ResultOrError<SubmissionResult, ValidationResult>> Submit(Package package, CancellationToken token)
        {
            if (package == null) throw new ArgumentNullException(nameof(package));

            using var response = await Send(package, Routes.Validate, token);
            return response.StatusCode switch
            {
                HttpStatusCode.OK => await ReadResponse<SubmissionResult>(response.Content),
                HttpStatusCode.BadRequest => await ReadResponse<ValidationResult>(response.Content),
                _ => throw new HttpRequestException($"Invalid response status code, {response.StatusCode} - {response.ReasonPhrase}")
            };
        }

        private Task<HttpResponseMessage> Send(Package package, string route, CancellationToken token)
        {
            if (package == null) throw new ArgumentNullException(nameof(package));
            if (route == null) throw new ArgumentNullException(nameof(route));

            var payload = _serializer.Serialize(package);

            var message = new HttpRequestMessage(HttpMethod.Post, route)
            {
                Content = new StringContent(payload, Encoding.UTF8, _settings.MediaType)
            };

            using var client = _httpClientFactory.CreateClient(nameof(LodgementClient));
            return client.SendAsync(message, token);
        }

        private static async Task<T> ReadResponse<T>(HttpContent content) where T : class
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