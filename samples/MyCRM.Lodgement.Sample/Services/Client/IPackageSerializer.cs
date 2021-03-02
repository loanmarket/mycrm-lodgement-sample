using MyCRMAPI.Lodgement.Models;

namespace MyCRM.Lodgement.Sample.Services.Client
{
    public interface IPackageSerializer
    {
        string Serialize(Package package);
    }
}
