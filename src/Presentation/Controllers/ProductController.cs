using Application.DTOs;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Presentation.Controllers;

[Route("api/v1/product")]
public class ProductController(IRepositoryProduct repositoryProduct, IRepositoryProductType repositoryProductType, IRepositoryProductBrand repositoryProductBrand, IMapper mapper) : AbstractBaseController
{
    private readonly IRepositoryProduct _repositoryProduct = repositoryProduct;
    private readonly IRepositoryProductType _repositoryProductType = repositoryProductType;
    private readonly IRepositoryProductBrand _repositoryProductBrand = repositoryProductBrand;
    private readonly IMapper _mapper = mapper;
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? sort = null, [FromQuery] int? brandId = null, [FromQuery] int? typeId = null,
        [FromQuery] bool isPagingEnabled = false, [FromQuery] int? skip = null, [FromQuery] int? take = null)
    {
        Expression<Func<Product, bool>> filter = p =>
            (brandId == null || p.ProductBrandId == brandId) &&
            (typeId == null || p.ProductTypeId == typeId);

        Func<IQueryable<Product>, IOrderedQueryable<Product>>? orderBy = null;
        switch (sort)
        {
            case "priceAsc":
                orderBy = q => q.OrderBy(x => x.Price);
                break;
            case "priceDesc":
                orderBy = q => q.OrderByDescending(x => x.Price);
                break;
            default:
                orderBy = q => q.OrderBy(x => x.Name);
                break;
        }
        return Ok(_mapper.Map<IEnumerable<ProductDTO>>(await _repositoryProduct.GetAllAsync(filter, orderBy, isPagingEnabled, skip, take)));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) => Ok(_mapper.Map<Product?, ProductDTO>(await _repositoryProduct.GetByIdAsync(id)));

    [HttpGet("types")]
    public async Task<IActionResult> GetAllProductsType() => Ok(await _repositoryProductType.GetAllAsync());

    [HttpGet("types/{id:int}")]
    public async Task<IActionResult> GetProductTypeById(int id) => Ok(await _repositoryProductType.GetByIdAsync(id));

    [HttpGet("brands")]
    public async Task<IActionResult> GetAllProductsBrands() => Ok(await _repositoryProductBrand.GetAllAsync());

    [HttpGet("brands/{id:int}")]
    public async Task<IActionResult> GetProductBrandsById(int id) => Ok(await _repositoryProductBrand.GetByIdAsync(id));

}

