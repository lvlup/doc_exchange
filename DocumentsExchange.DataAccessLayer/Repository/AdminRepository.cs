using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using Autofac.Extras.NLog;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.DataAccessLayer.Repository
{
    public class AdminRepository
    {
        private readonly ILogger _logger;
        private readonly IDbContextFactory<DocumentsExchangeContext> _contextFactory;

        public AdminRepository(IDbContextFactory<DocumentsExchangeContext> contextFactory, ILogger logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<WebSiteState> GetWebSiteState()
        {
            using (var context = _contextFactory.Create())
            {
                try
                {
                    return await context.Set<WebSiteState>().FirstOrDefaultAsync().ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                }
            }

            return null;
        }

        public async Task<bool> SetWebSiteState(bool newState)
        {
            using (var context = _contextFactory.Create())
            {
                try
                {
                    var state = await context.Set<WebSiteState>().FirstOrDefaultAsync().ConfigureAwait(false);
                    if (state == null)
                        throw new Exception("Site config is invalid");

                    state.IsActive = newState;
                    context.Entry(state).State = EntityState.Modified;
                    context.ChangeTracker.DetectChanges();

                    return await context.SaveChangesAsync().ConfigureAwait(false) > 0;
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                }
            }

            return false;
        }
    }
}
