﻿using System.ComponentModel.DataAnnotations;
using Business.Models;

namespace Business.Dtos;

public class ProjectCreateDto
{
    [Required(ErrorMessage = "Project name is required.")]
    [StringLength(50, ErrorMessage = "Product name cannot exceed 50 characters.")]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    [Required(ErrorMessage = "Start date is required.")]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }


    [Required(ErrorMessage = "Status is required.")]
    public int StatusId { get; set; }

    [Required(ErrorMessage = "Project manager is required.")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "User information is required.")]
    public UserModel User { get; set; } = null!;


    [Required(ErrorMessage = "Customer is required.")]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "Customer information is required.")]
    public CustomerModel Customer { get; set; } = null!;


    public int ContactPersonId { get; set; }
    public ContactPersonCreateDto ContactPerson { get; set; } = new ContactPersonCreateDto();


    [Required(ErrorMessage ="At least one service must be selected.")]
    public List<ProjectProductDto> ProjectProducts { get; set; } = new();
}
