using gbH60Services.Model;
using Microsoft.EntityFrameworkCore;

namespace gbH60Services.DAL
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly H60assignment2DbGbContext _productRepository;

        public ShoppingCartRepository(H60assignment2DbGbContext _context)
        {
            _productRepository = _context;
        }

        public void Add(ShoppingCart c)
        {
            try
            {
                _productRepository.ShoppingCarts.Add(c);
                _productRepository.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ShoppingCart? Find(int? id)
        {
            ShoppingCart? cart = _productRepository.ShoppingCarts.Find(id ?? 0);
            return cart;
        }

        public ShoppingCart FindByCustomer(int? custId)
        {
            ShoppingCart? cart = null;
            if (custId != null)
            {
                cart = _productRepository.ShoppingCarts.FirstOrDefault(x => x.CustomerId == custId.Value);
            }

            if (cart == null && custId != null)
            {
                cart = new ShoppingCart { CustomerId = custId.Value, DateCreated = DateTime.Now };
                Add(cart);
            }

            return cart;
        }

        public IEnumerable<ShoppingCart> GetAll()
        {
            IEnumerable<ShoppingCart> carts = _productRepository.ShoppingCarts.ToList();
            return carts;
        }

        public void Remove(int? id)
        {
            var cart = Find(id);
            try
            {
                if (cart == null)
                {
                    throw new KeyNotFoundException();
                }

                IEnumerable<CartItem> items = _productRepository.CartItems.Where(x => x.CartId == cart.CartId).ToList();

                foreach (var item in items)
                {
                    _productRepository.CartItems.Remove(item);
                }

                _productRepository.ShoppingCarts.Remove(cart);
                _productRepository.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(ShoppingCart s)
        {
            try
            {
                _productRepository.ShoppingCarts.Update(s);
                _productRepository.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddToCart(int? prodId, int? custId)
        {
            Product? product = _productRepository.Products.Find(prodId);

            if (product == null)
            {
                throw new KeyNotFoundException();
            }
            else if (product.Stock < 1)
            {
                throw new InvalidOperationException("Product is out of stock.");
            }

            try
            {
                ShoppingCart cart = FindByCustomer(custId);

                CartItem? item = _productRepository.CartItems.FirstOrDefault(x => x.CartId == cart.CartId && x.ProductId == product.ProductId);

                if (item == null)
                {
                    item = new CartItem() { CartId = cart.CartId, ProductId = product.ProductId, Quantity = 1, Price = (product.SellPrice ?? 0) };
                    _productRepository.CartItems.Add(item);
                }
                else
                {
                    item.Quantity += 1;
                    item.Price += (product.SellPrice ?? 0);
                    _productRepository.CartItems.Update(item);
                }
                product.Stock -= 1;
                _productRepository.Products.Update(product);

                _productRepository.SaveChanges();
            }
            catch (NullReferenceException)
            {
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemoveFromCart(int? prodId, int? custId)
        {
            Product? product = _productRepository.Products.Find(prodId);

            if (product == null)
            {
                throw new KeyNotFoundException();
            }

            try
            {
                ShoppingCart cart = FindByCustomer(custId);
                CartItem item = _productRepository.CartItems.FirstOrDefault(x => x.CartId == cart.CartId && x.ProductId == product.ProductId);

                _productRepository.CartItems.Remove(item);

                product.Stock += item.Quantity;
                _productRepository.Products.Update(product);

                _productRepository.SaveChanges();
            }
            catch (NullReferenceException)
            {
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemoveSingle(int? prodId, int? custId)
        {
            Product? product = _productRepository.Products.Find(prodId);

            if (product == null)
            {
                throw new KeyNotFoundException();
            }

            try
            {
                ShoppingCart cart = FindByCustomer(custId);
                CartItem item = _productRepository.CartItems.FirstOrDefault(x => x.CartId == cart.CartId && x.ProductId == product.ProductId);

                if (item.Quantity <= 1)
                {
                    RemoveFromCart(prodId, custId);
                    return;
                }

                item.Quantity -= 1;
                item.Price -= (product.SellPrice ?? 0);
                _productRepository.CartItems.Update(item);

                product.Stock += 1;
                _productRepository.Products.Update(product);

                _productRepository.SaveChanges();
            }
            catch (NullReferenceException)
            {
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<JoinProductCartItem> GetCartItems(int? custId)
        {
            if (custId != null && custId == 0) {
                return [];
            }
            try
            {
                ShoppingCart cart = FindByCustomer(custId);
                IEnumerable<CartItem> items = _productRepository.CartItems.Where(x => x.CartId == cart.CartId);
                List<JoinProductCartItem> cartProds = [];

                foreach (CartItem item in items)
                {
                    Product product = _productRepository.Products.Find(item.ProductId);
                    cartProds.Add(new JoinProductCartItem() { CartItem = item, Product = product });
                }

                return cartProds;
            }
            catch (NullReferenceException)
            {
                return [];
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
