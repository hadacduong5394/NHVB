using cvmk.context.IdentityConfiguration;
using cvmk.service.Interface;
using cvmksite.Models.ViewModel;
using hdcore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace cvmksite.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/customers")]
    public class CustomerController : ShareApiController
    {
        [HttpGet]
        [Route("getbyfilter")]
        public HttpResponseMessage GetbyFilter(HttpRequestMessage request, string code, string name, string taxcode, string email, int? page = 0, int pageSize = 10)
        {
            int currentPage = page.HasValue ? page.Value : 0;
            int total = 0;
            int com_id = CurrentUser.Instance.User.ComId;
            var items = IoC.Resolve<ICustomerService>().GetbyFilter(com_id, code, name, taxcode, email, currentPage, pageSize, out total).Select(n => new CustomerViewModel
            {
                Id = n.Id,
                Address = n.Address,
                Avatar = n.Avatar,
                Code = n.Code,
                Company = n.Company,
                CreateBy = n.CreateBy,
                CreateDate = n.CreateDate.ToString("dd/MM/yyyy"),
                Descreption = n.Descreption,
                Email = n.Email,
                Name = n.Name,
                PhoneNumber = n.PhoneNumber,
                Status = n.Status,
                TaxCode = n.TaxCode,
                Type = n.Type,
                ModifyBy = n.ModifyBy,
                ModifyDate = n.ModifyDate.HasValue ? n.ModifyDate.Value.ToString("dd/MM/yyyy") : ""
            }).ToList();
            var rs = new PaginationSet<CustomerViewModel>
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
            if (IoC.Resolve<ICustomerCodeService>().GenCode(out rs))
            {
                return request.CreateResponse(HttpStatusCode.OK, new { code = rs });
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, new { mess = "lỗi hệ thống, vui lòng thử lại sau." });
        }

        [HttpPost]
        [Route("create")]
        public HttpResponseMessage Create(HttpRequestMessage request, CustomerViewModel vm)
        {
            string mess = "";
            var entity = vm.UpdateModel();
            if (IoC.Resolve<ICustomerService>().Create(entity, out mess))
            {
                return request.CreateResponse(HttpStatusCode.OK, mess);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [HttpPut]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, CustomerViewModel vm)
        {
            string mess = "";
            var model = IoC.Resolve<ICustomerService>().GetbyKey(vm.Id);
            var entity = vm.UpdateModel(model);
            if (IoC.Resolve<ICustomerService>().Update(entity, out mess))
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
            if (IoC.Resolve<ICustomerService>().Delete(id, out mess))
            {
                return request.CreateResponse(HttpStatusCode.OK, mess);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [HttpGet]
        [Route("getbykey")]
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            var entity = IoC.Resolve<ICustomerService>().GetbyKey(id);
            var rs = new CustomerViewModel
            {
                Id = entity.Id,
                Address = entity.Address,
                Avatar = entity.Avatar,
                BirthDay = entity.BirthDay.HasValue ? entity.BirthDay.Value.ToString("yyyy/MM/dd") : string.Empty,
                Code = entity.Code,
                Company = entity.Company,
                CreateBy = entity.CreateBy,
                CreateDate = entity.CreateDate.ToString("dd/MM/yyyy"),
                Descreption = entity.Descreption,
                Email = entity.Email,
                Name = entity.Name,
                PhoneNumber = entity.PhoneNumber,
                Status = entity.Status,
                TaxCode = entity.TaxCode,
                Type = entity.Type,
                ModifyBy = entity.ModifyBy,
                ModifyDate = entity.ModifyDate.HasValue ? entity.ModifyDate.Value.ToString("dd/MM/yyyy") : ""
            };
            return request.CreateResponse(HttpStatusCode.OK, rs);
        }
    }
}
