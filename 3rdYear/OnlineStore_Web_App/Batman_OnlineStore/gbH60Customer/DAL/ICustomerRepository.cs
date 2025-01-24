using gbH60Customer.Model;

namespace gbH60Customer.DAL
{
    public interface ICustomerRepository
    {
        Task Update(Customer c);

        Task<IEnumerable<Customer>> DisplayAll();

        Task<Customer?> Find(int? id);

        Task<Customer?> FindByEmail(string email);

        Task Remove(int? id);

        Task Add(Customer c);
    }
}
