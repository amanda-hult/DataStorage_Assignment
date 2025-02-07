using Business.Dtos;
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
}
