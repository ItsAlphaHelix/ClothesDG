namespace ClothesDG.Extensions
{
    using AspNetCoreHero.ToastNotification;
    using ClothesDG.Core.Contracts;
    using ClothesDG.Core.Services;
    using ClothesDG.Core.WebScrapper;
    using ClothesDG.Data.Repositories;

    public static class ClothingStoreServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<IFavoritesService, FavoritesService>();
            services.AddScoped<IBagsService, BagsService>();
            services.AddScoped<IOrdersService, OrdersService>();
            services.AddScoped<IPaymentsService, PaymentsService>();
            services.AddScoped<ICustomersService, CustomersService>();
            services.AddScoped<IScrape, Scrape>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddNotyf(configuration =>
            {
                configuration.DurationInSeconds = 5;
                configuration.IsDismissable = true;
                configuration.Position = NotyfPosition.TopRight;
            });
            
            return services;
        }
    }
}
