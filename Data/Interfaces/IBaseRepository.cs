using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task<TEntity> CreateAsync(TEntity entity);
    Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeExpression = null);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeExpression = null);
    Task<TEntity> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity updatedEntity);
    Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate);
    Task<bool> AlreadyExistsAsync(Expression<Func<TEntity, bool>> predicate);
}
