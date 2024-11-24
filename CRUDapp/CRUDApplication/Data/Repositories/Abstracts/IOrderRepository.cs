using CRUDApplication.Entities;

namespace CRUDApplication.Data.Repositories.Abstracts
{
    public interface IOrderRepository : IRepository<Order>
    {
        Order GetOrderWithCustomer(int orderId);
    }
}
