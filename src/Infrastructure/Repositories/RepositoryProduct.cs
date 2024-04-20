using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RepositoryProduct(ApplicationDbContext context) : RepositoryBase<Product>(context), IRepositoryProduct
{
    public override async Task<Product?> GetByIdAsync(int id)
        => await _context.Products.Include(e => e.ProductBrand).Include(e => e.ProductType).FirstOrDefaultAsync(p => p.Id == id);

    public override async Task<IEnumerable<Product>> GetAll()
        => await _context.Products.Include(e => e.ProductBrand).Include(e => e.ProductType).ToListAsync();
}
