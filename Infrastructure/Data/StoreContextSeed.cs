using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        private static async Task SaveDataAsync<T>(string path, StoreContext context) where T : BaseEntity
        {
            var jsonData = File.ReadAllText(path);
            var data = JsonSerializer.Deserialize<List<T>>(jsonData);

            foreach (var item in data)
            {
                context.Set<T>().Add(item);
            }

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
            }
			catch (Exception ex)
			{
				var logger = loggerFactory.CreateLogger<StoreContextSeed>();
				logger.LogError(ex.Message);
			}
		}

   //     public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
   //     {
			//try
			//{
			//	if(!context.ProductBrands.Any())
			//	{
			//		var brandsData =
			//			File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
			//		var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

			//		foreach (var item in brands)
			//		{
			//			context.ProductBrands.Add(item);
			//		}

			//		await context.SaveChangesAsync();
			//	}

   //             if (!context.ProductTypes.Any())
   //             {
   //                 var typesData =
   //                     File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
   //                 var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

   //                 foreach (var item in types)
   //                 {
   //                     context.ProductTypes.Add(item);
   //                 }

   //                 await context.SaveChangesAsync();
   //             }

   //             if (!context.Products.Any())
   //             {
   //                 var productsData =
   //                     File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
   //                 var products = JsonSerializer.Deserialize<List<Product>>(productsData);

   //                 foreach (var item in products)
   //                 {
   //                     context.Products.Add(item);
   //                 }

   //                 await context.SaveChangesAsync();
   //             }
   //         }
			//catch (Exception ex)
			//{
   //             var logger = loggerFactory.CreateLogger<StoreContextSeed>();
			//	logger.LogError(ex.Message);
			//}
   //     }
    }
}
