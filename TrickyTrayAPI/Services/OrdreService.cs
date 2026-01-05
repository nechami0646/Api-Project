using TrickyTrayAPI.DTOs;
using TrickyTrayAPI.Models;
using TrickyTrayAPI.Repositories;

namespace TrickyTrayAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // שליפת סל מלא
        public async Task<OrderViewDto?> GetOrderViewAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderWithItemsAsync(orderId);
            if (order == null) return null;

            return new OrderViewDto
            {
                Id = order.Id,
                BuyerId = order.BuyerId,
                BuyerName = order.Buyer.Name,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString(),

                Items = order.OrderGifts.Select(og => new CartItemDto
                {
                    GiftId = og.GiftId,
                    GiftName = og.Gift.Name,
                    Price = og.Gift.Price,
                    Category = og.Gift.Category
                }).ToList(),

                TotalPrice = order.OrderGifts.Sum(og => og.Gift.Price)
            };
        }
        // יצירת טיוטת הזמנה חדשה
        public Task<Order> CreateDraftAsync(int buyerId)
        {
            return _orderRepository.CreateDraftAsync(buyerId);
        }

        // אישור הזמנה
        public async Task<bool> ConfirmOrderAsync(int orderId)
        {
            return await _orderRepository.ConfirmOrderAsync(orderId);
        }

    }
}
