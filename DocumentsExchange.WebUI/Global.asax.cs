using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac.Extras.NLog;
using Autofac.Integration.Mvc;
using DocumentsExchange.BusinessLayer.Identity;
using DocumentsExchange.Common;
using DocumentsExchange.DataLayer.Entity;
using DocumentsExchange.DataLayer.Identity;
using DocumentsExchange.WebUI.App_Start;
using Microsoft.AspNet.Identity;

namespace DocumentsExchange.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AutoFacCore.Init(
                new BusinessLayer.AutoFac.MainModule(),
                new AutoFac.MainModule(),
                 new NLogModule()
                );

            DependencyResolver.SetResolver(new AutofacDependencyResolver(AutoFacCore.Core));

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            InitRoles();
        }

        void InitRoles()
        {
            var roleManager = DependencyResolver.Current.GetService<AppRoleManager>();
            var userManager = DependencyResolver.Current.GetService<ApplicationUserManager>();

            if (!roleManager.RoleExists(Roles.Admin))
            {
                if (!roleManager.Create(new AppRole(Roles.Admin)).Succeeded)
                    throw new Exception("Initialization failed");
            }

            if (!roleManager.RoleExists(Roles.User))
            {
                if (!roleManager.Create(new AppRole(Roles.User)).Succeeded)
                    throw new Exception("Initialization failed");
            }

            if (!roleManager.RoleExists(Roles.Observer))
            {
                if (!roleManager.Create(new AppRole(Roles.Observer)).Succeeded)
                    throw new Exception("Initialization failed");
            }

            if (!roleManager.RoleExists(Roles.Technician))
            {
                if (!roleManager.Create(new AppRole(Roles.Technician)).Succeeded)
                    throw new Exception("Initialization failed");
            }

            var admin = userManager.FindByName("admin");
            if (admin == null)
            {
                var result = userManager.Create(new User {UserName = "admin",FirstName = "Иван",LastName = "Иванов"}, "admin123");
                if (!result.Succeeded)
                    throw new Exception("Initialization failed");

                admin = userManager.FindByName("admin");
                
                if (!userManager.AddToRole(admin.Id, Roles.Admin).Succeeded)
                    throw new Exception("Initialization failed");
            }
        }
    }
}
