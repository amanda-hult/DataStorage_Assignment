using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ProductRepository(DataContext context) : BaseRepository<ProductEntity>(context), IProductRepository
{
    public async Task<List<ProductEntity>> GetProductsByIdAsync(List<int> productIds)
    {
        return await _dbSet.Where(p => productIds.Contains(p.ProductId)).ToListAsync();
    }
}
