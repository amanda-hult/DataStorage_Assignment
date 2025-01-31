using Data.Entities;

namespace Data.Interfaces;

public interface IProductRepository : IBaseRepository<ProductEntity>
{
    Task<List<ProductEntity>> GetProductsByIdAsync(List<int> productIds);
}
