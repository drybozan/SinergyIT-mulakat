using CRUDApplication.Data.Contexts;
using CRUDApplication.Data.Repositories.Abstracts;
using CRUDApplication.Data.Repository;
using CRUDApplication.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRUDApplication.Data.Repositories.Concretes
{
    public class OrderRepository : EfRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context) { }

        public Order GetOrderWithCustomer(int orderId)
        {
            return _context.Orders
                           .Include(o => o.customer)
                           .FirstOrDefault(o => o.id == orderId);
        }
    }
}
