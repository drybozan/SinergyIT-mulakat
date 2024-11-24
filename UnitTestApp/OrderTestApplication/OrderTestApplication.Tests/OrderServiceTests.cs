using OrderTestApplication.Business.Abstracts;
using OrderTestApplication.Models;
using Moq;
using Xunit;
using OrderTestApplication.Business.Concrete;
namespace OrderTestApplication.Tests;

public class OrderServiceTests 
{

    //Bir sınıfı mocklayabilmek için o sınıfın implemente ettiği interface kullanılmalıdır. 
    private Mock<IStockService> mockStockService;
    private Mock<IPaymentService> mockPaymentService;
    private OrderManager orderManager; // test edilecek ana sınıf

    public OrderServiceTests()
    {
        mockStockService = new Mock<IStockService>();
        mockPaymentService = new Mock<IPaymentService>();
        orderManager = new OrderManager(mockStockService.Object, mockPaymentService.Object); //bağımlılıklarına mock nesneleri atıyoruz.
    }

    [Fact]
    public void PlaceOrder_ValidOrder_Successful()
    {
         

        mockStockService.Setup(s => s.CheckStock("P001", 2)).Returns(true); //2 adet P001 urunu varsa true dondur
        mockPaymentService.Setup(p => p.ProcessPayment("U001", 100m)).Returns(true); //U100 kullanıcıdan 100 birim odeme true dondurur

        // test edilecek veri
        var order = new Order
        {
            ProductId = "P001",
            Quantity = 2,
            UserId = "U001",
            TotalPrice = 100m
        };

        /*         
          Act, arrange aşamasında hazırlanan nesnelerin test edilecek olan metodun çalıştırıldığı bölümdür.test aksiyonu     
         */
        var result = orderManager.PlaceOrder(order); //siparisi işle

        // Assert
        //Test sürecinde şartın başarıyla işlendiği kontrol edilir

        Assert.True(result);

        // stok azaltma işlemini yapan metodun tam bir kez mi çalıştığı kontrolu yapılır 
        mockStockService.Verify(s => s.ReduceStock("P001", 2), Times.Once);
    }


    [Fact]
    public void PlaceOrder_InsufficientStock_ThrowsException()
    {
        // Arrange
      
        mockStockService.Setup(s => s.CheckStock("P001", 2)).Returns(false);//yeterli stok yoksa false dondurur

        var order = new Order
        {
            ProductId = "P001",
            Quantity = 2,
            UserId = "U001",
            TotalPrice = 100m
        };

        // Act & Assert
        //stok yetersizliğinden hata fırlattığını kontrol eder
        Assert.Throws<InvalidOperationException>(() => orderManager.PlaceOrder(order));

       // Stok yetersiz olduğu için ödeme işleminin hiç çağrılmadığını doğrular.
        mockPaymentService.Verify(p => p.ProcessPayment(It.IsAny<string>(), It.IsAny<decimal>()), Times.Never);
    }

