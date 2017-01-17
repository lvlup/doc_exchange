using Autofac.Integration.SignalR;
using DocumentsExchange.Common;
using DocumentsExchange.Hub.Config;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(DocumentsExchange.WebUI.Startup.Startup))]

namespace DocumentsExchange.WebUI.Startup
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });

            var resolver = new AutofacDependencyResolver(AutoFacCore.Core.BeginLifetimeScope());

            app.Map("/signalr", map =>
            {
                HubConfiguration hubConfiguration = new HubConfiguration();

                hubConfiguration.EnableDetailedErrors = true;
                hubConfiguration.EnableJavaScriptProxies = true;
                hubConfiguration.Resolver = resolver;
                //var hubConfiguration = new HubConfiguration
                //{
                //    EnableDetailedErrors = true,
                //    EnableJavaScriptProxies = true,
                //    Resolver = resolver
                //};

                var pipeline = resolver.Resolve<IHubPipeline>();
                pipeline.AddModule(resolver.Resolve<LoggingModule>());

                // Run the SignalR pipeline. We're not using MapSignalR
                // since this branch already runs under the "/signalr"
                // path.
                map.RunSignalR(hubConfiguration);
            });
        }
    }
}
