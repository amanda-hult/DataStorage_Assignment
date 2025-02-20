using System.Diagnostics;
using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Models.Responses;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;

namespace Business.Services;

public class UserService(IUserRepository userRepository, IProjectRepository projectRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IProjectRepository _projectRepository = projectRepository;

    // CREATE
    public async Task<ResultT<UserModel>> CreateUserAsync(UserCreateDto dto)
    {
        // check if user already exists
        var exists = await _userRepository.AlreadyExistsAsync(u => u.Email == dto.Email);
        if (exists)
            return ResultT<UserModel>.Conflict("A user with the same email already exists.");

        await _userRepository.BeginTransactionAsync();

        // create new user
        try
        {
            var createdEntity = await _userRepository.CreateAsync(UserFactory.Create(dto));

            if (createdEntity == null)
                throw new Exception("Failed to create user entity.");

            await _userRepository.SaveAsync();
            await _userRepository.CommitTransactionAsync();
            return ResultT<UserModel>.Created(UserFactory.Create(createdEntity));
        }
        catch (Exception ex)
        {
            await _userRepository.RollBackTransactionAsync();
            Debug.WriteLine($"Error creating user: {ex.Message}");
            return ResultT<UserModel>.Error("An error occured while creating the user.");
        }
    }

    // READ
    public async Task<ResultT<IEnumerable<UserModel>>> GetAllUsersAsync()
    {
        var users = (await _userRepository.GetAllAsync()).Select(UserFactory.Create).ToList();

        if (users.Count == 0)
            return ResultT<IEnumerable<UserModel>>.NotFound("No users found.");

        return ResultT<IEnumerable<UserModel>>.Ok(users);
    }

    public async Task<ResultT<UserModel>> GetUserByIdAsync(int id)
    {
        var userEntity = await _userRepository.GetAsync(u => u.UserId == id);

        if (userEntity == null)
            return ResultT<UserModel>.NotFound("User not found.");

        return ResultT<UserModel>.Ok(UserFactory.Create(userEntity));
    }

    public async Task<ResultT<UserEntity>> GetUserEntityByIdAsync(int id)
    {
        var userEntity = await _userRepository.GetAsync(u => u.UserId == id);

        if (userEntity == null)
            return ResultT<UserEntity>.NotFound("User not found.");

        return ResultT<UserEntity>.Ok(userEntity);
    }

    public async Task<ResultT<UserEntity>> GetUserEntityByEmailAsync(string email)
    {
        var userEntity = await _userRepository.GetAsync(u => u.Email == email);

        if (userEntity == null)
            return ResultT<UserEntity>.NotFound("User not found.");

        return ResultT<UserEntity>.Ok(userEntity);
    }


    // UPDATE
    public async Task<ResultT<UserModel>> UpdateUserAsync(UserUpdateDto dto)
    {
        // get user entity
        var userEntity = await _userRepository.GetAsync(u => u.UserId == dto.UserId);

        if (userEntity == null)
            return ResultT<UserModel>.NotFound("User not found.");

        // update user entity
        UserFactory.Update(userEntity, dto);
        var updatedUser = await _userRepository.UpdateAsync(u => u.UserId == dto.UserId, userEntity);

        return ResultT<UserModel>.Ok(UserFactory.Create(updatedUser));
    }

    // DELETE
    public async Task<Result> DeleteUserAsync(int id)
    {
        // get user entity
        var userEntity = await _userRepository.GetAsync(u => u.UserId == id);

        if (userEntity == null)
            return Result.NotFound("User not found.");

        // check if user exists in any project
        bool exists = await _projectRepository.AlreadyExistsAsync(u => u.UserId == id);
        if (exists)
            return Result.Error("User exists in a project and cannot be deleted.");

        // delete user
        var result = await _userRepository.DeleteAsync(u => u.UserId == id);
        return result ? Result.NoContent() : Result.Error("Unable to delete user.");
    }
}
