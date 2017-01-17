using System.Threading.Tasks;
using DocumentsExchange.DataAccessLayer;
using DocumentsExchange.DataLayer.Entity;
using DocumentsExchange.DataLayer.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace DocumentsExchange.BusinessLayer.Identity
{
    public class ApplicationUserManager : UserManager<User, int>
    {
        public ApplicationUserManager(IUserStore<User, int> store) : base(store)
        {
        }

        internal static ApplicationUserManager Create(DocumentsExchangeContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<User, AppRole, int, AppUserLogin, AppUserRole, AppUserClaim>(context));
            var provider = new DpapiDataProtectionProvider("Sample");

            manager.UserTokenProvider = new DataProtectorTokenProvider<User, int>(
                provider.Create("EmailConfirmation"));

            return manager;
        }
    }
}
