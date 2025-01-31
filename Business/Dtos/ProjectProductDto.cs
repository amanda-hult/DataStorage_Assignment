using System.ComponentModel.DataAnnotations;
using Business.Models;

namespace Business.Dtos;

public class ProjectProductDto
{
    [Range(0, int.MaxValue, ErrorMessage = "Hours must be greater than 0.")]
    public int? Hours { get; set; }

    //public int ProjectId { get; set; }
    //public ProjectEntity Project { get; set; } = null!;

    public int ProductId { get; set; }
    public ProductModel? Product { get; set; }
}
