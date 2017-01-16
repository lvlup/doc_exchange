using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.BusinessLayer.Services.Interfaces
{
   public interface IFileProvider
    {
        Task<IEnumerable<File>> GetFilesWithCategory(int orgId, int fileCategoryId);

        Task<File> Get(int id);

        Task<bool> Add(File file);

        Task<bool> Delete(int id);
    }
}
