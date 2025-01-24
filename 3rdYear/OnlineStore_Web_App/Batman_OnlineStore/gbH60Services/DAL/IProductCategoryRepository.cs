using gbH60Services.Model;

namespace gbH60Services.DAL
{
    public interface IProductCategoryRepository
    {
        void Update(ProductCategory p);

        IEnumerable<ProductCategory> GetAll();

        IEnumerable<Product> GetProductsOfCategory(int? id);

        ProductCategory? Find(int? id);

        IEnumerable<ProductCategory> FindByName(string name);

        void Remove(int? id);

        void Add(ProductCategory p);
    }
}
