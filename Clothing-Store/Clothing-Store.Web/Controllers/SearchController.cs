using Clothing_Store.Core.Contracts;
using Clothing_Store.Core.ViewModels.Products;
using Clothing_Store.Core.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Clothing_Store.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchService searchService;
        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }
        public async Task<IActionResult> Search(
            [FromQuery]PaginatedViewModel<ProductViewModel> model,
            string searchTerm,
            int page = 1)
        {
            ViewData["IsHomePage"] = false;
            ViewData["CurrentSearchWord"] = searchTerm;
            ViewData["CurrentPage"] = page;
            ViewData["CurrentSort"] = model.Sorting;
            ViewData["CurrentSelectedProducts"] = model.SelectedProducts;
            ViewData["CurrentSelectedSizes"] = model.SelectedSizes;
            ViewData["CurrentSelectedPrice"] = model.SelectedPrice;

            var products = this.searchService.SearchProductsByQueryAsQueryable(model, searchTerm);

            if (products == null && !products.Any())
            {
                return NotFound();
            }

            var paginated = await PaginatedList<ProductViewModel>.CreateAsync(products, page, 12);

            var viewModel = new PaginatedViewModel<ProductViewModel>
            {
                Models = paginated,
                MinPrice = model.MinPrice ?? 0,
                MaxPrice = model.MaxPrice ?? Math.Round(products.Max(x => x.Price))
            };

            return View(viewModel);
        }
    }
}
