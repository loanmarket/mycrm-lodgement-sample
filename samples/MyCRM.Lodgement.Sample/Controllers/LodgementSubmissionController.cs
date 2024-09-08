using MyCRM.Lodgement.Sample.Mapping;

namespace MyCRM.Lodgement.Sample.Controllers
{
    [ApiController]
    [Route("Lodgement")]
    public class LodgementSubmissionController : ControllerBase
    {
        private readonly ILodgementClient _lodgementClient;

        public LodgementSubmissionController(ILodgementClient lodgementClient)
        {
            _lodgementClient = lodgementClient ?? throw new ArgumentNullException(nameof(lodgementClient));
        }

        [HttpPost(Routes.Submit)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubmissionResult))]
        public async Task<IActionResult> Post(Package package, CancellationToken token)
        {
            var resultOrError = await _lodgementClient.Submit(package, token);

            if (resultOrError.IsError)
            {
                return BadRequest(resultOrError.Error);
            }

            return Ok(resultOrError.Result);
        }
        
        
        [HttpPost(Routes.SumbitTestPackage)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubmissionResult))]
        public async Task<IActionResult> PostTestPackage(PostTestPackageRequest request, CancellationToken token)
        {
            var model = request.MapToLodgementInformation();
            if (model is null)
            {
                return BadRequest();
            }
            
            var resultOrError = await _lodgementClient.SubmitSampleLixiPackage(model, token);
        
            if (resultOrError.IsError)
            {
                return BadRequest(resultOrError.Error);
            }
        
            return Ok(resultOrError.Result);
        }
    }
}