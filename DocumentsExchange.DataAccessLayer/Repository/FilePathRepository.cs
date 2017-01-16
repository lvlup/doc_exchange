using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.NLog;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.DataAccessLayer.Repository
{
   public class FilePathRepository
    {
        private readonly ILogger _logger;
        private readonly IDbContextFactory<DocumentsExchangeContext> _contextFactory;

        public FilePathRepository(IDbContextFactory<DocumentsExchangeContext> contextFactory, ILogger logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }



        public async Task<bool> Create(FilePath filePath)
        {
            bool result = false;
            using (var context = _contextFactory.Create())
            {
                try
                {
                    var file = new FilePath()
                    {
                        FileName = filePath.FileName
                    };

                    context.Set<FilePath>().Add(file);
                    context.ChangeTracker.DetectChanges();
                    result = await context.SaveChangesAsync().ConfigureAwait(false) > 0;
                }
                catch (Exception e)
                {
                    _logger.Error("Error adding file category to db: {0}", e);
                }
            }

            return result;
        }

        public async Task<FilePath> Get(int id)
        {
            using (var context = _contextFactory.Create())
            {
                try
                {
                    return await context.Set<FilePath>().FindAsync(id).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    _logger.Error("Error getting organization {0}", id);
                    _logger.Error(e);
                }
            }

            return null;
        }
    }
}
