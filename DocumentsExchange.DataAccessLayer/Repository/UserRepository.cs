using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Autofac.Extras.NLog;
using DocumentsExchange.DataLayer.Entity;
using DocumentsExchange.DataLayer.Identity;

namespace DocumentsExchange.DataAccessLayer.Repository
{
   public class UserRepository
    {
        private readonly ILogger _logger;
        private readonly IDbContextFactory<DocumentsExchangeContext> _contextFactory;

        public UserRepository(IDbContextFactory<DocumentsExchangeContext> contextFactory, ILogger logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            using (var context = _contextFactory.Create())
            {
                try
                {
                    var rolesSet = context.Set<AppRole>();

                    var usersWithRoles = await context.Set<User>()
                        .Select(u => new { User = u, Roles = u.Roles.Join(rolesSet, r => r.RoleId, r => r.Id, (x, y) => y.Name)})
                        .ToListAsync().ConfigureAwait(false);

                    usersWithRoles.ForEach(u => u.User.RoleList = string.Join(", ", u.Roles));

                    return usersWithRoles.Select(x => x.User);
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                }
            }

            return null;
        }

        public async Task<IEnumerable<User>> GetAll(int orgId, params int[] excludedUserIds)
        {
            using (var context = _contextFactory.Create())
            {
                try
                {
                    return await context.Set<User>()
                        .Where(x => !excludedUserIds.Contains(x.Id))
                        .Where(x => x.Organizations.Select(o => o.Id).Contains(orgId))
                        .ToListAsync().ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                }
            }

            return null;
        }

        public async Task<bool> Create(User user)
        {
            bool result = false;
            using (var context = _contextFactory.Create())
            {
                try
                {
                    var usr = new User()
                    {
                        FirstName = user.FirstName,
                        IsActive = user.IsActive,
                        LastName = user.LastName,
                        UserName = user.UserName
                    };

                    var organizations =
                        await
                            context.Set<Organization>()
                                .Join(user.OrganizationIds, x => x.Id, x => x, (x, y) => x)
                                .ToListAsync()
                                .ConfigureAwait(false);

                    organizations.ForEach(x => { usr.Organizations.Add(x); x.Users.Add(usr); });

                    context.Set<User>().Add(usr);
                    context.ChangeTracker.DetectChanges();
                    result = await context.SaveChangesAsync().ConfigureAwait(false) > 0;
                }
                catch (Exception e)
                {
                    _logger.Error("Error adding user to db: {0}", e);
                }
            }

            return result;
        }

        public async Task<User> Get(int id)
        {
            using (var context = _contextFactory.Create())
            {
                try
                {
                    var user =  await context.Set<User>()
                        .Select(u => new
                        {
                            User = u,
                            OrganizationIds = u.Organizations.Select(x => x.Id)
                        })
                        .FirstOrDefaultAsync(x => x.User.Id == id).ConfigureAwait(false);

                    if (user == null) return null;

                    user.User.OrganizationIds = user.OrganizationIds.ToArray();
                    return user.User;
                }
                catch (Exception e)
                {
                    _logger.Error("Error getting user {0}", id);
                    _logger.Error(e);
                }
            }

            return null;
        }

        public async Task<User> GetForEdit(int id)
        {
            using (var context = _contextFactory.Create())
            {
                try
                {
                    var rolesSet = context.Set<AppRole>();
                    var user = await context.Set<User>()
                        .Select(u => new
                        {
                            User = u,
                            Roles = u.Roles.Join(rolesSet, r => r.RoleId, r => r.Id, (x, y) => y.Name),
                            OrganizationIds = u.Organizations.Select(x => x.Id)
                        })
                        .FirstOrDefaultAsync(x => x.User.Id == id).ConfigureAwait(false);

                    if (user == null) return null;

                    user.User.RoleList = string.Join(", ", user.Roles);
                    user.User.OrganizationIds = user.OrganizationIds.ToArray();
                    return user.User;
                }
                catch (Exception e)
                {
                    _logger.Error("Error getting user {0}", id);
                    _logger.Error(e);
                }
            }

            return null;
        }

        public async Task<bool> Update(User user)
        {
            bool result = false;
            using (var context = _contextFactory.Create())
            {
                try
                {
                    var usersSet = context.Set<User>();
                    var existing =
                         await usersSet
                             .Include(u=>u.Organizations.Select(o=>o.Users))
                             .Where(x => x.Id == user.Id)
                             .FirstOrDefaultAsync();

                    if (existing == null)
                        throw new Exception("");

                    var organizations =
                        await
                            context.Set<Organization>()
                                .Include(o=>o.Users)
                                .Join(user.OrganizationIds, x => x.Id, x => x, (x, y) => x)
                                .ToListAsync()
                                .ConfigureAwait(false);

                    existing.UserName = user.UserName;
                    existing.FirstName = user.FirstName;
                    existing.LastName = user.LastName;
                    existing.IsActive = user.IsActive;

                    var removedOrgs = existing.Organizations.Except(organizations);
                    var addedOrgs = organizations.Except(existing.Organizations);

                    foreach (var rOrg in removedOrgs.ToList())
                    {
                        existing.Organizations.Remove(rOrg);
                        rOrg.Users.Remove(existing);
                    }

                    foreach (var aOrg in addedOrgs.ToList())
                    {
                        existing.Organizations.Add(aOrg);
                        aOrg.Users.Add(existing);

                    }

                    context.Entry(existing).State = EntityState.Modified;

                    context.ChangeTracker.DetectChanges();
                    result = await context.SaveChangesAsync().ConfigureAwait(false) > 0;
                }

                catch (Exception e)
                {
                    _logger.Error("Error updating user {0}", user.Id);
                    _logger.Error(e);
                }
            }

            return result;
        }

        public async Task<bool> Delete(int userId)
        {
            bool result = false;
            using (var context = _contextFactory.Create())
            {
                try
                {
                    var existingUser =
                        await context.Set<User>()
                            .Where(x => x.Id == userId)
                            .FirstOrDefaultAsync().ConfigureAwait(false);

                    if (existingUser == null)
                        throw new Exception("");

                    context.Set<User>().Remove(existingUser);

                    context.ChangeTracker.DetectChanges();
                    result = await context.SaveChangesAsync().ConfigureAwait(false) > 0;
                }

                catch (Exception e)
                {
                    _logger.Error("Error removing user {0} from db", userId);
                    _logger.Error(e);
                }
            }

            return result;
        }
    }
}
