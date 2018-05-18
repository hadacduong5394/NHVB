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
    [RoutePrefix("api/topmenu")]
    [Authorize]
    public class TopMenuController : ShareApiController
    {
        [HttpGet]
        [Route("gettopmenu")]
        public HttpResponseMessage GetTopMenu(HttpRequestMessage request)
        {
            try
            {
                var topSrv = IoC.Resolve<ITopMenuService>();
                var rs = topSrv.GetParents().Select(m => new TopMenuViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    OrderNumber = m.OrderNumber,
                    ParentId = m.ParentId,
                    UI_SREF = m.UI_SREF,
                    ICon = m.Icon
                });
                return request.CreateResponse(HttpStatusCode.OK, rs);
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
                var topSrv = IoC.Resolve<ITopMenuService>();
                return request.CreateResponse(HttpStatusCode.OK, topSrv.GetParents());
            }
            catch (Exception ex)
            {
                Log(ex);
                return request.CreateResponse(HttpStatusCode.BadRequest, TextHelper.ERROR_SYSTEM);
            }
        }

        [Route("getbyfilter")]
        [HttpGet]
        public HttpResponseMessage GetbyFilter(HttpRequestMessage request, int parentId = -1, int? page = 0, int pageSize = 10)
        {
            try
            {
                int currentPage = page.HasValue ? page.Value : 0;
                int total = 0;
                var topSrv = IoC.Resolve<ITopMenuService>();
                var items = topSrv.GetbyFilter(parentId, currentPage, pageSize, out total).Select(n => new TopMenuViewModel {
                    Id = n.Id,
                    Name = n.Name,
                    ParentId = n.ParentId,
                    ICon = n.Icon,
                    UI_SREF = n.UI_SREF,
                    OrderNumber = n.OrderNumber,
                    Status = n.Status
                }).ToList();
                var rs = new PaginationSet<TopMenuViewModel>
                {
                    Items = items,
                    Page = currentPage,
                    PageSize = pageSize,
                    TotalCount = total
                };

                return request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch (Exception ex)
            {
                IoC.Resolve<IErrorService>().TryLog(ex);
                return request.CreateResponse(HttpStatusCode.BadRequest, TextHelper.ERROR_SYSTEM);
            }
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, TopMenuViewModel vm)
        {
            string mess = "";
            if (IoC.Resolve<ITopMenuService>().Create(vm.UpdateModel(), out mess))
            {
                return request.CreateResponse(HttpStatusCode.OK, mess);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, TopMenuViewModel vm)
        {
            string mess = "";
            var topSrv = IoC.Resolve<ITopMenuService>();
            var entity = topSrv.GetbyKey(vm.Id);
            if (topSrv.Update(vm.UpdateModel(entity), out mess))
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
            var topSrv = IoC.Resolve<ITopMenuService>();
            if (topSrv.Delete(id, out mess))
            {
                return request.CreateResponse(HttpStatusCode.OK, mess);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [HttpGet]
        [Route("lastorder")]
        public HttpResponseMessage LoadLastOrder(HttpRequestMessage request, int parentId = -1)
        {
            var topSrv = IoC.Resolve<ITopMenuService>();
            int rs = 0;
            if (parentId != -1)
            {
                var entity = topSrv.GetChilds(parentId).OrderByDescending(n => n.OrderNumber).FirstOrDefault();
                rs = entity == null ? 0 : entity.OrderNumber + 1;
            }
            else
            {
                var entity = topSrv.GetParents().OrderByDescending(n => n.OrderNumber).FirstOrDefault();
                rs = entity == null ? 0 : entity.OrderNumber + 1;
            }
            return request.CreateResponse(HttpStatusCode.OK, rs);
        }

        [HttpGet]
        [Route("getbykey")]
        public HttpResponseMessage GetEntity(HttpRequestMessage request, int id)
        {
            var entity = IoC.Resolve<ITopMenuService>().GetbyKey(id);
            var rs = new TopMenuViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                ICon = entity.Icon,
                OrderNumber = entity.OrderNumber,
                ParentId = entity.ParentId,
                UI_SREF = entity.UI_SREF,
                Status = entity.Status
            };
            return request.CreateResponse(HttpStatusCode.OK, rs);
        }
    }
}