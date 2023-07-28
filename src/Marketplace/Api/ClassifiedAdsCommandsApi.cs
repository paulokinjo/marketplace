using Marketplace.Domain;
using Marketplace.Framework;
using Marketplace.Mongo;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static Marketplace.Contracts.ClassifiedAds;

namespace Marketplace.Api
{
    [Route("/ad")]
    public class ClassifiedAdsCommandsApi : Controller
    {
        private ILogger<ClassifiedAdsCommandsApi> logger;
        private readonly IMongoRepository<ClassifiedAd> repository;
        private readonly IApplicationService applicationService;

        public ClassifiedAdsCommandsApi(
            IMongoRepository<ClassifiedAd> repository,
            IApplicationService applicationService, ILogger<ClassifiedAdsCommandsApi> logger)
        {
            this.repository = repository;
            this.applicationService = applicationService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(repository.AsQueryable());

        [HttpPost]
        public async Task<IActionResult> Post(V1.Create request)
        {
            await HandleRequest(request);
            return Ok();
        }

        [Route("name")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.SetTitle request)
        {
            await HandleRequest(request);
            return Ok();
        }

        [Route("text")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.UpdateText request)
        {
            await HandleRequest(request);
            return Ok();
        }

        [Route("price")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.UpdatePrice request)
        {
            await HandleRequest(request); 
            return Ok();
        }

        [Route("publish")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.RequestToPublish request)
        {
            await HandleRequest(request);
            return Ok();
        }

        private async Task<IActionResult> HandleRequest<T>(T request)
        {
            try
            {
                logger.LogDebug("Handling HTTP request of type {type}", typeof(T).Name);
                await applicationService.Handle(request);
                return Ok();
            }
            catch (Exception e)
            {

                logger.LogError("Error handling the request", e);
                return new BadRequestObjectResult(new { error = e.Message, StackTrace = e.StackTrace });
            }
        }
    }
}
