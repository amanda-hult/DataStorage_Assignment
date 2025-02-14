using Business.Dtos;
using Business.Models;
using Business.Models.Responses;
using Data.Entities;

namespace Business.Interfaces;

public interface IUserService
{
    Task<ResultT<UserModel>> CreateUserAsync(UserCreateDto dto);
    Task<ResultT<IEnumerable<UserModel>>> GetAllUsersAsync();
    Task<ResultT<UserModel>> GetUserByIdAsync(int id);
    Task<ResultT<UserEntity>> GetUserEntityByEmailAsync(string email);
    Task<ResultT<UserModel>> UpdateUserAsync(UserUpdateDto dto);
    Task<Result> DeleteUserAsync(int id);
}
