using evercloud.Domain.Interfaces;
using evercloud.Domain.Models;

namespace evercloud.Service.Services
{
    public class AccountService(IAccountRepository accountRepository) : IAccountService
    {
        public async Task<Users?> FindByEmailAsync(string email)
        {
            return await accountRepository.FindByEmailAsync(email);
        }

        public async Task<Users?> FindByUsernameAsync(string username)
        {
            return await accountRepository.FindByUsernameAsync(username);
        }

        public async Task<bool> RegisterUserAsync(Users user, string password)
        {
            return await accountRepository.CreateUserAsync(user, password);
        }

        public async Task<bool> ResetPasswordAsync(Users user, string newPassword)
        {
            bool removed = await accountRepository.RemovePasswordAsync(user);
            if (!removed) return false;

            return await accountRepository.AddPasswordAsync(user, newPassword);
        }

        public async Task<bool> DeleteAccountAsync(Users user)
        {
            return await accountRepository.DeleteUserAsync(user);
        }
    }
}
