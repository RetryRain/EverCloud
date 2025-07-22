using evercloud.Domain.Models;
using System.Security.Claims;

namespace evercloud.Domain.Interfaces
{
    public interface ISupportService
    {
        Task<Users> GetCurrentUserAsync(ClaimsPrincipal principal);
        Task<List<SupportMessage>> GetMessagesForUserAsync(Users user);
        Task<List<SupportMessage>> GetMessagesByUserIdAsync(string userId);
        Task PostUserMessageAsync(Users user, string message);
        Task<List<AdminInboxUserViewModel>> GetAdminInboxUsersAsync();
        Task<SupportChatViewModel> GetAdminChatViewModelAsync(string userId);
        Task ReplyToUserAsync(string userId, string message);
        Task<List<Users>> GetUsersFromSupportMessagesAsync();
        Task<bool> IsUserInRoleAsync(Users user, string role);

    }
}
