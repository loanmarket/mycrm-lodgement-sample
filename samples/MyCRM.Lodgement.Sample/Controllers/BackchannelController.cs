using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyCRMAPI.Lodgement.Models;

namespace MyCRM.Lodgement.Sample.Controllers
{
    [ApiController]
    [Route("Lodgement/Backchannel")]
    public class BackchannelController
    {
        [HttpPost]
        public Task<IActionResult> Post(Package package, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
