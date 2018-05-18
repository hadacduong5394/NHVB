using cvmk.context.IdentityConfiguration;
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
    [RoutePrefix("api/groups")]
    [Authorize]
    public class GroupController : ShareApiController
    {
        [Route("getbyfilter")]
        [HttpGet]
        public HttpResponseMessage GetbyFilter(HttpRequestMessage request, string keyWord, int? page, int pageSize = 10)
        {
            try
            {
                int currentPage = page.HasValue ? page.Value : 0;
                int total = 0;
                var groupSrv = IoC.Resolve<IGroupService>();
                var comId = CurrentUser.Instance.User.ComId;
                var items = groupSrv.GetbyFilter(comId, keyWord, currentPage, pageSize, out total).Select(n => new GroupViewModel
                {
                    Id = n.Id,
                    Name = n.Name,
                    Descreption = n.Descreption,
                    Status = n.Status,
                    CreateBy = n.CreateBy,
                    UpdateBy = n.UpdateBy,
                    CreateDate = n.CreateDate.ToString("dd/MM/yyyy"),
                    UpdateDate = n.UpdateDate.HasValue ? n.UpdateDate.Value.ToString("dd/MM/yyyy") : ""
                });
                var result = new PaginationSet<GroupViewModel>()
                {
                    Page = currentPage,
                    PageSize = pageSize,
                    TotalCount = total,
                    Items = items.ToList()
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
        [RBACAuthorize(RoleName = "CREATE_GROUP_USER")]
        public HttpResponseMessage CreateNew(HttpRequestMessage request, GroupViewModel vm)
        {
            try
            {
                string message = "";
                var groupSrv = IoC.Resolve<IGroupService>();
                var group = vm.UpdateModel();
                group.CreateBy = User.Identity.Name;
                var lstRoleId = vm.DeserilizeRole();
                if (groupSrv.Create(group, lstRoleId, out message))
                {
                    return request.CreateResponse(HttpStatusCode.OK, message);
                }
                else
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, message);
                }
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
                var groupSrv = IoC.Resolve<IGroupService>();
                var entity = groupSrv.GetbyKey(id);
                var vm = new GroupViewModel
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Descreption = entity.Descreption,
                    Status = entity.Status,
                    CreateBy = entity.CreateBy,
                    CreateDate = entity.CreateDate.ToString("yyyy/MM/dd")
                };
                return request.CreateResponse(HttpStatusCode.OK, vm);
            }
            catch (Exception ex)
            {
                Log(ex);
                return request.CreateResponse(HttpStatusCode.BadRequest, TextHelper.ERROR_SYSTEM);
            }
        }

        [HttpPut]
        [Route("update")]
        [RBACAuthorize(RoleName = "EDIT_GROUP_USER")]
        public HttpResponseMessage Edit(HttpRequestMessage request, GroupViewModel vm)
        {
            try
            {
                var groupSrv = IoC.Resolve<IGroupService>();
                string message = "";
                var group = vm.UpdateModel();
                group.UpdateBy = "duonghd";
                group.UpdateDate = DateTime.Now;
                var lstRoleId = vm.DeserilizeRole();
                if (groupSrv.Update(group, lstRoleId, out message))
                {
                    return request.CreateResponse(HttpStatusCode.OK, message);
                }
                else
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, message);
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
        [RBACAuthorize(RoleName = "DELETE_GROUP_USER")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            try
            {
                var groupSrv = IoC.Resolve<IGroupService>();
                string message = "";
                groupSrv.Delete(id, out message);
                return request.CreateResponse(HttpStatusCode.OK, message);
            }
            catch (Exception ex)
            {
                Log(ex);
                return request.CreateResponse(HttpStatusCode.BadRequest, TextHelper.ERROR_SYSTEM);
            }
        }

        [HttpGet]
        [Route("viewdetail")]
        public HttpResponseMessage ViewDetail(HttpRequestMessage request, int id)
        {
            try
            {
                var groupSrv = IoC.Resolve<IGroupService>();
                var entity = groupSrv.GetbyKey(id);
                var vm = new GroupViewModel
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Descreption = entity.Descreption,
                    Status = entity.Status,
                    CreateBy = entity.CreateBy,
                    CreateDate = entity.CreateDate.ToString("dd/MM/yyyy"),
                    UpdateBy = entity.UpdateBy,
                    UpdateDate = entity.UpdateDate.HasValue ? entity.UpdateDate.Value.ToString("dd/MM/yyyy") : string.Empty
                };
                return request.CreateResponse(HttpStatusCode.OK, vm);
            }
            catch (Exception ex)
            {
                Log(ex);
                return request.CreateResponse(HttpStatusCode.BadRequest, TextHelper.ERROR_SYSTEM);
            }
        }

        [HttpGet]
        [Route("getgroups")]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            try
            {
                var groupSrv = IoC.Resolve<IGroupService>();
                var result = groupSrv.GetMulti(n => n.Status == true && n.ComId == CurrentUser.Instance.User.ComId).OrderByDescending(n => n.UpdateDate).Select(o => new GroupViewModel
                {
                    Id = o.Id,
                    Name = o.Name,
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

        [HttpGet]
        [Route("getgroupstoupdateuser")]
        public HttpResponseMessage GetGroupToUpdateUser(HttpRequestMessage request)
        {
            try
            {
                var groupSrv = IoC.Resolve<IGroupService>();
                if (CurrentUser.Instance.User.ComId == -1)
                {
                    var result = groupSrv.GetMulti(n => n.Status == true && n.ComId == CurrentUser.Instance.User.ComId).OrderByDescending(n => n.UpdateDate).Select(o => new GroupViewModel
                    {
                        Id = o.Id,
                        Name = o.Name,
                        Descreption = o.Descreption,
                        IsCheck = false
                    });
                    return request.CreateResponse(HttpStatusCode.OK, result);
                }
                else
                {
                    var result = groupSrv.GetMulti(n => n.Status == true && (n.ComId == CurrentUser.Instance.User.ComId || n.ComId == -1)).Where(n => !n.Name.Equals("System Admin") || !n.Name.Equals("Root")).OrderByDescending(n => n.UpdateDate).Select(o => new GroupViewModel
                    {
                        Id = o.Id,
                        Name = o.Name,
                        Descreption = o.Descreption,
                        IsCheck = false
                    });
                    return request.CreateResponse(HttpStatusCode.OK, result);
                }
            }
            catch (Exception ex)
            {
                Log(ex);
                return request.CreateResponse(HttpStatusCode.BadRequest, TextHelper.ERROR_SYSTEM);
            }
        }
    }
}