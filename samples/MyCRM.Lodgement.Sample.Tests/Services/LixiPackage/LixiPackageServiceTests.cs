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
    private readonly LixiPackageService _sut;
    private readonly string _basePackagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LixiPackageSamples");

    public LixiPackageServiceTests()
    {
        // Create an instance of LixiPackageService
        _sut = new LixiPackageService();
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
        var result = await _sut.GetPackageAsync(scenario);

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
        await _sut.SavePackageAsync(package, scenario);

        // Assert
        Assert.True(File.Exists(packagePath));

        var fileContent = await File.ReadAllTextAsync(packagePath);
        var savedPackage = JsonConvert.DeserializeObject<Package>(fileContent);

        Assert.Equal(package.Content.Application.SalesChannel.Aggregator.CompanyName, savedPackage.Content.Application.SalesChannel.Aggregator.CompanyName);
        
        // Clean up the file
        File.Delete(packagePath);
    }
}
