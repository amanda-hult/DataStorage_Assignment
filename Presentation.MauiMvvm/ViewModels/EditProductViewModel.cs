﻿using System.ComponentModel.DataAnnotations;
using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Presentation.MauiMvvm.ViewModels;

public partial class EditProductViewModel(IProductService productService) : ObservableObject, IQueryAttributable
{
    private readonly IProductService _productService = productService;

    #region Observable properties
    [ObservableProperty]
    private ProductUpdateDto _productUpdateDto = new ProductUpdateDto();

    [ObservableProperty]
    private ProductModel _productModel;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private Color _statusMessageColor = Colors.Black;
    #endregion

    [RelayCommand]
    public async Task UpdateProduct()
    {
        if (ProductUpdateDto == null)
            return;

        var validationContext = new ValidationContext(ProductModel);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(ProductModel, validationContext, validationResults, true);
        if (!isValid)
        {
            StatusMessage = string.Join("\n", validationResults.Select(x => x.ErrorMessage));
            StatusMessageColor = Colors.Firebrick;
            return;
        }

        var result = await _productService.UpdateProductAsync(ProductUpdateDto);
        if (result.StatusCode == 409)
        {
            StatusMessage = "A service with the same name and currency already exists.";
            StatusMessageColor = Colors.Firebrick;
            return;
        }
        if (result.Success)
        {
            StatusMessage = "Product updated successfully.";
            StatusMessageColor = Colors.Black;
        }
        else
        {
            StatusMessage = result.Message ?? "Failed to update product.";
            StatusMessageColor = Colors.Firebrick;
        }
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("ProductId", out var productIdObj) && productIdObj is string productIdStr && int.TryParse(productIdStr, out int productId))
        {
            var result = await _productService.GetProductByIdAsync(productId);
            if (result.Success && result.Data != null)
            {
                ProductModel = result.Data;
                ProductUpdateDto = new ProductUpdateDto
                {
                    ProductId = result.Data.ProductId,
                    ProductName = result.Data.ProductName,
                    Price = result.Data.Price,
                    Currency = result.Data.Currency
                };
            }
            else
            {
                StatusMessage = "Product not found.";
            }
        }
    }
}
