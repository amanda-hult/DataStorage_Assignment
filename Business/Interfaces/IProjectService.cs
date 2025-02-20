using Business.Dtos;
using Business.Models.Responses;
using Business.Models;

namespace Business.Interfaces;

public interface IProjectService
{
    Task<ResultT<DetailedProjectModel>> CreateProjectAsync(ProjectCreateDto dto);
    //Task<ResultT<List<DetailedProjectModel>>> GetAllProjectsWithDetailsAsync();
    Task<ResultT<List<BasicProjectModel>>> GetAllProjectsAsync();
    Task<ResultT<DetailedProjectModel>> GetProjectWithDetailsAsync(int id);
    Task<ResultT<DetailedProjectModel>> UpdateProjectAsync(ProjectUpdateDto dto);
    //Task<bool> ProjectExists(int id);
}
