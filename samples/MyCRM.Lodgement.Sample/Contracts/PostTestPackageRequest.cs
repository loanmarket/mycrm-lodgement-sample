using LMGTech.DotNetLixi;

namespace MyCRM.Lodgement.Sample.Models;

public record PostTestPackageRequest
{ 
    public required LoanApplicationScenario Scenario { get; init; }
    public required LixiCountry Country { get; init; }
}