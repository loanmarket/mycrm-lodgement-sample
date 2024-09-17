using LMGTech.DotNetLixi;

namespace MyCRM.Lodgement.Sample.Services.Client
{
    public interface ILodgementClient
    {
        Task<ValidationResult> Validate(Package package,LixiCountry country, CancellationToken token);
        Task<ResultOrError<SubmissionResult, ValidationResult>> Submit(Package package,LixiCountry country, CancellationToken token);
        Task<ResultOrError<SubmissionResult, ValidationResult>> SubmitSampleLixiPackage(
            SampleLodgementInformation lodgementInformation, CancellationToken token);
    }
}