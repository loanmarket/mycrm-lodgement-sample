using LMGTech.DotNetLixi;

namespace MyCRM.Lodgement.Sample.Services.Settings
{
    public class LodgementSettings
    {
        public string Url { get; set; }
        public string MediaType { get; set; }
        public LixiCountry Country { get; set; }
        public LixiVersion LixiPackageVersion { get; set; }
    }
}
