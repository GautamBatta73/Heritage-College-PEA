using gbH60A02.Model;

namespace gbH60A02.DAL
{
    public interface IProductRepository
    {
        Task Update(Product p);

        Task<IEnumerable<Product>> DisplayAll();

        Task<Product?> Find(int? id);

        Task<IEnumerable<Product>> FindByName(string name);

        Task<IEnumerable<JoinProductCategory>> DisplayByCategory();

        Task Remove(int? id);

        Task Add(Product p);
    }
}
