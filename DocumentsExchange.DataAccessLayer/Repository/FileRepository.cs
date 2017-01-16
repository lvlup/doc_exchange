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
    public class FileRepository
    {
        private readonly ILogger _logger;
        private readonly IDbContextFactory<DocumentsExchangeContext> _contextFactory;

        public FileRepository(IDbContextFactory<DocumentsExchangeContext> contextFactory, ILogger logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }


        public async Task<IEnumerable<File>> GetFilesWithCategory(int orgId,int fileCategoryId)
        {
            using (var context = _contextFactory.Create())
            {
                try
                {
                    var existingOrg = await context.Set<Organization>().Include(o=>o.Files.Select(f=>f.User))
                        .FirstOrDefaultAsync(o => o.Id == orgId).ConfigureAwait(false);

                    if (existingOrg == null)
                        throw new Exception("");

                    return existingOrg.Files.Where(f => f.CategoryId == fileCategoryId);
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                }
            }

            return null;
        } 

        public async Task<bool> Create(File file)
        {
            bool result = false;
            using (var context = _contextFactory.Create())
            {
                try
                {
                    File f = new File()
                    {
                        FileName = file.FileName,
                        AddedDateTime = file.AddedDateTime,
                        CategoryId = file.CategoryId,
                        OranizationId = file.OranizationId,
                        Content = file.Content,
                        ContentType = file.ContentType,
                        FileType = file.FileType,
                        User = file.User
                    };

                    context.Set<File>().Add(f);
                    context.ChangeTracker.DetectChanges();
                    result = await context.SaveChangesAsync().ConfigureAwait(false) > 0;
                }
                catch (Exception e)
                {
                    _logger.Error("Error adding file to db: {0}", e);
                }
            }

            return result;
        }

        public async Task<File> Get(int id)
        {
            using (var context = _contextFactory.Create())
            {
                try
                {
                    return await context.Set<File>().FindAsync(id).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    _logger.Error("Error getting file {0}", id);
                    _logger.Error(e);
                }
            }

            return null;
        }

       
        public async Task<bool> Delete(int fileId)
        {
            bool result = false;
            using (var context = _contextFactory.Create())
            {
                try
                {
                    var existingFile =
                        await context.Set<File>()
                            .Where(x => x.Id == fileId)
                            .FirstOrDefaultAsync();

                    if (existingFile == null)
                        throw new Exception("");

                    context.Set<File>().Remove(existingFile);

                    context.ChangeTracker.DetectChanges();
                    result = await context.SaveChangesAsync().ConfigureAwait(false) > 0;
                }

                catch (Exception e)
                {
                    _logger.Error("Error removing org {0} from db", fileId);
                    _logger.Error(e);
                }
            }

            return result;
        }
    }
}
