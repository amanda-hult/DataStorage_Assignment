using System.Linq.Expressions;
using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class UserRepository(DataContext context) : BaseRepository<UserEntity>(context), IUserRepository
{
    public async Task<UserEntity> GetUserWithProjectsAsync(Expression<Func<UserEntity, bool>> expression)
    {
        if (expression == null)
            throw new ArgumentNullException(nameof(expression), "Expression cannot be null.");

        return await _dbSet
            .Include(u => u.Projects)
                .ThenInclude(p => p.Customer)
            .Include(u => u.Projects)
                .ThenInclude(p => p.Status)
            .FirstOrDefaultAsync(expression) ?? null!;
    }
}
