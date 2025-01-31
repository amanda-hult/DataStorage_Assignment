using Data.Entities;
using System.Linq.Expressions;

namespace Data.Interfaces;

public interface IUserRepository : IBaseRepository<UserEntity>
{
    Task<UserEntity> GetUserWithProjectsAsync(Expression<Func<UserEntity, bool>> expression);
}
