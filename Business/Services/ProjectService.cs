using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Models.Responses;
using Data.Interfaces;

namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository, IProductService productService, IUserService userService, ICustomerService customerService, IContactPersonService contactPersonService, IStatusService statusService) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IProductService _productService = productService;
    private readonly IUserService _userService = userService;
    private readonly ICustomerService _customerService = customerService;
    private readonly IContactPersonService _contactPersonService = contactPersonService;
    private readonly IStatusService _statusService = statusService;

    // CREATE
    public async Task<ResultT<DetailedProjectModel>> CreateProjectAsync(ProjectCreateDto dto)
    {
        await _projectRepository.BeginTransactionAsync();

        try
        {
            // check if project with the same title and customerId exists
            string lowerCaseTitle = dto.Title.ToLower();
            bool exists = await _projectRepository.AlreadyExistsAsync(x => x.Title == lowerCaseTitle && x.CustomerId == dto.CustomerId);
            if (exists)
                return ResultT<DetailedProjectModel>.Conflict("A project with the same title and customerId already exists.");

            //get user
            var userEntity = await _userService.GetUserEntityByEmailAsync(dto.User.Email);

            //get status
            var statusEntity = await _statusService.GetStatusEntityByIdAsync(dto.StatusId);

            //get customer
            var customerEntity = await _customerService.GetCustomerEntityByNameAsync(dto.Customer.CustomerName);

            // check if contactperson exists or create contactperson
            var contactPersonEntity = await _contactPersonService.GetContactPersonEntityByEmailAsync(dto.ContactPerson.Email);
            if (contactPersonEntity.Data == null)
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
            await _projectRepository.CommitTransactionAsync();

            return ResultT<DetailedProjectModel>.Created(ProjectFactory.CreateDetailedProjectModel(createdProject));
        }
        catch (Exception ex)
        {
            await _projectRepository.RollBackTransactionAsync();
            return ResultT<DetailedProjectModel>.Error($"An error occured creating the project: {ex.Message}");
        }
    }

    // READ
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
    public async Task<ResultT<DetailedProjectModel>> UpdateProjectAsync(ProjectUpdateDto dto)
    {
        await _projectRepository.BeginTransactionAsync();

        try
        {
            // get project entity with details
            var projectEntity = await _projectRepository.GetProjectWithDetailsAsync(dto.ProjectId);
            if (projectEntity == null)
            {
                return ResultT<DetailedProjectModel>.NotFound("Project not found.");
            }

            // check if project with the same title and customerId exists
            string lowerCaseTitle = dto.Title.ToLower();
            bool exists = await _projectRepository.AlreadyExistsAsync(x => x.Title == lowerCaseTitle && x.CustomerId == dto.CustomerId);
            if (exists)
                return ResultT<DetailedProjectModel>.Conflict("A project with the same title and customerId already exists.");

            //get user
            var userEntity = await _userService.GetUserEntityByIdAsync(dto.UserId);
            if (userEntity.Data == null)
            {
                return ResultT<DetailedProjectModel>.NotFound("User not found.");
            }

            //get status
            var statusEntity = await _statusService.GetStatusEntityByIdAsync(dto.StatusId);
            if (statusEntity.Data == null)
            {
                return ResultT<DetailedProjectModel>.NotFound("Status not found.");
            }

            //get customer and create contactperson
            var customerEntity = await _customerService.GetCustomerEntityByIdAsync(dto.CustomerId);
            if (customerEntity.Data == null)
            {
                return ResultT<DetailedProjectModel>.NotFound("Customer not found.");
            }

            projectEntity.UserId = userEntity.Data.UserId;
            projectEntity.CustomerId = customerEntity.Data.CustomerId;
            projectEntity.StatusId = statusEntity.Data.StatusId;


            // check if new contactperson email is unique
            var existingContactPerson = await _contactPersonService.GetContactPersonEntityByIdAsync(dto.ContactPersonId);
            if (existingContactPerson.Data == null)
            {
                return ResultT<DetailedProjectModel>.NotFound("ContactPerson not found.");
            }

            if (!string.Equals(existingContactPerson.Data.Email, dto.ContactPerson.Email, StringComparison.OrdinalIgnoreCase))
            {
                var emailExists = await _contactPersonService.GetContactPersonEntityByEmailAsync(dto.ContactPerson.Email);
                if (emailExists.Data != null && emailExists.Data.ContactPersonId != dto.ContactPersonId)
                {
                    return ResultT<DetailedProjectModel>.Conflict("Email is already used by another contact person.");
                }
            }

            // connect contact person to customer
            CustomerFactory.Connect(customerEntity.Data, existingContactPerson.Data);

            // remove deleted products
            var existingProjectProducts = projectEntity.ProjectProducts.ToList();
            var updatedProductIds = dto.ProjectProducts.Select(pp => pp.ProductId).ToList();

            var productsToRemove = existingProjectProducts
                .Where(pp => !updatedProductIds.Contains(pp.ProductId))
                .ToList();

            foreach (var product in productsToRemove)
            {
                projectEntity.ProjectProducts.Remove(product);
            }

            // add new products
            foreach (var product in dto.ProjectProducts)
            {
                var existingProduct = existingProjectProducts.FirstOrDefault(pp => pp.ProductId == product.ProductId);
                if (existingProduct == null)
                {
                    projectEntity.ProjectProducts.Add(new Data.Entities.ProjectProductEntity
                    {
                        ProductId = product.ProductId,
                        ProjectId = dto.ProjectId,
                        Hours = product.Hours,
                    });
                }
                else
                {
                    existingProduct.Hours = product.Hours;
                }
            }

            // update project entity
            ProjectFactory.Update(projectEntity, dto);
            var updatedProject = await _projectRepository.UpdateProjectAsync(p => p.ProjectId == dto.ProjectId, projectEntity);
            await _projectRepository.CommitTransactionAsync();

            return ResultT<DetailedProjectModel>.Ok(ProjectFactory.CreateDetailedProjectModel(updatedProject));
        }
        catch (Exception ex)
        {
            await _projectRepository.RollBackTransactionAsync();
            return ResultT<DetailedProjectModel>.Error($"An error occured updating the project: {ex.Message}");
        }
    }
}
