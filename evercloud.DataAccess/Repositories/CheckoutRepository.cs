using evercloud.Domain.Models;
using evercloud.Domain.Interfaces;
using evercloud.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; // Add for logging

namespace evercloud.DataAccess.Repositories
{
    public class CheckoutRepository(AppDbContext context, ILogger<CheckoutRepository> logger) : ICheckoutRepository
    {
        public async Task<List<Purchase>> GetAllPurchasesAsync()
        {
            try
            {
                return await context.Purchases.ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in GetAllPurchasesAsync");
                return [];
            }
        }

        public async Task AddPurchaseAsync(Purchase purchase)
        {
            try
            {
                context.Purchases.Add(purchase);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in AddPurchaseAsync for purchase: {@Purchase}", purchase);
            }
        }

        public async Task DeletePurchaseAsync(int id)
        {
            try
            {
                var purchase = await context.Purchases.FindAsync(id);
                if (purchase != null)
                {
                    context.Purchases.Remove(purchase);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DeletePurchaseAsync for id: {Id}", id);
                // Optionally rethrow or handle as needed
            }
        }
    }
}
