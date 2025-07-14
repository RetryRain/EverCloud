using evercloud.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace evercloud.DataAccess.Data
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext<Users>(options)
    {
        public DbSet<Purchase> Purchases { get; set; }

    }
}
