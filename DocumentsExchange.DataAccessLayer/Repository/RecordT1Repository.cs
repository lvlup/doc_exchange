﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.NLog;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.DataAccessLayer.Repository
{
   public class RecordT1Repository
    {
        private readonly ILogger _logger;
        private readonly IDbContextFactory<DocumentsExchangeContext> _contextFactory;

        public RecordT1Repository(IDbContextFactory<DocumentsExchangeContext> contextFactory, ILogger logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<RecordT1>> GetRecordsFromTable1(int orgId)
        {
            using (var context = _contextFactory.Create())
            {
                try
                {
                    var records = await context.Set<Organization>()
                        .FirstOrDefaultAsync(x => x.Id == orgId).ConfigureAwait(false);

                    return records.RecordT1s;
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                }
            }

            return null;
        }

        public async Task<bool> Create(RecordT1 record)
        {
            bool result = false;
            using (var context = _contextFactory.Create())
            {
                try
                {
                    RecordT1 r1 = new RecordT1()
                    {
                        Id = record.Id
 
                    };

                    context.Set<RecordT1>().Add(r1);
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

        public async Task<RecordT1> Get(int id)
        {
            using (var context = _contextFactory.Create())
            {
                try
                {
                    return await context.Set<RecordT1>().FindAsync(id).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    _logger.Error("Error getting record {0}", id);
                    _logger.Error(e);
                }
            }

            return null;
        }

        public async Task<bool> Update(RecordT1 record)
        {
            bool result = false;
            using (var context = _contextFactory.Create())
            {
                try
                {
                    var recordSet = context.Set<RecordT1>();
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
                        await context.Set<RecordT1>()
                            .Where(x => x.Id == id)
                            .FirstOrDefaultAsync();

                    if (existingRecord == null)
                        throw new Exception("");

                    context.Set<RecordT1>().Remove(existingRecord);

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
