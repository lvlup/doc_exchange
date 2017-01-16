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
                        .Include(x => x.RecordT1s.Select(r => r.Log.Changes))
                        .Include(x => x.RecordT1s.Select(r => r.Log.User))
                        .Include(x => x.RecordT1s.Select(r => r.SenderUser))
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
                    //RecordT1 r1 = new RecordT1()
                    //{
                    //    Id = record.Id
 
                    //};

                    context.Set<RecordT1>().Add(record);
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

        public async Task<RecordT1> Get(Expression<Func<RecordT1, bool>> selector)
        {
            using (var context = _contextFactory.Create())
            {
                try
                {
                    return await context.Set<RecordT1>().FirstOrDefaultAsync(selector).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    _logger.Error("Error getting record");
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
                            .Include(x => x.Log)
                            .Where(x => x.Id == record.Id)
                            .FirstOrDefaultAsync().ConfigureAwait(false);

                    if (existing == null)
                        throw new Exception("");

                    List<Change> changes = new List<Change>();

                    var now = DateTime.UtcNow;

                    if (existing.NumberPaymentOrder != record.NumberPaymentOrder)
                    {
                        changes.Add(new Change()
                        {
                            CurrentValue = record.NumberPaymentOrder.ToString(),
                            OldValue = existing.NumberPaymentOrder.ToString(),
                            PropertyName = "п/п №",
                            UserId = record.SenderUserId,
                            TimeSpan = now
                        });
                    }

                    if (existing.SenderUserId != record.SenderUserId)
                    {
                        changes.Add(new Change()
                        {
                            CurrentValue = record.SenderUserId.ToString(),
                            OldValue = existing.SenderUserId.ToString(),
                            PropertyName = "Отправитель",
                            UserId = record.SenderUserId,
                            TimeSpan = now
                        });
                    }

                    if (existing.OrganizationSender != record.OrganizationSender)
                    {
                        changes.Add(new Change()
                        {
                            CurrentValue = record.OrganizationSender,
                            OldValue = existing.OrganizationSender,
                            PropertyName = "Наименование получателя",
                            UserId = record.SenderUserId,
                            TimeSpan = now
                        });
                    }

                    if (existing.OrganizationReceiver != record.OrganizationReceiver)
                    {
                        changes.Add(new Change()
                        {
                            CurrentValue = record.OrganizationReceiver,
                            OldValue = existing.OrganizationReceiver,
                            PropertyName = "Наименование получателя",
                            UserId = record.SenderUserId,
                            TimeSpan = now
                        });
                    }

                    if (existing.Amount != record.Amount)
                    {
                        changes.Add(new Change()
                        {
                            CurrentValue = record.Amount.ToString(),
                            OldValue = existing.Amount.ToString(),
                            PropertyName = "Сумма",
                            UserId = record.SenderUserId,
                            TimeSpan = now
                        });
                    }

                    if (existing.AmountInCurrency != record.AmountInCurrency)
                    {
                        changes.Add(new Change()
                        {
                            CurrentValue = record.AmountInCurrency.ToString(),
                            OldValue = existing.AmountInCurrency.ToString(),
                            PropertyName = "Сумма в указанной валюте",
                            UserId = record.SenderUserId,
                            TimeSpan = now
                        });
                    }

                    if (existing.Course != record.Course)
                    {
                        changes.Add(new Change()
                        {
                            CurrentValue = record.Course.ToString(),
                            OldValue = existing.Course.ToString(),
                            PropertyName = "Курс",
                            UserId = record.SenderUserId,
                            TimeSpan = now
                        });
                    }

                    if (existing.Percent != record.Percent)
                    {
                        changes.Add(new Change()
                        {
                            CurrentValue = record.Percent.ToString(),
                            OldValue = existing.Percent.ToString(),
                            PropertyName = "%",
                            UserId = record.SenderUserId,
                            TimeSpan = now
                        });
                    }

                    if (existing.Sent != record.Sent)
                    {
                        changes.Add(new Change()
                        {
                            CurrentValue = record.Sent.ToString(),
                            OldValue = existing.Sent.ToString(),
                            PropertyName = "Отправлено",
                            UserId = record.SenderUserId,
                            TimeSpan = now
                        });
                    }

                    if (existing.Swift != record.Swift)
                    {
                        changes.Add(new Change()
                        {
                            CurrentValue = record.Swift.ToString(),
                            OldValue = existing.Swift.ToString(),
                            PropertyName = "Swift",
                            UserId = record.SenderUserId,
                            TimeSpan = now
                        });
                    }

                    if (existing.Total != record.Total)
                    {
                        changes.Add(new Change()
                        {
                            CurrentValue = record.Total.ToString(),
                            OldValue = existing.Total.ToString(),
                            PropertyName = "Итог",
                            UserId = record.SenderUserId,
                            TimeSpan = now
                        });
                    }

                    record.Log = existing.Log;

                    changes.ForEach(change => record.Log.Changes.Add(change));
                    
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
