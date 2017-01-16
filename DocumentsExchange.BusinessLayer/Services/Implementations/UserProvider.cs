using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataAccessLayer.Repository;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.BusinessLayer.Services.Implementations
{
    public class UserProvider : IUserProvider
    {

        private readonly UserRepository _userRepository;

        public UserProvider(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userRepository.GetAll().ConfigureAwait(false);
        }

        public async Task<User> Get(int id)
        {
            return await _userRepository.Get(id).ConfigureAwait(false);
        }

        public async Task<bool> Add(User user)
        {
            return await _userRepository.Create(user).ConfigureAwait(false);
        }

        public async Task<bool> Update(User user)
        {
            return await _userRepository.Update(user).ConfigureAwait(false);
        }

        public async Task<bool> Delete(int id)
        {
            return await _userRepository.Delete(id).ConfigureAwait(false);
        }
    }
}
