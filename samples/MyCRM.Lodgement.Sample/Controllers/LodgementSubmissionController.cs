using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCRM.Lodgement.Server.Models;
using MyCRMAPI.Lodgement.Models;

namespace MyCRM.Lodgement.Server.Controllers
{
    [ApiController]
    [Route("Lodgement")]
    public class LodgementSubmissionController : ControllerBase
    {
        [HttpPost("Submit")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubmissionResult))]
        public Task<IActionResult> Post(Package package)
        {
            throw new NotImplementedException();
        }
    }
}