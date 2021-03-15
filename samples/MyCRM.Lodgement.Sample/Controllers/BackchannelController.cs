﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyCRM.Lodgement.Common.Models;

namespace MyCRM.Lodgement.Sample.Controllers
{
    /// <summary>
    /// The backchannel will be developed within the loan market application. 
    /// </summary>
    [ApiController]
    [Route("Lodgement/Backchannel")]
    public class BackchannelController
    {
        [HttpPost]
        public Task<IActionResult> Post(Backchannel model, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
