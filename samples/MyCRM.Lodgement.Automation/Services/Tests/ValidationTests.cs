using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace MyCRM.Lodgement.Automation.Services.Tests
{
    public class ValidationTests
    {
        [Theory]
        [InlineData("./App_Data/Validate_Test1.xml")]
        public async Task ValidateFailure(string fileName)
        {
            var package = await File.ReadAllTextAsync(fileName);
            var client = new LodgementClient();
            var result = await client.Validate(package);
            result.ReferenceId.Should().NotBeNullOrWhiteSpace("A reference is should be returned on both success and failure.");
            result.ValidationErrors.Should().NotBeNullOrEmpty("No validation errors were returned.");
        }
    }
}
