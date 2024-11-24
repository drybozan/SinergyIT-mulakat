using CRUDApplication.Core.Result;
using CRUDApplication.Entities.DTOs;

namespace CRUDApplication.Business.Abstracts
{
    public interface IOrderService
    {
        Result GetAllOrders();
        Result GetOrderById(int id);
        Result CreateOrder(OrderDto orderDto);
        Result UpdateOrder(OrderDto orderDto);
        Result DeleteOrder(int id);
    }
}
