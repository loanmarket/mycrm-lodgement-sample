using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using MyCRM.Lodgement.Common.Models;
using MyCRM.Lodgement.Sample.Services.Client;
using MyCRM.Lodgement.Sample.Services.Settings;
using MyCRMAPI.Lodgement.Models;
using Newtonsoft.Json.Linq;
using Xunit;

namespace MyCRM.Lodgement.Sample.Tests.Services.Client;

public class LodgementClientTests
{
    private class DelegatingHandlerStub : DelegatingHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handlerFunc;
        public DelegatingHandlerStub(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handlerFunc) => _handlerFunc = handlerFunc;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => _handlerFunc(request, cancellationToken);
    }

    private const string MediaType = "application/json";
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<IOptions<LodgementSettings>> _optionsMock;

    public LodgementClientTests()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _optionsMock = new Mock<IOptions<LodgementSettings>>();
    }

    [Fact]
    public async Task WhenValidatingAndOkResult_ThenReturnValidationResult()
    {
        var package = new Package();

        var validationResult = new ValidationResult
        {
            ExternalReferenceId = Guid.NewGuid().ToString(),
            ValidationErrors = []
        };

        var settings = new LodgementSettings
        {
            MediaType = MediaType,
            Url = "sample-url"
        };

        var clientHandlerStub = new DelegatingHandlerStub((request, cancellationToken) =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JObject.FromObject(validationResult).ToString(), Encoding.UTF8, MediaType)
            };
            return Task.FromResult(response);
        });

        _optionsMock.Setup(x => x.Value)
            .Returns(settings);

        var client = new HttpClient(clientHandlerStub) { BaseAddress = new Uri("https://example.com/") };

        _httpClientFactoryMock.Setup(_ => _.CreateClient(nameof(LodgementClient)))
            .Returns(client);

        var target = new LodgementClient(_httpClientFactoryMock.Object,
            _optionsMock.Object);

        var result = await target.Validate(package, CancellationToken.None);
        result.ExternalReferenceId.Should().Be(validationResult.ExternalReferenceId);
    }

    [Fact]
    public async Task WhenSubmittingAndOkResult_ThenReturnSubmissionResult()
    {
        var package = new Package();

        var submissionResult = new SubmissionResult
        {
            ReferenceId = Guid.NewGuid().ToString()
        };

        var settings = new LodgementSettings
        {
            MediaType = MediaType,
            Url = "sample-url"
        };

        var clientHandlerStub = new DelegatingHandlerStub((request, cancellationToken) =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JObject.FromObject(submissionResult).ToString(), Encoding.UTF8, MediaType)
            };
            return Task.FromResult(response);
        });

        _optionsMock.Setup(x => x.Value)
            .Returns(settings);

        var client = new HttpClient(clientHandlerStub) { BaseAddress = new Uri("https://example.com/") };

        _httpClientFactoryMock.Setup(_ => _.CreateClient(nameof(LodgementClient)))
            .Returns(client);

        var target = new LodgementClient(_httpClientFactoryMock.Object,
            _optionsMock.Object);

        var result = await target.Submit(package, CancellationToken.None);
        result.Result.ReferenceId.Should().Be(submissionResult.ReferenceId);
    }
}