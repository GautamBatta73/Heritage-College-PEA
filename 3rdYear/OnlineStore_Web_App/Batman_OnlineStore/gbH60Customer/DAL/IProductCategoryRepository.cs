using gbH60Customer.DTO;
using gbH60Customer.Model;

namespace gbH60Customer.DAL
{
    public interface IProductCategoryRepository
    {

        Task<IEnumerable<ProductCategory>> DisplayAll();

        Task<IEnumerable<ProductDTO>> DisplayProductsOfCategory(int? id);

        Task<ProductCategory?> Find(int? id);

        Task<IEnumerable<ProductCategory>> FindByName(string name);
    }
}
