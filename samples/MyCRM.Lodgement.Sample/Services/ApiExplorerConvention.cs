using Microsoft.AspNetCore.Mvc.ApplicationModels;
using MyCRM.Lodgement.Sample.Controllers;

namespace MyCRM.Lodgement.Sample.Services
{
    public class ApiExplorerConvention : IControllerModelConvention
    {
        public const string LodgementApi = "Lodgement";
        public const string BackchannelApi = "Backchannel";
        public const string LixiPackageApi = "LixiPackage";

        public void Apply(ControllerModel controller)
        {
            controller.ApiExplorer.GroupName = controller.ControllerType switch
            {
                Type type when type == typeof(BackchannelController) => BackchannelApi,
                Type type when type == typeof(LixiPackageController) => LixiPackageApi,
                Type type when type == typeof(LodgementSubmissionController) => LodgementApi,
                Type type when type == typeof(LodgementValidationController) => LodgementApi,
                _ => "Unknown item."
            };
        }
    }
}
