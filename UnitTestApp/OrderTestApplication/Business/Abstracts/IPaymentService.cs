namespace OrderTestApplication.Business.Abstracts
{
    public interface IPaymentService
    {
        bool ProcessPayment(string userId, decimal amount);
        void RefundPayment(string userId, decimal amount);
    }
}
