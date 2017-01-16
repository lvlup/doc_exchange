using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.BusinessLayer.Services.Interfaces
{
    public interface IOrganizationProvider
    {
        Task<IEnumerable<Organization>> GetAll();

        Task<Organization> Get(int id);

        Task<bool> Add(Organization org);

        Task<bool> Update(Organization org);

        Task<bool> Delete(int orgId);
    }
}
