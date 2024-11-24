using AutoMapper;
using CRUDApplication.Business.Concretes;
using CRUDApplication.Data.Repositories.Abstracts;
using CRUDApplication.Entities.DTOs;
using CRUDApplication.Entities;
using Moq;

namespace CRUDApplication.CRUDApplication.Tests.CRUDApplication.Tests.Managers
{
    public class OrderManagerTest
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly OrderManager _orderManager;

        public OrderManagerTest()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockMapper = new Mock<IMapper>();
            _orderManager = new OrderManager(_mockOrderRepository.Object, _mockMapper.Object);
        }

        // Test: GetOrderById
        [Fact]
        public void GetOrderById_ShouldReturnSuccessResult_WhenOrderExists()
        {
            // Arrange
            var order = new Order { id = 1, customerId = 1, orderDate = DateTime.Now };
            var orderDto = new OrderDto { Id = 1, customerId = 1, orderDate = DateTime.Now };

            _mockOrderRepository.Setup(repo => repo.GetById(1)).Returns(order);
            _mockMapper.Setup(mapper => mapper.Map<OrderDto>(order)).Returns(orderDto);

            // Act
            var result = _orderManager.GetOrderById(1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Order retrieved successfully", result.Message);
            Assert.IsType<OrderDto>(result.data);
        }

        [Fact]
        public void GetOrderById_ShouldReturnFailureResult_WhenOrderDoesNotExist()
        {
            // Arrange
            _mockOrderRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Order)null);

            // Act
            var result = _orderManager.GetOrderById(99);  // Non-existent order ID

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Order not found.", result.Message);
            Assert.Null(result.data);
        }

        // Test: GetAllOrders
        [Fact]
        public void GetAllOrders_ShouldReturnSuccessResult_WithOrderList()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { id = 1, customerId = 1, orderDate = DateTime.Now },
                new Order { id = 2, customerId = 2, orderDate = DateTime.Now }
            };
            var orderDtos = new List<OrderDto>
            {
                new OrderDto { Id = 1, customerId = 1, orderDate = DateTime.Now },
                new OrderDto { Id = 2, customerId = 2, orderDate = DateTime.Now }
            };

            _mockOrderRepository.Setup(repo => repo.GetAll()).Returns(orders);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<OrderDto>>(orders)).Returns(orderDtos);

            // Act
            var result = _orderManager.GetAllOrders();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Orders retrieved successfully", result.Message);
            Assert.IsType<List<OrderDto>>(result.data);
            Assert.Equal(2, ((List<OrderDto>)result.data).Count);
        }

        [Fact]
        public void GetAllOrders_ShouldReturnFailureResult_WhenNoOrdersFound()
        {
            // Arrange
            _mockOrderRepository.Setup(repo => repo.GetAll()).Returns(new List<Order>());

            // Act
            var result = _orderManager.GetAllOrders();

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("An error occurred while retrieving orders.", result.Message);
            Assert.Null(result.data);
        }

        // Test: CreateOrder
        [Fact]
        public void CreateOrder_ShouldReturnSuccessResult_WhenOrderIsCreated()
        {
            // Arrange
            var orderDto = new OrderDto { customerId = 1, orderDate = DateTime.Now };
            var order = new Order { id = 1, customerId = 1, orderDate = DateTime.Now };

            _mockMapper.Setup(mapper => mapper.Map<Order>(orderDto)).Returns(order);
            _mockOrderRepository.Setup(repo => repo.Add(order)).Verifiable();

            // Act
            var result = _orderManager.CreateOrder(orderDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Order created successfully.", result.Message);
            Assert.Equal(1, result.ReturnId);
            _mockOrderRepository.Verify(repo => repo.Add(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public void CreateOrder_ShouldReturnFailureResult_WhenDatabaseErrorOccurs()
        {
            // Arrange
            var orderDto = new OrderDto { customerId = 1, orderDate = DateTime.Now };
            var exception = new Exception("Database error");

            _mockMapper.Setup(mapper => mapper.Map<Order>(orderDto)).Throws(exception);

            // Act
            var result = _orderManager.CreateOrder(orderDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("An error occurred while creating the order.", result.Message);
            Assert.Null(result.data);
            Assert.Equal("Database error",result.Exception);  // Exception mesajını kontrol et
        }

        // Test: UpdateOrder
        [Fact]
        public void UpdateOrder_ShouldReturnSuccessResult_WhenOrderIsUpdated()
        {
            // Arrange
            var orderDto = new OrderDto { Id = 1, customerId = 1, orderDate = DateTime.Now };
            var order = new Order { id = 1, customerId = 1, orderDate = DateTime.Now };

            _mockOrderRepository.Setup(repo => repo.GetById(1)).Returns(order);  // `Id = 1` olan order'ı döndür
            _mockMapper.Setup(mapper => mapper.Map<Order>(orderDto)).Returns(order);
            _mockOrderRepository.Setup(repo => repo.Update(order)).Verifiable();

            // Act
            var result = _orderManager.UpdateOrder(orderDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Order updated successfully.", result.Message);
            _mockOrderRepository.Verify(repo => repo.Update(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public void UpdateOrder_ShouldReturnFailureResult_WhenOrderNotFound()
        {
            // Arrange
            var orderDto = new OrderDto { Id = 99, customerId = 1, orderDate = DateTime.Now };  // Non-existent order ID
            _mockOrderRepository.Setup(repo => repo.Update(It.IsAny<Order>())).Throws(new Exception("Order not found"));

            // Act
            var result = _orderManager.UpdateOrder(orderDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Order not found.", result.Message);
            Assert.Null(result.data);
        }

        [Fact]
        public void DeleteOrder_ShouldReturnSuccessResult_WhenOrderIsDeleted()
        {
            // Arrange
            var orderId = 1;
            var order = new Order { id = orderId, customerId = 1, orderDate = DateTime.Now };

            // Setup: The repository's GetById method should return the order
            _mockOrderRepository.Setup(repo => repo.GetById(orderId)).Returns(order);
            _mockOrderRepository.Setup(repo => repo.Delete(order)).Verifiable();

            // Act
            var result = _orderManager.DeleteOrder(orderId);  // We are now passing the ID

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Order deleted successfully.", result.Message);
            _mockOrderRepository.Verify(repo => repo.Delete(It.Is<Order>(o => o.id == orderId)), Times.Once);
        }

        [Fact]
        public void DeleteOrder_ShouldReturnFailureResult_WhenOrderNotFound()
        {
            // Arrange
            var orderId = 99;  // Non-existent order
            _mockOrderRepository.Setup(repo => repo.GetById(orderId)).Returns((Order)null);  // No order found

            // Act
            var result = _orderManager.DeleteOrder(orderId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Order not found.", result.Message);
            Assert.Null(result.data);
        }

        [Fact]
        public void DeleteOrder_ShouldReturnFailureResult_WhenDatabaseErrorOccurs()
        {
            // Arrange
            var orderId = 1;
            var order = new Order { id = orderId, customerId = 1, orderDate = DateTime.Now };
            var exceptionMessage = "Database error";

            _mockOrderRepository.Setup(repo => repo.GetById(orderId)).Returns(order);  // Order exists
            _mockOrderRepository.Setup(repo => repo.Delete(order)).Throws(new Exception(exceptionMessage));  // Throws exception on delete

            // Act
            var result = _orderManager.DeleteOrder(orderId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("An error occurred while deleting the order.", result.Message);
            Assert.Equal(exceptionMessage, result.Exception);
        }
    }
}
