using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.BusinessLayer.Services.Interfaces
{
    public class SearchParams
    {
        public string OrganizationSender { get; set; }

        public int NumberPaymentOrder { get; set; }
    }

    public interface IRecordT1Provider
    {
        Task<IEnumerable<RecordT1>> GetAll(int orgId);

        Task<RecordT1> Get(int orgId);

        Task<RecordT1> Find(SearchParams searchParams);

        Task<bool> Add(RecordT1 recordT1);

        Task<bool> Update(RecordT1 recordT1);

        Task<bool> Delete(int id);
    }
}
