using System;
using Moq;
using Xunit;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using LMGTech.DotNetLixi.Models;
using MyCRM.Lodgement.Common.Models;
using MyCRM.Lodgement.Sample.Services.LixiPackage;

public class LixiPackageServiceTests
{
    private readonly Mock<ILixiPackageService> _sut;
    private readonly LixiPackageService _lixiPackageService;
    private readonly string _basePackagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LixiPackageSamples");

    public LixiPackageServiceTests()
    {
        // Create a mock for ILixiPackageService
        _sut = new Mock<ILixiPackageService>();

        // Create an instance of LixiPackageService
        _lixiPackageService = new LixiPackageService();
    }

    [Fact]
    public async Task CreatePackageAsync_ShouldSetCorrectValues()
    {
        // Arrange
        var lodgementInformation = new SampleLodgementInformation
        {
            LoanId = 12345,
            Scenario = LoanApplicationScenario.NewLoanApplication
        };

        var package = new Package
        {
            ProductionData = true,
            Content = new PackageContent
            {
                Application = new Application
                {
                    Overview = new Overview(),
                    UniqueID = ""
                }
            }
        };

        // Mock the GetPackageAsync method to return a predefined package
        _sut.Setup(service => service.GetPackageAsync(It.IsAny<LoanApplicationScenario>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(package);

        // Act
        var result = await _lixiPackageService.CreatePackageAsync(lodgementInformation);

        // Assert
        Assert.False(result.ProductionData);
        Assert.Equal(lodgementInformation.LoanId.ToString(), result.Content.Application.Overview.BrokerApplicationReferenceNumber);
        Assert.Equal($"LoanScenario-{lodgementInformation.LoanId}", result.Content.Application.UniqueID);
    }

    [Fact]
    public async Task GetPackageAsync_ShouldReturnPackageFromFile()
    {
        // Arrange
        var scenario = LoanApplicationScenario.NewLoanApplication;
        var fileName = $"{scenario}.json";
        var packagePath = Path.Combine(_basePackagePath, fileName);

        var packageContent = new Package
        {
            Content = new PackageContent
            {
                Application = new Application { }
            },
            UniqueID = "test-123"
        };

        var jsonContent = JsonConvert.SerializeObject(packageContent);

        // Simulate the file content
        File.WriteAllText(packagePath, jsonContent);

        // Act
        var result = await _lixiPackageService.GetPackageAsync(scenario);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(packageContent.UniqueID, result.UniqueID);

        // Clean up the file
        File.Delete(packagePath);
    }

    [Fact]
    public async Task SavePackageAsync_ShouldWritePackageToFile()
    {
        // Arrange
        var package = new Package
        {
            Content = new PackageContent
            {
                Application = new Application { SalesChannel = new SalesChannel()
                {
                    Aggregator = new SalesChannelAggregator()
                    {
                        CompanyName = "testCompanyAggregator"
                    }
                }}
            }
        };
        var scenario = LoanApplicationScenario.NewLoanApplication;
        var fileName = $"{scenario}.json";
        var packagePath = Path.Combine(_basePackagePath, fileName);

        // Act
        await _lixiPackageService.SavePackageAsync(package, scenario);

        // Assert
        Assert.True(File.Exists(packagePath));

        var fileContent = await File.ReadAllTextAsync(packagePath);
        var savedPackage = JsonConvert.DeserializeObject<Package>(fileContent);

        Assert.Equal(package.Content.Application.SalesChannel.Aggregator.CompanyName, savedPackage.Content.Application.SalesChannel.Aggregator.CompanyName);

        // Clean up the file
        File.Delete(packagePath);
    }
}
