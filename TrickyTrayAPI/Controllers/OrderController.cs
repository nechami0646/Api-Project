using Microsoft.AspNetCore.Mvc;
using TrickyTrayAPI.DTOs;
using TrickyTrayAPI.Services;

[ApiController]
[Route("api/order")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IOrderGiftService _orderGiftService;

    public OrderController(IOrderService orderService, IOrderGiftService orderGiftService)
    {
        _orderService = orderService;
        _orderGiftService = orderGiftService;
    }

    // יצירת הזמנה חדשה (טיוטה)
    [HttpPost("create/{buyerId}")]
    public async Task<IActionResult> Create(int buyerId)
    {
        var order = await _orderService.CreateDraftAsync(buyerId);
        return Ok(new { orderId = order.Id });
    }



    [HttpGet("purchases/sorted")]
    public async Task<ActionResult<List<OrderGiftDto>>> GetSortedPurchases([FromQuery] string sortBy)
    {
        // קריאה ל-Service וקבלת הרשימה הממוינת של ה-DTOs
        var result = await _orderGiftService.GetSortedOrderGiftsAsync(sortBy);

        if (result == null) return NotFound();

        return Ok(result);
    }
}
