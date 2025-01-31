namespace Business.Models;

public class CustomerModel
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = null!;
    public ContactPersonModel ContactPerson { get; set; } = null!;
}
