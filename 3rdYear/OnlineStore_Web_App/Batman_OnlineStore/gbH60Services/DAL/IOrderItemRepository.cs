using gbH60Services.Model;

namespace gbH60Services.DAL
{
    public interface IOrderItemRepository
    {
        void Update(OrderItem o);

        IEnumerable<OrderItem> GetAll();

        OrderItem? Find(int? id);

        void Remove(int? id);

        void Add(OrderItem o);
    }
}
