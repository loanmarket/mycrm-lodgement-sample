using MyCRMAPI.Lodgement.Models;

namespace MyCRM.Lodgement.Sample.Controllers
{
    [ApiController]
    [Route("Lodgement")]
    public class LodgementValidationController : ControllerBase
    {
        private readonly ILodgementClient _lodgementClient;

        public LodgementValidationController(ILodgementClient lodgementClient)
        {
            _lodgementClient = lodgementClient ?? throw new ArgumentNullException(nameof(lodgementClient));
        }

        [HttpPost(Routes.Validate)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ValidationResult))]
        public async Task<IActionResult> Post(Package package, CancellationToken token)
        {
            var validationResult = await _lodgementClient.Validate(package, token);
            return Ok(validationResult);
        }
    }
}
