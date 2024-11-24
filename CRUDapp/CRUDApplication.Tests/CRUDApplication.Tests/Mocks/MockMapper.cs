using AutoMapper;
using CRUDApplication.Entities;
using CRUDApplication.Entities.DTOs;
using Moq;

namespace CRUDApplication.CRUDApplication.Tests.CRUDApplication.Tests.Mocks
{
    public class MockMapper
    {
        public static Mock<IMapper> GetMockMapper()
        {
            var mockMapper = new Mock<IMapper>();

            // Setup for mapping CustomerDto to Customer
            mockMapper.Setup(mapper => mapper.Map<Customer>(It.IsAny<CustomerDto>())).Returns((CustomerDto dto) => new Customer { id = 4, name = dto.name, email = dto.email });

            //  Customer to CustomerDto
            mockMapper.Setup(mapper => mapper.Map<CustomerDto>(It.IsAny<Customer>())).Returns((Customer customer) => new CustomerDto { id = customer.id, name = customer.name, email = customer.email });

            // Setup Order to OrderDto list
            mockMapper.Setup(mapper => mapper.Map<IEnumerable<CustomerDto>>(It.IsAny<IEnumerable<CustomerDto>>()))
                .Returns((IEnumerable<Customer> orders) => orders.Select(o => new CustomerDto { id = o.id, name = o.name }));

            // Setup  Order to OrderDto
            mockMapper.Setup(mapper => mapper.Map<OrderDto>(It.IsAny<Order>()))
                .Returns((Order order) => new OrderDto { Id = order.id, productName = order.productName });
                      

            // Setup Order to OrderDto list
            mockMapper.Setup(mapper => mapper.Map<IEnumerable<OrderDto>>(It.IsAny<IEnumerable<Order>>()))
                .Returns((IEnumerable<Order> orders) => orders.Select(o => new OrderDto { Id = o.id, productName = o.productName }));

            return mockMapper;
        }
    }
}
