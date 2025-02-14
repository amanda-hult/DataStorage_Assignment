using System.Diagnostics;
using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Models.Responses;
using Data.Interfaces;
using Data.Repositories;

namespace Business.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProductService _productService;
    private readonly IUserService _userService;
    private readonly ICustomerService _customerService;
    private readonly IContactPersonService _contactPersonService;
    private readonly IStatusService _statusService;

    public ProjectService(IProjectRepository projectRepository, IProductService productService, IUserService userService, ICustomerService customerService, IContactPersonService contactPersonService, IStatusService statusService)
    {
        _projectRepository = projectRepository;
        _productService = productService;
        _userService = userService;
        _customerService = customerService;
        _contactPersonService = contactPersonService;
        _statusService = statusService;
    }

    // CREATE
    public async Task<ResultT<DetailedProjectModel>> CreateProjectAsync(ProjectCreateDto dto)
    {
        //using var transaction = await _projectRepository.BeginTransactionAsync();

        try
        {
            if (dto == null)
                return ResultT<DetailedProjectModel>.Error("Project data is null");


            if (dto.User == null || string.IsNullOrWhiteSpace(dto.User.Email))
                return ResultT<DetailedProjectModel>.Error("User information is missing or invalid");

            Debug.WriteLine($"Creating project for user: {dto.User.Email}");

            //get user
            var userEntity = await _userService.GetUserEntityByEmailAsync(dto.User.Email);


            //get status
            var statusEntity = await _statusService.GetStatusEntityByIdAsync(dto.StatusId);


            //get customer and create contactperson
            var customerEntity = await _customerService.GetCustomerEntityByNameAsync(dto.Customer.CustomerName);




            // check if contactperson exists or create contactperson
            if (dto.ContactPerson == null)
                return ResultT<DetailedProjectModel>.Error("ContactPerson is null");

            if (string.IsNullOrWhiteSpace(dto.ContactPerson.Email))
                return ResultT<DetailedProjectModel>.Error("Contact person email is missing or invalid");


            var contactPersonEntity = await _contactPersonService.GetContactPersonEntityByEmailAsync(dto.ContactPerson.Email);
            if (contactPersonEntity == null || contactPersonEntity.Data == null)
            {
                var result = await _contactPersonService.CreateContactPersonAsync(dto.ContactPerson);
                if (result.Success)
                    contactPersonEntity = await _contactPersonService.GetContactPersonEntityByEmailAsync(dto.ContactPerson.Email);
            }

            // connect contact person to customer
            CustomerFactory.Connect(customerEntity.Data, contactPersonEntity.Data);



            //get products
            var productIds = dto.ProjectProducts.Select(pp => pp.ProductId).ToList();

            var productResult = await _productService.GetProductEntitiesByIdAsync(productIds);

            if (!productResult.Success || productResult == null)
                return ResultT<DetailedProjectModel>.NotFound($"Failed to load products.");

            var productEntities = productResult.Data;

            var validProjectProducts = dto.ProjectProducts
                .Where(pp => productEntities.Any(p => p.ProductId == pp.ProductId))
                .GroupBy(pp => pp.ProductId)
                .Select(g => g.First())
                .ToList();

            dto.ProjectProducts = validProjectProducts;

            //create project
            var projectEntity = ProjectFactory.Create(dto, userEntity.Data, customerEntity.Data, statusEntity.Data, productEntities);

            var createdProject = await _projectRepository.CreateProjectAsync(projectEntity);


            //create and connect projectProduct with hours per product

            //await transaction.CommitAsync();

            //save changes


            Debug.WriteLine($"createdProject: {(createdProject != null ? "Exists" : "NULL")}");
            Debug.WriteLine($"createdProject.Customer: {(createdProject?.Customer != null ? "Exists" : "NULL")}");
            Debug.WriteLine($"createdProject.Customer.ContactPerson: {(createdProject?.Customer?.ContactPerson != null ? "Exists" : "NULL")}");
            Debug.WriteLine($"createdProject.User: {(createdProject?.User != null ? "Exists" : "NULL")}");
            Debug.WriteLine($"createdProject.Status: {(createdProject?.Status != null ? "Exists" : "NULL")}");
            Debug.WriteLine($"createdProject.ProjectProducts count: {createdProject?.ProjectProducts?.Count ?? 0}");


            return ResultT<DetailedProjectModel>.Created(ProjectFactory.CreateDetailedProjectModel(createdProject));

        }
        catch (Exception ex)
        {
            //await transaction.RollbackAsync();
            return ResultT<DetailedProjectModel>.Error($"An error occured creating the project: {ex.Message}");
        }

    }

    // READ
    public async Task<ResultT<List<DetailedProjectModel>>> GetAllProjectsWithDetailsAsync()
    {
        var projects = (await _projectRepository.GetAllProjectsWithDetailsAsync()).Select(ProjectFactory.CreateDetailedProjectModel).ToList();

        if (projects.Count == 0)
            return ResultT<List<DetailedProjectModel>>.NotFound("No projects found.");

        return ResultT<List<DetailedProjectModel>>.Ok(projects);
    }

    public async Task<ResultT<List<BasicProjectModel>>> GetAllProjectsAsync()
    {
        var projects = (await _projectRepository.GetAllProjectsAsync()).Select(ProjectFactory.CreateBasicProjectModel).ToList();

        if (projects.Count == 0)
            return ResultT<List<BasicProjectModel>>.NotFound("No projects found.");

        return ResultT<List<BasicProjectModel>>.Ok(projects);
    }

    public async Task<ResultT<DetailedProjectModel>> GetProjectWithDetailsAsync(int id)
    {
        var projectEntity = await _projectRepository.GetProjectWithDetailsAsync(id);

        if (projectEntity == null)
        {
            return ResultT<DetailedProjectModel>.NotFound("Project not found.");
        }

        return ResultT<DetailedProjectModel>.Ok(ProjectFactory.CreateDetailedProjectModel(projectEntity));
    }

    // UPDATE

    // DELETE
}
