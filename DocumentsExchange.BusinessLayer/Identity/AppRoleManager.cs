using DocumentsExchange.DataAccessLayer;
using DocumentsExchange.DataLayer.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DocumentsExchange.BusinessLayer.Identity
{
    public class AppRoleManager : RoleManager<AppRole, int>
    {
        public AppRoleManager(IRoleStore<AppRole, int> store) : base(store)
        {
        }

        internal static AppRoleManager Create(DocumentsExchangeContext context)
        {
            return new AppRoleManager(new RoleStore<AppRole, int, AppUserRole>(context));
        }
    }
}
