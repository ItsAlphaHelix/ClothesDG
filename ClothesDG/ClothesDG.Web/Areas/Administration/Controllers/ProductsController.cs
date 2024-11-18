using ClothesDG.Core.ViewModels.Products;
using ClothesDG.Core.WebScrapper;
using ClothesDG.Data.Data.Models;
using ClothesDG.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ClothesDG.Areas.Administration.Controllers
{
    public class ProductsController : AdministrationController
    {
        private readonly IScrape scrape;
        private readonly IRepository<Product> productsRepository;
        public ProductsController(IScrape scrape, IRepository<Product> productsRepository)
        {
            this.scrape = scrape;
            this.productsRepository = productsRepository;
        }

        [HttpGet]
        public IActionResult Add()
        {
            this.ViewData["IsHomePage"] = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductInputViewModel model)
        {
            this.ViewData["IsHomePage"] = false;

            var product = await this.productsRepository.AllAsNoTracking()
                .Where(x => x.LCProductId == model.ProductId && x.LCProductColorId == model.ProductColorId)
                .FirstOrDefaultAsync();

            if (product != null)
            {
                return BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.scrape.ScrapeProduct(model);

            return RedirectToAction(nameof(Add));
        }
    }
}
