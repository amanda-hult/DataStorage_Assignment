using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[Index(nameof(Title), nameof(CustomerId), IsUnique = true)]
public class ProjectEntity
{
    [Key]
    public int ProjectId { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string Title { get; set; } = null!;

    [Column(TypeName = "TEXT")]
    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "date")]
    public DateTime StartDate { get; set; }


    [Column(TypeName = "date")]
    public DateTime? EndDate { get; set; }



    public int StatusId { get; set; }
    [ForeignKey("StatusId")]
    public StatusEntity Status { get; set; } = null!;


    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public UserEntity User { get; set; } = null!;


    public int CustomerId { get; set; }
    [ForeignKey("CustomerId")]
    public CustomerEntity Customer { get; set; } = null!;


    public ICollection<ProjectProductEntity> ProjectProducts { get; set; } = new List<ProjectProductEntity>();
}
