using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class ProductFactory
{
    public static ProductEntity Create(ProductCreateDto dto)
    {
        return new ProductEntity
        {
            ProductName = dto.ProductName,
            Price = dto.Price,
            Currency = dto.Currency
        };
    }

    public static ProductModel Create(ProductEntity entity)
    {
        return new ProductModel
        {
            ProductId = entity.ProductId,
            ProductName = entity.ProductName,
            Price = entity.Price,
            Currency = entity.Currency
        };
    }

    public static void Update(ProductEntity entity, ProductUpdateDto dto)
    {
        entity.ProductName = dto.ProductName;
        entity.Price = dto.Price;
        entity.Currency = dto.Currency;
    }
}
