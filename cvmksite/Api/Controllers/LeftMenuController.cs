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
    [RoutePrefix("api/leftmenu")]
    [Authorize]
    public class LeftMenuController : ShareApiController
    {
        [HttpGet]
        [Route("getleftmenu")]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            try
            {
                var lmSrv = IoC.Resolve<ILeftMenuService>();
                var leftParents = lmSrv.GetParents().Select(m => new LeftMenuParentViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    UI_SREF = m.UI_SREF,
                    Icon = m.Icon
                });
                return request.CreateResponse(HttpStatusCode.OK, leftParents);
            }
            catch (Exception ex)
            {
                Log(ex);
                return request.CreateResponse(HttpStatusCode.BadRequest, TextHelper.ERROR_SYSTEM);
            }
        }

        [HttpGet]
        [Route("getparents")]
        public HttpResponseMessage GetParents(HttpRequestMessage request)
        {
            try
            {
                var lmSrv = IoC.Resolve<ILeftMenuService>();
                return request.CreateResponse(HttpStatusCode.OK, lmSrv.GetParents());
            }
            catch (Exception ex)
            {
                Log(ex);
                return request.CreateResponse(HttpStatusCode.BadRequest, TextHelper.ERROR_SYSTEM);
            }
        }

        [HttpGet]
        [Route("getbyfilter")]
        public HttpResponseMessage GetbyFilter(HttpRequestMessage request, int parentId = -1, int? page = 0, int pageSize = 10)
        {
            try
            {
                var leftSrv = IoC.Resolve<ILeftMenuService>();
                int currentPage = page.HasValue ? page.Value : 0;
                int total = 0;
                var items = leftSrv.GetbyFilter(parentId, currentPage, pageSize, out total).Select(n => new LeftMenuViewModel
                {
                    Id = n.Id,
                    Icon = n.Icon,
                    Name = n.Name,
                    Status = n.Status,
                    OrderNumber = n.OrderNumber,
                    ParentId = n.ParentId,
                    UI_SREF = n.UI_SREF,
                    CreateBy = n.CreateBy,
                    CreateDate = n.CreateDate.ToString("dd/MM/yyyy"),
                    ModifyBy = n.ModifyBy,
                    ModifyDate = n.ModifyDate.HasValue ? n.ModifyDate.Value.ToString("dd/MM/yyyy") : string.Empty
                }).ToList();
                var result = new PaginationSet<LeftMenuViewModel>
                {
                    Page = currentPage,
                    Items = items,
                    PageSize = pageSize,
                    TotalCount = total
                };

                return request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Log(ex);
                return request.CreateResponse(HttpStatusCode.BadRequest, TextHelper.ERROR_SYSTEM);
            }
        }

        [HttpPost]
        [Route("create")]
        public HttpResponseMessage Create(HttpRequestMessage request, LeftMenuViewModel vm)
        {
            var leftSrv = IoC.Resolve<ILeftMenuService>();
            var entity = vm.UpdateModel();
            string message = "";
            if (leftSrv.Create(entity, out message))
            {
                return request.CreateResponse(HttpStatusCode.OK, message);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, message);
        }

        [HttpGet]
        [Route("getbykey")]
        public HttpResponseMessage GetEntity(HttpRequestMessage request, int id)
        {
            var entity = IoC.Resolve<ILeftMenuService>().GetbyKey(id);
            var rs = new LeftMenuViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Icon = entity.Icon,
                OrderNumber = entity.OrderNumber,
                ParentId = entity.ParentId,
                UI_SREF = entity.UI_SREF,
                Status = entity.Status
            };
            return request.CreateResponse(HttpStatusCode.OK, rs);
        }

        [HttpPut]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, LeftMenuViewModel vm)
        {
            var leftSrv = IoC.Resolve<ILeftMenuService>();
            var entity = leftSrv.GetbyKey(vm.Id);
            var model = vm.UpdateModel(entity);
            string message = "";
            if (leftSrv.Update(model, out message))
            {
                return request.CreateResponse(HttpStatusCode.OK, message);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, message);
        }

        [HttpGet]
        [Route("lastorder")]
        public HttpResponseMessage LoadLastOrder(HttpRequestMessage request, int parentId = -1)
        {
            var leftSrv = IoC.Resolve<ILeftMenuService>();
            int rs = 0;
            if (parentId != -1)
            {
                var entity = leftSrv.GetChilds(parentId).OrderByDescending(n => n.OrderNumber).FirstOrDefault();
                rs = entity == null ? 0 : entity.OrderNumber + 1;
            }
            else
            {
                var entity = leftSrv.GetParents().OrderByDescending(n => n.OrderNumber).FirstOrDefault();
                rs = entity == null ? 0 : entity.OrderNumber + 1;
            }
            return request.CreateResponse(HttpStatusCode.OK, rs);
        }

        [HttpDelete]
        [Route("delete")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            string message = "";
            if (IoC.Resolve<ILeftMenuService>().Delete(id, out message))
            {
                return request.CreateResponse(HttpStatusCode.OK, message);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, message);
        }

    }
}