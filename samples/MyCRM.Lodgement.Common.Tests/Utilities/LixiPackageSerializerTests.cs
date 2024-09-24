
using LMGTech.DotNetLixi;
using LMGTech.DotNetLixi.Models;
using MyCRM.Lodgement.Common.Utilities;

namespace MyCRM.Lodgement.Common.Tests;

using Newtonsoft.Json.Linq;
using System;
using System.IO;
using Xunit;

public class LixiPackageSerializerTests
{


    [Fact]
    public void Serialize_ShouldSerializeToJson_WhenMediaTypeIsJson_AndCountryIsAustralia()
    {
        // Arrange
        var package = CreateTestPackage();

        // Act
        var result = LixiPackageSerializer.Serialize(package, LixiCountry.Australia,LixiVersion.Cal2635, "application/json");

        // Assert
        Assert.NotNull(result);
        Assert.Contains("\"@CompanyName\":\"Test Company\"", result);
    }

    [Fact]
    public void Serialize_ShouldSerializeToXml_WhenMediaTypeIsXml_AndCountryIsAustralia()
    {
        // Arrange
        var package = CreateTestPackage();

        // Act
        var result = LixiPackageSerializer.Serialize(package, LixiCountry.Australia,LixiVersion.Cal2635, "application/xml");

        // Assert
        Assert.NotNull(result);
        Assert.Contains("CompanyName=\"Test Company\"", result);
    }



    [Fact]
    public void Serialize_ShouldSerializeToXml_WhenMediaTypeIsXml_AndCountryIsNewZealand()
    {
        // Arrange
        var package = CreateTestPackage();

        // Act
        var result = LixiPackageSerializer.Serialize(package, LixiCountry.NewZealand,LixiVersion.Cnz218, "application/xml");

        // Assert
        Assert.NotNull(result);
        Assert.Contains("CompanyName=\"Test Company\"", result);
    }

    [Fact]
    public void Serialize_ShouldThrowNotImplementedException_WhenMediaTypeIsUnsupported()
    {
        // Arrange
        var package = CreateTestPackage();

        // Act & Assert
        var ex = Assert.Throws<NotImplementedException>(() =>
            LixiPackageSerializer.Serialize(package, LixiCountry.Australia,LixiVersion.Cal2635, "application/unsupported"));
        Assert.Equal("Media Type application/unsupported not supported.", ex.Message);
    }

    [Fact]
    public void Serialize_ShouldThrowArgumentNullException_WhenPackageIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            LixiPackageSerializer.Serialize(null, LixiCountry.Australia,LixiVersion.Cal2635, "application/json"));
    }

    [Fact]
    public void ObfuscateJson_ShouldObfuscateAllProperties_WhenNoPropertiesToObfuscateSpecified()
    {
        // Arrange
        var package = JObject.FromObject(CreateTestPackage());

        // Act
        var result = LixiPackageSerializer.ObfuscateJson(package);

        // Assert
        Assert.NotNull(result);
        Assert.DoesNotContain("123456789", result);  // BrokerApplicationReferenceNumber should be obfuscated
        Assert.DoesNotContain("Test Company", result);  // CompanyName should be obfuscated
        Assert.DoesNotContain("test@gmail.com", result);  // ABN should be obfuscated
        Assert.DoesNotContain("0450000111", result);  // Phone number should be obfuscated
    }

    [Fact]
    public void ObfuscateJson_ShouldObfuscateOnlySpecifiedProperties()
    {
        // Arrange
        var package = JObject.FromObject(CreateTestPackage());
        string[] propertiesToObfuscate = { "CompanyName", "UniqueID" };

        // Act
        var result = LixiPackageSerializer.ObfuscateJson(package, propertiesToObfuscate);

        // Assert
        Assert.NotNull(result);
        Assert.DoesNotContain("LoanScenario-12345", result);  //  UniqueID should be obfuscated
        Assert.DoesNotContain("Test Company", result);  // CompanyName should be obfuscated
    }
    
    
    private Package CreateTestPackage()
    {
        return new Package
        {
            // Populate with test data
            Content = new PackageContent
            {
                Application = new Application
                {
                    SalesChannel = new SalesChannel()
                    {
                        Company = new SalesChannelCompany()
                        {
                            CompanyName = "Test Company",
                            BusinessNumber = "123456789"
                        },
                        LoanWriter = new SalesChannelLoanWriter()
                        {
                            Contact = new SalesChannelLoanWriterContact()
                            {
                                Email = "test@gmail.com",
                                Mobile = new PhoneType()
                                {
                                    Number = "0450000111"
                                }
                            }
                        }
                    },
                    UniqueID = "LoanScenario-12345"
                }
            }
        };
    }
}
