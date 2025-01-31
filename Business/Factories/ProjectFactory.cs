using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class ProjectFactory
{
    public static BasicProjectModel Show(ProjectEntity entity)
    {
        return new BasicProjectModel
        {
            Title = entity.Title,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            CustomerName = entity.Customer.CustomerName,
            StatusName = entity.Status.StatusName
        };
    }

    public static DetailedProjectModel Create(ProjectEntity entity)
    {
        return new DetailedProjectModel
        {
            ProjectId = entity.ProjectId,
            Title = entity.Title,
            Description = entity.Description,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,

            Status = new StatusDto
            {
                StatusId = entity.Status.StatusId,
                StatusName = entity.Status.StatusName,
            },

            Customer = new CustomerModel
            {
                CustomerId = entity.Customer.CustomerId,
                CustomerName = entity.Customer.CustomerName,
            },

            ContactPerson = new ContactPersonModel
            {
                ContactPersonId = entity.Customer.ContactPerson.ContactPersonId,
                FirstName = entity.Customer.ContactPerson.FirstName,
                LastName = entity.Customer.ContactPerson.LastName,
                Email = entity.Customer.ContactPerson.Email,
                Phone = entity.Customer.ContactPerson.Phone,
            },

            User = new UserModel
            {
                UserId = entity.User.UserId,
                FirstName = entity.User.FirstName,
                LastName = entity.User.LastName,
                Email = entity.User.Email,
            },

            ProjectProducts = entity.ProjectProducts.Select(pp => new ProjectProductDto
            {
                ProductId = pp.Product.ProductId,
                Hours = pp.Hours,
                Product = new ProductModel
                {
                    ProductId = pp.Product.ProductId,
                    ProductName = pp.Product.ProductName,
                    Price = pp.Product.Price,
                    Currency = pp.Product.Currency,
                }
            }).ToList()
        };
    }

    public static ProjectEntity Create(CreateProjectDto project, UserEntity user, CustomerEntity customer, StatusEntity status)
    {
        return new ProjectEntity
        {
            Title = project.Title,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            StatusId = status.StatusId,
            Status = status,
            UserId = user.UserId,
            User = user,
            CustomerId = customer.CustomerId,
            Customer = customer,
            ProjectProducts = project.ProjectProducts.Select(pp => new ProjectProductEntity
            {
                ProductId = pp.ProductId,
                Hours = pp.Hours,
            }).ToList()
        };
    }
}
