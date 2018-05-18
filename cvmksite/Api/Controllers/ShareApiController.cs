using cvmk.service.Interface;
using hdcore;
using System;
using System.Web.Http;

namespace cvmksite.Api.Controllers
{
    public class ShareApiController : ApiController
    {
        public void Log(Exception ex)
        {
            var errSrv = IoC.Resolve<IErrorService>();
            errSrv.TryLog(ex);
        }
    }
}