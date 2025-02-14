using Business.Models;
using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class ProjectUpdateDto
{
    [Required]
    public int ProjectId { get; set; }

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
    public UserModel? User { get; set; }


    [Required(ErrorMessage = "Customer is required.")]
    public int CustomerId { get; set; }
    public CustomerModel? Customer { get; set; }


    public int ContactPersonId { get; set; }
    public ContactPersonCreateDto ContactPerson { get; set; } = new();


    [Required(ErrorMessage = "At least one service must be selected.")]
    public List<ProjectProductDto> ProjectProducts { get; set; } = new();
}
