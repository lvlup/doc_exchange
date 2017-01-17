using Autofac;
using Autofac.Integration.SignalR;
using DocumentsExchange.Hub.Config;
using DocumentsExchange.Hub.Models;
using DocumentsExchange.Hub.Services;
using DocumentsExchange.Hub.Utils;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace DocumentsExchange.Hub.AutoFac
{
    public class MainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HubUserIdProvider>().As<IUserIdProvider>();
            builder.Register(c => new ConnectionPool<HubUser, string>(u => u.Id)).SingleInstance();
            builder.RegisterType<PresenceMonitor>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterHubs(typeof(PresenceMonitor).Assembly).ExternallyOwned();
            builder.Register(
                c =>
                    JsonSerializer.Create(new JsonSerializerSettings()
                    {
                        ContractResolver = new SignalRContractResolver()
                    })).As<JsonSerializer>();

            builder.RegisterType<LoggingModule>().AsSelf();
        }
    }
}
