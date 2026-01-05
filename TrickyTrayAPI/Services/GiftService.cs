using TrickyTrayAPI.DTOs;
using TrickyTrayAPI.Models;
using TrickyTrayAPI.Repositories;

namespace TrickyTrayAPI.Services
{
    public class GiftService : IGiftService
    {
        private readonly IGiftRepository _giftRepo;
        private readonly SystemStateService _systemStateService;
        private readonly IOrderGiftService _orderGiftService;
        public GiftService(IGiftRepository giftRepo, SystemStateService systemStateService, IOrderGiftService orderGiftService)
        {
            _giftRepo = giftRepo;
            _systemStateService = systemStateService;
            _orderGiftService = orderGiftService;
        }

        public async Task<IEnumerable<GiftViewDto>> GetAllAsync()
        {
            var state = _systemStateService.GetState();
            if (state.Status != SystemState.SaleStatus.Active)
            {
                throw new InvalidOperationException("המכירה החלה אין אפשרות להוסיף תורמים חדשים");
            }
            var gifts = await _giftRepo.GetAllAsync();

            return gifts.Select(b => MapToViewDto(b));
        }

        public async Task<GiftViewDto?> GetByIdAsync(int id)
        {
            var gift = await _giftRepo.GetByIdAsync(id);

            return gift != null ? MapToViewDto(gift) : null;
        }
        private GiftViewDto MapToViewDto(Gift g)
        {
            return new GiftViewDto
            {
                Id = g.Id,
                Name = g.Name,
                Price = g.Price,
                ImageUrl = g.ImageUrl,
                DonorName = g.Donor?.Name ?? "תורם אנונימי"
            };
        }

        public async Task<GiftViewDto> CreateAsync(GiftCreateDto createDto)
        {
            var state = _systemStateService.GetState();
            if (state.Status != SystemState.SaleStatus.Draft)
            {
                throw new InvalidOperationException("המכירה החלה אין אפשרות להוסיף מתנות חדשות");
            }

            var gift = new Gift
            {
                Name = createDto.Name,
                Description = createDto.Description,
                Price = createDto.Price,
                ImageUrl = createDto.ImageUrl,
                Category = createDto.Category,
                DonorId = createDto.DonorId
            };


            var savedGift = await _giftRepo.CreateAsync(gift);


            return MapToViewDto(savedGift);
        }

        public async Task<GiftViewDto?> UpdateAsync(int id, GiftUpdateDto updateDto)
        {
            var existingGift = await _giftRepo.GetByIdAsync(id);
            if (existingGift == null) return null;

            if (updateDto.Name != null) existingGift.Name = updateDto.Name;
            if (updateDto.Description != null) existingGift.Description = updateDto.Description;
            if (updateDto.Price != 0) existingGift.Price = updateDto.Price;
            if (updateDto.DonorId.HasValue) existingGift.DonorId = updateDto.DonorId.Value;
            if (updateDto.Category != null) existingGift.Category = updateDto.Category;
            if (updateDto.ImageUrl != null) existingGift.ImageUrl = updateDto.ImageUrl;


            var update = await _giftRepo.UpdateAsync(existingGift);
            return update != null ? MapToViewDto(update) : null;
        }

        //public async Task<bool> DeleteAsync(int id)
        //{
        //    var state = _systemStateService.GetState();

        //    // Explicitly handle each sale status so every code path returns or throws.
        //    if (state.Status == SystemState.SaleStatus.Finished)
        //    {
        //        // When sale is finished, disallow deletion.
        //        throw new InvalidOperationException("המכירה הסתיימה אין אפשרות למחוק מתנות");
        //    }

        //    if (state.Status == SystemState.SaleStatus.Active)
        //    {
        //        var purchases = await _orderGiftService.GetOrdersForGiftAsync(id);
        //        // If there are any purchases for this gift, do not allow delete.
        //        if (purchases != null && purchases.Any())
        //        {
        //            throw new InvalidOperationException("לא ניתן למחוק מתנה שקיימות רכישות עבורה");
        //        }
        //        return await _giftRepo.DeleteAsync(id);
        //    }
        //}

        public async Task<List<GiftViewDto>> SearchGiftsAsync(string? name, string? donorName, int? buyersCount)
        {
            var gifts = await _giftRepo.SearchGiftsAsync(name, donorName, buyersCount);

            return gifts.Select(g => new GiftViewDto
            {
                Id = g.Id,
                Name = g.Name,
                DonorName = g.Donor?.Name ?? "תורם אנונימי",
                ImageUrl = g.ImageUrl
            }).ToList();
        }

        public async Task<List<GiftViewDto>> GetSortedGiftsAsync(string sortBy)
        {
            var gifts = await _giftRepo.GetSortedGiftsAsync(sortBy);

            return gifts.Select(g => new GiftViewDto
            {
                Id = g.Id,
                Name = g.Name,
                Price = g.Price,
                DonorName = g.Donor?.Name ?? "תורם אנונימי",
                ImageUrl = g.ImageUrl,
                Category = g.Category
            }).ToList();
        }


    }
}