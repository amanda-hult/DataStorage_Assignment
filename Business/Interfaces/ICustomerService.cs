using Business.Dtos;
using Business.Models.Responses;
using Business.Models;
using Data.Entities;

namespace Business.Interfaces;

public interface ICustomerService
{
    Task<ResultT<CustomerModel>> CreateCustomerAsync(CustomerCreateDto dto);
    Task<ResultT<IEnumerable<CustomerModel>>> GetAllCustomersAsync();
    Task<ResultT<CustomerModel>> GetCustomerByIdAsync(int id);
    Task<ResultT<CustomerEntity>> GetCustomerEntityByIdAsync(int id);
    Task<ResultT<CustomerEntity>> GetCustomerEntityByNameAsync(string name);
    Task<ResultT<CustomerModel>> UpdateCustomerAsync(CustomerUpdateDto updateDto);
    Task<Result> DeleteCustomerAsync(int id);
}
