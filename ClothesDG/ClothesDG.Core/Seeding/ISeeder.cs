using ClothesDG.Data.Data;

namespace ClothesDG.Core.Seeding
{
    public interface ISeeder
    {
        Task SeedAsync(ClothesDGContext dbContext, IServiceProvider serviceProvider);
    }
}
