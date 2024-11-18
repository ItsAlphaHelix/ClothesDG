using ClothesDG.Core.Seeding;
using ClothesDG.Data.Data;

namespace ClothesDG.Middlewares
{
    public static class SeedRoleMiddlewareExtension
    {
        public static WebApplication Seed(
        this WebApplication app)
        {
            var serviceScope = app.Services.CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<ClothesDGContext>();
            new ClothesDGContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();

            return app;
        }
    }
}
