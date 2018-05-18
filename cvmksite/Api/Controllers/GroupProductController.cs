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
    [RoutePrefix("api/groupproduct")]
    public class GroupProductController : ShareApiController
    {
        [Route("getbyfilter")]
        [HttpGet]
        public HttpResponseMessage GetbyFilter(HttpRequestMessage request, string name, int page = 0, int pageSize = 10)
        {
            int total = 0;
            int comId = CurrentUser.Instance.User.ComId;
            var srv = IoC.Resolve<IGroupProductCategoryService>();
            var items = srv.GetbyFilter(comId, name, page, pageSize, out total).Select(n => new GroupProductViewModel {
                Id = n.Id,
                Name = n.Name,
                Status = n.Status
            }).ToList();
            var rs = new PaginationSet<GroupProductViewModel>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };

            return request.CreateResponse(HttpStatusCode.OK, rs);
        }

        [Route("getgroupproducts")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            var srv = IoC.Resolve<IGroupProductCategoryService>();
            return request.CreateResponse(HttpStatusCode.OK, srv.GetMulti(n => n.ComId == CurrentUser.Instance.User.ComId));
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, GroupProductViewModel vm)
        {
            string message = "";
            var model = vm.UpdateModel();
            var srv = IoC.Resolve<IGroupProductCategoryService>();
            if (srv.Create(model, out message))
            {
                return request.CreateResponse(HttpStatusCode.OK, srv.GetAll());
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, message);
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, GroupProductViewModel vm)
        {
            string message = "";
            var srv = IoC.Resolve<IGroupProductCategoryService>();
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
            if (IoC.Resolve<IGroupProductCategoryService>().Delete(id, out mess))
            {
                return request.CreateResponse(HttpStatusCode.OK, mess);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [Route("getbykey")]
        [HttpGet]
        public HttpResponseMessage GetbyKey(HttpRequestMessage request, int id)
        {
            var s = IoC.Resolve<IGroupProductCategoryService>();
            var entity = s.GetbyKey(id);
            var rs = new GroupProductViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Status = entity.Status
            };
            return request.CreateResponse(HttpStatusCode.OK, rs);
        }
    }
}
