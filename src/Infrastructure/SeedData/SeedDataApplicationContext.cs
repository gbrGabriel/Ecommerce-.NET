using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Infrastructure.SeedData;

public class SeedDataApplicationContext
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        try
        {
            await SeedProductBrandsAsync(context);
            await SeedProductTypesAsync(context);
            await SeedProductsAsync(context);

            Console.WriteLine("Dados de seed carregados com sucesso.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao carregar dados de seed: {ex.Message}");
            throw;
        }
    }

    private static async Task SeedProductBrandsAsync(ApplicationDbContext context)
    {
        if (!await context.ProductBrands.AnyAsync())
        {
            var brandsData = await ReadJsonFileAsync("../Infrastructure/SeedData/brands.json");

            if (!string.IsNullOrEmpty(brandsData))
            {
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                if (brands != null && brands.Count > 0)
                {
                    context.ProductBrands.AddRange(brands);
                    await context.SaveChangesAsync();
                }
            }
        }
    }

    private static async Task SeedProductTypesAsync(ApplicationDbContext context)
    {
        if (!await context.ProductTypes.AnyAsync())
        {
            var typesData = await ReadJsonFileAsync("../Infrastructure/SeedData/types.json");

            if (!string.IsNullOrEmpty(typesData))
            {
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                if (types != null && types.Count > 0)
                {
                    context.ProductTypes.AddRange(types);
                    await context.SaveChangesAsync();
                }
            }
        }
    }

    private static async Task SeedProductsAsync(ApplicationDbContext context)
    {
        if (!await context.Products.AnyAsync())
        {
            var productsData = await ReadJsonFileAsync("../Infrastructure/SeedData/products.json");

            if (!string.IsNullOrEmpty(productsData))
            {
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if (products != null && products.Count > 0)
                {
                    context.Products.AddRange(products);
                    await context.SaveChangesAsync();
                }
            }
        }
    }

    private static async Task<string?> ReadJsonFileAsync(string filePath)
    {
        string jsonData;
        try
        {
            using var streamReader = new StreamReader(filePath);
            jsonData = await streamReader.ReadToEndAsync();
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Arquivo não encontrado: {filePath}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao ler arquivo JSON: {ex.Message}");
            throw;
        }

        return jsonData;
    }
}