    [Fact]
    public void PlaceOrder_PaymentFails_ThrowsException()
    {
        
        mockStockService.Setup(s => s.CheckStock("P001", 2)).Returns(true); //stok kontrolu basarılı
        mockPaymentService.Setup(p => p.ProcessPayment("U001", 100m)).Returns(false); //odeme basarisiz

        var order = new Order
        {
            ProductId = "P001",
            Quantity = 2,
            UserId = "U001",
            TotalPrice = 100m
        };

        // Act & Assert
        //odeme basarısız oldugunda InvalidOperationException fırlat
        Assert.Throws<InvalidOperationException>(() => orderManager.PlaceOrder(order));

        //odeme basarısız old. icin stok azaltma yapılmaması gerektiğinden kontrolu sağlar
        mockStockService.Verify(s => s.ReduceStock(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
    }

    // Test: Assert.Equal/NotEqual
    [Fact]
    public void PlaceOrder_TotalPrice_ShouldBeEqual()
    {
        // Arrange
        var order = new Order { ProductId = "P001", Quantity = 1, UserId = "U001", TotalPrice = 50m };

        // Act
        var totalPrice = order.TotalPrice;

        // Assert
        Assert.Equal(50m, totalPrice); // beklenen değere eşittir.
        Assert.NotEqual(100m, totalPrice); // Beklenmeyen değere eşit değildir.
    }

    // Test: Assert.Contains/DoesNotContain
    [Fact]
    public void OrderProductIds_ShouldContainSpecificId()
    {
        // Arrange
        var productIds = new List<string> { "P001", "P002", "P003" };

        // Assert
        Assert.Contains("P002", productIds); //belirtilen id listede bulunur
        Assert.DoesNotContain("P004", productIds); //listede bulunmaz
    }

    // Test: Assert.True/False
    [Fact]
    public void StockAvailability_ShouldBeTrue()
    {
        // Arrange
        mockStockService.Setup(s => s.CheckStock("P001", 1)).Returns(true);

        // Act
        var isStockAvailable = mockStockService.Object.CheckStock("P001", 1);

        // Assert
        Assert.True(isStockAvailable); //stok mevcut
        Assert.False(!isStockAvailable); //stok mevcut durumu negatif değil
    }


    // Test: Assert.Match/DoesNotMatch
    [Fact]
    public void UserId_ShouldMatchRegex()
    {
        // Arrange
        var userId = "U12345";

        // Assert
        Assert.Matches("^U[0-9]{5}$", userId); //userId formatı regex desenine uyar 
        Assert.DoesNotMatch("^A[0-9]{5}$", userId); //yanlış bir desene uymaz 
    }

    // Test: Assert.StartsWith/EndsWith
    [Fact]
    public void ProductId_ShouldStartAndEndWithSpecificChars()
    {
        // Arrange
        var productId = "P001";

        // Assert
        Assert.StartsWith("P", productId);
        Assert.EndsWith("1", productId);
    }

    // Test: Assert.Empty/NotEmpty
    [Fact]
    public void ProductList_ShouldNotBeEmpty()
    {
        // Arrange
        var products = new List<string> { "P001", "P002" };

        // Assert
        Assert.NotEmpty(products);
    }

    // Test: Assert.InRange/NotInRange
    [Fact]
    public void Quantity_ShouldBeWithinRange()
    {
        // Arrange
        var quantity = 5;

        // Assert
        Assert.InRange(quantity, 1, 10);
        Assert.NotInRange(quantity, 11, 20);
    }

    // Test: Assert.Single
    [Fact]
    public void SingleOrder_ShouldExist()
    {
        // Arrange
        var orders = new List<Order> { new Order { ProductId = "P001", Quantity = 1, UserId = "U001", TotalPrice = 50m } };

        // Assert
        Assert.Single(orders);
    }

    // Test: Assert.IsType/IsNotType
    [Fact]
    public void Order_ShouldBeCorrectType()
    {
        // Arrange
        var order = new Order { ProductId = "P001", Quantity = 1, UserId = "U001", TotalPrice = 50m };

        // Assert
        Assert.IsType<Order>(order);
        Assert.IsNotType<string>(order);
    }

    // Test: Assert.Null/NotNull
    [Fact]
    public void Order_ShouldNotBeNull()
    {
        // Arrange
        var order = new Order { ProductId = "P001", Quantity = 1, UserId = "U001", TotalPrice = 50m };

        // Assert
        Assert.NotNull(order);
    }

    [Theory]
    [InlineData("P001", 1, true)]   // Minimum geçerli miktar
    [InlineData("P002", 5, true)]   // Orta geçerli miktar
    [InlineData("P003", 0, false)]  // Geçersiz miktar: sıfır
    [InlineData("P004", -3, false)] // Geçersiz miktar: negatif değer
    [InlineData("P005", 101, false)] // Geçersiz miktar: maksimumun üstünde
    public void ValidateOrderQuantity(string productId, int quantity, bool expectedResult)
    {
        // Arrange
        const int minQuantity = 1;
        const int maxQuantity = 100;

        // Act
        var isValid = quantity >= minQuantity && quantity <= maxQuantity;

        // Assert
        Assert.Equal(expectedResult, isValid);
    }



    // Test: MemberData 
    [Theory]
    [MemberData(nameof(GetOrderData))] //farklı sipariş dataları GetOrderData metodundan alınır ve kullanılır
    public void PlaceOrder_MemberDataTests(string productId, int quantity, decimal totalPrice, bool expectedResult)
    {
        // Arrange
        mockStockService.Setup(s => s.CheckStock(productId, quantity)).Returns(expectedResult);

        var order = new Order
        {
            ProductId = productId,
            Quantity = quantity,
            UserId = "U001",
            TotalPrice = totalPrice
        };

        // Act & Assert
        if (expectedResult) //beklenen sonuc dogrulanır
            Assert.True(mockStockService.Object.CheckStock(productId, quantity));
        else
            Assert.False(mockStockService.Object.CheckStock(productId, quantity));
    }

    public static IEnumerable<object[]> GetOrderData()
    {
        yield return new object[] { "P001", 1, 50m, true };
        yield return new object[] { "P002", 2, 100m, false };
    }

    // Test: ClassData 
    [Theory]
    [ClassData(typeof(OrderTestData))] //daha kompleks test verileri gelir
    public void PlaceOrder_ClassDataTests(Order order, bool expectedResult)
    {
        // Arrange
        mockStockService.Setup(s => s.CheckStock(order.ProductId, order.Quantity)).Returns(expectedResult);

        // Act & Assert
        if (expectedResult)
            Assert.True(mockStockService.Object.CheckStock(order.ProductId, order.Quantity));
        else
            Assert.False(mockStockService.Object.CheckStock(order.ProductId, order.Quantity));
    }


    public class OrderTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new Order { ProductId = "P001", Quantity = 1, UserId = "U001", TotalPrice = 50m }, true };
            yield return new object[] { new Order { ProductId = "P002", Quantity = 2, UserId = "U002", TotalPrice = 100m }, false };
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }

}
