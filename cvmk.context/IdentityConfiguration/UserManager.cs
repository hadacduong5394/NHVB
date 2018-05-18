using Microsoft.AspNet.Identity.Owin;
using System.Web;

namespace cvmk.context.IdentityConfiguration
{
    public class UserManager
    {
        private static UserManager _instance;

        private UserManager()
        {
        }

        public static UserManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserManager();
                }
                return _instance;
            }
        }

        private ApplicationUserManager _userManagerment;
        private ApplicationSignInManager _signInManager;

        public ApplicationUserManager UserManagerment
        {
            get
            {
                return _userManagerment ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManagerment = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
    }
}