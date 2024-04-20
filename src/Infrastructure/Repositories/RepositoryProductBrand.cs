using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

public class RepositoryProductBrand(ApplicationDbContext context) : RepositoryBase<ProductBrand>(context), IRepositoryProductBrand
{
}
