using Autofac;
using DocumentsExchange.BusinessLayer.Identity;
using DocumentsExchange.BusinessLayer.Services.Implementations;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataAccessLayer.Repository;

namespace DocumentsExchange.BusinessLayer.AutoFac
{
    public class MainModule : Module
    {
       protected override void Load(ContainerBuilder builder)
       {
           builder.RegisterModule(new DataAccessLayer.AutoFac.MainModule());
           builder.RegisterType<OrganizationRepository>().AsSelf();
           builder.RegisterType<FileCategoryRepository>().AsSelf();
           builder.RegisterType<FileRepository>().AsSelf();
           builder.RegisterType<LogRepository>().AsSelf();
           builder.RegisterType<MessageRepository>().AsSelf();
           builder.RegisterType<RecordT1Repository>().AsSelf();
           builder.RegisterType<RecordT2Repository>().AsSelf();
           builder.RegisterType<UserRepository>().AsSelf();
           builder.RegisterType<FilePathRepository>().AsSelf();

           builder.RegisterType<GetCurrencyCourse>().As<IGetCurrencyCourse>();
           builder.RegisterType<UserProvider>().As<IUserProvider>();
           builder.RegisterType<FilePathProvider>().As<IFilePathProvider>();
           builder.RegisterType<FileCategoryProvider>().As<IFileCategoryProvider>();
           builder.RegisterType<FileProvider>().As<IFileProvider>();
           builder.RegisterType<OrganizationProvider>().As<IOrganizationProvider>();
           builder.RegisterType<RecordT1Provider>().As<IRecordT1Provider>();
           builder.RegisterType<RecordT2Provider>().As<IRecordT2Provider>();

           builder.RegisterType<ApplicationUserManagerFactory>().AsSelf();
           builder.RegisterType<AppRoleManagerFactory>().AsSelf();

           builder.RegisterAdapter<ApplicationUserManagerFactory, ApplicationUserManager>(x => x.Create());
           builder.RegisterAdapter<AppRoleManagerFactory, AppRoleManager>(x => x.Create());

           builder.RegisterType<FileValidator>().As<IFileValidator>();
       }
    }
}
