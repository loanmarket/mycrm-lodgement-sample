namespace MyCRM.Lodgement.Sample.Controllers
{
    [ApiController]
    [Route("LixiPackage")]
    public class LixiPackageController : ControllerBase
    {
        private readonly ILixiPackageService _lixiPackageService;

        public LixiPackageController(ILixiPackageService lixiPackageService)
        {
            _lixiPackageService = lixiPackageService;
        }

        [HttpPost(Routes.SaveLixiPackageSample)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult>  SaveLixiPackage([FromBody]SaveLixiPackageRequest request, CancellationToken token)
        {
            await _lixiPackageService.SavePackageAsync( request.LixiPackage, request.Scenario,request.BeObfuscated,token);
            return Ok();
        }
        
        [HttpGet(Routes.GetLixiPackageSample+"/{scenario}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Package>>  GetLixiPackage([FromRoute]LoanApplicationScenario scenario, CancellationToken token)
        {
            var package = await _lixiPackageService.GetPackageAsync(scenario, token);
            if (package is null) return NotFound();
            return Ok(package);
        }
    }
}
