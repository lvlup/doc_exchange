using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentsExchange.BusinessLayer.Models.User;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.BusinessLayer.Services.Interfaces
{
    public interface IUserProvider
    {
        Task<IEnumerable<User>> GetAll();

        Task<User> Get(int id);

        Task<UserEditInfo> GetForEdit(int id);

        Task<IEnumerable<OrganizationInfo>> GetOrganizations();

        Task<bool> Add(User user, string password);

        Task<bool> Update(User user);

        Task<bool> ResetPassword(int userId, string password);

        Task<bool> Delete(int id);
    }
}
