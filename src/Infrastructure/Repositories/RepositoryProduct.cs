using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class RepositoryProduct(ApplicationDbContext context) : RepositoryBase<Product>(context), IRepositoryProduct
{
    public override async Task<Product?> GetByIdAsync(int id)
        => await _context.Products.Include(e => e.ProductBrand).Include(e => e.ProductType).FirstOrDefaultAsync(p => p.Id == id);

    public override async Task<IEnumerable<Product>> GetAllAsync(
        Expression<Func<Product, bool>>? filter = null,
        Func<IQueryable<Product>, IOrderedQueryable<Product>>? orderBy = null,
        bool isPagingEnabled = false,
        int? skip = null,
        int? take = null)
    {
        IQueryable<Product> query = _context.Products
            .Include(e => e.ProductBrand)
            .Include(e => e.ProductType);

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        if (isPagingEnabled && skip != null && take != null)
        {
            query = query.Skip(skip.Value).Take(take.Value);
        }

        return await query.ToListAsync();
    }
}
