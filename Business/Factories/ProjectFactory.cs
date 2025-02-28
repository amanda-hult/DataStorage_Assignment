﻿using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class ProjectFactory
{
    public static ProjectUpdateDto Create(DetailedProjectModel model)
    {
        return new ProjectUpdateDto
        {
            ProjectId = model.ProjectId,
            Title = model.Title,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            StatusId = model.Status.StatusId,
            UserId = model.User.UserId,
            User = new UserModel
            {
                UserId = model.User.UserId,
                FirstName = model.User.FirstName,
                LastName = model.User.LastName,
                Email = model.User.Email,
            },
            CustomerId = model.Customer.CustomerId,
            Customer = new CustomerModel
            {
                CustomerId = model.Customer.CustomerId,
                CustomerName = model.Customer.CustomerName,
                ContactPerson = model.ContactPerson != null ? new ContactPersonModel
                {
                    ContactPersonId = model.ContactPerson.ContactPersonId,
                    FirstName = model.ContactPerson.FirstName,
                    LastName = model.ContactPerson.LastName,
                    Email = model.ContactPerson.Email,
                    Phone = model.ContactPerson.Phone,
                } : null
            },
            ContactPersonId = model.ContactPerson!.ContactPersonId,
            ContactPerson = new ContactPersonUpdateDto
            {
                ContactPersonId = model.ContactPerson.ContactPersonId,
                FirstName = model.ContactPerson.FirstName,
                LastName = model.ContactPerson.LastName,
                Email = model.ContactPerson.Email,
                Phone = model.ContactPerson.Phone,
            },
            ProjectProducts = model.ProjectProducts.Select(pp => new ProjectProductDto
            {
                ProductId = pp.Product!.ProductId,
                Hours = pp.Hours
            }).ToList(),
        };  
    }

    public static BasicProjectModel CreateBasicProjectModel(ProjectEntity entity)
    {
        return new BasicProjectModel
        {
            ProjectId = entity.ProjectId,
            Title = entity.Title,
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
            }
        };
    }

    public static ProjectEntity Create(ProjectCreateDto project, UserEntity user, CustomerEntity customer, StatusEntity status, List<ProductEntity> products)
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
                Product = products.FirstOrDefault(p => p.ProductId == pp.ProductId)
            }).ToList()
        };
    }

    public static DetailedProjectModel CreateDetailedProjectModel(ProjectEntity entity)
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

    public static void Update(ProjectEntity entity, ProjectUpdateDto dto)
    {
        entity.ProjectId = dto.ProjectId;
        entity.Title = dto.Title;
        entity.Description = dto.Description;
        entity.StartDate = dto.StartDate;
        entity.EndDate = dto.EndDate;
        entity.Customer.ContactPerson.ContactPersonId = dto.ContactPersonId;
        entity.Customer.ContactPerson.FirstName = dto.ContactPerson.FirstName;
        entity.Customer.ContactPerson.LastName = dto.ContactPerson.LastName;
        entity.Customer.ContactPerson.Email = dto.ContactPerson.Email;
        entity.Customer.ContactPerson.Phone = dto.ContactPerson.Phone;

        entity.ProjectProducts = dto.ProjectProducts.Select(p => new ProjectProductEntity
        {
            ProductId = p.ProductId,
            Hours = p.Hours,
        }).ToList();
    }
}
