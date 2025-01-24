using gbH60Services.Model;

namespace gbH60Services.DAL
{
    public interface ICustomerRepository
    {
        void Update(Customer c);

        IEnumerable<Customer> GetAll();

        Customer? Find(int? id);

        Customer? FindByEmail(string email);

        void Remove(int? id);

        void Add(Customer c);
    }
}
