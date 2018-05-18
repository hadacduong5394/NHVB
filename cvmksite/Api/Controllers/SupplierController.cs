using cvmk.context.IdentityConfiguration;
using cvmk.service.Interface;
using cvmksite.Models.ViewModel;
using hdcore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace cvmksite.Api.Controllers
{
    [RoutePrefix("api/supplier")]
    [Authorize]
    public class SupplierController : ShareApiController
    {
        [Route("getbyfilter")]
        [HttpGet]
        public HttpResponseMessage GetbyFilter(HttpRequestMessage request, string name, string email, string taxcode, string phone, int? page, int pageSize = 10)
        {
            var srv = IoC.Resolve<ISupplierService>();
            int currentPage = page.HasValue ? page.Value : 0;
            int total = 0;
            int com_id = CurrentUser.Instance.User.ComId;
            var items = srv.GetbyFilter(com_id, name, email, taxcode, phone, currentPage, pageSize, out total).Select(n => new SupplierViewModel {
                Id = n.Id,
                Address = n.Address,
                Name = n.Name,
                Code = n.Code,
                Email = n.Email,
                Logo = n.Logo,
                TaxCode = n.TaxCode,
                PhoneNumber = n.PhoneNumber,
                Status = n.Status,
            }).ToList();
            var rs = new PaginationSet<SupplierViewModel>
            {
                Items = items,
                Page = currentPage,
                PageSize = pageSize,
                TotalCount = total
            };
            return request.CreateResponse(HttpStatusCode.OK, rs);
        }

        [HttpGet]
        [Route("genarecode")]
        public HttpResponseMessage GenareCode(HttpRequestMessage request)
        {
            string rs = string.Empty;
            if (IoC.Resolve<ISupplierCodeService>().GenCode(out rs))
            {
                return request.CreateResponse(HttpStatusCode.OK, new { code = rs });
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, new { mess = "lỗi hệ thống, vui lòng thử lại sau." });
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, SupplierViewModel vm)
        {
            string mess = "";
            var entity = vm.UpdateModel();
            if (IoC.Resolve<ISupplierService>().Create(entity, out mess))
            {
                return request.CreateResponse(HttpStatusCode.OK, mess);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, SupplierViewModel vm)
        {
            string mess = "";
            var model = IoC.Resolve<ISupplierService>().GetbyKey(vm.Id);
            var entity = vm.UpdateModel(model);
            if (IoC.Resolve<ISupplierService>().Update(entity, out mess))
            {
                return request.CreateResponse(HttpStatusCode.OK, mess);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            string mess = "";
            if (IoC.Resolve<ISupplierService>().Delete(id, out mess))
            {
                return request.CreateResponse(HttpStatusCode.OK, mess);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [Route("getbykey")]
        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            var entity = IoC.Resolve<ISupplierService>().GetbyKey(id);
            var rs = new SupplierViewModel
            {
                Id = entity.Id,
                Address = entity.Address,
                Code = entity.Code,
                Email = entity.Email,
                Logo = entity.Logo,
                Descreption = entity.Descreption,
                Name = entity.Name,
                PhoneNumber = entity.PhoneNumber,
                Status = entity.Status,
                TaxCode = entity.TaxCode,
            };
            return request.CreateResponse(HttpStatusCode.OK, rs);
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return request.CreateResponse(HttpStatusCode.OK, IoC.Resolve<ISupplierService>().GetMulti(n => n.Status == true && n.ComId == CurrentUser.Instance.User.ComId));
        }
    }
}
