using Microsoft.AspNetCore.Mvc;
using TrickyTrayAPI.Models;
using TrickyTrayAPI.Services;

namespace TrickyTrayAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "manager")]
    public class RaffleController : ControllerBase
    {
        private readonly RaffleService _raffleService;

        public RaffleController(RaffleService raffleService)
        {
            _raffleService = raffleService;
        }

        [HttpPost("{giftId}")]
        public async Task<ActionResult<Buyer>> Raffle(int giftId)
        {
            var winner = await _raffleService.RaffleAsync(giftId);

            if (winner == null)
                return NotFound("No participants for this gift");

            return Ok(winner);
        }

        [HttpGet("winners-report")]
        public async Task<IActionResult> GetWinnersReport()
        {
            var report = await _raffleService.GetWinnersReportAsync();
            return Ok(report);
        }

        [HttpGet("total-revenue")]
        public async Task<IActionResult> GetTotalRevenue()
        {
            var revenue = await _raffleService.GetTotalRevenueAsync();
            return Ok(new { totalRevenue = revenue });
        }


    }
}
