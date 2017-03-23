
using System.Net;
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
            return Ok();
        }

        [HttpGet]
        public OkResult bla()
        {
            return Ok();
        }
    }
}
