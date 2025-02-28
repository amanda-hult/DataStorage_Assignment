﻿using System.ComponentModel.DataAnnotations;
using Business.Dtos;
using Business.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Presentation.MauiMvvm.ViewModels;

public partial class AddProductViewModel(IProductService productService) : ObservableObject
{
    private readonly IProductService _productService = productService;

    #region Observable properties
    [ObservableProperty]
    private ProductCreateDto _productCreateDto = new();

    [ObservableProperty]
    private string _statusMessage = null!;

    [ObservableProperty]
    private Color _statusMessageColor = Colors.Black;
    #endregion

    [RelayCommand]
    public async Task AddProduct()
    {
        if (ProductCreateDto == null)
        {
            StatusMessage = "Inavlid data.";
            StatusMessageColor = Colors.Firebrick;
            return;
        }

        var validationContext = new ValidationContext(ProductCreateDto);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(ProductCreateDto,validationContext, validationResults, true);

        if (!isValid)
        {
            StatusMessage = string.Join("\n", validationResults.Select(x => x.ErrorMessage));
            StatusMessageColor = Colors.Firebrick;
            return;
        }

        var result = await _productService.CreateProductAsync(ProductCreateDto);
        if (result.StatusCode == 409)
        {
            StatusMessage = "A service with the same name and currency already exists.";
            StatusMessageColor = Colors.Firebrick;
            return;
        }
        if (result.Success)
        {
            StatusMessage = "Service was added successfully";
            StatusMessageColor = Colors.Black;
            ProductCreateDto = new ProductCreateDto();
        }
        else
        {
            StatusMessage = result.Message ?? "Service could not be added.";
            StatusMessageColor = Colors.Firebrick;
        }
    }
}
