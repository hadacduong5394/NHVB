using cvmk.context.IdentityConfiguration;
using cvmksite.Models;
using hdcore;
using hdcore.Utils;
using hdidentity.Interface;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Http;

namespace cvmksite.Api.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private readonly string fromMail = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
        private readonly string displayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
        private readonly string emailPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
        private readonly string host = ConfigurationManager.AppSettings["SMTPHost"].ToString();
        private readonly string port = ConfigurationManager.AppSettings["SMTPPort"].ToString();
        private readonly bool ssl = bool.Parse(ConfigurationManager.AppSettings["EnabledSSL"].ToString());
        private readonly int maxTimeStamp = 2 * 3600 * 24;
        private readonly string _domain = ConfigurationManager.AppSettings["AppDomain"].ToString();

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<HttpResponseMessage> Login(HttpRequestMessage request, string userName, string password)
        {
            if (!ModelState.IsValid)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await UserManager.Instance.SignInManager.PasswordSignInAsync(userName, password, false, shouldLockout: false);
            return request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("lostpassword")]
        public HttpResponseMessage LostPassword(HttpRequestMessage request, LostPasswordModel model)
        {
            try
            {
                var user = IoC.Resolve<IUserService>().GetSingleByCondition(n => n.UserName.Equals(model.UserName));
                if (model.UserName.Equals(user.UserName) && model.Email.Equals(user.Email))
                {
                    string keyId = user.Id;
                    string nonce = Guid.NewGuid().ToString("N");
                    string link = _domain + "/api/account/resetpassword?keyId=" + keyId + "&nonce=" + nonce;
                    string contentMail = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/Content/html/EmailLostpassword.html"));
                    contentMail = contentMail.Replace("{{account}}", user.UserName);
                    contentMail = contentMail.Replace("{{link}}", link);
                    var timeStamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    MemoryCache.Default.Add(nonce, timeStamp, DateTimeOffset.UtcNow.AddSeconds(maxTimeStamp));

                    EmailHelper.SendEmail(fromMail, user.Email, "Hệ thống thông báo", contentMail, displayName, emailPassword, host, port, ssl);
                    return request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                IoC.Resolve<cvmk.service.Interface.IErrorService>().TryLog(ex);
                return request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("resetpassword")]
        public HttpResponseMessage ResetPassword(HttpRequestMessage request, string keyId, string nonce)
        {
            try
            {
                var timeStamp = MemoryCache.Default.Get(nonce);
                if (timeStamp != null)
                {
                    var nowTimeStamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    int time = (int)timeStamp;
                    if ((nowTimeStamp - time) <= maxTimeStamp)
                    {
                        var userService = IoC.Resolve<IUserService>();
                        var current = userService.GetbyKey(keyId);
                        var newPass = TextHelper.GenerateRandomText(12);
                        string message = "";
                        if (userService.ResetPassword(keyId, newPass, out message))
                        {
                            string contentMail = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/Content/html/NewPassword.html"));
                            contentMail = contentMail.Replace("{{NewPass}}", newPass);
                            EmailHelper.SendEmail(fromMail, current.Email, "Hệ thống thông báo", contentMail, displayName, emailPassword, host, port, ssl);
                            return request.CreateResponse(HttpStatusCode.OK, message);
                        }
                    }
                }
                return request.CreateResponse(HttpStatusCode.BadRequest, TextHelper.ERROR_SYSTEM);
            }
            catch (Exception ex)
            {
                IoC.Resolve<cvmk.service.Interface.IErrorService>().TryLog(ex);
                return request.CreateResponse(HttpStatusCode.BadRequest, TextHelper.ERROR_SYSTEM);
            }
            finally
            {
                MemoryCache.Default.Remove(nonce);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("changepassword")]
        public HttpResponseMessage ChangePassword(HttpRequestMessage request, ChangePasswordModel vm)
        {
            try
            {
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    var userSrv = IoC.Resolve<IUserService>();
                    var entity = userSrv.GetSingleByCondition(n => n.UserName == vm.UserName);

                    if (userSrv.ChangePassword(entity.Id, vm.CurrenPassword, vm.NewPassWord, out message))
                    {
                        return request.CreateResponse(HttpStatusCode.OK, message);
                    }
                }
                return request.CreateResponse(HttpStatusCode.BadRequest, message ?? string.Join(",", ModelState.Values));
            }
            catch (Exception ex)
            {
                IoC.Resolve<cvmk.service.Interface.IErrorService>().TryLog(ex);
                return request.CreateResponse(HttpStatusCode.BadRequest, TextHelper.ERROR_SYSTEM);
            }
        }

        [Route("logout")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage LogOut(HttpRequestMessage request)
        {
            UserManager.Instance.SignInManager.AuthenticationManager.SignOut();
            return request.CreateResponse(HttpStatusCode.OK);
        }
    }
}