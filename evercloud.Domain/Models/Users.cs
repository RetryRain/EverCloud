using evercloud.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace evercloud.Domain.Models
{
    public class Users : IdentityUser
    {
        public required string FullName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<Purchase> Purchases { get; set; } = new();

        public ICollection<SupportMessage> SupportMessages { get; set; }

    }
}
