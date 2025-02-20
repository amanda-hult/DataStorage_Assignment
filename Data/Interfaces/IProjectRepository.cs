using System.Linq.Expressions;
using Data.Entities;

namespace Data.Interfaces;

public interface IProjectRepository : IBaseRepository<ProjectEntity>
{
    Task<ProjectEntity> CreateProjectAsync(ProjectEntity entity);
    //Task<List<ProjectEntity>> GetAllProjectsWithDetailsAsync();
    Task<List<ProjectEntity>> GetAllProjectsAsync();
    Task<List<ProjectEntity>> GetProjectsbyStatusAsync(string statusName);
    Task<ProjectEntity> GetProjectWithDetailsAsync(int id);
    Task<ProjectEntity> UpdateProjectAsync(Expression<Func<ProjectEntity, bool>> predicate, ProjectEntity updatedEntity);
}
