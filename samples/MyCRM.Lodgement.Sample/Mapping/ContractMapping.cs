namespace MyCRM.Lodgement.Sample.Mapping;

public static class ContractMapping
{
    public static SampleLodgementInformation MapToLodgementInformation(this PostTestPackageRequest request)
    {
        if(request is null)  throw new ArgumentNullException(nameof(request), "Source model cannot be null.");
        Random random = new Random();
        return new SampleLodgementInformation
        {
            LoanId = random.Next(0, int.MaxValue),
            Country = request.Country,
            Scenario = request.Scenario
        };
    }

}