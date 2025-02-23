using Business.Dtos;

namespace Business.Models;

public class BasicProjectModel
{
    public int ProjectId { get; set; }
    public string Title { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public CustomerModel Customer { get; set; } = null!;
    public StatusDto Status { get; set; } = null!;
}
