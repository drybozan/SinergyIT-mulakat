using CRUDApplication.Entities;

namespace CRUDApplication.Data.Repositories.Abstracts
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        // Customer'a özgü işlemler bu arabirimde tanımlanır
        Customer GetCustomerWithOrders(int customerId);
    }
}
