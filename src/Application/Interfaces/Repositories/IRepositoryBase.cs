using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Interfaces.Repositories;

public interface IRepositoryBase<TEntity> : IDisposable where TEntity : class
{
    Task<bool> SaveChangesAsync();
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(TEntity entity);
    Task<TEntity?> GetByIdAsync(int id);
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<Product, bool>>? filter = null,
        Func<IQueryable<Product>, IOrderedQueryable<Product>>? orderBy = null,
        bool isPagingEnabled = false,
        int? skip = null,
        int? take = null);
    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);
}
