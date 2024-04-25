using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class RepositoryBase<TEntity>(ApplicationDbContext context) : IRepositoryBase<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext _context = context;

    public async Task<bool> SaveChangesAsync()
    {
        int result = 0;
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                result = await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
        return result > 0;
    }

    public async Task<bool> UpdateAsync(TEntity entity)
    {
        int result = 0;
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                _context.ChangeTracker.Clear();
                _context.Set<TEntity>().Update(entity);

                result = await _context.SaveChangesAsync();

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction?.Rollback();
                throw;
            }
        }
        return result > 0;
    }

    public async Task<bool> DeleteAsync(TEntity entity)
    {
        int result = 0;

        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                _context.ChangeTracker.Clear();
                _context.Set<TEntity>().Remove(entity);

                result = await _context.SaveChangesAsync();

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
        return result > 0;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<Product, bool>>? filter = null,
        Func<IQueryable<Product>, IOrderedQueryable<Product>>? orderBy = null,
        bool isPagingEnabled = false,
        int? skip = null,
        int? take = null)
        => await _context.Set<TEntity>().ToListAsync();

    public virtual async Task<TEntity?> GetByIdAsync(int id)
             => await _context.Set<TEntity>().FindAsync(id);

    public void Add(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Add(entity);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        try
        {
            foreach (var entity in entities)
            {
                if (_context.Entry(entity).State == EntityState.Detached)
                {
                    _context.Set<TEntity>().Add(entity);
                }
                else
                {
                    _context.Set<TEntity>().Attach(entity);
                    _context.Entry(entity).State = EntityState.Modified;
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
