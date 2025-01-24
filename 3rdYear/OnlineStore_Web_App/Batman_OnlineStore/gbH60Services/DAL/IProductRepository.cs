using gbH60Services.Model;

namespace gbH60Services.DAL
{
    public interface IProductRepository
    {
        void Update(Product p);

        IEnumerable<Product> GetAll();

        Product? Find(int? id);

        IEnumerable<Product> FindByName(string name);

        IEnumerable<JoinProductCategory> GetByCategory();

        void Remove(int? id);

        void Add(Product p);
    }
}
