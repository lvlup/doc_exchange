using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac.Extras.NLog;
using Autofac.Integration.Mvc;
using DocumentsExchange.BusinessLayer.Identity;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
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
                new Hub.AutoFac.MainModule(),
                new NLogModule()
                );

            DependencyResolver.SetResolver(new AutofacDependencyResolver(AutoFacCore.Core));

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            InitRoles();
        }

        protected void Application_BeginRequest()
        {
            IBlockIpService biph = DependencyResolver.Current.GetService<IBlockIpService>();
            
            if (biph.IsIpBlocked(HttpContext.Current.Request.UserHostAddress))
            {
                if (HttpContext.Current.Request.Path.Contains("/SiteStopped/Banned"))
                    return;

                Response.Redirect("SiteStopped/Banned");
            }
        }

        protected void Application_AuthorizeRequest(Object sender, EventArgs e)
        {
            IAdminProvider adminProvider = DependencyResolver.Current.GetService<IAdminProvider>();
            ApplicationUserManager userManager = DependencyResolver.Current.GetService<ApplicationUserManager>();

            if (!adminProvider.GetWebSiteState().Result.IsActive && (!HttpContext.Current.User?.IsInRole(Roles.Admin) ?? true))
            {
                if (HttpContext.Current.Request.Path.Contains("/SiteStopped"))
                    return;

                Response.Redirect("SiteStopped/Index");
                return;
            }

            var user = userManager.FindById(HttpContext.Current.User?.Identity.GetUserId<int>() ?? 0);
            if (user != null && !user.IsActive)
            {
                if (HttpContext.Current.Request.Path.Contains("/SiteStopped"))
                    return;

                HttpContext.Current.GetOwinContext().Authentication.SignOut();
                Response.Redirect("SiteStopped/LoginDeactivated");
            }
        }

        void InitRoles()
        {
            var roleManager = DependencyResolver.Current.GetService<AppRoleManager>();
            var userManager = DependencyResolver.Current.GetService<ApplicationUserManager>();
            var orgProvider = DependencyResolver.Current.GetService<IOrganizationProvider>();

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
                var result = userManager.Create(new User
                {
                    UserName = "admin",
                    FirstName = "Иван",
                    LastName = "Иванов",
                    IsActive = true,
                    Organizations =  orgProvider.GetAll().Result.ToList(),
                    ActivityDateTime = DateTime.UtcNow
                }, "admin123");

                if (!result.Succeeded)
                    throw new Exception("Initialization failed");

                admin = userManager.FindByName("admin");

                if (!userManager.AddToRole(admin.Id, Roles.Admin).Succeeded)
                    throw new Exception("Initialization failed");
            }

            var pedik = userManager.FindByName("pedik");
            if (pedik == null)
            {
                var result = userManager.Create(new User {UserName = "pedik", FirstName = "pedik", LastName = "pedik"},
                    "pedik123");
                if (!result.Succeeded)
                    throw new Exception("Initialization failed");

                pedik = userManager.FindByName("pedik");

                if (!userManager.AddToRole(pedik.Id, Roles.User).Succeeded)
                    throw new Exception("Initialization failed");
            }
        }
    }
}

   
