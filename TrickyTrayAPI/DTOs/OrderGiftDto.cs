namespace TrickyTrayAPI.DTOs
{
    public class OrderGiftDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string BuyerName { get; set; } = string.Empty;

        public string GiftName { get; set; } = string.Empty;
        public double GiftPrice { get; set; }
    }
}
