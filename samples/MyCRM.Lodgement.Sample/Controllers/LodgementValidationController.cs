using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCRM.Lodgement.Sample.Models;
using MyCRMAPI.Lodgement.Models;

namespace MyCRM.Lodgement.Sample.Controllers
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
