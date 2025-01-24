using gbH60Customer.Model;

namespace gbH60Customer.DTO
{
    public class JoinProductCategoryDTO
    {
        public ProductCategory ProdCat { get; set; }
        public IEnumerable<ProductDTO> Products { get; set; }

        public JoinProductCategoryDTO()
        {
        }

        public JoinProductCategoryDTO(JoinProductCategory p)
        {
            ProdCat = p.ProdCat;
            Products = ProductDTO.ToList(p.Products.ToList());
        }

        public static IEnumerable<JoinProductCategoryDTO> ToList(List<JoinProductCategory> p)
        {
            List<JoinProductCategoryDTO> list = [];

            p.ForEach(x => list.Add(new JoinProductCategoryDTO(x)));

            return list;
        }
    }
}
