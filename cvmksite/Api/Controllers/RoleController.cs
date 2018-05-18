using cvmk.context.IdentityConfiguration;
using cvmk.service.Interface;
using cvmksite.Api.FilterConfig;
using cvmksite.Models.ViewModel;
using hdcore;
using hdcore.Utils;
using hdidentity.Interface;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace cvmksite.Api.Controllers
{
    [RoutePrefix("api/roles")]
    [Authorize]
    public class RoleController : ShareApiController
    {
        [Route("getbyfilter")]
        [HttpGet]
        public HttpResponseMessage GetbyFilter(HttpRequestMessage request, string roleName, int? page, int pageSize = 10)
        {
            try
            {
                int currentPage = page.HasValue ? page.Value : 0;
                int total = 0;
                var roleSrv = IoC.Resolve<IRoleService>();
                var items = roleSrv.GetbyFilter(roleName, currentPage, pageSize, out total).Select(p => new RoleViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Descreption = p.Descreption,
                    Status = p.Status,
                    CreateBy = p.CreateBy,
                    CreateDate = p.CreateDate.ToString("dd/MM/yyyy")
                }).ToList();
                var result = new PaginationSet<RoleViewModel>()
                {
                    Page = currentPage,
                    PageSize = pageSize,
                    TotalCount = total,
                    Items = items
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
        [RBACAuthorize(RoleName = "ROLE_CREATE")]
        public HttpResponseMessage CreateNew(HttpRequestMessage request, RoleViewModel vm)
        {
            try
            {
                var roleSrv = IoC.Resolve<IRoleService>();
                if (!roleSrv.CheckContainName(vm.Id, vm.Name))
                {
                    vm.CreateBy = "duonghd";
                    var model = vm.UpdateModel();
                    string message = "";
                    roleSrv.Create(model, out message);
                    return request.CreateResponse(HttpStatusCode.OK, message);
                }
                else
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, "Tên này đã tồn tại trước đó.");
                }
            }
            catch (Exception ex)
            {
                Log(ex);
                return request.CreateResponse(HttpStatusCode.BadRequest, TextHelper.ERROR_SYSTEM);
            }
        }

        [HttpPut]
        [Route("update")]
        [RBACAuthorize(RoleName = "EDIT_ROLE")]
        public HttpResponseMessage Update(HttpRequestMessage request, RoleViewModel vm)
        {
            try
            {
                var roleSrv = IoC.Resolve<IRoleService>();
                if (!roleSrv.CheckContainName(vm.Id, vm.Name))
                {
                    vm.UpdateBy = "duonghd";
                    var model = vm.UpdateModel();
                    model.UpdateDate = DateTime.Now;
                    string message = "";
                    roleSrv.Update(model, out message);
                    return request.CreateResponse(HttpStatusCode.OK, message);
                }
                else
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, "Tên này đã tồn tại trước đó.");
                }
            }
            catch (Exception ex)
            {
                Log(ex);
                return request.CreateResponse(HttpStatusCode.BadRequest, TextHelper.ERROR_SYSTEM);
            }
        }

        [HttpDelete]
        [Route("delete")]
        [RBACAuthorize(RoleName = "ROLE_DELETE")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            try
            {
                var roleSrv = IoC.Resolve<IRoleService>();
                string message = "";
                roleSrv.Delete(id, out message);
                return request.CreateResponse(HttpStatusCode.OK, message);
            }
            catch (Exception ex)
            {
                Log(ex);
                return request.CreateResponse(HttpStatusCode.BadRequest, TextHelper.ERROR_SYSTEM);
            }
        }

        [HttpGet]
        [Route("getbykey/{id:int}")]
        public HttpResponseMessage GetbyKey(HttpRequestMessage request, int id)
        {
            try
            {
                var roleSrv = IoC.Resolve<IRoleService>();
                var entity = roleSrv.GetbyKey(id);
                var rs = new RoleViewModel
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Descreption = entity.Descreption,
                    CreateBy = entity.CreateBy,
                    CreateDate = entity.CreateDate.ToString("yyyy/MM/dd"),
                    Status = entity.Status,
                };
                return request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch (Exception ex)
            {
                Log(ex);
                return request.CreateResponse(HttpStatusCode.BadRequest, TextHelper.ERROR_SYSTEM);
            }
        }

        [HttpGet]
        [Route("detail")]
        public HttpResponseMessage ViewDetail(HttpRequestMessage request, int id)
        {
            try
            {
                var roleSrv = IoC.Resolve<IRoleService>();
                var entity = roleSrv.GetbyKey(id);
                var rs = new RoleViewModel
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Descreption = entity.Descreption,
                    CreateBy = entity.CreateBy,
                    CreateDate = entity.CreateDate.ToString("dd/MM/yyyy"),
                    Status = entity.Status,
                    UpdateBy = entity.UpdateBy,
                    UpdateDate = entity.UpdateDate.HasValue ? entity.UpdateDate.Value.ToString("dd/MM/yyyy") : ""
                };
                return request.CreateResponse(HttpStatusCode.OK, rs);
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
                var result = IoC.Resolve<IRoleService>().GetbyUserRole(CurrentUser.Instance.User.ComId).Select(o => new RoleViewModel
                {
                    Id = o.Id,
                    Descreption = o.Descreption,
                    IsCheck = false
                });
                return request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Log(ex);
                return request.CreateResponse(HttpStatusCode.BadRequest, TextHelper.ERROR_SYSTEM);
            }
        }
    }
}