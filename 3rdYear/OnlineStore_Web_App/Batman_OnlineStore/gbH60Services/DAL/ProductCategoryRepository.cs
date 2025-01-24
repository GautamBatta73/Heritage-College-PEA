using gbH60Services.Model;

namespace gbH60Services.DAL
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly H60assignment2DbGbContext _productRepository;

        public ProductCategoryRepository(H60assignment2DbGbContext _context)
        {
            _productRepository = _context;
        }

        public void Add(ProductCategory p)
        {
            try
            {
                _productRepository.ProductCategories.Add(p);
                _productRepository.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ProductCategory> GetAll()
        {
            IEnumerable<ProductCategory> prodCats = _productRepository.ProductCategories.OrderBy(x => x.ProdCat).ToList();
            return prodCats;
        }

        public IEnumerable<Product> GetProductsOfCategory(int? id)
        {
            IEnumerable<Product> prods = _productRepository.Products.Where(x => x.ProdCatId == id.Value).OrderBy(x => x.Name).ToList();
            return prods;
        }

        public ProductCategory? Find(int? id)
        {
            ProductCategory? prodCat = _productRepository.ProductCategories.Find(id ?? 0); //maybe change to .FirstOrDefault(x => x.CategoryId == id);
            return prodCat;
        }

        public IEnumerable<ProductCategory> FindByName(string name)
        {
            IEnumerable<ProductCategory> prodCats = _productRepository.ProductCategories
                .Where(x => x.ProdCat.Contains(name));
            return prodCats;
        }

        public void Remove(int? id)
        {
            var prodCat = Find(id);
            try
            {
                if (prodCat == null)
                {
                    throw new KeyNotFoundException();
                }

                _productRepository.ProductCategories.Remove(prodCat);
                _productRepository.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(ProductCategory p)
        {
            try
            {
                _productRepository.ProductCategories.Update(p);
                _productRepository.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
