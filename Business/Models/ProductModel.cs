namespace Business.Models;

public class ProductModel
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public decimal Price { get; set; }
    public string Currency { get; set; } = null!;
}
