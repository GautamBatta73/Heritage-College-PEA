using gbH60Customer.DTO;
using gbH60Customer.Model;

namespace gbH60Customer.DAL
{
    public interface IProductRepository
    {

        Task<IEnumerable<ProductDTO>> DisplayAll();

        Task<ProductDTO?> Find(int? id);

        Task<IEnumerable<JoinProductCategoryDTO>> FindByName(string name);

        Task<IEnumerable<JoinProductCategoryDTO>> DisplayByCategory();
    }
}
