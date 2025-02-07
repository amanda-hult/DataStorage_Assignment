using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Models.Responses;
using Data.Interfaces;

namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository, ICustomerRepository customerRepository, IUserRepository userRepository, IStatusRepository statusRepository, IContactPersonRepository contactPersonRepository, IProductRepository productRepository) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IStatusRepository _statusRepository = statusRepository;
    private readonly IContactPersonRepository _contactPersonRepository = contactPersonRepository;
    private readonly IProductRepository _productRepository = productRepository;

    // CREATE
    //public async Task<ResultT<DetailedProjectModel>> CreateProjectAsync(ProjectCreateDto dto)
    //{
    //    using var transaction = await _projectRepository.BeginTransactionAsync();

    //    try
    //    {
    //        //validate or create user
    //        var user = await _userRepository.GetAsync(u => u.UserId == dto.UserId);
    //        if (user == null)
    //        {
    //            user = await _userRepository.CreateAsync(UserFactory.Create(dto.User));
    //        }

    //        //validate or create customer + contactperson
    //        var customer = await _customerRepository.GetAsync(c => c.CustomerId == dto.CustomerId);
    //        if (customer == null)
    //        {
    //            var contactPerson = await _contactPersonRepository.GetAsync(cp => cp.Email == dto.Customer.ContactPerson.Email);
    //            contactPerson ??= await _contactPersonRepository.CreateAsync(ContactPersonFactory.Create(dto.Customer.ContactPerson));

    //            customer = await _customerRepository.CreateAsync(CustomerFactory.Create(dto.Customer, contactPerson));
    //        }

    //        //validate status
    //        var status = await _statusRepository.GetAsync(s => s.StatusId == dto.StatusId);
    //        if (status  == null)
    //        {
    //            return ResultT<DetailedProjectModel>.NotFound("Status not found.");
    //        }

    //        //validate products
    //        var productIds = dto.ProjectProducts.Select(pp => pp.ProductId).ToList();

    //        var products = await _productRepository.GetProductsByIdAsync(productIds);

    //        if (products.Count != productIds.Count)
    //            return ResultT<DetailedProjectModel>.NotFound($"Product/products does not exists.");

    //        //create project
    //        var projectEntity = ProjectFactory.Create(dto, user, customer, status);
    //        var createdProject = await _projectRepository.CreateProjectAsync(projectEntity);

    //        //create and connect projectProduct with hours per product
    //        await transaction.CommitAsync();
    //        //save changes


    //        return ResultT<DetailedProjectModel>.Created(ProjectFactory.Create(createdProject));

    //    }
    //    catch (Exception ex)
    //    { 
    //        await transaction.RollbackAsync();
    //        return ResultT<DetailedProjectModel>.Error($"An error occured creating the project: {ex.Message}");
    //    }

    //}

    // READ

    // UPDATE

    // DELETE
}
