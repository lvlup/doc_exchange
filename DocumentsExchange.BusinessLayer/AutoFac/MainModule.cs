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
           builder.RegisterType<OrganizationRelevanceValidator>().As<IOrganizationRelevanceValidator>();
           builder.RegisterType<MessagesProvider>().As<IMessagesProvider>();
       }
    }
}
