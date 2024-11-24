namespace CRUDApplication.Entities
{
    public class Order
    {
        public int id { get; set; }
        public string productName { get; set; }
        public int quantity { get; set; }
        public DateTime orderDate { get; set; }

        // Siparişin ait olduğu müşteri
        public int customerId { get; set; }
        public virtual Customer customer { get; set; }
    }
}
