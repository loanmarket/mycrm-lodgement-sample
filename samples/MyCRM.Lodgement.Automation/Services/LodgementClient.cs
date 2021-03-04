using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MyCRM.Lodgement.Common;
using MyCRM.Lodgement.Common.Extensions;
using MyCRM.Lodgement.Common.Models;

namespace MyCRM.Lodgement.Automation.Services
{
    public class LodgementClient : IDisposable
    {
        private readonly HttpClient _client;

        public LodgementClient()
        {
            var automationSettings = Configuration.AutomationSettings;

            _client = new HttpClient
            {
                BaseAddress = new Uri(automationSettings.Url)
            };
        }

        public async Task<ValidationResult> Validate(string package)
        {
            using var response = await Send(package, Routes.Validate);
            return response.StatusCode != HttpStatusCode.OK
                ? throw new HttpRequestException($"Invalid response status code, {response.StatusCode} - {response.ReasonPhrase}")
                : await response.Content.ReadResponse<ValidationResult>();
        }

        public async Task<ResultOrError<SubmissionResult, ValidationResult>> Submit(string package)
        {
            if (package == null) throw new ArgumentNullException(nameof(package));

            using var response = await Send(package, Routes.Validate);
            return response.StatusCode switch
            {
                HttpStatusCode.OK => await response.Content.ReadResponse<SubmissionResult>(),
                HttpStatusCode.BadRequest => await response.Content.ReadResponse<ValidationResult>(),
                _ => throw new HttpRequestException($"Invalid response status code, {response.StatusCode} - {response.ReasonPhrase}")
            };
        }

        private Task<HttpResponseMessage> Send(string payload, string route)
        {
            if (payload == null) throw new ArgumentNullException(nameof(payload));
            if (route == null) throw new ArgumentNullException(nameof(route));

            var message = new HttpRequestMessage(HttpMethod.Post, route)
            {
                Content = new StringContent(payload, Encoding.UTF8, "application/json")
            };

            return _client.SendAsync(message);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}