namespace CRUDApplication.Entities
{
    public class Customer
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }

        // Bir müşteri birden fazla siparişe sahip olabilir
        public virtual ICollection<Order> orders { get; set; }
    }
}
