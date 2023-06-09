﻿using Microsoft.AspNetCore.Mvc;
using static Marketplace.Contracts.ClassifiedAds;

namespace Marketplace.Api
{
    [Route("/ad")]
    public class ClassifiedAdsCommandsApi : Controller
    {
        private readonly ClassifiedAdsApplicationService applicationService;

        public ClassifiedAdsCommandsApi(ClassifiedAdsApplicationService applicationService)
            => this.applicationService = applicationService;

        [HttpPost]
        public async Task<IActionResult> Post(V1.Create request)
        {
            await applicationService.Handle(request);
            return Ok();
        }

        [Route("name")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.SetTitle request)
        {
            await applicationService.Handle(request); 
            return Ok();
        }

        [Route("text")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.UpdateText request)
        {
            await applicationService.Handle(request);
            return Ok();
        }

        [Route("price")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.UpdatePrice request)
        {
            await applicationService.Handle(request);
            return Ok();
        }

        [Route("publish")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.RequestToPublish request)
        {
            await applicationService.Handle(request);
            return Ok();
        }
    }
}
