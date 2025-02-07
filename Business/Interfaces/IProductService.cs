using Business.Dtos;
using Business.Models.Responses;
using Business.Models;

namespace Business.Interfaces;

public interface IProductService
{
    Task<ResultT<ProductModel>> CreateProductAsync(ProductCreateDto dto);
    Task<ResultT<IEnumerable<ProductModel>>> GetAllProductsAsync();

    Task<ResultT<ProductModel>> GetProductAsync(string name);

    Task<ResultT<ProductModel>> UpdateProductAsync(int id, ProductUpdateDto updateDto);
    Task<Result> DeleteProductAsync(int id);
}
