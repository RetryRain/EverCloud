using evercloud.DataAccess.Data;
using evercloud.Domain.Models;
using evercloud.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace evercloud.Controllers
{
    [Authorize]
    public class SupportController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public SupportController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 🔄 Redirect admin to AdminInbox
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction(nameof(AdminInbox));
            }

            var user = await _userManager.GetUserAsync(User);
            var messages = await _context.SupportMessages
                .Where(m => m.User == user)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

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
                _context.SupportMessages.Add(welcome);
                await _context.SaveChangesAsync();
                messages.Add(welcome);
            }

            // ✅ Mark unread admin messages as read
            var unreadAdminMessages = messages
                .Where(m => m.Sender == "Admin" && !m.IsRead)
                .ToList();

            foreach (var msg in unreadAdminMessages)
            {
                msg.IsRead = true;
            }

            if (unreadAdminMessages.Any())
            {
                await _context.SaveChangesAsync();
            }

            return View(messages);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return RedirectToAction("Index");

            var user = await _userManager.GetUserAsync(User);

            var newMessage = new SupportMessage
            {
                User = user,
                Sender = "User",
                MessageContent = message,
                Timestamp = DateTime.Now,
                IsRead = false
            };

            _context.SupportMessages.Add(newMessage);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // 🛠 Admin view of users with messages (includes unread logic)
        [Authorize(Roles = "Admin")]
        public IActionResult AdminInbox()
        {
            var usersWithMessages = _context.Users
                .Include(u => u.SupportMessages)
                .Where(u => u.SupportMessages.Any())
                .Select(u => new AdminInboxUserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FullName = u.FullName,
                    Email = u.Email,
                    HasUnread = u.SupportMessages.Any(m => m.Sender == "User" && !m.IsRead)
                })
                .ToList();

            return View("AdminInbox", usersWithMessages);
        }

        // 📬 Admin chats with a selected user and marks messages as read
        [Authorize(Roles = "Admin")]
        public IActionResult AdminChat(string userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return NotFound();

            var messages = _context.SupportMessages
                .Where(m => m.User.Id == userId)
                .OrderBy(m => m.Timestamp)
                .ToList();

            // ✅ Mark all unread user messages as read when admin opens chat
            var unreadMessages = messages.Where(m => m.Sender == "User" && !m.IsRead).ToList();
            foreach (var msg in unreadMessages)
            {
                msg.IsRead = true;
            }

            if (unreadMessages.Any())
            {
                _context.SaveChanges();
            }

            var viewModel = new SupportChatViewModel
            {
                SelectedUser = user,
                Messages = messages
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult ReplyToUser(string userId, string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return RedirectToAction("AdminChat", new { userId });

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return NotFound();

            var newMessage = new SupportMessage
            {
                User = user,
                Sender = "Admin",
                MessageContent = message,
                Timestamp = DateTime.Now,
                IsRead = false // 🔁 FIXED: Admin messages are unread for user
            };

            _context.SupportMessages.Add(newMessage);
            _context.SaveChanges();

            return RedirectToAction("AdminChat", new { userId });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminPanel()
        {
            var users = _context.SupportMessages
                .Select(m => m.User)
                .Distinct()
                .ToList();

            return View(users);
        }
    }
}
