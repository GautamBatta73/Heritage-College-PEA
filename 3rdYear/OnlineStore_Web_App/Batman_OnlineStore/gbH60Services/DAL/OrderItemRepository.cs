using gbH60Services.Model;
using Microsoft.EntityFrameworkCore;

namespace gbH60Services.DAL
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly H60assignment2DbGbContext _productRepository;

        public OrderItemRepository(H60assignment2DbGbContext _context)
        {
            _productRepository = _context;
        }

        public void Add(OrderItem o)
        {
            try
            {
                _productRepository.OrderItems.Add(o);
                _productRepository.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OrderItem? Find(int? id)
        {
            OrderItem? orderItem = _productRepository.OrderItems.Find(id ?? 0);
            return orderItem;
        }

        public IEnumerable<OrderItem> GetAll()
        {
            IEnumerable<OrderItem> orderItems = _productRepository.OrderItems.ToList();
            return orderItems;
        }

        public void Remove(int? id)
        {
            var orderItem = Find(id);
            try
            {
                if (orderItem == null)
                {
                    throw new KeyNotFoundException();
                }

                _productRepository.OrderItems.Remove(orderItem);
                _productRepository.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(OrderItem o)
        {
            try
            {
                _productRepository.OrderItems.Update(o);
                _productRepository.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
