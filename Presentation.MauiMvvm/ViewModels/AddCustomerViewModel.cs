using Business.Dtos;
using Business.Interfaces;
using Business.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Presentation.MauiMvvm.ViewModels;

public partial class AddCustomerViewModel : ObservableObject
{
    private readonly ICustomerService _customerService;

    public AddCustomerViewModel(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [ObservableProperty]
    private CustomerCreateDto _customerCreateDto = new();

    [ObservableProperty]
    private string _statusMessage = null!;

    [RelayCommand]
    public async Task AddCustomer()
    {
        var result = await _customerService.CreateCustomerAsync(CustomerCreateDto);
        if (result.Success)
        {
            StatusMessage = "Service was added successfully";
            CustomerCreateDto = new CustomerCreateDto();
        }
        else
        {
            StatusMessage = result.Message ?? "Service could not be added.";
        }
    }
}
