using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Models.Responses;
using Data.Interfaces;

namespace Business.Services;

public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;

    // CREATE
    public async Task<ResultT<CustomerModel>> CreateCustomerAsync(CustomerCreateDto dto)
    {
        //check if customer already exists
        var exists = await _customerRepository.AlreadyExistsAsync(c => c.CustomerName == dto.CustomerName);
        if (exists)
            return ResultT<CustomerModel>.Conflict("A customer with the same name already exists.");

        //create new product
        var createdEntity = await _customerRepository.CreateAsync(CustomerFactory.Create(dto));

        if (createdEntity != null)
        {
            return ResultT<CustomerModel>.Created(CustomerFactory.Create(createdEntity));
        }
        else
        {
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

    public Task<ResultT<CustomerModel>> GetCustomerAsync(string name)
    {
        throw new NotImplementedException();
    }

    // UPDATE
    public Task<ResultT<CustomerModel>> UpdateCustomerAsync(int id, CustomerUpdateDto updateDto)
    {
        throw new NotImplementedException();
    }

    // DELETE
    public async Task<Result> DeleteCustomerAsync(int id)
    {
        var result = await _customerRepository.DeleteAsync(c => c.CustomerId == id);
        return result ? Result.NoContent() : Result.Error("Unable to delete customer.");
    }
}
