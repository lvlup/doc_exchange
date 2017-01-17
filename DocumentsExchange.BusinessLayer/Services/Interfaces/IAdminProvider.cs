using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentsExchange.DataAccessLayer.Repository;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.BusinessLayer.Services.Interfaces
{
    public interface IAdminProvider
    {
        Task<WebSiteState> GetWebSiteState();

        Task<bool> SetWebSiteState(bool state);
    }

    class AdminProvider : IAdminProvider
    {
        private readonly AdminRepository _adminRepository;

        public AdminProvider(AdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<WebSiteState> GetWebSiteState()
        {
            return await _adminRepository.GetWebSiteState().ConfigureAwait(false);
        }

        public async Task<bool> SetWebSiteState(bool state)
        {
            return await _adminRepository.SetWebSiteState(state).ConfigureAwait(false);
        }
    }
}
