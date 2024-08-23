using MyCRMAPI.Lodgement.Models;

namespace MyCRM.Lodgement.Sample.Services.LixiPackage;

public interface ILixiPackageService
{
    Task<Package> CreatePackageAsync(int loanId, LoanApplicationScenario scenario, CancellationToken token = default);
}