using Application.DTOs;
using Application.Resolvers;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        CreateMap<Product, ProductDTO>().ForMember(e => e.ProductBrand, e => e.MapFrom(e => e.ProductBrand.Name)).
            ForMember(e => e.ProductType, e => e.MapFrom(e => e.ProductType.Name)).
            ForMember(e => e.ImageUrl, e => e.MapFrom<ProductUrlResolver>());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }
}
