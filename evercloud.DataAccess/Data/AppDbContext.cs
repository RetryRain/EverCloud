using evercloud.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace evercloud.DataAccess.Data
{
    public class AppDbContext : IdentityDbContext<Users>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }

        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<SupportMessage> SupportMessages { get; set; }

    }
}
