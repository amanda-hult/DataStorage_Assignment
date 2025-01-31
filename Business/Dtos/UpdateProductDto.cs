using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class UpdateProductDto
{
    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(50, ErrorMessage = "Product name cannot exceed 50 characters.")]
    public string ProductName { get; set; } = null!;

    [Required(ErrorMessage = "Price is required.")]
    // Add validation for price
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Currency is required.")]
    [StringLength(3, ErrorMessage = "Currency must be exactly 3 characters.")]
    public string Currency { get; set; } = null!;
}
