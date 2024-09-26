using System.Net;
using System.Net.Http;
using System.Text;
using LMGTech.DotNetLixi;
using MyCRM.Lodgement.Common.Utilities;

namespace MyCRM.Lodgement.Sample.Services.Client
{
    public class LodgementClient : ILodgementClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILixiPackageService _lixiPackageService;
        private readonly LodgementSettings _settings;

        public LodgementClient(IHttpClientFactory httpClientFactory,
            ILixiPackageService lixiPackageService,
            IOptions<LodgementSettings> options)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _lixiPackageService = lixiPackageService;
            _settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<ValidationResult> Validate(Package package, CancellationToken token)
        {
            using var response = await SendAsync(package, Routes.Validate, token);
            return response.StatusCode switch
            {
                HttpStatusCode.OK => await ReadResponse<ValidationResult>(response.Content),
                _ => throw new HttpRequestException(
                    $"Invalid response status code, {response.StatusCode} - {response.ReasonPhrase}")
            };
        }

        public async Task<ResultOrError<SubmissionResult, ValidationResult>> Submit(Package package,
            CancellationToken token)
        {
            using var response = await SendAsync(package, Routes.Submit, token);
            return response.StatusCode switch
            {
                HttpStatusCode.OK => await ReadResponse<SubmissionResult>(response.Content),
                HttpStatusCode.BadRequest => await ReadResponse<ValidationResult>(response.Content),
                _ => throw new HttpRequestException(
                    $"Invalid response status code, {response.StatusCode} - {response.ReasonPhrase}")
            };
        }

        public async Task<ResultOrError<SubmissionResult, ValidationResult>> SubmitSampleLixiPackage(
            SampleLodgementInformation lodgementInformation, CancellationToken token)
        {

            var package = await _lixiPackageService.CreatePackageAsync(lodgementInformation, token);

            using var response = await SendAsync(package,Routes.Submit, token);
            return response.StatusCode switch
            {
                HttpStatusCode.OK => await ReadResponse<SubmissionResult>(response.Content),
                HttpStatusCode.BadRequest => await ReadResponse<ValidationResult>(response.Content),
                _ => throw new HttpRequestException(
                    $"Invalid response status code, {response.StatusCode} - {response.ReasonPhrase}")
            };
        }

        private async Task<HttpResponseMessage> SendAsync(Package package, string route, CancellationToken token)
        {
            if (package == null) throw new ArgumentNullException(nameof(package));
            if (route == null) throw new ArgumentNullException(nameof(route));
            
            var payload = LixiPackageSerializer.Serialize(package,_settings.Country,_settings.LixiPackageVersion,_settings.MediaType);
            var message = new HttpRequestMessage(HttpMethod.Post, $"Lodgement/{route}")
            {
                Content = new StringContent(payload, Encoding.UTF8, _settings.MediaType)
            };

            using var client = _httpClientFactory.CreateClient(nameof(LodgementClient));
            return await client.SendAsync(message, token);
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