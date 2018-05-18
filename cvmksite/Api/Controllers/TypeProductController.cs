using cvmk.context.domain;
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
    [Authorize]
    [RoutePrefix("api/typeproduct")]
    public class TypeProductController : ShareApiController
    {
        [Route("gettypeproducts")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            var srv = IoC.Resolve<ITypeProductCategoryService>();
            return request.CreateResponse(HttpStatusCode.OK, srv.GetMulti(n => n.ComId == CurrentUser.Instance.User.ComId));
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, TypeProductViewModel vm)
        {
            string message = "";
            var model = vm.UpdateModel();
            var srv = IoC.Resolve<ITypeProductCategoryService>();
            if (srv.Create(model, out message))
            {
                return request.CreateResponse(HttpStatusCode.OK, srv.GetAll());
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, message);
        }

        [Route("getbyfilter")]
        [HttpGet]
        public HttpResponseMessage GetbyFilter(HttpRequestMessage request, string name, int page = 0, int pageSize = 10)
        {
            int total = 0;
            int comId = CurrentUser.Instance.User.ComId;
            var srv = IoC.Resolve<ITypeProductCategoryService>();
            var items = srv.GetbyFilter(comId, name, page, pageSize, out total).Select(n => new TypeProductViewModel
            {
                Id = n.Id,
                Name = n.Name,
                Status = n.Status
            }).ToList();
            var rs = new PaginationSet<TypeProductViewModel>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };

            return request.CreateResponse(HttpStatusCode.OK, rs);
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, TypeProductViewModel vm)
        {
            string message = "";
            var srv = IoC.Resolve<ITypeProductCategoryService>();
            var model = vm.UpdateModel(srv.GetbyKey(vm.Id));

            if (srv.Update(model, out message))
            {
                return request.CreateResponse(HttpStatusCode.OK, srv.GetAll());
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, message);
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            string mess = "";
            if (IoC.Resolve<ITypeProductCategoryService>().Delete(id, out mess))
            {
                return request.CreateResponse(HttpStatusCode.OK, mess);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [Route("getbykey")]
        [HttpGet]
        public HttpResponseMessage GetbyKey(HttpRequestMessage request, int id)
        {
            var s = IoC.Resolve<ITypeProductCategoryService>();
            var entity = s.GetbyKey(id);
            var rs = new TypeProductViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Status = entity.Status
            };
            return request.CreateResponse(HttpStatusCode.OK, rs);
        }
    }
}
