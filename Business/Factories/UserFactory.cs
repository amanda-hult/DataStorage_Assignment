using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class UserFactory
{
    public static UserEntity Create(UserCreateDto dto)
    {
        return new UserEntity
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email
        };
    }

    public static UserModel Create(UserEntity entity)
    {
        return new UserModel
        {
            UserId = entity.UserId,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
        };
    }

    public static UserWithProjectDto ShowWithProjectDetails(UserEntity entity)
    {
        return new UserWithProjectDto
        {
            UserId = entity.UserId,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Projects = entity.Projects.Select(p => new BasicProjectModel
            {
                Title = p.Title,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                CustomerName = p.Customer.CustomerName,
                StatusName = p.Status.StatusName
            }).ToList()
        };
    }

    public static void Update(UserEntity entity, UserUpdateDto dto)
    {
        entity.UserId = dto.UserId;
        entity.FirstName = dto.FirstName;
        entity.LastName = dto.LastName;
        entity.Email = dto.Email;
    }
}
