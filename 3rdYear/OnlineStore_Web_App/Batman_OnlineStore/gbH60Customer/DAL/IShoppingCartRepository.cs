using gbH60Customer.DTO;

namespace gbH60Customer.DAL
{
    public interface IShoppingCartRepository
    {
        Task AddToCart(int? prodId, int? custId);

        Task RemoveFromCart(int? prodId, int? custId);

        Task<IEnumerable<JoinProductCartItemDTO>> GetCartItems(int? custId);

        Task RemoveSingle(int? prodId, int? custId);
    }
}
