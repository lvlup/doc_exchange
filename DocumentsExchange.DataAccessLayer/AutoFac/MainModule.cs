using System.Data.Entity.Infrastructure;
using Autofac;
using DocumentsExchange.DataAccessLayer.Repository;

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

            builder.RegisterType<OrganizationRepository>().AsSelf();
            builder.RegisterType<FileCategoryRepository>().AsSelf();
            builder.RegisterType<FileRepository>().AsSelf();
            builder.RegisterType<LogRepository>().AsSelf();
            builder.RegisterType<MessageRepository>().AsSelf();
            builder.RegisterType<RecordT1Repository>().AsSelf();
            builder.RegisterType<RecordT2Repository>().AsSelf();
            builder.RegisterType<UserRepository>().AsSelf();
            builder.RegisterType<FilePathRepository>().AsSelf();

            builder.RegisterType<AdminRepository>().AsSelf();
            builder.RegisterType<MessageRepository>().AsSelf();
        }
    }
}
