using Business.Models;

namespace Business.Dtos;

public class CustomerWithProjectDto
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = null!;
    public List<BasicProjectModel> Projects { get; set; } = new List<BasicProjectModel>();
}
