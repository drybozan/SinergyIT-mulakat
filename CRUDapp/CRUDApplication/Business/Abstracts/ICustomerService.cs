using CRUDApplication.Core.Result;
using CRUDApplication.Entities.DTOs;

namespace CRUDApplication.Business.Abstracts
{
    public interface ICustomerService
    {
        Result GetAllCustomers();
        Result GetCustomerById(int id);
        Result CreateCustomer(CustomerDto customer);
        Result UpdateCustomer(CustomerDto customer);
        Result DeleteCustomer(int id);
    }
}
