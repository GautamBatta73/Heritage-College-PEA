using gbH60A02.Model;

namespace gbH60A02.DAL
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
