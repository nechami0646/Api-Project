using TrickyTrayAPI.Models;

namespace TrickyTrayAPI.Repositories
{
    public interface IOrderGiftRepository
    {
        Task<bool> AddGiftAsync(int orderId, int giftId);
        Task<bool> RemoveGiftAsync(int orderId, int giftId);
        Task<List<OrderGift>> GetOrdersForGiftAsync(int giftId);

        Task<List<OrderGift>> GetAllWinnersAsync();
        Task SaveChangesAsync();
        IQueryable<OrderGift> GetAllWithDetails();
    }
}