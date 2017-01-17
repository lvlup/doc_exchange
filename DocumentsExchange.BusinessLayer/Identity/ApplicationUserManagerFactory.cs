using System.Data.Entity.Infrastructure;
using DocumentsExchange.DataAccessLayer;
using DocumentsExchange.DataLayer.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace DocumentsExchange.BusinessLayer.Identity
{
    public class ApplicationUserManagerFactory
    {
        private readonly IDbContextFactory<DocumentsExchangeContext> _contextFactory;

        public ApplicationUserManagerFactory(IDbContextFactory<DocumentsExchangeContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public ApplicationUserManager Create()
        {
            return ApplicationUserManager.Create(_contextFactory.Create());
        }

        public ApplicationUserManager Create(DocumentsExchangeContext context)
        {
            return ApplicationUserManager.Create(context);
        }
    }
}
