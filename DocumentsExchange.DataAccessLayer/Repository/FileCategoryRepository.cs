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
   public class FileCategoryRepository
    {
        private readonly ILogger _logger;
        private readonly IDbContextFactory<DocumentsExchangeContext> _contextFactory;

        public FileCategoryRepository(IDbContextFactory<DocumentsExchangeContext> contextFactory, ILogger logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<FileCategory>> GetAll()
        {
            using (var context = _contextFactory.Create())
            {
                try
                {
                    return await context.Set<FileCategory>().ToListAsync().ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                }
            }

            return null;
        }

        public async Task<bool> Create(FileCategory fileCategory)
        {
            bool result = false;
            using (var context = _contextFactory.Create())
            {
                try
                {
                    FileCategory category = new FileCategory()
                    {
                      Name = fileCategory.Name,
                      CreatedDateTime = fileCategory.CreatedDateTime,
                      Files = fileCategory.Files,
                      IsActive = fileCategory.IsActive
                    };

                    context.Set<FileCategory>().Add(category);
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

        public async Task<FileCategory> Get(int id)
        {
            using (var context = _contextFactory.Create())
            {
                try
                {
                    return await context.Set<FileCategory>().FindAsync(id).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    _logger.Error("Error getting organization {0}", id);
                    _logger.Error(e);
                }
            }

            return null;
        }

        public async Task<bool> Update(FileCategory fileCategory)
        {
            bool result = false;
            using (var context = _contextFactory.Create())
            {
                try
                {
                    var categoriesSet = context.Set<FileCategory>();
                    var existing =
                        await categoriesSet
                            .Where(x => x.Id == fileCategory.Id)
                            .FirstOrDefaultAsync().ConfigureAwait(false);

                    if (existing == null)
                        throw new Exception("");

                    context.Entry(existing).State = EntityState.Detached;
                    categoriesSet.Attach(fileCategory);
                    context.Entry(fileCategory).State = EntityState.Modified;

                    context.ChangeTracker.DetectChanges();
                    result = await context.SaveChangesAsync().ConfigureAwait(false) > 0;
                }

                catch (Exception e)
                {
                    _logger.Error("Error updating organiation {0}", fileCategory.Id);
                    _logger.Error(e);
                }
            }

            return result;
        }

        public async Task<bool> Delete(int fileCategoryId)
        {
            bool result = false;
            using (var context = _contextFactory.Create())
            {
                try
                {
                    var existingFileCategory =
                        await context.Set<FileCategory>()
                            .Where(x => x.Id == fileCategoryId)
                            .FirstOrDefaultAsync().ConfigureAwait(false);

                    if (existingFileCategory == null)
                        throw new Exception("");

                    context.Set<FileCategory>().Remove(existingFileCategory);

                    context.ChangeTracker.DetectChanges();
                    result = await context.SaveChangesAsync().ConfigureAwait(false) > 0;
                }

                catch (Exception e)
                {
                    _logger.Error("Error removing file category {0} from db", fileCategoryId);
                    _logger.Error(e);
                }
            }

            return result;
        }
    }
}
