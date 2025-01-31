using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class CustomerFactory
{
    public static CustomerWithProjectDto Create(CustomerEntity entity)
    {
        return new CustomerWithProjectDto
        {
            CustomerId = entity.CustomerId,
            CustomerName = entity.CustomerName,
            Projects = entity.Projects.Select(p => new BasicProjectModel
            {
                Title = p.Title,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                StatusName = p.Status.StatusName
            }).ToList()
        };
    }


    public static CustomerEntity Create(CreateCustomerDto dto, ContactPersonEntity contactPerson)
    {
        return new CustomerEntity
        {
            CustomerName = dto.CustomerName,
            ContactPersonId = contactPerson.ContactPersonId,
            ContactPerson = contactPerson
        };
    }
}
