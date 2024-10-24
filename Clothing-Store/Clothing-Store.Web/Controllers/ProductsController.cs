namespace Clothing_Store.Controllers
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.ViewModels.Products;
    using Clothing_Store.Core.ViewModels.Reviews;
    using Clothing_Store.Core.ViewModels.Shared;
    using Clothing_Store.Data.Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService productsService;
        private readonly UserManager<ApplicationUser> usersManager;
        public ProductsController(
            IProductsService productsService,
            UserManager<ApplicationUser> usersManager)
            : base(usersManager, null)
        {
            this.productsService = productsService;
            this.usersManager = usersManager;
        }

        public async Task<IActionResult> All([FromQuery] PaginatedViewModel<ProductViewModel> model, int page = 1)
        {
            ViewData["IsHomePage"] = false;
            ViewData["CurrentPage"] = page;
            ViewData["CurrentSort"] = model.Sorting;
            ViewData["CurrentSelectedProducts"] = model.SelectedProducts;
            ViewData["CurrentSelectedSizes"] = model.SelectedSizes;
            ViewData["CurrentSelectedPrice"] = model.SelectedPrice;

            IQueryable<ProductViewModel> products;

            if (model.SelectedSizes == null && model.MaxPrice == null && model.MaxPrice == null)
            {
                 products = this.productsService.GetAllProductsAsQueryable(model, null, null);
            }
            else
            {
                products = this.productsService.FilterProductsAsQueryable(model);
            }

            var paginated = await PaginatedList<ProductViewModel>.CreateAsync(products, page, 12);

            var viewModel = new PaginatedViewModel<ProductViewModel>()
            {
                Models = paginated,
                MinPrice = model.MinPrice ?? 0,
                MaxPrice = model.MaxPrice ?? 100
            };

            try
            {
                if (!paginated.Any())
                {
                    throw new InvalidOperationException("Няма намерени продукти");
                }
            }
            catch (Exception ex)
            {
                ViewData["Title"] = ex.Message;

                return View(viewModel);
            }

            return View(viewModel);
        }

        public async Task<IActionResult> AllMenProducts([FromQuery] PaginatedViewModel<ProductViewModel> model, string productName, int page = 1)
        {
            ViewData["IsHomePage"] = false;
            ViewData["CurrentPage"] = page;
            ViewData["CurrentSort"] = model.Sorting;
            ViewData["CurrentSelectedProducts"] = productName;
            ViewData["CurrentSelectedSizes"] = model.SelectedSizes;
            ViewData["CurrentSelectedPrice"] = model.SelectedPrice;

            IQueryable<ProductViewModel> products;

            if (model.SelectedSizes == null && model.MaxPrice == null && model.MaxPrice == null)
            {
                products = this.productsService.GetAllProductsAsQueryable(model, null, null);
            }
            else
            {
                products = this.productsService.FilterProductsAsQueryable(model);
            }

            var paginated = await PaginatedList<ProductViewModel>.CreateAsync(products, page, 12);

            var viewModel = new PaginatedViewModel<ProductViewModel>()
            {
                Models = paginated,
                MinPrice = model.MinPrice ?? 0,
                MaxPrice = model.MaxPrice ?? 100
                //Math.Round(products.Max(x => x.Price))
            };

            return View(viewModel);
        }

        public async Task<IActionResult> AllWomenProducts([FromQuery] PaginatedViewModel<ProductViewModel> model, string productName, int page = 1)
        {
            ViewData["IsHomePage"] = false;
            ViewData["CurrentPage"] = page;
            ViewData["CurrentSort"] = model.Sorting;
            ViewData["CurrentSelectedProducts"] = productName;
            ViewData["CurrentSelectedSizes"] = model.SelectedSizes;
            ViewData["CurrentSelectedPrice"] = model.SelectedPrice;

            IQueryable<ProductViewModel> products;

            if (model.SelectedSizes == null && model.MaxPrice == null && model.MaxPrice == null)
            {
                products = this.productsService.GetAllProductsAsQueryable(model, null, null);
            }
            else
            {
                products = this.productsService.FilterProductsAsQueryable(model);
            }

            var paginated = await PaginatedList<ProductViewModel>.CreateAsync(products, page, 12);

            var viewModel = new PaginatedViewModel<ProductViewModel>()
            {
                Models = paginated,
                MinPrice = model.MinPrice ?? 0,
                MaxPrice = model.MaxPrice ?? 100
                //Math.Round(products.Max(x => x.Price), MidpointRounding.AwayFromZero)
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetSmallDetails(int productId)
        {
            var product = await this.productsService.GetProductByIdAsync(productId);

            return PartialView("_ProductModalPartial", product);
        }
        public async Task<IActionResult> ProductDetails(int id, int pageNumber, int pageSize = 3)
        {
            ViewData["IsHomePage"] = false;

            if (this.User.Identity.IsAuthenticated)
            {
                var user = await GetUserAsync();

                ViewBag.UserFullName = $"{user.FirstName} {user.LastName}";
                ViewBag.UserProfileImageUrl = user.ProfileImageUrl;
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var product = await this.productsService.GetProductDetailsByIdAsync(id, pageNumber, pageSize);
                return PartialView("_ProductReviewsPartial", product);
            }
            else
            {

                var product = await this.productsService.GetProductDetailsByIdAsync(id, 1, pageSize);
                var recommendedProducts = await this.productsService.GetRecommendedProductsAsync(id);

                product.RecommendedProducts = recommendedProducts;

                return View(product);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostProductReview(PostProductReviewViewModel productReview)
        {
            var user = await GetUserAsync();
            
            if (!ModelState.IsValid)
            {

                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            await this.productsService.PostProductReviewAsync(productReview, user.Id);
            return Json(productReview);
        }

        public IActionResult IsUserLogin()
        {
            bool isLoggedIn = User.Identity.IsAuthenticated;

            var result = new { IsLoggedIn = isLoggedIn };

            return Ok(result);
        }

        public async Task<IActionResult> Search(
            [FromQuery] PaginatedViewModel<ProductViewModel> model,
            string searchBy,
            int page = 1)
        {
            ViewData["IsHomePage"] = false;
            ViewData["CurrentSearchWord"] = searchBy;
            ViewData["CurrentPage"] = page;
            ViewData["CurrentSort"] = model.Sorting;
            ViewData["CurrentSelectedProducts"] = model.SelectedProducts;
            ViewData["CurrentSelectedSizes"] = model.SelectedSizes;
            ViewData["CurrentSelectedPrice"] = model.SelectedPrice;

            var products = this.productsService.SearchProductsByQueryAsQueryable(model, searchBy);

            if (products == null && !products.Any())
            {
                return NotFound();
            }
            products = this.productsService.FilterProductsAsQueryable(model);
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
