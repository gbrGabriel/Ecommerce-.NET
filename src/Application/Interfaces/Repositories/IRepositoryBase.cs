namespace Application.Interfaces.Repositories;

public interface IRepositoryBase<TEntity> : IDisposable where TEntity : class
{
    Task<bool> SaveChangesAsync();
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(TEntity entity);
    Task<TEntity?> GetByIdAsync(int id);
    Task<IEnumerable<TEntity>> GetAll();
    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);
}
