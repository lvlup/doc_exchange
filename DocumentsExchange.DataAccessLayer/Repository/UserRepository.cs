using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.NLog;
using DocumentsExchange.DataLayer.Entity;

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
                    return await context.Set<User>()
                        .Include(u=>u.Roles)
                        .ToListAsync().ConfigureAwait(false);
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
                        UserName = user.UserName,
                        Messages = user.Messages
                    };

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
                    return await context.Set<User>().FindAsync(id).ConfigureAwait(false);
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
                             .Where(x => x.Id == user.Id)
                             .FirstOrDefaultAsync();

                    if (existing == null)
                        throw new Exception("");

                    context.Entry(existing).State = EntityState.Detached;
                    usersSet.Attach(user);
                    context.Entry(user).State = EntityState.Modified;

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
                            .FirstOrDefaultAsync();

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
