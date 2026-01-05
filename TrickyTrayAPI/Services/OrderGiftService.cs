using Microsoft.EntityFrameworkCore; // חובה בשביל ToListAsync()
using TrickyTrayAPI.DTOs; // חובה בשביל OrderByDescending()
using TrickyTrayAPI.Models;
using TrickyTrayAPI.Repositories;

namespace TrickyTrayAPI.Services
{
    public class OrderGiftService : IOrderGiftService
    {
        private readonly IOrderGiftRepository _orderGiftRepository;

        public OrderGiftService(IOrderGiftRepository orderGiftRepository)
        {
            _orderGiftRepository = orderGiftRepository;
        }

        // הוספת מתנה לסל
        public async Task<bool> AddGiftAsync(int orderId, int giftId)
        {
            return await _orderGiftRepository.AddGiftAsync(orderId, giftId);
        }

        // הסרת מתנה מהסל
        public async Task<bool> RemoveGiftAsync(int orderId, int giftId)
        {
            return await _orderGiftRepository.RemoveGiftAsync(orderId, giftId);
        }

        // שליפת הזמנות של מתנה מסוימת
        public async Task<List<OrderGift>> GetOrdersForGiftAsync(int giftId)
        {
            return await _orderGiftRepository.GetOrdersForGiftAsync(giftId);
        }





        public async Task<List<OrderGiftDto>> GetSortedOrderGiftsAsync(string sortBy)
        {
            // שליפת הרכישות כולל כל המידע מסביב
            var query = _orderGiftRepository.GetAllWithDetails();

            if (sortBy?.ToLower() == "price")
            {
                // ממיינים את הרכישות לפי מחיר המתנה שבתוכן
                query = query.OrderByDescending(og => og.Gift.Price);
            }
            else if (sortBy?.ToLower() == "popularity")
            {
                // ממיינים רכישות לפי כמה המתנה הזו פופולרית באופן כללי
                query = query.OrderByDescending(og =>
                    _orderGiftRepository.GetAllWithDetails().Count(inner => inner.GiftId == og.GiftId));
            }

            // המרה ל-DTO שלך
            return await query.Select(og => new OrderGiftDto
            {
                OrderId = og.OrderId,
                OrderDate = og.Order.OrderDate,
                BuyerName = og.Order.Buyer.Name,// או כל שדה אחר שיש לך ב-Buyer
                GiftName = og.Gift.Name,
                GiftPrice = og.Gift.Price
            }).ToListAsync();
        }
    }
}

