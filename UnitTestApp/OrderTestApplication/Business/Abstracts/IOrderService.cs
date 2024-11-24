using OrderTestApplication.Models;

namespace OrderTestApplication.Business.Abstracts
{
    public interface IOrderService
    {
        bool PlaceOrder(Order order);
    }
}
