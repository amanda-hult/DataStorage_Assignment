using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class ContactPersonFactory
{
    public static ContactPersonEntity Create(ContactPersonCreateDto dto)
    {
        return new ContactPersonEntity
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone
        };
    }

    public static ContactPersonModel Create(ContactPersonEntity entity)
    {
        return new ContactPersonModel
        {
            ContactPersonId = entity.ContactPersonId,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            Phone = entity.Phone
        };
    }
}
