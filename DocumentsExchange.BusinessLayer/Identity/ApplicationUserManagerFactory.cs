using System.Data.Entity.Infrastructure;
using DocumentsExchange.DataAccessLayer;

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
    }
}
