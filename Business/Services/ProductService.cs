using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Models.Responses;
using Data.Interfaces;

namespace Business.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    private readonly IProductRepository _productRepository = productRepository;

    // CREATE
    public async Task<ResultT<ProductModel>> CreateProductAsync(ProductCreateDto dto)
    {
        //check if product already exists
        var exists = await _productRepository.AlreadyExistsAsync(p => p.ProductName == dto.ProductName && p.Currency == dto.Currency);
        if (exists)
            return ResultT<ProductModel>.Conflict("A product with the same name and currency already exists.");

        //create new product
        var createdEntity = await _productRepository.CreateAsync(ProductFactory.Create(dto));

        if (createdEntity != null)
        {
            return ResultT<ProductModel>.Created(ProductFactory.Create(createdEntity));
        }
        else
        {
            return ResultT<ProductModel>.Error("An error occured while creating the product.");
        }
    }

    // READ
    public async Task<ResultT<IEnumerable<ProductModel>>> GetAllProductsAsync()
    {
        var products = (await _productRepository.GetAllAsync()).Select(ProductFactory.Create).ToList();


        if (products.Count == 0)
            return ResultT<IEnumerable<ProductModel>>.NotFound("No products found.");


        return ResultT<IEnumerable<ProductModel>>.Ok(products);
    }

    public async Task<ResultT<ProductModel>> GetProductAsync(string name)
    {
        var productEntity = await _productRepository.GetAsync(p => p.ProductName == name);

        if (productEntity == null)
            return ResultT<ProductModel>.NotFound("Product not found.");

        return ResultT<ProductModel>.Ok(ProductFactory.Create(productEntity));
    }


    // UPDATE
    public async Task<ResultT<ProductModel>> UpdateProductAsync(int id, ProductUpdateDto updateDto)
    {
        // get product entity
        var productEntity = await _productRepository.GetAsync(p => p.ProductId == id);

        if (productEntity == null)
            return ResultT<ProductModel>.NotFound("Product not found.");

        //update product entity
        ProductFactory.Update(productEntity, updateDto);
        var updatedProduct = await _productRepository.UpdateAsync(p => p.ProductId == id, productEntity);

        return ResultT<ProductModel>.Ok(ProductFactory.Create(updatedProduct));
    }

    // DELETE
    public async Task<Result> DeleteProductAsync(int id)
    {
        // get product entity
        var productEntity = await _productRepository.GetAsync(p => p.ProductId == id);

        if (productEntity == null)
            return Result.NotFound("Product not found.");

        //delete product
        var result = await _productRepository.DeleteAsync(p => p.ProductId == id);
        return result ? Result.NoContent() : Result.Error("Unable to delete product.");
    }
}
