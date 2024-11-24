using CRUDApplication.Data.Repositories.Abstracts;
using CRUDApplication.Entities;
using Moq;

namespace CRUDApplication.CRUDApplication.Tests.CRUDApplication.Tests.Mocks
{
    public class MockCustomerRepository
    {
        public static Mock<ICustomerRepository> GetMockCustomerRepository()
        {
            var mockRepo = new Mock<ICustomerRepository>();

            // Setup methods of the repository (e.g., GetAll, GetById)
            mockRepo.Setup(repo => repo.GetAll()).Returns(new List<Customer>
            {
                new Customer { id = 1, name = "John Doe" },
                new Customer { id = 2, name = "Jane Smith" }
            });

            mockRepo.Setup(repo => repo.GetById(1)).Returns(new Customer { id = 1, name = "John Doe" });
            mockRepo.Setup(repo => repo.GetById(2)).Returns(new Customer { id = 2, name = "Jane Smith" });

            return mockRepo;
        }
    
}
}
