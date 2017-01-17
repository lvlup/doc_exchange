using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.NLog;
using DocumentsExchange.DataAccessLayer.Models;
using DocumentsExchange.DataLayer.Entity;
using DocumentsExchange.Models;

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

        public async Task<ItemsResult<Message>> Get(int orgId, PageInfo pageInfo)
        {
            using (var context = _contextFactory.Create())
            {
                try
                {
                    var query = context.Set<Message>()
                        .Where(x => x.OrganizationId == orgId);

                    var messages = await query
                        .Select(x => new
                        {
                            Message = x,
                            UserName = x.Sender.FirstName + " " + x.Sender.LastName,
                            Total = query.Count()
                        })
                        .OrderByDescending(x => x.Message.TimeStamp)
                        .Skip((pageInfo.Page - 1)*pageInfo.PageSize)
                        .Take(pageInfo.PageSize)
                        .ToListAsync().ConfigureAwait(false);

                    messages.ForEach(x => x.Message.UserName = x.UserName);
                    return new ItemsResult<Message>(messages.Select(x => x.Message), messages.FirstOrDefault()?.Total ?? 0);
                }
                catch (Exception e)
                {
                    _logger.Error("Error adding user to db: {0}", e);
                }
            }

            return null;
        }
    }
}
