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
    [RoutePrefix("api/floor")]
    public class FloorController : ShareApiController
    {
        [Route("getbyfilter")]
        [HttpGet]
        public HttpResponseMessage GetbyFilter(HttpRequestMessage request, string name, int page = 0, int pageSize = 10)
        {
            int total = 0;
            int comId = CurrentUser.Instance.User.ComId;
            var items = IoC.Resolve<IFloorService>().GetbyFilter(comId, name, page, pageSize, out total).Select(n => new FloorViewModel
            {
                Id = n.Id,
                Name = n.Name,
                CreateBy = n.CreateBy,
                Descreption = n.Descreption,
                Status = n.Status,
                VIP = n.VIP
            }).ToList();
            var rs = new PaginationSet<FloorViewModel>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };

            return request.CreateResponse(HttpStatusCode.OK, rs);
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, FloorViewModel vm)
        {
            string message = "";
            var floor = vm.UpdateModel();
            if (IoC.Resolve<IFloorService>().Create(floor, out message))
            {
                return request.CreateResponse(HttpStatusCode.OK, message);
            }

            return request.CreateResponse(HttpStatusCode.BadRequest, message);
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, FloorViewModel vm)
        {
            string message = "";
            var entity = IoC.Resolve<IFloorService>().GetbyKey(vm.Id);
            var floor = vm.UpdateModel(entity);
            if (IoC.Resolve<IFloorService>().Update(floor, out message))
            {
                return request.CreateResponse(HttpStatusCode.OK, message);
            }

            return request.CreateResponse(HttpStatusCode.BadRequest, message);
        }

        [Route("getbykey")]
        [HttpGet]
        public HttpResponseMessage GetbyKey(HttpRequestMessage request, int id)
        {
            var entity = IoC.Resolve<IFloorService>().GetbyKey(id);
            var vm = new FloorViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Descreption = entity.Descreption,
                Status = entity.Status,
                VIP = entity.VIP
            };

            return request.CreateResponse(HttpStatusCode.OK, vm);
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            string message = "";
            if (IoC.Resolve<IFloorService>().Delete(id, out message))
            {
                return request.CreateResponse(HttpStatusCode.OK, message);
            }

            return request.CreateResponse(HttpStatusCode.BadRequest, message);
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            var rs = IoC.Resolve<IFloorService>().GetMulti(n => n.Status == true && n.ComId == CurrentUser.Instance.User.ComId);
            return request.CreateResponse(HttpStatusCode.OK, rs);
        }
    }
}
