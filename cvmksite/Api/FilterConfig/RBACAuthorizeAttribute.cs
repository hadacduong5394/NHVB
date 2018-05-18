using cvmk.context.IdentityConfiguration;
using hdcontext.IdentityDomain;
using hdcore;
using hdidentity.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace cvmksite.Api.FilterConfig
{
    public class RBACAuthorizeAttribute : AuthorizeAttribute
    {
        public string RoleName { get; set; }
        public RBACAuthorizeAttribute(): base()
        {

        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            try
            {
                var userId = CurrentUser.Instance.User.Id;
                var groups = IoC.Resolve<IUserGroupService>().GetMulti(n => n.UserId.Equals(userId));
                var lstRoleId = new List<Role>();
                foreach (var group in groups)
                {
                    var roleIds = IoC.Resolve<IRoleGroupService>().GetMulti(n => n.GroupId.Equals(group.GroupId)).Select(n => n.RoleId);
                    foreach (var id in roleIds)
                    {
                        lstRoleId.Add(IoC.Resolve<IRoleService>().GetbyKey(id));
                    }
                }

                return lstRoleId.Distinct().Any(n => n.Name == RoleName);
            }
            catch (Exception ex)
            {
                IoC.Resolve<cvmk.service.Interface.IErrorService>().TryLog(ex);
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (!IsAuthorized(actionContext))
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
        }
    }
}