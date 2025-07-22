using evercloud.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace evercloud.Controllers
{
    [Authorize]
    public class SupportController : Controller
    {
        private readonly ISupportService _supportService;

        public SupportController(ISupportService supportService)
        {
            _supportService = supportService;
        }

        // User support inbox
        //public async Task<IActionResult> Index()
        //{
        //    var user = await _supportService.GetCurrentUserAsync(User);
        //    var messages = await _supportService.GetMessagesForUserAsync(user);
        //    return View(messages);
        //}
        public async Task<IActionResult> Index()
        {
            var user = await _supportService.GetCurrentUserAsync(User);

            if (await _supportService.IsUserInRoleAsync(user, "Admin"))
            {
                return RedirectToAction(nameof(AdminInbox));
            }

            var messages = await _supportService.GetMessagesForUserAsync(user);
            return View(messages);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return RedirectToAction("Index");

            var user = await _supportService.GetCurrentUserAsync(User);
            await _supportService.PostUserMessageAsync(user, message);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminInbox()
        {
            var usersWithMessages = await _supportService.GetAdminInboxUsersAsync();
            return View("AdminInbox", usersWithMessages);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminChat(string userId)
        {
            var viewModel = await _supportService.GetAdminChatViewModelAsync(userId);
            if (viewModel == null) return NotFound();

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReplyToUser(string userId, string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return RedirectToAction("AdminChat", new { userId });

            await _supportService.ReplyToUserAsync(userId, message);
            return RedirectToAction("AdminChat", new { userId });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminPanel()
        {
            var users = await _supportService.GetUsersFromSupportMessagesAsync();
            return View(users);
        }
    }
}
