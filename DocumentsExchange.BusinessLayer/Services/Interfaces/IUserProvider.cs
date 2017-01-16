using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.BusinessLayer.Services.Interfaces
{
    public interface IUserProvider
    {
        Task<IEnumerable<User>> GetAll();

        Task<User> Get(int id);

        Task<bool> Add(User user);

        Task<bool> Update(User user);

        Task<bool> Delete(int id);
    }
}
