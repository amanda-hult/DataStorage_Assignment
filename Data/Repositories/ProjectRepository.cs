using System.Diagnostics;
using System.Linq.Expressions;
using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ProjectRepository(DataContext context) : BaseRepository<ProjectEntity>(context), IProjectRepository
{
    // CREATE
    public async Task<ProjectEntity> CreateProjectAsync(ProjectEntity entity)
    {
        if (entity == null)
            return null!;

        try
        {
            await _dbSet.AddAsync(entity);
            await context.SaveChangesAsync();

            var createdProject = await _dbSet
                .Include(p => p.User)
                .Include(p => p.Customer)
                    .ThenInclude(c => c.ContactPerson)
                .Include(p => p.Status)
                .Include(p => p.ProjectProducts)
                    .ThenInclude(pp => pp.Product)
                .FirstOrDefaultAsync(p => p.ProjectId == entity.ProjectId);

            return createdProject;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error creating {nameof(ProjectEntity)} entity: {ex.Message}");
            return null!;
        }
    }

    // READ
    public async Task<List<ProjectEntity>> GetAllProjectsAsync()
    {
        return await _dbSet
                .Include(p => p.Customer)
                .Include(p => p.Status)
                .ToListAsync();
    }

    public async Task<List<ProjectEntity>> GetProjectsbyStatusAsync(string statusName)
    {
        return await _dbSet
            .Where(p => p.Status.StatusName == statusName)
            .Include(p => p.Customer)
            .Include(p => p.Status)
            .ToListAsync();
    }

    public async Task<ProjectEntity> GetProjectWithDetailsAsync(int id)
    {
        var project = await _dbSet
            .Include(p => p.User)
            .Include(p => p.Customer)
                .ThenInclude(c => c.ContactPerson)
            .Include(p => p.Status)
            .Include(p => p.ProjectProducts)
                .ThenInclude(pp => pp.Product)
            .FirstOrDefaultAsync(p => p.ProjectId == id);

        if (project == null)
        {
            Debug.Write($"Project with id {id} not found.");
            return null!;
        }

        return project;
    }

    // UPDATE
    public async Task<ProjectEntity> UpdateProjectAsync(Expression<Func<ProjectEntity, bool>> predicate, ProjectEntity updatedEntity)
    {
        if (updatedEntity == null)
            return null!;

        try
        {
            var existingEntity = await _dbSet
                .Include(p => p.User)
                .Include(p => p.Customer)
                    .ThenInclude(c => c.ContactPerson)
                .Include(p => p.Status)
                .Include(p => p.ProjectProducts)
                    .ThenInclude(pp => pp.Product).FirstOrDefaultAsync(predicate);

            if (existingEntity == null)
                return null!;

            _context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);

            _context.Update(existingEntity);
            await _context.SaveChangesAsync();

            return existingEntity;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating {nameof(ProjectEntity)} entity: {ex.Message}");
            return null!;
        }
    }
}
