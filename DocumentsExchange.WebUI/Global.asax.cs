using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac.Extras.NLog;
using Autofac.Integration.Mvc;
using DocumentsExchange.Common;

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
        }
    }
}
