using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class ProductUpdateDto
{
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Service name is required.")]
    [StringLength(50, ErrorMessage = "Service name cannot exceed 50 characters.")]
    public string ProductName { get; set; } = null!;

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, 9999999999999999.99, ErrorMessage = "Invalid price.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Currency is required.")]
    [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "Currency must be exactly 3 characters.")]
    public string Currency { get; set; } = null!;
}
