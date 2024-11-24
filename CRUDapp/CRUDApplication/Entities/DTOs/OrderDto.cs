namespace CRUDApplication.Entities.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int customerId { get; set; }

        public string productName { get; set; }
        public int quantity { get; set; }
        public DateTime orderDate { get; set; }
    }
}
