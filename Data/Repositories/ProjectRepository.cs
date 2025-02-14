using System.Diagnostics;
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

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            Debug.WriteLine($"ProjectEntity Title: {entity.Title}");
            Debug.WriteLine($"ProjectEntity StatusId: {entity.StatusId}");
            Debug.WriteLine($"ProjectEntity UserId: {entity.UserId}");
            Debug.WriteLine($"ProjectEntity CustomerId: {entity.CustomerId}");
            Debug.WriteLine($"ProjectEntity ProjectProducts count: {entity.ProjectProducts.Count}");

            //if (entity.User == null) Debug.WriteLine("Error: User is NULL");
            //if (entity.Customer == null) Debug.WriteLine("Error: Customer is NULL");
            //if (entity.Status == null) Debug.WriteLine("Error: Status is NULL");
            //foreach (var pp in entity.ProjectProducts)
            //{
            //    if (pp.Product == null)
            //        Debug.WriteLine($"Error: Product with ID {pp.ProductId} is NULL in ProjectProducts");
            //}
            foreach (var pp in entity.ProjectProducts)
            {
                Debug.WriteLine($"ProjectProduct: ProjectId={entity.ProjectId}, ProductId={pp.ProductId}, Hours={pp.Hours}");
            }


            await _dbSet.AddAsync(entity);
            await context.SaveChangesAsync();

            Debug.WriteLine($"Project saved with ID: {entity.ProjectId}");
            await transaction.CommitAsync();

            Debug.WriteLine("Transaction committed. Fetching project with includes..");

            var createdProject = await _dbSet
                .Include(p => p.User)
                .Include(p => p.Customer)
                    .ThenInclude(c => c.ContactPerson)
                .Include(p => p.Status)
                .Include(p => p.ProjectProducts)
                    .ThenInclude(pp => pp.Product)
                .FirstOrDefaultAsync(p => p.ProjectId == entity.ProjectId);

            Debug.WriteLine($"createdProject in projectRepository: {(createdProject != null ? "Exists" : "NULL")}");

            return createdProject;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Debug.WriteLine($"Error creating {nameof(ProjectEntity)} entity: {ex.Message}");
            return null!;
        }
    }

    // READ
    public async Task<List<ProjectEntity>> GetAllProjectsWithDetailsAsync()
    {
        return await _dbSet
                .Include(p => p.User)
                .Include(p => p.Customer)
                    .ThenInclude(c => c.ContactPerson)
                .Include(p => p.Status)
                .Include(p => p.ProjectProducts)
                    .ThenInclude(pp => pp.Product)
                .ToListAsync();
    }

    public async Task<List<ProjectEntity>> GetAllProjectsAsync()
    {
        return await _dbSet
                .Include(p => p.Customer)
                    //.ThenInclude(c => c.CustomerName)
                .Include(p => p.Status)
                    //.ThenInclude(s => s.StatusName)
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
}
