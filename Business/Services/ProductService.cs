﻿using System.Diagnostics;
using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Models.Responses;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class ProductService(IProductRepository productRepository, IProjectRepository projectRepository) : IProductService
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IProjectRepository _projectRepository = projectRepository;

    // CREATE
    public async Task<ResultT<ProductModel>> CreateProductAsync(ProductCreateDto dto)
    {
        //check if product already exists
        string lowerCaseName = dto.ProductName.ToLower();
        bool exists = await _productRepository.AlreadyExistsAsync(p => p.ProductName.ToLower() == lowerCaseName && p.Currency == dto.Currency);
        if (exists)
            return ResultT<ProductModel>.Conflict("A product with the same name and currency already exists.");

        await _productRepository.BeginTransactionAsync();

        //create new product
        try
        {
            var createdEntity = await _productRepository.CreateAsync(ProductFactory.Create(dto));

            if (createdEntity == null)
                throw new Exception("Failed to create product entity.");

            await _productRepository.SaveAsync();
            await _productRepository.CommitTransactionAsync();
            return ResultT<ProductModel>.Created(ProductFactory.Create(createdEntity));
        }
        catch (Exception ex)
        {
            await _productRepository.RollBackTransactionAsync();
            Debug.WriteLine($"Error creating product: {ex.Message}");
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

    public async Task<ResultT<List<ProductEntity>>> GetProductEntitiesByIdAsync(List<int> ids)
    {
        var products = await _productRepository.GetProductsByIdAsync(ids);

        if (products == null)
            return ResultT<List<ProductEntity>>.NotFound("Products not found.");

        return ResultT<List<ProductEntity>>.Ok(products);
    }

    public async Task<ResultT<ProductModel>> GetProductByIdAsync(int id)
    {
        var productEntity = await _productRepository.GetAsync(u => u.ProductId == id);

        if (productEntity == null)
            return ResultT<ProductModel>.NotFound("Product not found.");

        return ResultT<ProductModel>.Ok(ProductFactory.Create(productEntity));
    }

    // UPDATE
    public async Task<ResultT<ProductModel>> UpdateProductAsync(ProductUpdateDto dto)
    {
        // get product entity
        var productEntity = await _productRepository.GetAsync(p => p.ProductId == dto.ProductId);

        if (productEntity == null)
            return ResultT<ProductModel>.NotFound("Product not found.");

        //update product entity
        ProductFactory.Update(productEntity, dto);
        var updatedProduct = await _productRepository.UpdateAsync(p => p.ProductId == dto.ProductId, productEntity);

        return ResultT<ProductModel>.Ok(ProductFactory.Create(updatedProduct));
    }

    // DELETE
    public async Task<Result> DeleteProductAsync(int id)
    {
        // get product entity
        var productEntity = await _productRepository.GetAsync(p => p.ProductId == id);
        if (productEntity == null)
            return Result.NotFound("Product not found.");

        // check if product exists in any project
        bool existsInProject = await _projectRepository.AlreadyExistsAsync(p => p.ProjectProducts.Any(p => p.ProductId == id));
        if (existsInProject)
            return Result.Error("Product exists in a project and cannot be deleted.");

        //delete product
        var result = await _productRepository.DeleteAsync(p => p.ProductId == id);
        return result ? Result.NoContent() : Result.Error("Unable to delete product.");
    }
}
