using LMGTech.DotNetLixi;

namespace MyCRM.Lodgement.Sample.Services.Client
{
    public interface ILodgementClient
    {
        Task<ValidationResult> Validate(Package package, CancellationToken token);
        Task<ResultOrError<SubmissionResult, ValidationResult>> Submit(Package package, CancellationToken token);
        Task<ResultOrError<SubmissionResult, ValidationResult>> SubmitSampleLixiPackage(
            SampleLodgementInformation lodgementInformation, CancellationToken token);
    }
}