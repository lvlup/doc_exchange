using System;
using System.Linq;
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
        private readonly UserStore<User, AppRole, int, AppUserLogin, AppUserRole, AppUserClaim> _store;

        public ApplicationUserManager(UserStore<User, AppRole, int, AppUserLogin, AppUserRole, AppUserClaim> store)
            : base(store)
        {
            _store = store;
        }

        internal static ApplicationUserManager Create(DocumentsExchangeContext context)
        {
            var manager =
                new ApplicationUserManager(
                    new UserStore<User, AppRole, int, AppUserLogin, AppUserRole, AppUserClaim>(context));
            var provider = new DpapiDataProtectionProvider("Sample");

            manager.UserTokenProvider = new DataProtectorTokenProvider<User, int>(
                provider.Create("EmailConfirmation"));

            return manager;
        }

        public async Task<bool> AddUser(User user, string password)
        {
            using (var dbContextTransaction = _store.Context.Database.BeginTransaction())
            {
                try
                {
                    var fromDb = await this.FindByNameAsync(user.UserName).ConfigureAwait(false);
                    var result = await this.AddPasswordAsync(fromDb.Id, password).ConfigureAwait(false);

                    if (!result.Succeeded)
                        throw new Exception(string.Join(",", result.Errors));

                    result = await this.AddToRolesAsync(fromDb.Id,
                        user.RoleList.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => x.Trim())
                            .ToArray()).ConfigureAwait(false);

                    if (!result.Succeeded)
                        throw new Exception(string.Join(",", result.Errors));


                    dbContextTransaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                }
            }

            return false;
        }

        public async Task<bool> UpdateUserRoles(int userId, string roles)
        {
            using (var dbContextTransaction = _store.Context.Database.BeginTransaction())
            {
                try
                {
                    var oldRoles = await this.GetRolesAsync(userId).ConfigureAwait(false);
                    string[] newRoles =
                        roles.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => x.Trim())
                            .ToArray();

                    var result = await this.RemoveFromRolesAsync(userId, oldRoles.Except(newRoles).ToArray()).ConfigureAwait(false);
                    if (!result.Succeeded)
                        throw new Exception(string.Join(",", result.Errors));

                    result = await this.AddToRolesAsync(userId, newRoles.Except(oldRoles).ToArray()).ConfigureAwait(false);
                    if (!result.Succeeded)
                        throw new Exception(string.Join(",", result.Errors));

                    dbContextTransaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                }
            }

            return false;
        }
    }
}
