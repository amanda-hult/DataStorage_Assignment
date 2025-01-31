namespace Business.Models;

public class BasicProjectModel
{
    public string Title { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? CustomerName { get; set; }
    public string StatusName { get; set; } = null!;
}
