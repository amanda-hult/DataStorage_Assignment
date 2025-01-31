using Data.Entities;

namespace Data.Interfaces;

public interface IProjectRepository : IBaseRepository<ProjectEntity>
{
    Task<ProjectEntity> CreateProjectAsync(ProjectEntity entity);
    Task<List<ProjectEntity>> GetProjectsbyStatusAsync(string statusName);
}
