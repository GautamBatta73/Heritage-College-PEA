using gbH60A02.Model;

namespace gbH60A02.DAL
{
    public interface IProductCategoryRepository
    {
        Task Update(ProductCategory p);

        Task<IEnumerable<ProductCategory>> DisplayAll();

        Task<IEnumerable<Product>> DisplayProductsOfCategory(int? id);

        Task<ProductCategory?> Find(int? id);

        Task<IEnumerable<ProductCategory>> FindByName(string name);

        Task Remove(int? id);

        Task Add(ProductCategory p);
    }
}
