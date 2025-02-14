using Business.Dtos;
using Business.Interfaces;
using Business.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Presentation.MauiMvvm.ViewModels;

public partial class EditProductViewModel : ObservableObject, IQueryAttributable
{
    private readonly IProductService _productService;

    public EditProductViewModel(IProductService productService)
    {
        _productService = productService;
    }

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private ProductUpdateDto _productUpdateDto = new ProductUpdateDto();

    [RelayCommand]
    public async Task UpdateProduct()
    {
        if (_productUpdateDto == null)
            return;

        var result = await _productService.UpdateProductAsync(_productUpdateDto);

        if (result.Success)
        {
            StatusMessage = "Product updated successfully.";
        }
        else
        {
            StatusMessage = result.Message ?? "Failed to update product.";
        }
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {

        if (query.TryGetValue("ProductId", out var productIdObj) && productIdObj is string productIdStr && int.TryParse(productIdStr, out int productId))
        {
            var result = await _productService.GetProductByIdAsync(productId);
            if (result.Success && result.Data != null)
            {
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
