using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[Index(nameof(CustomerName), IsUnique = true)]
public class CustomerEntity
{
    [Key]
    public int CustomerId { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(100)")]
    public string CustomerName { get; set; } = null!;


    public int ContactPersonId { get; set; }
    [ForeignKey("ContactPersonId")]
    public ContactPersonEntity ContactPerson { get; set; } = null!;

    public ICollection<ProjectEntity> Projects { get; set; } = new List<ProjectEntity>();
}
