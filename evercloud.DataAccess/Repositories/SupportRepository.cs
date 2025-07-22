using evercloud.DataAccess.Data;
using evercloud.Domain.Interfaces;
using evercloud.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


namespace evercloud.DataAccess.Repositories
{
    public class SupportRepository : ISupportRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public SupportRepository(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Users> GetUserAsync(ClaimsPrincipal principal)
        {
            return await _userManager.GetUserAsync(principal);
        }

        public async Task<Users> GetUserByIdAsync(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<List<SupportMessage>> GetMessagesByUserAsync(Users user)
        {
            return await _context.SupportMessages
                .Where(m => m.User.Id == user.Id)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }

        public async Task<List<SupportMessage>> GetMessagesByUserIdAsync(string userId)
        {
            return await _context.SupportMessages
                .Where(m => m.User.Id == userId)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }

        public async Task AddMessageAsync(SupportMessage message)
        {
            await _context.SupportMessages.AddAsync(message);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Users>> GetAllUsersWithMessagesAsync()
        {
            return await _context.Users
                .Include(u => u.SupportMessages)
                .Where(u => u.SupportMessages.Any())
                .ToListAsync();
        }

        public async Task<List<SupportMessage>> GetAllMessagesAsync()
        {
            return await _context.SupportMessages.ToListAsync();
        }

        public async Task<List<Users>> GetDistinctUsersFromMessagesAsync()
        {
            return await _context.SupportMessages
                .Select(m => m.User)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<AdminInboxUserViewModel>> GetAdminInboxUsersAsync()
        {
            var usersWithMessages = await _context.Users
                .Include(u => u.SupportMessages)
                .Where(u => u.SupportMessages.Any())
                .ToListAsync();

            var result = new List<AdminInboxUserViewModel>();

            foreach (var user in usersWithMessages)
            {
                // Exclude admins
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                    continue;

                var lastMessage = user.SupportMessages
                    .OrderByDescending(m => m.Timestamp)
                    .FirstOrDefault();

                result.Add(new AdminInboxUserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    HasUnread = user.SupportMessages.Any(m => m.Sender == "User" && !m.IsRead),
                    LastMessageTime = lastMessage?.Timestamp
                });
            }

            return result
                .OrderByDescending(u => u.HasUnread)
                .ThenByDescending(u => u.LastMessageTime)
                .ToList();
        }

        public async Task<bool> IsUserInRoleAsync(Users user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }

    }
}
