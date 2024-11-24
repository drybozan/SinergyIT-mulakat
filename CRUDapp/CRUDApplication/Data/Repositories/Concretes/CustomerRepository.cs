using CRUDApplication.Data.Contexts;
using CRUDApplication.Data.Repositories.Abstracts;
using CRUDApplication.Data.Repository;
using CRUDApplication.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRUDApplication.Data.Repositories.Concretes
{
    public class CustomerRepository : EfRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(AppDbContext context) : base(context) { }

        public Customer GetCustomerWithOrders(int customerId)
        {
            return _context.Customers
                           .Include(c => c.orders)
                           .FirstOrDefault(c => c.id == customerId);
        }
    }
}
