using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using Autofac.Extras.NLog;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.DataAccessLayer.Repository
{
   public class LogRepository
    {
        private readonly ILogger _logger;
        private readonly IDbContextFactory<DocumentsExchangeContext> _contextFactory;

        public LogRepository(IDbContextFactory<DocumentsExchangeContext> contextFactory, ILogger logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Log> Get(int id)
        {
            using (var context = _contextFactory.Create())
            {
                try
                {
                    return await context.Set<Log>()
                        .Include(l=>l.Changes)
                        .Include(l => l.User)
                        .SingleOrDefaultAsync(l=>l.Id == id)
                        .ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    _logger.Error("Error getting file {0}", id);
                    _logger.Error(e);
                }
            }

            return null;
        }

    }
}
