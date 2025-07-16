using Microsoft.AspNetCore.Identity;
using evercloud.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using evercloud.Domain.Models;

namespace evercloud.DataAccess.Repositories
{
    public class AccountRepository(UserManager<Users> userManager, ILogger<AccountRepository> logger) : IAccountRepository
    {
        public async Task<Users?> FindByEmailAsync(string email)
        {
            try
            {
                return await userManager.FindByEmailAsync(email);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in FindByEmailAsync for email: {Email}", email);
                return null;
            }
        }

        public async Task<Users?> FindByUsernameAsync(string username)
        {
            try
            {
                return await userManager.FindByNameAsync(username);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in FindByUsernameAsync for username: {Username}", username);
                return null;
            }
        }

        public async Task<bool> CreateUserAsync(Users user, string password)
        {
            try
            {
                var result = await userManager.CreateAsync(user, password);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in CreateUserAsync for user: {User}", user?.UserName);
                return false;
            }
        }

        public async Task<bool> RemovePasswordAsync(Users user)
        {
            try
            {
                var result = await userManager.RemovePasswordAsync(user);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in RemovePasswordAsync for user: {User}", user?.UserName);
                return false;
            }
        }

        public async Task<bool> AddPasswordAsync(Users user, string newPassword)
        {
            try
            {
                var result = await userManager.AddPasswordAsync(user, newPassword);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in AddPasswordAsync for user: {User}", user?.UserName);
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(Users user)
        {
            try
            {
                var result = await userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DeleteUserAsync for user: {User}", user?.UserName);
                return false;
            }
        }
    }
}
