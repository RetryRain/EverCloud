using evercloud.DataAccess.Data;
using evercloud.Domain.Models;
using Microsoft.EntityFrameworkCore;
using evercloud.Domain.Interfaces;
using Microsoft.Extensions.Logging; 

namespace evercloud.DataAccess.Repositories
{
    public class PurchaseRepository(AppDbContext context, ILogger<PurchaseRepository> logger) : IPurchaseRepository
    {
        public async Task<IEnumerable<Purchase>> GetAllAsync()
        {
            try
            {
                return await context.Purchases
                    .Include(p => p.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in GetAllAsync");
                return Enumerable.Empty<Purchase>();
            }
        }

        public async Task<Purchase?> GetByIdAsync(int id)
        {
            try
            {
                return await context.Purchases
                    .Include(p => p.User)
                    .Include(p => p.Plan)
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in GetByIdAsync for id: {Id}", id);
                return null;
            }
        }

        public async Task<IEnumerable<Purchase>> GetByUserIdAsync(string userId)
        {
            try
            {
                return await context.Purchases
                    .Include(p => p.Plan)
                    .Where(p => p.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in GetByUserIdAsync for userId: {UserId}", userId);
                return Enumerable.Empty<Purchase>();
            }
        }

        public async Task AddAsync(Purchase purchase)
        {
            try
            {
                await context.Purchases.AddAsync(purchase);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in AddAsync for purchase: {@Purchase}", purchase);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var purchase = await context.Purchases.FindAsync(id);
                if (purchase != null)
                {
                    context.Purchases.Remove(purchase);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DeleteAsync for id: {Id}", id);
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in SaveChangesAsync");
            }
        }
    }
}
