using MyCRMAPI.Lodgement.Models;

namespace MyCRM.Lodgement.Sample.Services.LixiPackage;

public class LixiPackageService : ILixiPackageService
{
    private readonly string _packagSamplesBasePath = AppDomain.CurrentDomain.BaseDirectory + "\\LixiPackageSamples\\";

    public async Task<Package> CreatePackageAsync(int loanId, LoanApplicationScenario scenario,
        CancellationToken token = default)
    {
        var packagePath = _packagSamplesBasePath + Enum.GetName(typeof(LoanApplicationScenario), scenario) + ".json";
        var package = await GetPackageFromJsonAsync(packagePath, token);

        package.ProductionData = YesNoList.No;
        package.Content.Application.Overview.BrokerApplicationReferenceNumber = loanId.ToString();
        package.Content.Application.UniqueID = $"LoanScenario-{loanId}";

        return package;
    }

    private async Task<Package> GetPackageFromJsonAsync(string path, CancellationToken token)
    {
        var fileContent = await File.ReadAllTextAsync(path, token);
        return JsonConvert.DeserializeObject<Package>(fileContent);
    }
}