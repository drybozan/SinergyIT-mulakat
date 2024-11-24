using CRUDApplication.Data.Repositories.Abstracts;
using CRUDApplication.Entities;
using Moq;

namespace CRUDApplication.CRUDApplication.Tests.CRUDApplication.Tests.Mocks
{
    public class MockOrderRepository
    {
        public static Mock<IOrderRepository> GetMockOrderRepository()
        {
            var mockRepo = new Mock<IOrderRepository>();

            // Setup methods of the repository (e.g., GetAll, GetById)
            mockRepo.Setup(repo => repo.GetAll()).Returns(new List<Order>
            {
                new Order { id = 1, productName = "Product 1" },
                new Order { id = 2, productName = "Product 2" }
            });

            mockRepo.Setup(repo => repo.GetById(1)).Returns(new Order { id = 1, productName = "Product 1" });
            mockRepo.Setup(repo => repo.GetById(2)).Returns(new Order { id = 2, productName = "Product 2" });

            return mockRepo;
        }
    }
}
