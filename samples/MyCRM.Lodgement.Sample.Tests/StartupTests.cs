using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace MyCRM.Lodgement.Sample.Tests
{
    public class StartupTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string TextHtml = "text/html; charset=utf-8";

        private readonly WebApplicationFactory<Startup> _factory;

        public StartupTests(WebApplicationFactory<Startup> factory) => _factory = factory;

        [Theory]
        [InlineData("/swagger")]
        public async Task HtmlEndpointsRespondWithHtml(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.ToString().Should().Be(TextHtml);
        }
    }
}
