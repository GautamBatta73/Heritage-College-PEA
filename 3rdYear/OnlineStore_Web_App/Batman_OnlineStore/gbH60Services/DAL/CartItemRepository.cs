using gbH60Services.Model;
using Microsoft.EntityFrameworkCore;

namespace gbH60Services.DAL
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly H60assignment2DbGbContext _productRepository;

        public CartItemRepository(H60assignment2DbGbContext _context)
        {
            _productRepository = _context;
        }

        public void Add(CartItem c)
        {
            try
            {
                _productRepository.CartItems.Add(c);
                _productRepository.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CartItem? Find(int? id)
        {
            CartItem? cartItem = _productRepository.CartItems.Find(id ?? 0);
            return cartItem;
        }

        public IEnumerable<CartItem> GetAll()
        {
            IEnumerable<CartItem> cartItems = _productRepository.CartItems.ToList();
            return cartItems;
        }

        public void Remove(int? id)
        {
            var cartItem = Find(id);
            try
            {
                if (cartItem == null)
                {
                    throw new KeyNotFoundException();
                }

                _productRepository.CartItems.Remove(cartItem);
                _productRepository.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(CartItem c)
        {
            try
            {
                _productRepository.CartItems.Update(c);
                _productRepository.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
