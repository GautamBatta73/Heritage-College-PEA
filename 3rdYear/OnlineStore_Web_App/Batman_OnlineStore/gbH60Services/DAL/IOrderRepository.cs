using gbH60Services.Model;

namespace gbH60Services.DAL
{
    public interface IOrderRepository
    {
        void Update(Order o);

        IEnumerable<Order> GetAll();

        IEnumerable<Order> GetByDateFulfilled(DateTime? date);

        IEnumerable<Order> GetByCustomer(int? custId);

        Order? Find(int? id);

        void Add(Order o);

        void Checkout(int? custId);
    }
}
