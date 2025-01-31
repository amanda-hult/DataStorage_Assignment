using Business.Dtos;

namespace Business.Models;

public class DetailedProjectModel
{
    public int ProjectId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public StatusDto Status { get; set; } = null!;
    public CustomerModel Customer { get; set; } = null!;
    public ContactPersonModel ContactPerson { get; set; } = null!;
    public UserModel User { get; set; } = null!;

    public List<ProjectProductDto> ProjectProducts { get; set; } = new List<ProjectProductDto>();
}
