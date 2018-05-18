using cvmksite.Api.FilterConfig;
using System.Web.Http;

namespace cvmksite.Api.Controllers
{
    [RoutePrefix("api/home")]
    //[Authentication]
    public class HomeController : ShareApiController
    {
        [HttpGet]
        [Route("TestMethod")]
        public string TestMethod()
        {
            return "Hello, TEDU Member. ";
        }
    }
}