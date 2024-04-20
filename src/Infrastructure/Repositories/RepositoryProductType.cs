using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

public class RepositoryProductType(ApplicationDbContext context) : RepositoryBase<ProductType>(context), IRepositoryProductType
{
}
