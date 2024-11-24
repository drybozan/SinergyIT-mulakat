namespace OrderTestApplication.Models
{
    public class Order
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public string UserId { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
