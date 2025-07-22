using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using evercloud.Domain.Models;

namespace evercloud.Domain.Interfaces
{
    public interface ISupportRepository
    {
        Task<Users> GetUserByIdAsync(string userId);
        Task<Users> GetUserAsync(ClaimsPrincipal principal);
        Task<List<SupportMessage>> GetMessagesByUserAsync(Users user);
        Task<List<SupportMessage>> GetMessagesByUserIdAsync(string userId);
        Task AddMessageAsync(SupportMessage message);
        Task SaveChangesAsync();

        Task<List<SupportMessage>> GetAllMessagesAsync();
        Task<List<Users>> GetAllUsersWithMessagesAsync();
        Task<List<Users>> GetDistinctUsersFromMessagesAsync();
        Task<List<AdminInboxUserViewModel>> GetAdminInboxUsersAsync();
        Task<bool> IsUserInRoleAsync(Users user, string role);
    }
}
