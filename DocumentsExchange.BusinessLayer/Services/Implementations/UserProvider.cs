using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using DocumentsExchange.BusinessLayer.Identity;
using DocumentsExchange.BusinessLayer.Models.User;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataAccessLayer;
using DocumentsExchange.DataAccessLayer.Repository;
using DocumentsExchange.DataLayer.Entity;
using Microsoft.AspNet.Identity;

namespace DocumentsExchange.BusinessLayer.Services.Implementations
{
    public class UserProvider : IUserProvider
    {
        private readonly UserRepository _userRepository;
        private readonly OrganizationRepository _organizationRepository;
        private readonly ApplicationUserManagerFactory _userManagerFactory;
        private readonly IDbContextFactory<DocumentsExchangeContext> _contextFactory;

        public UserProvider(
            UserRepository userRepository, 
            OrganizationRepository organizationRepository, 
            ApplicationUserManagerFactory userManagerFactory,
            IDbContextFactory<DocumentsExchangeContext> contextFactory)
        {
            _userRepository = userRepository;
            _organizationRepository = organizationRepository;
            _userManagerFactory = userManagerFactory;
            _contextFactory = contextFactory;
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
                using (var context = _contextFactory.Create())
                using (var userManager = _userManagerFactory.Create(context))
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var fromDb = await userManager.FindByNameAsync(user.UserName).ConfigureAwait(false);
                        var result = await userManager.AddPasswordAsync(fromDb.Id, password).ConfigureAwait(false);

                        if (!result.Succeeded)
                            throw new Exception(string.Join(",", result.Errors));

                        result = await userManager.AddToRolesAsync(fromDb.Id,
                                    user.RoleList.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(x => x.Trim())
                                        .ToArray()).ConfigureAwait(false);

                        if (!result.Succeeded)
                            throw new Exception(string.Join(",", result.Errors));

                        
                        dbContextTransaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }

            return false;
        }

        public async Task<bool> Update(User user)
        {
            if (await _userRepository.Update(user).ConfigureAwait(false))
            {
                using (var context = _contextFactory.Create())
                using (var userManager = _userManagerFactory.Create(context))
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var roles = await userManager.GetRolesAsync(user.Id).ConfigureAwait(false);
                        string[] newRoles =
                            user.RoleList.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => x.Trim())
                                .ToArray();

                        var result = await userManager.RemoveFromRolesAsync(user.Id, roles.Except(newRoles).ToArray()).ConfigureAwait(false);
                        if (!result.Succeeded)
                            throw new Exception(string.Join(",", result.Errors));

                        result = await userManager.AddToRolesAsync(user.Id, newRoles.Except(roles).ToArray()).ConfigureAwait(false);
                        if (!result.Succeeded)
                            throw new Exception(string.Join(",", result.Errors));
                        
                        dbContextTransaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }

            return false;
        }

        public async Task<bool> ResetPassword(int userId, string password)
        {
            bool result = false;
            using (var manager = _userManagerFactory.Create())
            {
                try
                {
                    string resetToken = await manager.GeneratePasswordResetTokenAsync(userId).ConfigureAwait(false);
                    IdentityResult passwordChangeResult = await manager.ResetPasswordAsync(userId, resetToken, password).ConfigureAwait(false);
                    result = passwordChangeResult.Succeeded;
                }
                catch (Exception e)
                {
                    
                }
                
            }

            return result;
        }

        public async Task<bool> Delete(int id)
        {
            return await _userRepository.Delete(id).ConfigureAwait(false);
        }
    }
}
