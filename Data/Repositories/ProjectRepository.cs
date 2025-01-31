using System.Diagnostics;
using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ProjectRepository(DataContext context) : BaseRepository<ProjectEntity>(context), IProjectRepository
{

    public async Task<ProjectEntity> CreateProjectAsync(ProjectEntity entity)
    {
        if (entity == null)
            return null!;

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await _dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return entity;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Debug.WriteLine($"Error creating {nameof(ProjectEntity)} entity: {ex.Message}");
            return null!;
        }
    }

    public async Task<List<ProjectEntity>> GetProjectsbyStatusAsync(string statusName)
    {
        return await _dbSet
            .Where(p => p.Status.StatusName == statusName)
            .Include(p => p.Customer)
            .Include(p => p.Status)
            .ToListAsync();
    }
}
