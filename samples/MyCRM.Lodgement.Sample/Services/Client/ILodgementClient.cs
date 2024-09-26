using System.Threading;
using System.Threading.Tasks;
using MyCRM.Lodgement.Common.Models;
using MyCRMAPI.Lodgement.Models;

namespace MyCRM.Lodgement.Sample.Services.Client
{
    public interface ILodgementClient
    {
        Task<ValidationResult> Validate(Package package, CancellationToken token);
        Task<ResultOrError<SubmissionResult, ValidationResult>> Submit(Package package, CancellationToken token);
    }
}
