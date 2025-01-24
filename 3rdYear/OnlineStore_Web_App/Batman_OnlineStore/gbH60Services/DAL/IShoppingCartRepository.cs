using gbH60Services.Model;

namespace gbH60Services.DAL
{
    public interface IShoppingCartRepository
    {
        void Update(ShoppingCart s);

        IEnumerable<ShoppingCart> GetAll();

        ShoppingCart? Find(int? id);

        ShoppingCart FindByCustomer(int? custId);

        void Remove(int? id);

        void Add(ShoppingCart c);

        void AddToCart(int? prodId, int? custId);

        void RemoveFromCart(int? prodId, int? custId);

        void RemoveSingle(int? prodId, int? custId);

        IEnumerable<JoinProductCartItem> GetCartItems(int? custId);
    }
}
