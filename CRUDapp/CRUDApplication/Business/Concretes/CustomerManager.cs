using AutoMapper;
using CRUDApplication.Business.Abstracts;
using CRUDApplication.Core.Result;
using CRUDApplication.Data.Repositories.Abstracts;
using CRUDApplication.Entities;
using CRUDApplication.Entities.DTOs;

namespace CRUDApplication.Business.Concretes
{
    public class CustomerManager : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerManager(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public Result GetAllCustomers()
        {
            try
            {
                var customers = _customerRepository.GetAll();
                if (customers == null || !customers.Any())
                {
                    return new Result { IsSuccess = false, Message = "An error occurred while retrieving customers.", data = null };
                }

                var customerDtos = _mapper.Map<List<CustomerDto>>(customers);
                return new Result { IsSuccess = true, Message = "Customers retrieved successfully", data = customerDtos };
            }
            catch (Exception ex)
            {
                return new Result { IsSuccess = false, Message = "An error occurred while retrieving customers.", data = null };
            }
        }

        public Result GetCustomerById(int id)
        {
            var customer = _customerRepository.GetById(id);
            if (customer == null)
            {
                return new Result().Fail("Customer not found.");
            }

            var customerDto = _mapper.Map<CustomerDto>(customer);
            return new Result().Success("Customer retrieved successfully", customerDto);
        }

        public Result CreateCustomer(CustomerDto customerDto)
        {
            try
            {
                var customer = _mapper.Map<Customer>(customerDto);
                _customerRepository.Add(customer);
                return new Result().Success("Customer created successfully.", customer.id);
            }
            catch (Exception ex)
            {
                return new Result().Fail("An error occurred while creating the customer.", ex.Message);
            }
        }

        public Result UpdateCustomer(CustomerDto customerDto)
        {
            var customer = _customerRepository.GetById(customerDto.id);
            if (customer == null)
            {
                return new Result().Fail("Customer not found.");
            }

            try
            {
                _mapper.Map(customerDto, customer);
                _customerRepository.Update(customer);
                return new Result().Success("Customer updated successfully.", customer.id);
            }
            catch (Exception ex)
            {
                return new Result().Fail("An error occurred while updating the customer.", ex.Message);
            }
        }

        public Result DeleteCustomer(int id)
        {
            var customer = _customerRepository.GetById(id);
            if (customer == null)
            {
                return new Result().Fail("Customer not found.");
            }

            try
            {
                _customerRepository.Delete(customer);
                return new Result().Success("Customer deleted successfully.");
            }
            catch (Exception ex)
            {
                return new Result().Fail("An error occurred while deleting the customer.", ex.Message);
            }
        }
    }
}
