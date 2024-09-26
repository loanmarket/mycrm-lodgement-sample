using Microsoft.AspNetCore.Mvc.ApplicationModels;
using MyCRM.Lodgement.Sample.Controllers;

namespace MyCRM.Lodgement.Sample.Services
{
    public class ApiExplorerConvention : IControllerModelConvention
    {
        public const string LodgementApi = "Lodgement";
        public const string BackchannelApi = "Backchannel";

        public void Apply(ControllerModel controller)
        {
            controller.ApiExplorer.GroupName = controller.ControllerType == typeof(BackchannelController) ? 
                BackchannelApi : 
                LodgementApi;
        }
    }
}
