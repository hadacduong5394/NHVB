using cvmk.context.IdentityConfiguration;
using cvmk.service.Interface;
using cvmksite.Api.FilterConfig;
using cvmksite.Models.ViewModel;
using hdcore;
using hdcore.Utils;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace cvmksite.Api.Controllers
{
    [RoutePrefix("api/company")]
    [Authorize]
    public class CompanyInfoController : ShareApiController
    {
        [HttpGet]
        [Route("info")]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            try
            {
                var cSrv = IoC.Resolve<ICompanyInfoService>();
                return request.CreateResponse(HttpStatusCode.OK, cSrv.GetbyKey(CurrentUser.Instance.User.ComId));
            }
            catch (Exception ex)
            {
                Log(ex);
                return request.CreateResponse(HttpStatusCode.BadRequest, TextHelper.ERROR_SYSTEM);
            }
        }

        [HttpGet]
        [Route("getall")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            try
            {
                var cSrv = IoC.Resolve<ICompanyInfoService>();
                return request.CreateResponse(HttpStatusCode.OK, cSrv.GetCompanies());
            }
            catch (Exception ex)
            {
                Log(ex);
                return request.CreateResponse(HttpStatusCode.BadRequest, TextHelper.ERROR_SYSTEM);
            }
        }

        [HttpPut]
        [Route("changeinfo")]
        public HttpResponseMessage ChangeInfo(HttpRequestMessage request, CompanyInfoViewmodel vm)
        {
            string mess = "";
            var srv = IoC.Resolve<ICompanyInfoService>();
            if (srv.ChangeInfo(vm.UpdateModel(srv.GetbyKey(vm.Id)), out mess))
            {
                return request.CreateResponse(HttpStatusCode.OK, mess);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [HttpGet]
        [Route("getbyfilter")]
        public HttpResponseMessage GetbyFilter(HttpRequestMessage request, int page = 0, int pageSize = 10)
        {
            int total = 0;
            var srv = IoC.Resolve<ICompanyInfoService>();
            var items = srv.GetbyFilter(page, pageSize, out total).Select(n => new CompanyInfoViewmodel {
                Id = n.Id,
                LongTittle = n.LongTittle,
                Name = n.Name,
                Status = n.Status,
                CreateBy = n.CreateBy,
                CreateDate = n.CreateDate.ToString("dd/MM/yyyy")
            }).ToList();
            var rs = new PaginationSet<CompanyInfoViewmodel>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };
            return request.CreateResponse(HttpStatusCode.OK, rs);
        }

        [HttpPost]
        [Route("create")]
        public HttpResponseMessage Create(HttpRequestMessage request, CompanyInfoViewmodel vm)
        {
            string mess = "";
            var entity = vm.UpdateModel();
            if (IoC.Resolve<ICompanyInfoService>().Create(entity, out mess))
            {
                return request.CreateResponse(HttpStatusCode.OK, mess);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [HttpDelete]
        [Route("delete")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            string mess = "";
            if (IoC.Resolve<ICompanyInfoService>().Delete(id, out mess))
            {
                return request.CreateResponse(HttpStatusCode.OK, mess);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [HttpGet]
        [Route("getbykey")]
        public HttpResponseMessage GetbyKey(HttpRequestMessage request, int id)
        {
            try
            {
                var cSrv = IoC.Resolve<ICompanyInfoService>();
                return request.CreateResponse(HttpStatusCode.OK, cSrv.GetbyKey(id));
            }
            catch (Exception ex)
            {
                Log(ex);
                return request.CreateResponse(HttpStatusCode.BadRequest, TextHelper.ERROR_SYSTEM);
            }
        }
    }
}