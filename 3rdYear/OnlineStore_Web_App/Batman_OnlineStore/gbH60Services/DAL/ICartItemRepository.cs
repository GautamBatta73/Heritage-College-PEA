using gbH60Services.Model;

namespace gbH60Services.DAL
{
    public interface ICartItemRepository
    {
        void Update(CartItem c);

        IEnumerable<CartItem> GetAll();

        CartItem? Find(int? id);

        void Remove(int? id);

        void Add(CartItem c);
    }
}
