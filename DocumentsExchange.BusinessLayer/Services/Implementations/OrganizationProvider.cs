using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataAccessLayer.Repository;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.BusinessLayer.Services.Implementations
{
    public class OrganizationProvider: IOrganizationProvider
    {
        private readonly OrganizationRepository _organizationRepository;

        public OrganizationProvider(OrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<IEnumerable<Organization>> GetAll()
        {
            return await _organizationRepository.GetOrganizations().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Organization>> GetUseOrganizations(int userId)
        {
            return await _organizationRepository.GetUserOrganizations(userId).ConfigureAwait(false);
        }

        public async Task<Organization> Get(int orgId)
        {
            return await _organizationRepository.Get(orgId).ConfigureAwait(false);
        }

        public async Task<bool> Add(Organization org)
        {
            return await _organizationRepository.Create(org).ConfigureAwait(false);
        }

        public async Task<bool> Update(Organization org)
        {
            return await _organizationRepository.Update(org).ConfigureAwait(false);
        }

        public async Task<bool> Delete(int orgId)
        {
            return await _organizationRepository.Delete(orgId).ConfigureAwait(false);
        }
    }
}
