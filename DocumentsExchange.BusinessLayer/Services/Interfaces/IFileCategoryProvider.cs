using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.BusinessLayer.Services.Interfaces
{
    public interface IFileCategoryProvider
    {
        Task<IEnumerable<FileCategory>> GetAll();

        Task<FileCategory> Get(int id);

        Task<bool> Add(FileCategory fileCategory);

        Task<bool> Update(FileCategory fileCategory);

        Task<bool> Delete(int id);
    }
}
