using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class CustomerUpdateDto
{
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "Customer name is required.")]
    [StringLength(100, ErrorMessage = "Customer name cannot exceed 100 characters.")]
    public string CustomerName { get; set; } = null!;

    public int? ContactPersonId { get; set; }
}
