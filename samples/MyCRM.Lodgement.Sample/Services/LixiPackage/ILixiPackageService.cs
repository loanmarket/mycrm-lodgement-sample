using LMGTech.DotNetLixi;

namespace MyCRM.Lodgement.Sample.Services.LixiPackage;

public interface ILixiPackageService
{
    Task<Package> CreatePackageAsync(SampleLodgementInformation lodgementInformation, CancellationToken token = default);
    Task<Package> GetPackageAsync(LoanApplicationScenario scenario, CancellationToken token = default);
    Task SavePackageAsync(Package package, LoanApplicationScenario scenario,bool beObfscuted=true, CancellationToken token = default);

}