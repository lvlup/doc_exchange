using System.Data.Entity.Infrastructure;
using Autofac;

namespace DocumentsExchange.DataAccessLayer.AutoFac
{
    public class MainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DocumentsExchangeContextFactory>()
                .As<IDbContextFactory<DocumentsExchangeContext>>()
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}
