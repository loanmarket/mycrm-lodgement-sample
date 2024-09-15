using LMGTech.DotNetLixi;
using MyCRM.Lodgement.Common.Utilities;
using Newtonsoft.Json.Linq;

namespace MyCRM.Lodgement.Sample.Services.LixiPackage;

public class LixiPackageService : ILixiPackageService
{
    private readonly string _packagSamplesBasePath =Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LixiPackageSamples"); 
    private readonly string[] _propertiesToObfuscate = { "CompanyName", "Email", "Number", "ABN", "WebAddress", "BusinessNumber" };
    public async Task<Package> CreatePackageAsync(SampleLodgementInformation lodgementInformation,
        CancellationToken token = default)
    {
        var package = await GetPackageAsync(lodgementInformation.Scenario, token);

        package.ProductionData = false;
        package.Content.Application.Overview.BrokerApplicationReferenceNumber = lodgementInformation.LoanId.ToString();
        package.Content.Application.UniqueID = $"LoanScenario-{lodgementInformation.LoanId}";

        return package;
    }

    public async Task<Package> GetPackageAsync(LoanApplicationScenario scenario, CancellationToken token = default)
    {
        var fileName = Enum.GetName(typeof(LoanApplicationScenario), scenario) + ".json";
        var packagePath = Path.Combine(_packagSamplesBasePath , fileName);
        return await GetPackageFromJsonAsync(packagePath, token);
    }
    public async Task SavePackageAsync(Package package,LoanApplicationScenario scenario,bool beObfuscated=false, CancellationToken token = default)
    {
        var fileName = Enum.GetName(typeof(LoanApplicationScenario), scenario) + ".json";
        var packagePath = Path.Combine(_packagSamplesBasePath , fileName);
        await SavePackageToJsonAsync(package,packagePath,beObfuscated, token);
    } 
    
    private async Task<Package> GetPackageFromJsonAsync(string path, CancellationToken token= default)
    {
        try
        {
            var fileContent = await File.ReadAllTextAsync(path, token);
            return JsonConvert.DeserializeObject<Package>(fileContent);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
        
    }
    
    private async Task SavePackageToJsonAsync(Package package,string path,bool beObfuscated=false, CancellationToken token=default)
    {
        try
        {
            string jsonObj =JsonConvert.SerializeObject(package);
            if(beObfuscated)
                jsonObj = LixiPackageSerializer.ObfuscateJson(JToken.Parse(jsonObj),_propertiesToObfuscate);
            await File.WriteAllTextAsync(path,jsonObj, token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
        
    }
}