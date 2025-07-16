using evercloud.Domain.Models;

namespace evercloud.Domain.Interfaces
{
    public interface IAccountService
    {
        Task<Users?> FindByEmailAsync(string email);
        Task<Users?> FindByUsernameAsync(string username);
        Task<bool> RegisterUserAsync(Users user, string password);
        Task<bool> ResetPasswordAsync(Users user, string newPassword);
        Task<bool> DeleteAccountAsync(Users user);
    }
}
