using System.Linq.Expressions;
using Data.Entities;

namespace Data.Interfaces;

public interface ICustomerRepository : IBaseRepository<CustomerEntity>
{
    Task<CustomerEntity> GetCustomerWithProjectsAsync(Expression<Func<CustomerEntity, bool>> expression);
}
