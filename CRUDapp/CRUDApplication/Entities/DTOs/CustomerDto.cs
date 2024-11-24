namespace CRUDApplication.Entities.DTOs
{
    public class CustomerDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public List<OrderDto> orders { get; set; }
    }
}
