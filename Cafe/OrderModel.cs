namespace Cafe
{
    public class OrderModel
    {
        public int Id { get; set; }
        public DateTime? DateAndTime { get; set; }
        public string? ClientName { get; set; }
        public string? FoodList { get; set; }
        public decimal? СurrentPrice { get; set; }
        public short? TableNumber { get; set; }
        public short? ClientCount { get; set; }
        public string? OrderStatus { get; set; }
    }
}