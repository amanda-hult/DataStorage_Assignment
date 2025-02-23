using System.Diagnostics;
using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Models.Responses;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class CustomerService(ICustomerRepository customerRepository, IProjectRepository projectRepository) : ICustomerService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IProjectRepository _projectRepository = projectRepository;

    // CREATE
    public async Task<ResultT<CustomerModel>> CreateCustomerAsync(CustomerCreateDto dto)
    {
        //check if customer already exists
        string lowerCaseName = dto.CustomerName.ToLower();
        bool exists = await _customerRepository.AlreadyExistsAsync(c => c.CustomerName == lowerCaseName);
        if (exists)
            return ResultT<CustomerModel>.Conflict("A customer with the same name already exists.");

        await _customerRepository.BeginTransactionAsync();

        //create new customer
        try
        {
            var createdEntity = await _customerRepository.CreateAsync(CustomerFactory.Create(dto));

            if (createdEntity == null)
                throw new Exception("Failed to create customer entity.");

            await _customerRepository.SaveAsync();
            await _customerRepository.CommitTransactionAsync();
            return ResultT<CustomerModel>.Created(CustomerFactory.Create(createdEntity));
        }
        catch (Exception ex)
        {
            await _customerRepository.RollBackTransactionAsync();
            Debug.WriteLine($"Error creating customer: {ex.Message}");
            return ResultT<CustomerModel>.Error("An error occured while creating the customer.");
        }
    }

    // READ
    public async Task<ResultT<IEnumerable<CustomerModel>>> GetAllCustomersAsync()
    {
        var customers = (await _customerRepository.GetAllAsync()).Select(CustomerFactory.Create).ToList();

        if (customers.Count == 0)
            return ResultT<IEnumerable<CustomerModel>>.NotFound("No customers found.");

        return ResultT<IEnumerable<CustomerModel>>.Ok(customers);
    }

    public async Task<ResultT<CustomerModel>> GetCustomerByIdAsync(int id)
    {
        var customerEntity = await _customerRepository.GetAsync(u => u.CustomerId == id);

        if (customerEntity == null)
            return ResultT<CustomerModel>.NotFound("Customer not found.");

        return ResultT<CustomerModel>.Ok(CustomerFactory.Create(customerEntity));
    }

    public async Task<ResultT<CustomerEntity>> GetCustomerEntityByIdAsync(int id)
    {
        var customerEntity = await _customerRepository.GetAsync(u => u.CustomerId == id);

        if (customerEntity == null)
            return ResultT<CustomerEntity>.NotFound("Customer not found.");

        return ResultT<CustomerEntity>.Ok(customerEntity);
    }

    public async Task<ResultT<CustomerEntity>> GetCustomerEntityByNameAsync(string name)
    {
        var customerEntity = await _customerRepository.GetAsync(u => u.CustomerName == name);

        if (customerEntity == null)
            return ResultT<CustomerEntity>.NotFound("Customer not found.");

        return ResultT<CustomerEntity>.Ok(customerEntity);
    }

    // UPDATE
    public async Task<ResultT<CustomerModel>> UpdateCustomerAsync(CustomerUpdateDto dto)
    {
        // get user entity
        var customerEntity = await _customerRepository.GetAsync(c => c.CustomerId == dto.CustomerId);

        if (customerEntity == null)
            return ResultT<CustomerModel>.NotFound("Customer not found.");

        // update user entity
        CustomerFactory.Update(customerEntity, dto);
        var updatedCustomer = await _customerRepository.UpdateAsync(c => c.CustomerId == dto.CustomerId, customerEntity);

        return ResultT<CustomerModel>.Ok(CustomerFactory.Create(updatedCustomer));
    }

    // DELETE
    public async Task<Result> DeleteCustomerAsync(int id)
    {
        // get customer entity
        var customerEntity = _customerRepository.GetAsync(c => c.CustomerId == id);
        if (customerEntity == null)
            return Result.NotFound("Customer not found.");

        // check if customer exists in any project
        bool existsInProject = await _projectRepository.AlreadyExistsAsync(p => p.CustomerId == id);
        if (existsInProject)
            return Result.Error("Customer exists in a project and cannot be deleted.");
        
        //delete customer
        var result = await _customerRepository.DeleteAsync(c => c.CustomerId == id);
        return result ? Result.NoContent() : Result.Error("Unable to delete customer.");
    }
}
