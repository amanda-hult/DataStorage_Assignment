using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProjectProductEntity
{
    [Column(TypeName = "varchar(5)")]
    public int? Hours { get; set; }

    public int ProjectId { get; set; }
    [ForeignKey("ProjectId")]
    public ProjectEntity Project { get; set; } = null!;

    public int ProductId { get; set; }
    [ForeignKey("ProductId")]
    public ProductEntity Product { get; set; } = null!;
}
