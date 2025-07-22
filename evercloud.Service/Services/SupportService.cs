using evercloud.Domain.Interfaces;
using evercloud.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace evercloud.Service.Services
{
    public class SupportService : ISupportService
    {
        private readonly ISupportRepository _supportRepository;

        public SupportService(ISupportRepository supportRepository)
        {
            _supportRepository = supportRepository;
        }

        public async Task<Users> GetCurrentUserAsync(ClaimsPrincipal principal)
        {
            return await _supportRepository.GetUserAsync(principal);
        }

        public async Task<List<SupportMessage>> GetMessagesForUserAsync(Users user)
        {
            var messages = await _supportRepository.GetMessagesByUserAsync(user);

            if (!messages.Any())
            {
                var welcome = new SupportMessage
                {
                    User = user,
                    Sender = "Admin",
                    MessageContent = "Hi! How may we help you today?",
                    Timestamp = DateTime.Now,
                    IsRead = true
                };

                await _supportRepository.AddMessageAsync(welcome);
                await _supportRepository.SaveChangesAsync();

                messages.Add(welcome);
            }

            // Mark unread admin messages as read
            var unreadAdminMessages = messages
                .Where(m => m.Sender == "Admin" && !m.IsRead)
                .ToList();

            foreach (var msg in unreadAdminMessages)
            {
                msg.IsRead = true;
            }

            if (unreadAdminMessages.Any())
            {
                await _supportRepository.SaveChangesAsync();
            }

            return messages.OrderBy(m => m.Timestamp).ToList();
        }

        public async Task PostUserMessageAsync(Users user, string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            var newMessage = new SupportMessage
            {
                User = user,
                Sender = "User",
                MessageContent = message,
                Timestamp = DateTime.Now,
                IsRead = false
            };

            await _supportRepository.AddMessageAsync(newMessage);
            await _supportRepository.SaveChangesAsync();
        }

        public async Task<List<AdminInboxUserViewModel>> GetAdminInboxUsersAsync()
        {
            return await _supportRepository.GetAdminInboxUsersAsync();
        }

        public async Task<SupportChatViewModel> GetAdminChatViewModelAsync(string userId)
        {
            var user = await _supportRepository.GetUserByIdAsync(userId);
            if (user == null) return null;

            var messages = await _supportRepository.GetMessagesByUserIdAsync(userId);

            var unreadUserMessages = messages
                .Where(m => m.Sender == "User" && !m.IsRead)
                .ToList();

            foreach (var msg in unreadUserMessages)
            {
                msg.IsRead = true;
            }

            if (unreadUserMessages.Any())
            {
                await _supportRepository.SaveChangesAsync();
            }

            return new SupportChatViewModel
            {
                SelectedUser = user,
                Messages = messages.OrderBy(m => m.Timestamp).ToList()
            };
        }

        public async Task ReplyToUserAsync(string userId, string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            var user = await _supportRepository.GetUserByIdAsync(userId);
            if (user == null) return;

            var reply = new SupportMessage
            {
                User = user,
                Sender = "Admin",
                MessageContent = message,
                Timestamp = DateTime.Now,
                IsRead = false
            };

            await _supportRepository.AddMessageAsync(reply);
            await _supportRepository.SaveChangesAsync();
        }

        public async Task<List<Users>> GetUsersFromSupportMessagesAsync()
        {
            return await _supportRepository.GetDistinctUsersFromMessagesAsync();
        }

        public async Task<List<SupportMessage>> GetMessagesByUserIdAsync(string userId)
        {
            return await _supportRepository.GetMessagesByUserIdAsync(userId);
        }

        // 🔥 Added this method so you can check roles in controller cleanly
        public async Task<bool> IsUserInRoleAsync(Users user, string role)
        {
            return await _supportRepository.IsUserInRoleAsync(user, role);
        }
    }
}
