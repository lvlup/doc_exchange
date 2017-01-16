using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataAccessLayer.Repository;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.BusinessLayer.Services.Implementations
{
   public class RecordT1Provider: IRecordT1Provider
    {
        private readonly RecordT1Repository _recordT1Repository;

        public RecordT1Provider(RecordT1Repository recordT1Repository)
        {
            _recordT1Repository = recordT1Repository;
        }

        public async Task<IEnumerable<RecordT1>> GetAll(int orgId)
        {
            return await _recordT1Repository.GetRecordsFromTable1(orgId).ConfigureAwait(false);
        }

       public async Task<bool> Add(RecordT1 recordT1)
       {
           return await _recordT1Repository.Create(recordT1).ConfigureAwait(false);
       }

       public async Task<bool> Update(RecordT1 recordT1)
       {
           return await _recordT1Repository.Update(recordT1).ConfigureAwait(false);
       }

       public async Task<bool> Delete(int id)
       {
           return await _recordT1Repository.Delete(id).ConfigureAwait(false);
       }
    }
}
