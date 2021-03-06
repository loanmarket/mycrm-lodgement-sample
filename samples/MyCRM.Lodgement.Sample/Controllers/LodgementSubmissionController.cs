﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCRM.Lodgement.Common;
using MyCRM.Lodgement.Common.Models;
using MyCRM.Lodgement.Sample.Services.Client;
using MyCRMAPI.Lodgement.Models;

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
    }
}