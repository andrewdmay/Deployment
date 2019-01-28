using Microsoft.AspNetCore.Mvc;

namespace Deployment.Api
{
    [Route("/healthcheck")]
    public class HealthcheckController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
