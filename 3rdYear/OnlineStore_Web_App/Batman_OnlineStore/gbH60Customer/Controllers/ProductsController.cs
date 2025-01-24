using gbH60Customer.DAL;
using Microsoft.AspNetCore.Mvc;

namespace gbH60Customer.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _repo;
        private readonly IProductCategoryRepository _catRepo;

        public ProductsController(IProductRepository repo, IProductCategoryRepository catRepo)
        {
            _repo = repo;
            _catRepo = catRepo;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["CustId"] = (HttpContext.Session.GetInt32("CustId") ?? 0);
            return View(await _repo.DisplayByCategory());
        }

        [HttpPost]
        public async Task<IActionResult> Index(string name)
        {
            ViewData["name"] = name ?? "";
            if (string.IsNullOrEmpty(name))
            {
                return View(await _repo.DisplayByCategory());
            }
            else
            {
                return View(await _repo.FindByName(name));
            }
        }

        public async Task<IActionResult> DetailsAsync(int? id)
        {
            var product = await _repo.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            ViewData["Category"] = (await _catRepo.Find(product.ProdCatId)).ProdCat;
            ViewData["CustId"] = (HttpContext.Session.GetInt32("CustId") ?? 0);
            return View(product);
        }
    }
}
