﻿using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class CustomerFactory
{
    public static CustomerEntity Create(CustomerCreateDto dto)
    {
        return new CustomerEntity
        {
            CustomerName = dto.CustomerName,
            ContactPersonId = dto.ContactPersonId
        };
    }

    public static CustomerModel Create(CustomerEntity entity)
    {
        return new CustomerModel
        {
            CustomerId = entity.CustomerId,
            CustomerName = entity.CustomerName,
        };
    }

    //public static CustomerEntity Create(CustomerModel model)
    //{
    //    return new CustomerEntity
    //    {
    //        CustomerId = model.CustomerId,
    //        CustomerName = model.CustomerName,
    //    };
    //}


    //public static CustomerWithProjectDto Create(CustomerEntity entity)
    //{
    //    return new CustomerWithProjectDto
    //    {
    //        CustomerId = entity.CustomerId,
    //        CustomerName = entity.CustomerName,
    //        Projects = entity.Projects.Select(p => new BasicProjectModel
    //        {
    //            Title = p.Title,
    //            StartDate = p.StartDate,
    //            EndDate = p.EndDate,
    //            StatusName = p.Status.StatusName
    //        }).ToList()
    //    };
    //}


    public static void Connect(CustomerEntity customer, ContactPersonEntity contactPerson)
    {
        customer.ContactPersonId = contactPerson.ContactPersonId;
        customer.ContactPerson = contactPerson;
    }

    public static void Update(CustomerEntity entity, CustomerUpdateDto dto)
    {
        entity.CustomerId = dto.CustomerId;
        entity.CustomerName = dto.CustomerName;
    }
}
