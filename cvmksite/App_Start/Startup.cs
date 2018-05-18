using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using cvmk.context;
using cvmk.context.IdentityConfiguration;
using cvmksite.Models.ViewModel;
using hd.data.SQLServerHelper;
using hdcontext;
using hdcontext.IdentityDomain;
using hddata.DBFactory;
using hddata.UnitOfWork;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(cvmksite.App_Start.Startup))]

namespace cvmksite.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigAutofac(app);
            ConfigureAuth(app);
        }

        private void ConfigAutofac(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<hddata.UnitOfWork.UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<cvmk.context.dbFactory.DbFactory>().As<IDbFactory>().InstancePerRequest();
            builder.RegisterType<Context>().AsSelf().InstancePerRequest();

            builder.RegisterType<ApplicationUserStore>().As<IUserStore<ApplicationUser>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register(c => app.GetDataProtectionProvider()).InstancePerRequest();

            hdcore.ConfigAutofac.RegisterControllers(builder);

            hdcore.ConfigAutofac.RegisterAssemblyTypes<hdidentity.Implement.ErrorService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<hdidentity.Implement.GroupService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<hdidentity.Implement.RoleGroupService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<hdidentity.Implement.RoleService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<hdidentity.Implement.UserGroupService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<hdidentity.Implement.UserService>(builder);

            hdcore.ConfigAutofac.RegisterAssemblyTypes<cvmk.service.Implement.ErrorService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<cvmk.service.Implement.CompanyInfoService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<cvmk.service.Implement.LeftMenuService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<cvmk.service.Implement.TopMenuService>(builder);

            hdcore.ConfigAutofac.RegisterAssemblyTypes<cvmk.service.Implement.ArticleCategoryService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<cvmk.service.Implement.SupplierService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<cvmk.service.Implement.CustomerService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<cvmk.service.Implement.RoomService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<cvmk.service.Implement.FloorService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<cvmk.service.Implement.TypeProductCategoryService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<cvmk.service.Implement.ProductService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<cvmk.service.Implement.ProductPropertisService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<cvmk.service.Implement.GroupProductCategoryService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<cvmk.service.Implement.ImportProductService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<cvmk.service.Implement.ImportProductDetailService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<cvmk.service.Implement.OrderService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<cvmk.service.Implement.OrderDetailService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<cvmk.service.Implement.MaterialService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<cvmk.service.Implement.ProductMaterialService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<cvmk.service.Implement.CustomerCodeService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<cvmk.service.Implement.SupplierCodeService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<cvmk.service.Implement.ProductCodeService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<cvmk.service.Implement.MaterialCodeService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<cvmk.service.Implement.OrderCodeService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<cvmk.service.Implement.ImportProductCodeService>(builder);

            Autofac.IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container);
        }
    }
}