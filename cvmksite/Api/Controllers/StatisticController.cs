using cvmk.context.IdentityConfiguration;
using cvmksite.Helper;
using cvmksite.Models.ViewModel;
using hd.data.SQLServerHelper;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace cvmksite.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/statistic")]
    public class StatisticController : ShareApiController
    {
        [HttpGet]
        [Route("getrevanues")]
        public HttpResponseMessage GetRevanues(HttpRequestMessage request, int year, int month)
        {
            try
            {
                string fromDate = DayHelper.GetFirstDayOfMonth(month, year).ToString("yyyy/MM/dd");
                string toDate = DayHelper.GetLastDayOfMonth(month, year).ToString("yyyy/MM/dd");
                var parameters = new SqlParameter[]{
                    new SqlParameter("@fromDate", fromDate),
                    new SqlParameter("@toDate", toDate),
                    new SqlParameter("@comid", CurrentUser.Instance.User.ComId.ToString())
                };
                var result = new StoreprocedureHelper<GetRevanuesStatisticViewModel>().ExecuteStoreprocedure("exec GetRevanuesStatiticsSP @fromDate,@toDate,@comid", parameters).ToList().OrderBy(n => n.Date);
                return request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Log(ex);
                return request.CreateResponse(HttpStatusCode.BadRequest, "Lỗi hệ thống.");
            }
        }
        [HttpGet]
        [Route("gettotalimports")]
        public HttpResponseMessage GetTotalImports(HttpRequestMessage request, int year, int month)
        {
            try
            {
                string fromDate = DayHelper.GetFirstDayOfMonth(month, year).ToString("yyyy/MM/dd");
                string toDate = DayHelper.GetLastDayOfMonth(month, year).ToString("yyyy/MM/dd");
                var parameters = new SqlParameter[]{
                    new SqlParameter("@fromDate", fromDate),
                    new SqlParameter("@toDate", toDate),
                    new SqlParameter("@comid", CurrentUser.Instance.User.ComId.ToString())
                };
                var result = new StoreprocedureHelper<GetTotalImportViewModel>().ExecuteStoreprocedure("exec GetTotalImportSP @fromDate,@toDate,@comid", parameters).ToList().OrderBy(n => n.Date);
                return request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Log(ex);
                return request.CreateResponse(HttpStatusCode.BadRequest, "Lỗi hệ thống.");
            }
        }

    }
}