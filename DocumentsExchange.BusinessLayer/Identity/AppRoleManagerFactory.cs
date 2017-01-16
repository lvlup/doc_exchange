using System.Data.Entity.Infrastructure;
using DocumentsExchange.DataAccessLayer;

namespace DocumentsExchange.BusinessLayer.Identity
{
    public class AppRoleManagerFactory
    {
        private readonly IDbContextFactory<DocumentsExchangeContext> _contextFactory;

        public AppRoleManagerFactory(IDbContextFactory<DocumentsExchangeContext> contextFactory )
        {
            _contextFactory = contextFactory;
        }

        public AppRoleManager Create()
        {
            return AppRoleManager.Create(_contextFactory.Create());
        }
    }
}
