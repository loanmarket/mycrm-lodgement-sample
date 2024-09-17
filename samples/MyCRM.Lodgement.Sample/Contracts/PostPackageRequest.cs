using LMGTech.DotNetLixi;

namespace MyCRM.Lodgement.Sample.Models;

public record PostPackageRequest
{
    public required LixiCountry  Country { get; set; }
    public required Package LixiPackage { get; set; }
}