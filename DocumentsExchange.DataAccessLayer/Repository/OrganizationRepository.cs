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

    public class OrganizationRepository 
    {
        private readonly ILogger _logger;
        private readonly IDbContextFactory<DocumentsExchangeContext> _contextFactory;

        public OrganizationRepository(IDbContextFactory<DocumentsExchangeContext> contextFactory, ILogger logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

       public async Task<IEnumerable<Organization>> GetOrganizations()
        {
            using (var context = _contextFactory.Create())
            {
                try
                {
                    return await context.Set<Organization>().ToListAsync().ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                }
            }

            return null;
        }

        public async Task<bool> Create(Organization organization)
        {
            bool result = false;
            using (var context = _contextFactory.Create())
            {
                try
                {
                    context.Set<Organization>().Add(organization);
                    context.ChangeTracker.DetectChanges();
                    result = await context.SaveChangesAsync().ConfigureAwait(false) > 0;
                }
                catch (Exception e)
                {
                    _logger.Error("Error adding organization to db: {0}", e);
                }
            }

            return result;
        }

        public async Task<Organization> Get(int id)
        {
            using (var context = _contextFactory.Create())
            {
                try
                {
                    return await context.Set<Organization>().FindAsync(id).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    _logger.Error("Error getting organization {0}", id);
                    _logger.Error(e);
                }
            }

            return null;
        }

        public async Task<bool> Update(Organization organization)
        {
            bool result = false;
            using (var context = _contextFactory.Create())
            {
                try
                {
                    var organizationSet = context.Set<Organization>();
                    var existing =
                        await organizationSet
                            .Where(x => x.Id == organization.Id)
                            .FirstOrDefaultAsync().ConfigureAwait(false);

                    if (existing == null)
                        throw new Exception("");

                    context.Entry(existing).State = EntityState.Detached;
                    organizationSet.Attach(organization);
                    context.Entry(organization).State = EntityState.Modified;

                    context.ChangeTracker.DetectChanges();
                    result = await context.SaveChangesAsync().ConfigureAwait(false) > 0;
                }

                catch (Exception e)
                {
                    _logger.Error("Error updating organiation {0}", organization.Id);
                    _logger.Error(e);
                }
            }

            return result;
        }

        public async Task<bool> Delete(int organizationId)
        {
            bool result = false;
            using (var context = _contextFactory.Create())
            {
                try
                {
                    var existingOrg =
                        await context.Set<Organization>()
                            .Where(x => x.Id == organizationId)
                            .FirstOrDefaultAsync();

                    if (existingOrg == null)
                        throw new Exception("");

                    context.Set<Organization>().Remove(existingOrg);

                    context.ChangeTracker.DetectChanges();
                    result = await context.SaveChangesAsync().ConfigureAwait(false) > 0;
                }
       
                catch (Exception e)
                {
                    _logger.Error("Error removing org {0} from db", organizationId);
                    _logger.Error(e);
                }
            }

            return result;
        }
    }
}
