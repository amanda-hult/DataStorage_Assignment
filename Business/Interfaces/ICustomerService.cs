using Business.Dtos;
using Business.Models.Responses;
using Business.Models;

namespace Business.Interfaces;

public interface ICustomerService
{
    Task<ResultT<CustomerModel>> CreateCustomerAsync(CustomerCreateDto dto);
    Task<ResultT<IEnumerable<CustomerModel>>> GetAllCustomersAsync();
    Task<ResultT<CustomerModel>> GetCustomerAsync(string name);
    Task<ResultT<CustomerModel>> UpdateCustomerAsync(int id, CustomerUpdateDto updateDto);
    Task<Result> DeleteCustomerAsync(int id);
}
