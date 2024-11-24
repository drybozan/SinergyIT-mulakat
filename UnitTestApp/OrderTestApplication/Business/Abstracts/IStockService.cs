namespace OrderTestApplication.Business.Abstracts
{
    public interface IStockService
    {
        bool CheckStock(string productId, int quantity);
        void ReduceStock(string productId, int quantity);
    }
}
