using System.Threading.Tasks;
using Xunit;

namespace MyCRM.Lodgement.Automation.Services.Tests
{
    public class ValidationTests
    {
        [Theory]
        [InlineData("./App_Data/Validate_Test1.xml")]
        [InlineData("./App_Data/Validate_Test2.xml")]
        [InlineData("./App_Data/Validate_Test3.xml")]
        [InlineData("./App_Data/Validate_Test4.xml")]
        public async Task ValidateFailure(string fileName)
        {
            var settings = Configuration.AutomationSettings;

            var client = new LodgementClient();
            var result = await client.Validate("");
        }
    }
}
