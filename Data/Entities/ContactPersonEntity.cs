using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[Index(nameof(Email), IsUnique = true)]
public class ContactPersonEntity
{
    [Key]
    public int ContactPersonId { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string FirstName { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string LastName { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(150)")]
    public string Email { get; set; } = null!;

    [Required]
    [Column(TypeName = "varchar(20)")]
    public string Phone { get; set; } = null!;


    public ICollection<CustomerEntity> Customers { get; set; } = new List<CustomerEntity>();
}
