using System.Linq.Expressions;
using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class CustomerRepository(DataContext context) : BaseRepository<CustomerEntity>(context), ICustomerRepository
{
    public async Task<CustomerEntity> GetCustomerWithProjectsAsync(Expression<Func<CustomerEntity, bool>> expression)
    {
        if (expression == null)
            throw new ArgumentNullException(nameof(expression), "Expression cannot be null.");

        return await _dbSet
            .Include(c => c.Projects)
                .ThenInclude(p => p.Status)
            .FirstOrDefaultAsync(expression) ?? null!;
    }
}
