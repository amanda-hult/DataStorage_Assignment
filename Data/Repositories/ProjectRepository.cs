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
            Debug.WriteLine($"Error creating {nameof(ProjectEntity)} entity: {ex.Message}");
            return null!;
        }
    }

    // READ
    //public async Task<List<ProjectEntity>> GetAllProjectsWithDetailsAsync()
    //{
    //    return await _dbSet
    //            .Include(p => p.User)
    //            .Include(p => p.Customer)
    //                .ThenInclude(c => c.ContactPerson)
    //            .Include(p => p.Status)
    //            .Include(p => p.ProjectProducts)
    //                .ThenInclude(pp => pp.Product)
    //            .ToListAsync();
    //}

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

            //existingEntity.Status = updatedEntity.Status;
            //existingEntity.User = updatedEntity.User;
            //existingEntity.Customer = updatedEntity.Customer;

            //if (updatedEntity.Customer.ContactPerson != null)
            //{
            //    existingEntity.Customer.ContactPerson = updatedEntity.Customer.ContactPerson;
            //}
            //existingEntity.ProjectProducts.Clear();
            //foreach (var product in updatedEntity.ProjectProducts)
            //{
            //    existingEntity.ProjectProducts.Add(product);
            //}
            var existingStatus = await _context.Statuses.FindAsync(updatedEntity.StatusId);
            if (existingStatus != null)
            {
                existingEntity.StatusId = existingStatus.StatusId;
                existingEntity.Status = existingStatus;
                _context.Entry(existingEntity).Reference(e => e.Status).IsModified = true;
            }

            Debug.WriteLine($"Updating user: {updatedEntity.User.UserId} - {updatedEntity.User.FirstName}");
            var existingUser = await _context.Users.FindAsync(updatedEntity.User.UserId);
            if (existingUser == null)
            {
                Debug.WriteLine($"User with ID {updatedEntity.User.UserId} not found in database.");
            }
            if (existingUser != null)
            {
                existingUser.FirstName = updatedEntity.User.FirstName;
                existingUser.LastName = updatedEntity.User.LastName;
                existingUser.Email = updatedEntity.User.Email;

                existingEntity.UserId = updatedEntity.UserId;
                existingEntity.User = existingUser;
                _context.Entry(existingUser).State = EntityState.Modified;
                _context.Entry(existingEntity).Reference(e => e.User).IsModified = true;
            }

            if (updatedEntity.Customer != null)
            {
                existingEntity.CustomerId = updatedEntity.CustomerId;
                existingEntity.Customer = updatedEntity.Customer;
            }

            if (updatedEntity.ProjectProducts != null)
            {
                existingEntity.ProjectProducts = updatedEntity.ProjectProducts;
            }

            _context.Update(existingEntity);

            Debug.WriteLine($"Saving changes for Project ID: {existingEntity.ProjectId}");
            await _context.SaveChangesAsync();
            Debug.WriteLine("Changes saved successfully.");

            return existingEntity;

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating {nameof(ProjectEntity)} entity: {ex.Message}");
            return null!;
        }
    }
}
