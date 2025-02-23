using Business.Dtos;
using Business.Models.Responses;
using Business.Models;
using Data.Entities;

namespace Business.Interfaces;

public interface IProductService
{
    Task<ResultT<ProductModel>> CreateProductAsync(ProductCreateDto dto);
    Task<ResultT<IEnumerable<ProductModel>>> GetAllProductsAsync();
    Task<ResultT<List<ProductEntity>>> GetProductEntitiesByIdAsync(List<int> ids);
    Task<ResultT<ProductModel>> GetProductByIdAsync(int id);
    Task<ResultT<ProductModel>> UpdateProductAsync(ProductUpdateDto updateDto);
    Task<Result> DeleteProductAsync(int id);
}
