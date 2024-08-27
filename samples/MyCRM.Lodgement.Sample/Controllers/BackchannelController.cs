using MyCRMAPI.Lodgement.Models;

namespace MyCRM.Lodgement.Sample.Controllers
{
    /// <summary>
    /// The backchannel will be developed within the loan market application. 
    /// </summary>
    [ApiController]
    [Route("Lodgement/{LixiCode}/Backchannel")]
    public class BackchannelController
    {
        [HttpPost]
        public Task<IActionResult> Post([FromRoute] string lixiCode, [FromBody] Package model, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
