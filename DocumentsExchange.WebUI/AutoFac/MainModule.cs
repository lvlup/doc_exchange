using System.Web;
using Autofac;
using Autofac.Integration.Mvc;

namespace DocumentsExchange.WebUI.AutoFac
{
    public class MainModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterControllers(typeof(MainModule).Assembly);
            builder.RegisterFilterProvider();
        }
    }
}