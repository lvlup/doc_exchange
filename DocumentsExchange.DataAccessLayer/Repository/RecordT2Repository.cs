using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Autofac.Extras.NLog;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.DataAccessLayer.Repository
{
   public class RecordT2Repository
    {
        private readonly ILogger _logger;
        private readonly IDbContextFactory<DocumentsExchangeContext> _contextFactory;

        public RecordT2Repository(IDbContextFactory<DocumentsExchangeContext> contextFactory, ILogger logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<RecordT2>> GetRecordsFromTable2(int orgId)
        {
            using (var context = _contextFactory.Create())
            {
                try
                {
                    var records = await context.Set<Organization>()
                        .Include(o=>o.RecordT2s.Select(r => r.Log.Changes))
                        .Include(o => o.RecordT2s.Select(r => r.Log.User))
                        .FirstOrDefaultAsync(x => x.Id == orgId).ConfigureAwait(false);

                    return records.RecordT2s;
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                }
            }

            return null;
        }

        public async Task<bool> Create(RecordT2 record)
        {
            bool result = false;
            using (var context = _contextFactory.Create())
            {
                try
                {
                    context.Set<RecordT2>().Add(record);
                    context.ChangeTracker.DetectChanges();
                    result = await context.SaveChangesAsync().ConfigureAwait(false) > 0;
                }
                catch (Exception e)
                {
                    _logger.Error("Error adding record to db: {0}", e);
                }
            }

            return result;
        }

        public async Task<RecordT2> Get(int id)
        {
            using (var context = _contextFactory.Create())
            {
                try
                {
                    return await context.Set<RecordT2>().FindAsync(id).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    _logger.Error("Error getting record {0}", id);
                    _logger.Error(e);
                }
            }

            return null;
        }

        public async Task<RecordT2> Get(Expression<Func<RecordT2, bool>> selector)
        {
            using (var context = _contextFactory.Create())
            {
                try
                {
                    return await context.Set<RecordT2>().FirstOrDefaultAsync(selector).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    _logger.Error("Error getting record");
                    _logger.Error(e);
                }
            }

            return null;
        }

        public async Task<bool> Update(RecordT2 record)
        {
            bool result = false;
            using (var context = _contextFactory.Create())
            {
                try
                {
                    var recordSet = context.Set<RecordT2>();
                    var existing =
                        await recordSet
                            .Where(x => x.Id == record.Id)
                            .FirstOrDefaultAsync();

                    if (existing == null)
                        throw new Exception("");

                    context.Entry(existing).State = EntityState.Detached;
                    recordSet.Attach(record);
                    context.Entry(record).State = EntityState.Modified;

                    context.ChangeTracker.DetectChanges();
                    result = await context.SaveChangesAsync().ConfigureAwait(false) > 0;
                }

                catch (Exception e)
                {
                    _logger.Error("Error updating record {0}", record.Id);
                    _logger.Error(e);
                }
            }

            return result;
        }

        public async Task<bool> Delete(int id)
        {
            bool result = false;
            using (var context = _contextFactory.Create())
            {
                try
                {
                    var existingRecord =
                        await context.Set<RecordT2>()
                            .Where(x => x.Id == id)
                            .FirstOrDefaultAsync();

                    if (existingRecord == null)
                        throw new Exception("");

                    context.Set<RecordT2>().Remove(existingRecord);

                    context.ChangeTracker.DetectChanges();
                    result = await context.SaveChangesAsync().ConfigureAwait(false) > 0;
                }

                catch (Exception e)
                {
                    _logger.Error("Error removing record with id = {0} from db", id);
                    _logger.Error(e);
                }
            }

            return result;
        }
    }
}
