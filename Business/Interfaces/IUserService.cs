using Business.Dtos;
using Business.Models;
using Business.Models.Responses;

namespace Business.Interfaces;

public interface IUserService
{
    Task<ResultT<UserModel>> CreateUserAsync(UserCreateDto dto);
    Task<ResultT<IEnumerable<UserModel>>> GetAllUsersAsync();
    Task<ResultT<UserModel>> GetUserByIdAsync(int id);
    Task<ResultT<UserModel>> UpdateUserAsync(UserUpdateDto dto);
    Task<Result> DeleteUserAsync(int id);
}
