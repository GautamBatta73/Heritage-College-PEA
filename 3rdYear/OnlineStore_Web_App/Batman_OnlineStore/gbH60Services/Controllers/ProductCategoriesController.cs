using gbH60Services.DAL;
using gbH60Services.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gbH60Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IProductCategoryRepository _repo;

        public ProductCategoriesController(IProductCategoryRepository repo)
        {
            _repo = repo;
        }

        // GET: api/ProductCategories
        [HttpGet]
        public IEnumerable<ProductCategory> GetProductCategories()
        {
            return _repo.GetAll();
        }

        [HttpGet("products/{id}")]
        public ActionResult<IEnumerable<Product>> GetCategoryProducts(int id)
        {
            var prods = _repo.GetProductsOfCategory(id);

            if (prods == null || !prods.Any())
            {
                return NotFound();
            }

            return Ok(prods);
        }

        [HttpGet("name/{name}")]
        public ActionResult<IEnumerable<ProductCategory>> GetCategoryByName(string name)
        {
            var prodCats = _repo.FindByName(name);

            if (prodCats == null || !prodCats.Any())
            {
                return NotFound();
            }

            return Ok(prodCats);
        }

        // GET: api/ProductCategories/5
        [HttpGet("{id}")]
        public ActionResult<ProductCategory> GetProductCategory(int id)
        {
            var productCategory = _repo.Find(id);

            if (productCategory == null)
            {
                return NotFound();
            }

            return productCategory;
        }

        // PUT: api/ProductCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutProductCategory(int id, ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                if (id != productCategory.CategoryId)
                {
                    return BadRequest();
                }

                try
                {
                    _repo.Update(productCategory);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_repo.Find(id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }

                return NoContent();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // POST: api/ProductCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<ProductCategory> PostProductCategory(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(productCategory);
                return CreatedAtAction("GetProductCategory", new { id = productCategory.CategoryId }, productCategory);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/ProductCategories/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProductCategory(int id)
        {
            var productCategory = _repo.Find(id);
            if (productCategory == null)
            {
                return NotFound();
            }

            _repo.Remove(id);
            return NoContent();
        }
    }
}
