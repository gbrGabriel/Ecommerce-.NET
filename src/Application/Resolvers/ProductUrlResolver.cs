using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Application.Resolvers;

public class ProductUrlResolver(IConfiguration configuration) : IValueResolver<Product, ProductDTO, string>
{
    private readonly IConfiguration _configuration = configuration;

    public string Resolve(Product product, ProductDTO productDTO, string url, ResolutionContext context)
    {
        if (!string.IsNullOrWhiteSpace(product.ImageUrl))
            return $"{_configuration["UrlApi"]}{product.ImageUrl}";

        return string.Empty;
    }
}
