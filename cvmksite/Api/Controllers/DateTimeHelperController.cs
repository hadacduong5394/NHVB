using cvmksite.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace cvmksite.Api.Controllers
{
    [RoutePrefix("api/datetimehelper")]
    public class DateTimeHelperController : ApiController
    {
        [HttpGet]
        [Route("getyears")]
        public HttpResponseMessage GetYears(HttpRequestMessage request)
        {
            var result = YearHelper.GetYears();
            return request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("getmonths")]
        public HttpResponseMessage GetMonths(HttpRequestMessage request)
        {
            var result = MonthHelper.GetMonths();
            return request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("getmonthlimit")]
        public HttpResponseMessage GetLimitedMonths(HttpRequestMessage request, int year)
        {
            if (DateTime.Now.Year == year)
            {
                var result = MonthHelper.GetMonthLimit(DateTime.Now.Month);
                return request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                var result = MonthHelper.GetMonths();
                return request.CreateResponse(HttpStatusCode.OK, result);
            }
        }

    }
}
