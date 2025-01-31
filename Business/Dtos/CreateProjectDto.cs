using Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class CreateProjectDto
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


    public int? UserId { get; set; }
    public CreateUserDto? User { get; set; }


    public int? CustomerId { get; set; }
    public CreateCustomerDto? Customer { get; set; }


    public int? ContactPersonId { get; set; }
    public CreateContactPersonDto? ContactPerson { get; set; }



    public List<ProjectProductDto> ProjectProducts { get; set; } = new();
}
