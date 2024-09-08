namespace MyCRM.Lodgement.Sample.Models;

public record SaveLixiPackageRequest
{
    public required LoanApplicationScenario Scenario{ get; init; }
    public bool BeObfuscated{ get; init; }
    public required Package LixiPackage { get; init; }
}