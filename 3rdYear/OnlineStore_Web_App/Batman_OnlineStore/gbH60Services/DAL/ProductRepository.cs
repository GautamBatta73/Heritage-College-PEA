using gbH60Services.Model;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Linq;

namespace gbH60Services.DAL
{
    public class ProductRepository : IProductRepository
    {
        private readonly H60assignment2DbGbContext _productRepository;

        public ProductRepository(H60assignment2DbGbContext _context)
        {
            _productRepository = _context;
        }

        public void Add(Product p)
        {
            try
            {
                _productRepository.Products.Add(p);
                _productRepository.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Product> GetAll()
        {
            IEnumerable<Product> prods = _productRepository.Products.OrderBy(x => x.Name).ToList();
            return prods;
        }

        public IEnumerable<JoinProductCategory> GetByCategory()
        {
            IEnumerable<JoinProductCategory> groupedProducts = GetAll().GroupBy(p => new { p.ProdCatId })
                  .Select(group => new JoinProductCategory
                  {
                      ProdCat = _productRepository.ProductCategories.Find(group.Key.ProdCatId),
                      Products = group.OrderBy(x => x.Name).ToList()
                  }).OrderBy(x => x.ProdCat.ProdCat).ToList();

            return groupedProducts;
        }

        public Product? Find(int? id)
        {
            Product? prod = _productRepository.Products.Find(id ?? 0);
            return prod;
        }

        public IEnumerable<Product> FindByName(string name)
        {
            IEnumerable<Product> prods = _productRepository.Products
                .Where(x =>
                    x.Name.Contains(name) ||
                    (x.Description ?? "").Contains(name)
            );
            return prods;
        }

        public void Remove(int? id)
        {
            var prod = Find(id);
            try
            {
                if (prod == null)
                {
                    throw new KeyNotFoundException();
                }

                _productRepository.Products.Remove(prod);
                _productRepository.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(Product p)
        {
            try
            {
                _productRepository.Products.Update(p);
                _productRepository.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
