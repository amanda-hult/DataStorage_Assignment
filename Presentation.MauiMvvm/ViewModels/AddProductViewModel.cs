using Business.Dtos;
using Business.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Presentation.MauiMvvm.ViewModels;

public partial class AddProductViewModel : ObservableObject
{
    private readonly IProductService _productService;

    public AddProductViewModel(IProductService productService)
    {
        _productService = productService;
    }

    [ObservableProperty]
    private ProductCreateDto _productCreateDto = new();

    [ObservableProperty]
    private string _statusMessage = null!;

    [RelayCommand]
    public async Task AddProduct()
    {
        var result = await _productService.CreateProductAsync(ProductCreateDto);
        if (result.Success)
        {
            StatusMessage = "Service was added successfully";
            ProductCreateDto = new ProductCreateDto();
        }
        else
        {
            StatusMessage = result.Message ?? "Service could not be added.";
        }
    }

}
