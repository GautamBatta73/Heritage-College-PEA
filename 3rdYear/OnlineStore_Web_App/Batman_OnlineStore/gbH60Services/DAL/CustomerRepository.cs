using gbH60Services.Model;
using Microsoft.EntityFrameworkCore;

namespace gbH60Services.DAL
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly H60assignment2DbGbContext _productRepository;
        private readonly IShoppingCartRepository _shopRepo;

        public CustomerRepository(H60assignment2DbGbContext _context, IShoppingCartRepository shopRepo)
        {
            _productRepository = _context;
            _shopRepo = shopRepo;
        }

        public void Add(Customer c)
        {
            try
            {
                _productRepository.Customers.Add(c);
                _productRepository.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Customer? Find(int? id)
        {
            Customer? cust = _productRepository.Customers.Find(id ?? 0);
            return cust;
        }

        public Customer? FindByEmail(string email)
        {
            Customer? custs = _productRepository.Customers
                .FirstOrDefault(x =>
                    x.Email.Equals(email)
                    );

            return custs;
        }

        public IEnumerable<Customer> GetAll()
        {
            IEnumerable<Customer> custs = _productRepository.Customers.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            return custs;
        }

        public void Remove(int? id)
        {
            var cust = Find(id);
            try
            {
                if (cust == null)
                {
                    throw new KeyNotFoundException();
                }


                var shopCart = _shopRepo.FindByCustomer(id);
                _shopRepo.Remove(shopCart.CartId);

                if (cust.Orders.Count != 0)
                {
                    throw new DbUpdateConcurrencyException();
                }

                _productRepository.Customers.Remove(cust);
                _productRepository.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(Customer c)
        {
            try
            {
                _productRepository.Customers.Update(c);
                _productRepository.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
