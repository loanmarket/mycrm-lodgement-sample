using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MyCRM.Lodgement.Server.Models;
using MyCRMAPI.Lodgement.Models;

namespace MyCRM.Lodgement.Server.Controllers
{
    [ApiController]
    [Route("Lodgement")]
    public class LodgementValidationController : ControllerBase
    {
        [HttpPost("Validate")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ValidationResult))]
        public Task<IActionResult> Post(Package package)
        {
            throw new NotImplementedException();
        }
    }
}
