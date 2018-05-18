using cvmk.context.IdentityConfiguration;
using cvmksite.Api.FilterConfig;
using cvmksite.Models.ViewModel;
using hdcontext.IdentityDomain;
using hdcore;
using hdcore.Utils;
using hddata.UnitOfWork;
using hdidentity.Interface;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace cvmksite.Api.Controllers
{
    [RoutePrefix("api/users")]
    [Authorize]
    public class UserController : ShareApiController
    {
        [HttpGet]
        [Route("getbyfilter")]
        public HttpResponseMessage GetbyFilter(HttpRequestMessage request, int? teamId, string userName, string email, string phoneNumber, int? page, int pageSize = 10)
        {
            try
            {
                var currentPage = page.HasValue ? page.Value : 0;
                int total = 0;
                var userService = IoC.Resolve<IUserService>();
                var items = userService.GetbyFilter(CurrentUser.Instance.User.Id, CurrentUser.Instance.User.ComId, teamId, userName, email, phoneNumber, currentPage, pageSize, out total).Select(p => new UserViewModel
                {
                    Id = p.Id,
                    UserName = p.UserName,
                    Email = p.Email,
                    PhoneNumber = p.PhoneNumber,
                    Address = p.Address,
                    BirthDay = p.BirthDay.HasValue ? p.BirthDay.Value.ToString("dd/MM/yyyy") : string.Empty,
                    CreateDate = p.CreateDate.ToString("dd/MM/yyyy"),
                    FullName = p.FullName,
                    TeamId = p.TeamId,
                    ComId = p.ComId,
                    Status = p.Status
                }).ToList();
                var result = new PaginationSet<UserViewModel>
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
        [RBACAuthorize(RoleName = "CREATE_USER")]
        public HttpResponseMessage CreateNew(HttpRequestMessage request, UserViewModel vm)
        {
            try
            {
                var userSrv = IoC.Resolve<IUserService>();
                string message = "";
                var groupIds = JsonConvert.DeserializeObject<List<int>>(vm.InputGroupId);
                var entity = vm.UpdateModel();
                if (userSrv.Create(entity, vm.PassWord, groupIds, out message))
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

        [HttpPut]
        [Route("update")]
        [RBACAuthorize(RoleName = "EDIT_USER")]
        public HttpResponseMessage Update(HttpRequestMessage request, UserViewModel vm)
        {
            try
            {
                var userSrv = IoC.Resolve<IUserService>();
                string message = "";
                var groupIds = JsonConvert.DeserializeObject<List<int>>(vm.InputGroupId);
                var entity = userSrv.GetbyKey(vm.Id);
                entity.UserName = vm.UserName;
                entity.FullName = vm.FullName;
                entity.Email = vm.Email;
                entity.Address = vm.Address;
                entity.PhoneNumber = vm.PhoneNumber;
                entity.Status = vm.Status;
                entity.TeamId = vm.TeamId;
                entity.BirthDay = DateTime.Now;
                if (CurrentUser.Instance.User.ComId == -1)
                {
                    entity.ComId = vm.ComId;
                }
                entity.UpdateBy = User.Identity.Name;
                entity.UpdateDate = DateTime.Now;


                if (userSrv.Update(entity, groupIds, out message))
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
        [RBACAuthorize(RoleName = "DELETE_USER")]
        public HttpResponseMessage Delete(HttpRequestMessage request, string userName)
        {
            var userSrv = IoC.Resolve<IUserService>();
            string message = "";
            var entity = userSrv.GetSingleByCondition(n => n.UserName == userName);
            if (userSrv.DeleteUser(entity, out message))
            {
                return request.CreateResponse(HttpStatusCode.OK, message);
            }

            return request.CreateResponse(HttpStatusCode.BadRequest, message);
        }

        [HttpGet]
        [Route("getbyusername")]
        public HttpResponseMessage Get(HttpRequestMessage request, string username)
        {
            try
            {
                var userSrv = IoC.Resolve<IUserService>();
                var p = userSrv.GetSingleByCondition(n => n.UserName == username);
                var result = new UserViewModel()
                {
                    Id = p.Id,
                    UserName = p.UserName,
                    Email = p.Email,
                    PhoneNumber = p.PhoneNumber,
                    Address = p.Address,
                    BirthDay = p.BirthDay.HasValue ? p.BirthDay.Value.ToString("yyyy/MM/dd") : string.Empty,
                    CreateDate = p.CreateDate.ToString("yyyy/MM/dd"),
                    FullName = p.FullName,
                    TeamId = p.TeamId,
                    ComId = p.ComId,
                    Status = p.Status
                };
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