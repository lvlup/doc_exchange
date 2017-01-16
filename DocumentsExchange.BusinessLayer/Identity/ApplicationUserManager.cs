using DocumentsExchange.DataAccessLayer;
using DocumentsExchange.DataLayer.Entity;
using DocumentsExchange.DataLayer.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DocumentsExchange.BusinessLayer.Identity
{
    public class ApplicationUserManager : UserManager<User, int>
    {
        public ApplicationUserManager(IUserStore<User, int> store) : base(store)
        {
        }

        internal static ApplicationUserManager Create(DocumentsExchangeContext context)
        {
            return new ApplicationUserManager(new UserStore<User, AppRole, int, AppUserLogin, AppUserRole, AppUserClaim>(context));
        }
    }
}
