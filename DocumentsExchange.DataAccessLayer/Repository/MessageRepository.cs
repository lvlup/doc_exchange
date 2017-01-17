using System;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using Autofac.Extras.NLog;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.DataAccessLayer.Repository
{
   public class MessageRepository
    {
        private readonly ILogger _logger;
        private readonly IDbContextFactory<DocumentsExchangeContext> _contextFactory;

        public MessageRepository(IDbContextFactory<DocumentsExchangeContext> contextFactory, ILogger logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<bool> Create(Message message)
        {
            bool result = false;
            using (var context = _contextFactory.Create())
            {
                try
                {
                    context.Set<Message>().Add(message);
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
    }
}
