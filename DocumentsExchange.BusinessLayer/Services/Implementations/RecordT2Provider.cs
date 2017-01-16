using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataAccessLayer.Repository;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.BusinessLayer.Services.Implementations
{
    public class RecordT2Provider : IRecordT2Provider
    {
        private readonly RecordT2Repository _recordT2Repository;

        public RecordT2Provider(RecordT2Repository recordT2Repository)
        {
            _recordT2Repository = recordT2Repository;
        }

        public async Task<IEnumerable<RecordT2>> GetAll(int orgId)
        {
            return await _recordT2Repository.GetRecordsFromTable2(orgId).ConfigureAwait(false);
        }

        public async Task<bool> Add(RecordT2 recordT2)
        {
            return await _recordT2Repository.Create(recordT2).ConfigureAwait(false);
        }

        public async Task<bool> Update(RecordT2 recordT2)
        {
            return await _recordT2Repository.Update(recordT2).ConfigureAwait(false);
        }

        public async Task<bool> Delete(int id)
        {
            return await _recordT2Repository.Delete(id).ConfigureAwait(false);
        }
    }
}
