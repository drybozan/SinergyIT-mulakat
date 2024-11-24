using AutoMapper;
using CRUDApplication.Business.Concretes;
using CRUDApplication.CRUDApplication.Tests.CRUDApplication.Tests.Mocks;
using CRUDApplication.Data.Repositories.Abstracts;
using CRUDApplication.Entities.DTOs;
using CRUDApplication.Entities;
using Moq;
using Xunit;

namespace CRUDApplication.CRUDApplication.Tests.CRUDApplication.Tests.Managers
{
    public class CustomerManagerTests
    {
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CustomerManager _customerManager;

        public CustomerManagerTests()
        {
            _mockCustomerRepository = MockCustomerRepository.GetMockCustomerRepository();
            _mockMapper = MockMapper.GetMockMapper();
            _customerManager = new CustomerManager(_mockCustomerRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public void GetCustomerById_ReturnsSuccessResultWithCustomerDto()
        {
            // Arrange
            var customer = new Customer { id = 1, name = "John Doe" };
            var customerDto = new CustomerDto { id = 1, name = "John Doe" };

            _mockCustomerRepository.Setup(repo => repo.GetById(1)).Returns(customer);
            _mockMapper.Setup(mapper => mapper.Map<CustomerDto>(customer)).Returns(customerDto);

            // Act
            var result = _customerManager.GetCustomerById(1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Customer retrieved successfully", result.Message);
            Assert.NotNull(result.data);
        }

        [Fact]
        public void GetAllCustomers_ShouldReturnSuccessResult_WithCustomerList()
        {
            // Arrange
            var customers = new List<Customer>
        {
            new Customer { id = 1, name = "derya", email = "derya.com" },
            new Customer { id = 2, name = "ali", email = "ali.com" },
            new Customer { id = 3, name = "mehmet", email = "mehmetqgmail.com" }
        };

            var customerDtos = new List<CustomerDto>
        {
            new CustomerDto { id = 1, name = "derya", email = "derya.com" },
            new CustomerDto { id = 2, name = "ali", email = "ali.com" },
            new CustomerDto { id = 3, name = "mehmet", email = "mehmetqgmail.com" }
        };

            _mockCustomerRepository.Setup(repo => repo.GetAll()).Returns(customers);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<CustomerDto>>(customers)).Returns(customerDtos);

            // Act
            var result = _customerManager.GetAllCustomers();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Customers retrieved successfully", result.Message);
            Assert.IsType<List<CustomerDto>>(result.data);  
            Assert.Equal(3, ((List<CustomerDto>)result.data).Count); 
            Assert.Equal("derya", ((List<CustomerDto>)result.data).First().name); 
        }

        [Fact]
        public void GetAllCustomers_ShouldReturnFailureResult_WhenNoCustomersFound()
        {
            // Arrange
            _mockCustomerRepository.Setup(repo => repo.GetAll()).Returns(new List<Customer>());

            // Act
            var result = _customerManager.GetAllCustomers();

            // Assert
            Assert.False(result.IsSuccess);  // Testin başarısız dönmesi gerektiğini kontrol et
            Assert.Equal("An error occurred while retrieving customers.", result.Message);  // Hata mesajının doğru olduğunu kontrol et
            Assert.Null(result.data);  // Data'nın null olması gerektiğini kontrol et
        }


        [Fact]
        public void GetCustomerById_ShouldReturnSuccessResult_WithCustomerDto()
        {
            // Arrange
            var customer = new Customer { id = 1, name = "derya", email = "derya.com" };
            var customerDto = new CustomerDto { id = 1, name = "derya", email = "derya.com" };

            _mockCustomerRepository.Setup(repo => repo.GetById(1)).Returns(customer);
            _mockMapper.Setup(mapper => mapper.Map<CustomerDto>(customer)).Returns(customerDto);

            // Act
            var result = _customerManager.GetCustomerById(1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Customer retrieved successfully", result.Message);
            Assert.IsType<CustomerDto>(result.data);  
            Assert.Equal("derya", ((CustomerDto)result.data).name);  
        }

        [Fact]
        public void CreateCustomer_ShouldReturnSuccessResult_WhenCustomerIsCreated()
        {
            // Arrange
            var customerDto = new CustomerDto { name = "zeynep", email = "zeynep@example.com" };
            var customer = new Customer { id = 4, name = "zeynep", email = "zeynep@example.com" };

            _mockMapper.Setup(mapper => mapper.Map<Customer>(customerDto)).Returns(customer);
            _mockCustomerRepository.Setup(repo => repo.Add(customer)).Verifiable();

            // Act
            var result = _customerManager.CreateCustomer(customerDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Customer created successfully.", result.Message);
            Assert.Equal(4, result.ReturnId);
            _mockCustomerRepository.Verify(repo => repo.Add(It.IsAny<Customer>()), Times.Once);
        }


        [Fact]
        public void CreateCustomer_ShouldReturnFailureResult_WhenDatabaseErrorOccurs()
        {
            // Arrange
            var customerDto = new CustomerDto { name = "zeynep", email = "zeynep@example.com" };

            // Setup mapper to throw exception
            _mockMapper.Setup(mapper => mapper.Map<Customer>(customerDto));

            // Act
            var result = _customerManager.CreateCustomer(customerDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("An error occurred while creating the customer.", result.Message);
            Assert.Null(result.data);  // Data should be null in case of error
        }
    }
}
