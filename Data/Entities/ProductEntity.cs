using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[Index(nameof(ProductName), nameof(Currency), IsUnique = true)]
public class ProductEntity
{
    [Key]
    public int ProductId { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string ProductName { get; set; } = null!;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required]
    [Column(TypeName = "char(3)")]
    public string Currency {  get; set; } = null!;

    public ICollection<ProjectProductEntity> ProjectProducts { get; set; } = new List<ProjectProductEntity>();
}
