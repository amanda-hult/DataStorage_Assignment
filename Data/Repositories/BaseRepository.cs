﻿using System.Diagnostics;
using System.Linq.Expressions;
using Data.Contexts;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data.Repositories;

public abstract class BaseRepository<TEntity>(DataContext context) : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly DataContext _context = context;
    protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    private IDbContextTransaction _transaction = null!;

    #region Transaction Management
    public virtual async Task BeginTransactionAsync()
    {
        _transaction ??= await _context.Database.BeginTransactionAsync();
    }

    public virtual async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null!;
        }
    }

    public virtual async Task RollBackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null!;
        }
    }
    #endregion


    // CREATE
    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        if (entity == null)
            return null!;

        try
        {
            await _dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error creating {nameof(TEntity)} entity: {ex.Message}");
            return null!;
        }
    }

    // READ
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeExpression = null)
    {
        IQueryable<TEntity> query = _dbSet;

        if (includeExpression != null)
            query = includeExpression(query);

        return await query.ToListAsync();
    }

    public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeExpression = null)
    {
        IQueryable<TEntity> query = _dbSet;

        if (includeExpression != null)
            query = includeExpression(query);

        return await query.FirstOrDefaultAsync(predicate);
    }

    // UPDATE
    public virtual async Task<TEntity> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity updatedEntity)
    {
        if (updatedEntity == null)
            return null!;      

        try
        {
            var existingEntity = await _dbSet.FirstOrDefaultAsync(predicate);
            if (existingEntity == null)
                return null!;

            _context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
            await context.SaveChangesAsync();
            return existingEntity;

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating {nameof(TEntity)} entity: {ex.Message}");
            return null!;
        }
    }

    // DELETE
    public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entityToDelete = await _dbSet.FirstOrDefaultAsync(predicate);
            if (entityToDelete == null) return false;

            _dbSet.Remove(entityToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error removing {nameof(TEntity)} entity: {ex.Message}");
            return false!;
        }
    }

    public virtual async Task<bool> AlreadyExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            return await _dbSet.AnyAsync(predicate);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error checking existence: {ex.Message}");
            throw;
        }
    }
}


