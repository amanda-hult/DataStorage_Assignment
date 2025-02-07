using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class CustomerCreateDto
{
    [Required(ErrorMessage = "Customername is required.")]
    [StringLength(100, ErrorMessage = "Customername cannot exceed 100 characters.")]
    public string CustomerName { get; set; } = null!;

    public int? ContactPersonId { get; set; }
}
