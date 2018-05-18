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
    [RoutePrefix("api/room")]
    public class RoomController : ShareApiController
    {
        [Route("getbyfilter")]
        [HttpGet]
        public HttpResponseMessage GetbyFilter(HttpRequestMessage request, int floorId, string name, int page = 0, int pageSize = 10)
        {
            int total = 0;
            int com_id = CurrentUser.Instance.User.ComId;
            var items = IoC.Resolve<IRoomService>().GetbyFilter(com_id, floorId, name, page, pageSize, out total).Select(n => new RoomViewModel
            {
                Id = n.Id,
                Name = n.Name,
                CreateBy = n.CreateBy,
                Descreption = n.Descreption,
                Status = n.Status,
                IsWorking = n.IsWorking,
            }).ToList();
            var rs = new PaginationSet<RoomViewModel>
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
        public HttpResponseMessage Create(HttpRequestMessage request, RoomViewModel vm)
        {
            string message = "";
            var entity = vm.UpdateModel();
            if (IoC.Resolve<IRoomService>().Create(entity, out message))
            {
                return request.CreateResponse(HttpStatusCode.OK, message);
            }

            return request.CreateResponse(HttpStatusCode.BadRequest, message);
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, RoomViewModel vm)
        {
            string message = "";
            var entity = IoC.Resolve<IRoomService>().GetbyKey(vm.Id);
            var room = vm.UpdateModel(entity);
            if (IoC.Resolve<IRoomService>().Update(room, out message))
            {
                return request.CreateResponse(HttpStatusCode.OK, message);
            }

            return request.CreateResponse(HttpStatusCode.BadRequest, message);
        }

        [Route("getbykey")]
        [HttpGet]
        public HttpResponseMessage GetbyKey(HttpRequestMessage request, int id)
        {
            var entity = IoC.Resolve<IRoomService>().GetbyKey(id);
            var vm = new RoomViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Descreption = entity.Descreption,
                FloorId = entity.FloorId,
                Status = entity.Status,
                IsWorking = entity.IsWorking,
            };

            return request.CreateResponse(HttpStatusCode.OK, vm);
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            string message = "";
            if (IoC.Resolve<IRoomService>().Delete(id, out message))
            {
                return request.CreateResponse(HttpStatusCode.OK, message);
            }

            return request.CreateResponse(HttpStatusCode.BadRequest, message);
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, int floorId)
        {
            return request.CreateResponse(HttpStatusCode.OK, IoC.Resolve<IRoomService>().GetbyFloorId(floorId));
        }

        [Route("getbyfloorid")]
        [HttpGet]
        public HttpResponseMessage GetbyFloorId(HttpRequestMessage request, int floorId)
        {
            return request.CreateResponse(HttpStatusCode.OK, IoC.Resolve<IRoomService>().GetbyFloorId(floorId));
        }

        [Route("getstatusworkingtable")]
        [HttpGet]
        public HttpResponseMessage GetStatus(HttpRequestMessage request, int tableId)
        {
            return request.CreateResponse(HttpStatusCode.OK, IoC.Resolve<IRoomService>().GetbyKey(tableId).IsWorking);
        }

        [Route("loadtablenotworking")]
        [HttpGet]
        public HttpResponseMessage LoadTableNotWorking(HttpRequestMessage request, int floorId)
        {
            int comid = CurrentUser.Instance.User.ComId;
            var tableSrv = IoC.Resolve<IRoomService>();
            var tables = tableSrv.GetTableIsNotWorking(comid, floorId);
            return request.CreateResponse(HttpStatusCode.OK, tables);
        }

        [Route("loadtableworking")]
        [HttpGet]
        public HttpResponseMessage LoadTableWorking(HttpRequestMessage request, int momentTableId, int floorId)
        {
            int comid = CurrentUser.Instance.User.ComId;
            var tableSrv = IoC.Resolve<IRoomService>();
            var tables = tableSrv.GetTableWorking(comid, momentTableId, floorId);
            return request.CreateResponse(HttpStatusCode.OK, tables);
        }
    }
}
