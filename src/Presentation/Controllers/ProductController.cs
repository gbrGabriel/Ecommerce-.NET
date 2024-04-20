using Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/v1/product")]
public class ProductController(IRepositoryProduct repositoryProduct, IRepositoryProductType repositoryProductType, IRepositoryProductBrand repositoryProductBrand) : AbstractBaseController
{
    private readonly IRepositoryProduct _repositoryProduct = repositoryProduct;
    private readonly IRepositoryProductType _repositoryProductType = repositoryProductType;
    private readonly IRepositoryProductBrand _repositoryProductBrand = repositoryProductBrand;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _repositoryProduct.GetAll());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) => Ok(await _repositoryProduct.GetByIdAsync(id));

    [HttpGet("types")]
    public async Task<IActionResult> GetAllProductsType() => Ok(await _repositoryProductType.GetAll());

    [HttpGet("types/{id:int}")]
    public async Task<IActionResult> GetProductTypeById(int id) => Ok(await _repositoryProductType.GetByIdAsync(id));

    [HttpGet("brands")]
    public async Task<IActionResult> GetAllProductsBrands() => Ok(await _repositoryProductBrand.GetAll());

    [HttpGet("brands/{id:int}")]
    public async Task<IActionResult> GetProductBrandsById(int id) => Ok(await _repositoryProductBrand.GetByIdAsync(id));
}

