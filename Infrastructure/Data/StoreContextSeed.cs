using Core.Entities;
using Core.Entities.OrderAggregate;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

public class StoreContextSeed
{
    private static async Task SaveDataAsync<T>(string path, StoreContext context) where T : BaseEntity
    {
        var jsonData = File.ReadAllText(path);
        var data = JsonSerializer.Deserialize<List<T>>(jsonData);

        foreach (var item in data)
            context.Set<T>().Add(item);

        await context.SaveChangesAsync();
    }

    public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
    {
        try
        {
            if (!await context.ProductBrands.AnyAsync())
                await SaveDataAsync<ProductBrand>("../Infrastructure/Data/SeedData/brands.json", context);

            if (!await context.ProductTypes.AnyAsync())
                await SaveDataAsync<ProductType>("../Infrastructure/Data/SeedData/types.json", context);

            if (!await context.Products.AnyAsync())
                await SaveDataAsync<Product>("../Infrastructure/Data/SeedData/products.json", context);

            if (!await context.DeliveryMethods.AnyAsync())
                await SaveDataAsync<DeliveryMethod>("../Infrastructure/Data/SeedData/delivery.json", context);
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<StoreContextSeed>();
            logger.LogError(ex.Message);
        }
    }
}