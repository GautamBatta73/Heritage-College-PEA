namespace gbH60Customer.DAL
{
    public interface IOrderRepository
    {
        Task<string?> CheckCreditCard(string? cardNum);
        Task Checkout(int? custId);
    }
}
