using gbH60A02.DAL;
using gbH60A02.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace gbH60A02.Controllers
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

        [Authorize(Roles = "Manager,Clerk")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _repo.DisplayByCategory());
        }

        [Authorize(Roles = "Manager,Clerk")]
        [HttpPost]
        public async Task<IActionResult> Index(Product product)
        {
            var validatedProduct = await _repo.Find(product.ProductId);

            if (ModelState.IsValid && validatedProduct != null)
            {
                validatedProduct.SellPrice = product.SellPrice;
                validatedProduct.Stock = product.Stock;
                await _repo.Update(validatedProduct);
            }

            return View(await _repo.DisplayByCategory());
        }

        [Authorize(Roles = "Manager,Clerk")]
        public async Task<IActionResult> DetailsAsync(int? id)
        {
            var product = await _repo.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            ViewData["Category"] = (await _catRepo.Find(product.ProdCatId)).ProdCat;
            return View(product);
        }

        [Authorize(Roles = "Manager,Clerk")]
        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {
            ViewData["ProdCatId"] = new SelectList((await _catRepo.DisplayAll()), "CategoryId", "ProdCat");
            return View();
        }

        [Authorize(Roles = "Manager,Clerk")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync([Bind("ProductId,ProdCatId,Name,ImageURL,Description,Manufacturer,Stock,BuyPrice,SellPrice")] Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _repo.Add(product);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    return View(product);
                }

            }
            ViewData["ProdCatId"] = new SelectList((await _catRepo.DisplayAll()), "CategoryId", "ProdCat");
            return View(product);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> EditAsync(int? id)
        {
            var product = await _repo.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            ViewData["ProdCatId"] = new SelectList((await _catRepo.DisplayAll()), "CategoryId", "ProdCat");
            return View(product);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int id, [Bind("ProductId,ProdCatId,Name,ImageURL,Description,Manufacturer,Stock,BuyPrice,SellPrice")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repo.Update(product);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    return View(product);
                }
            }

            ViewData["ProdCatId"] = new SelectList((await _catRepo.DisplayAll()), "CategoryId", "ProdCat");
            return View(product);
        }

        [Authorize(Roles = "Manager,Clerk")]
        [HttpGet]
        public async Task<IActionResult> DeleteAsync(int? id)
        {
            ViewData["FKError"] = false;

            var product = await _repo.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            ViewData["Category"] = (await _catRepo.Find(product.ProdCatId)).ProdCat;
            return View(product);
        }

        [Authorize(Roles = "Manager,Clerk")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedAsync(int id)
        {
            var product = await _repo.Find(id);
            try
            {
                await _repo.Remove(id);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ViewData["FKError"] = true;
            }
            catch (Exception)
            {
                return View(product);
            }

            return View(product);
        }
    }
}
