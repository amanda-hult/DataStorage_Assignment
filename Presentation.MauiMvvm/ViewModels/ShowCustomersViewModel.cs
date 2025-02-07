using System.Collections.ObjectModel;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Presentation.MauiMvvm.ViewModels;

public partial class ShowCustomersViewModel : ObservableObject
{
    private readonly ICustomerService _customerService;

    public ShowCustomersViewModel(ICustomerService customerService)
    {
        _customerService = customerService;
        CustomerList = new ObservableCollection<CustomerModel>();
    }

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private ObservableCollection<CustomerModel> _customerList;

    [RelayCommand]
    private async Task LoadCustomers()
    {
        var result = await _customerService.GetAllCustomersAsync();

        if (result.Success && result.Data != null)
        {
            CustomerList.Clear();
            foreach (var customer in result.Data)
            {
                CustomerList.Add(customer);
            }
        }
        else
        {
            StatusMessage = "Couldn't load users.";
        }
    }

    [RelayCommand]
    private async Task NavigateToEditCustomer(CustomerModel customer)
    {
        var parameters = new ShellNavigationQueryParameters
        {
            { "CustomerId", customer.CustomerId.ToString() }
        };
        await Shell.Current.GoToAsync("EditCustomerView", parameters);
    }
}
