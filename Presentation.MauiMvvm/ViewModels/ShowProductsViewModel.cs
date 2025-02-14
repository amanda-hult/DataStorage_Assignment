using System.Collections.ObjectModel;
using Business.Interfaces;
using Business.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Presentation.MauiMvvm.ViewModels;

public partial class ShowProductsViewModel : ObservableObject
{
    private readonly IProductService _productService;

    public ShowProductsViewModel(IProductService productService)
    {
        _productService = productService;
        ProductList = new ObservableCollection<ProductModel>();
    }

    [ObservableProperty]
    private string _statusMessage = null!;

    [ObservableProperty]
    private ObservableCollection<ProductModel> _productList;


    [RelayCommand]
    private async Task LoadAllProducts()
    {
        var result = await _productService.GetAllProductsAsync();

        if (result.Success && result.Data != null)
        {
            ProductList.Clear();
            foreach (var product in result.Data)
            {
                ProductList.Add(product);
            }
        }
        else
        {
            StatusMessage = "Couldn't load services.";
        }
    }

    [RelayCommand]
    private async Task DeleteProduct(ProductModel product)
    {
        if (product == null)
        {
            StatusMessage = "Invalid product.";
            return;
        }
        var result = await _productService.DeleteProductAsync(product.ProductId);

        if (result.Success)
        {
            ProductList.Remove(product);
            StatusMessage = "Product deleted successfully.";
        }
        else
        {
            StatusMessage = "Failed to delete product.";
        }
    }

    [RelayCommand]
    private async Task NavigateToEditProduct(ProductModel product)
    {
        var parameters = new ShellNavigationQueryParameters
        {
            { "ProductId", product.ProductId.ToString() }
        };
        await Shell.Current.GoToAsync("EditProductView", parameters);
    }

    [RelayCommand]
    private async Task NavigateToAddProduct()
    {
        await Shell.Current.GoToAsync("AddProductView");
    }
}
