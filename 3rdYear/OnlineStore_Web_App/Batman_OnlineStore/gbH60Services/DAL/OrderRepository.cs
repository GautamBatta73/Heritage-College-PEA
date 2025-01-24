using gbH60Services.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ServiceReference1;
using System.ComponentModel.Design;
using System.Drawing.Text;
using System.ServiceModel.Channels;
using System.ServiceModel;

namespace gbH60Services.DAL
{
    public class OrderRepository : IOrderRepository
    {
        private readonly H60assignment2DbGbContext _productRepository;
        private readonly IShoppingCartRepository _shopRepo;

        public OrderRepository(H60assignment2DbGbContext _context, IShoppingCartRepository shopRepo)
        {
            _productRepository = _context;
            _shopRepo = shopRepo;
        }

        public void Add(Order o)
        {
            try
            {
                _productRepository.Orders.Add(o);
                _productRepository.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Order? Find(int? id)
        {
            Order? order = _productRepository.Orders.Find(id ?? 0);
            return order;
        }

        public IEnumerable<Order> GetAll()
        {
            IEnumerable<Order> orders = _productRepository.Orders.ToList();
            return orders;
        }

        public void Update(Order o)
        {
            try
            {
                _productRepository.Orders.Update(o);
                _productRepository.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Order> GetByCustomer(int? custId)
        {
            List<Order> orders = [];
            if (custId != null)
            {
                orders = _productRepository.Orders.Where(x => x.CustomerId == custId.Value).ToList();
            }

            return orders;
        }

        public IEnumerable<Order> GetByDateFulfilled(DateTime? date)
        {
            List<Order> orders = _productRepository.Orders.Where(x => x.DateFulfilled == date).ToList();

            return orders;
        }

        public void Checkout(int? custId)
        {
            Customer? cust = _productRepository.Customers.Find(custId);
            if (cust == null)
            {
                throw new KeyNotFoundException();
            }

            try
            {
                ShoppingCart cart = _shopRepo.FindByCustomer(custId);
                Add(new() { CustomerId = custId, DateCreated = DateTime.Now, DateFulfilled = DateTime.Today.AddDays(5), Total = 0, Taxes = 0 });
                Order order = _productRepository.Orders.Where(x => x.CustomerId == custId && x.Total == 0 && x.Taxes == 0).OrderBy(x => x.DateCreated).FirstOrDefault();
                List<OrderItem> orders = _shopRepo.GetCartItems(custId)
                    .Select(x => x.CartItem)
                    .Select(x => new OrderItem()
                    {
                        OrderId = order.OrderId,
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        Price = x.Price
                    }).ToList();

                orders.ForEach(x => _productRepository.OrderItems.Add(x));
                var subTotal = orders.Sum(x => x.Price);
                order.Taxes = subTotal * 0.13m;
                order.Total = subTotal + order.Taxes;

                _productRepository.Orders.Update(order);
                _shopRepo.Remove(cart.CartId);

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

        
    }
}
