using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.BusinessLayer.Services.Interfaces
{
    public interface IRecordT2Provider
    {
        Task<IEnumerable<RecordT2>> GetAll(int orgId);

        Task<bool> Add(RecordT2 recordT1);

        Task<bool> Update(RecordT2 recordT1);

        Task<bool> Delete(int id);
        Task<RecordT2> Find(SearchParams searchParams);
    }
}
