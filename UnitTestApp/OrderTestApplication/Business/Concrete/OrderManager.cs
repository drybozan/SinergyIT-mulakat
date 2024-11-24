using OrderTestApplication.Business.Abstracts;
using OrderTestApplication.Models;

namespace OrderTestApplication.Business.Concrete
{
    public class OrderManager : IOrderService
    {
        private readonly IStockService _stockService;
        private readonly IPaymentService _paymentService;

        public OrderManager(IStockService stockService, IPaymentService paymentService)
        {
            _stockService = stockService;
            _paymentService = paymentService;
        }


        public bool PlaceOrder(Order order)
        {
            if (!_stockService.CheckStock(order.ProductId, order.Quantity))
                throw new InvalidOperationException("Insufficient stock!");

            if (!_paymentService.ProcessPayment(order.UserId, order.TotalPrice))
                throw new InvalidOperationException("Payment failed!");

            try
            {
                _stockService.ReduceStock(order.ProductId, order.Quantity);
                return true;
            }
            catch
            {
                _paymentService.RefundPayment(order.UserId, order.TotalPrice);
                throw;
            }
        
    }
    }
}
