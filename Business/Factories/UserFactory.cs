using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class UserFactory
{
    public static UserWithProjectDto Create(UserEntity entity)
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

    public static UserEntity Create(CreateUserDto dto)
    {
        return new UserEntity
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email
        };
    }
}
