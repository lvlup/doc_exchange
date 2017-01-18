using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentsExchange.BusinessLayer.Identity;
using DocumentsExchange.BusinessLayer.Models.User;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataAccessLayer.Repository;
using DocumentsExchange.DataLayer.Entity;
using Microsoft.AspNet.Identity;

namespace DocumentsExchange.BusinessLayer.Services.Implementations
{
    public class UserProvider : IUserProvider
    {
        private readonly UserRepository _userRepository;
        private readonly OrganizationRepository _organizationRepository;
        private readonly ApplicationUserManager _userManager;

        public UserProvider(
            UserRepository userRepository, 
            OrganizationRepository organizationRepository, 
            ApplicationUserManager userManager)
        {
            _userRepository = userRepository;
            _organizationRepository = organizationRepository;
            _userManager = userManager;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userRepository.GetAll().ConfigureAwait(false);
        }

        public async Task<User> Get(int id)
        {
            return await _userRepository.Get(id).ConfigureAwait(false);
        }

        public async Task<UserEditInfo> GetForEdit(int id)
        {
            var userTask = _userRepository.GetForEdit(id);
            var orgsTask = _organizationRepository.GetOrganizations();
            await Task.WhenAll(userTask, orgsTask);

            return new UserEditInfo()
            {
                User = userTask.Result,
                Organizations = orgsTask.Result.GroupJoin(userTask.Result.OrganizationIds,
                    x => x.Id,
                    x => x,
                    (x, y) => new OrganizationInfo()
                    {
                        Id = x.Id,
                        Name = x.Name, IsSelected = y.Any()
                    })
            };
        }

        public async Task<IEnumerable<OrganizationInfo>> GetOrganizations()
        {
            return (await _organizationRepository.GetOrganizations().ConfigureAwait(false)).Select(
                x => new OrganizationInfo()
                {
                    Id = x.Id,
                    Name = x.Name
                });
        }

        public async Task<bool> Add(User user, string password)
        {
            if (await _userRepository.Create(user).ConfigureAwait(false))
            {
                return await _userManager.AddUser(user, password);
            }

            return false;
        }

        public async Task<bool> Update(User user)
        {
            if (await _userRepository.Update(user).ConfigureAwait(false))
            {
                return await _userManager.UpdateUserRoles(user.Id, user.RoleList);
            }

            return false;
        }

        public async Task<bool> ResetPassword(int userId, string password)
        {
            bool result = false;
            try
            {
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(userId).ConfigureAwait(false);
                IdentityResult passwordChangeResult = await _userManager.ResetPasswordAsync(userId, resetToken, password).ConfigureAwait(false);
                result = passwordChangeResult.Succeeded;
            }
            catch (Exception e)
            {

            }

            return result;
        }

        public async Task<bool> Delete(int id)
        {
            return await _userRepository.Delete(id).ConfigureAwait(false);
        }
    }
}
