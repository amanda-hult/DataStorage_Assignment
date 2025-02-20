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
    private Color _statusMessageColor = Colors.Black;

    [ObservableProperty]
    private ObservableCollection<ProductModel> _productList;

    [ObservableProperty]
    private bool _isSortedAscendingByProductId = true;

    [ObservableProperty]
    private bool _isSortedAscendingByProductName = false;

    [ObservableProperty]
    private bool _isSortedAscendingByPrice = false;

    [ObservableProperty]
    private bool _isSortedAscendingByCurrency = false;


    [RelayCommand]
    private async Task LoadAllProducts()
    {
        StatusMessage = string.Empty;

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

        bool confirm = await Shell.Current.DisplayAlert("Confirmation", "Please confirm deletion", "Confirm", "Cancel");
        if (confirm)
        {
            var result = await _productService.DeleteProductAsync(product.ProductId);

            if (result.Success)
            {
                ProductList.Remove(product);
                StatusMessage = "Product deleted successfully.";
                StatusMessageColor = Colors.Black;
            }
            else
            {
                StatusMessage = "Product exists in a project and cannot be deleted.";
                StatusMessageColor = Colors.Firebrick;
            }
        }
    }

    [RelayCommand]
    private void SortProductListByProductId()
    {
        if (ProductList == null)
            return;

        if (IsSortedAscendingByProductId)
        {
            var sortedList = ProductList.OrderByDescending(p => p.ProductId).ToList();
            ProductList = new ObservableCollection<ProductModel>(sortedList);
        }
        else
        {
            var sortedList = ProductList.OrderBy(p => p.ProductId).ToList();
            ProductList = new ObservableCollection<ProductModel>(sortedList);
        }

        IsSortedAscendingByProductId = !IsSortedAscendingByProductId;
    }

    [RelayCommand]
    private void SortProductListByProductName()
    {
        if (ProductList == null)
            return;

        if (IsSortedAscendingByProductName)
        {
            var sortedList = ProductList.OrderByDescending(p => p.ProductName).ToList();
            ProductList = new ObservableCollection<ProductModel>(sortedList);
        }
        else
        {
            var sortedList = ProductList.OrderBy(p => p.ProductName).ToList();
            ProductList = new ObservableCollection<ProductModel>(sortedList);
        }

        IsSortedAscendingByProductName = !IsSortedAscendingByProductName;
    }

    [RelayCommand]
    private void SortProductListByPrice()
    {
        if (ProductList == null)
            return;

        if (IsSortedAscendingByPrice)
        {
            var sortedList = ProductList.OrderByDescending(p => p.Price).ToList();
            ProductList = new ObservableCollection<ProductModel>(sortedList);
        }
        else
        {
            var sortedList = ProductList.OrderBy(p => p.Price).ToList();
            ProductList = new ObservableCollection<ProductModel>(sortedList);
        }

        IsSortedAscendingByPrice = !IsSortedAscendingByPrice;
    }

    [RelayCommand]
    private void SortProductListByCurrency()
    {
        if (ProductList == null)
            return;

        if (IsSortedAscendingByCurrency)
        {
            var sortedList = ProductList.OrderByDescending(p => p.Currency).ToList();
            ProductList = new ObservableCollection<ProductModel>(sortedList);
        }
        else
        {
            var sortedList = ProductList.OrderBy(p => p.Currency).ToList();
            ProductList = new ObservableCollection<ProductModel>(sortedList);
        }

        IsSortedAscendingByCurrency = !IsSortedAscendingByCurrency;
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
