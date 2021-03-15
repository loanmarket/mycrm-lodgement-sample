using MyCRMAPI.Lodgement.Models;

namespace MyCRM.Lodgement.Common.Models
{
    public class BackchannelModel
    {
        public BackchannelEvent Event { get; set; }
        public Package Package { get; set; }
    }
}