using comp_api.common;
using Microsoft.AspNetCore.Mvc;

namespace comp_api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class MWCloudController : Controller
    {
        // POST api/values
        [HttpPost]
        public OkResult Notification()
        {
            RabbitMQHelper.init();
            RabbitMQHelper.publisher("competitionEndpointQueue", "*** Testantando API *****");

            return Ok();
        }

        [HttpGet]
        public OkResult bla()
        {
            return Ok();
        }
    }
}
