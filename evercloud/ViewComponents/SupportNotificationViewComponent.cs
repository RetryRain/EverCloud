using evercloud.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace evercloud.ViewComponents
{
    public class SupportNotificationViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public SupportNotificationViewComponent(AppDbContext context)
        {
            _context = context;
        }

        //public async Task<IViewComponentResult> InvokeAsync()
        //{
        //    var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return View(false); // Not logged in
        //    }

        //    var hasUnread = await _context.SupportMessages
        //        .AnyAsync(m => m.UserId == userId && m.Sender == "Admin" && !m.IsRead);

        //    return View(hasUnread);
        //}

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return View(false); // Not logged in
            }

            // Check if the user is an admin
            bool isAdmin = HttpContext.User.IsInRole("Admin");

            bool hasUnread = false;

            if (isAdmin)
            {
                // Admin: check if any users sent unread messages
                hasUnread = await _context.SupportMessages
                    .AnyAsync(m => m.Sender == "User" && !m.IsRead);
            }
            else
            {
                // Regular user: check if admin sent unread messages to *this* user
                hasUnread = await _context.SupportMessages
                    .AnyAsync(m => m.UserId == userId && m.Sender == "Admin" && !m.IsRead);
            }

            return View(hasUnread);
        }

    }

}
