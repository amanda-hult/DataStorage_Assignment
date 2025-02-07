using System.Collections.ObjectModel;
using System.Diagnostics;
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
    }

    [ObservableProperty]
    private string _statusMessage = null!;

    [ObservableProperty]
    private ObservableCollection<ProductModel> _productList = new();

    //[ObservableProperty]
    //private ProductModel? editingProduct;

    [ObservableProperty]
    private Dictionary<ProductModel, bool> _editingProducts = new();

    [RelayCommand]
    private async Task LoadAllProducts()
    {
        var result = await _productService.GetAllProductsAsync();

        if (result.Success && result.Data != null)
        {
            _productList = new ObservableCollection<ProductModel>(result.Data);

        }
        else
        {
            _statusMessage = "Couldn't load services.";
        }
    }

    [RelayCommand]
    private async Task NavigateToAddProduct()
    {
        await Shell.Current.GoToAsync("AddProductView");
    }

    [RelayCommand]
    private void ToggleIsEditing(ProductModel product)
    {
        if (_editingProducts.ContainsKey(product))
        {
            _editingProducts[product] = !_editingProducts[product];
        }
        else
        {
            _editingProducts[product] = true;
        }
        //IsEditing = !IsEditing;
        //if (EditingProduct == product)
        //    SetProperty(ref editingProduct, null);
        //else
        //    SetProperty(ref editingProduct, product);

        //Debug.Write($"EditingProduct: {EditingProduct?.ProductName}");

    }
}
