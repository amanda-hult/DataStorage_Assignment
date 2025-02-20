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
    private Color _statusMessageColor = Colors.Black;

    [ObservableProperty]
    private ObservableCollection<CustomerModel> _customerList;


    [ObservableProperty]
    private bool _isSortedAscendingByCustomerId = true;

    [ObservableProperty]
    private bool _isSortedAscendingByCustomerName = false;

    [RelayCommand]
    private async Task LoadCustomers()
    {
        StatusMessage = string.Empty;

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
            StatusMessageColor = Colors.Firebrick;
        }
    }

    [RelayCommand]
    private async Task DeleteCustomer(CustomerModel customer)
    {
        if (customer == null)
        {
            StatusMessage = "Invalid customer.";
            return;
        }

        bool confirm = await Shell.Current.DisplayAlert("Confirmation", "Please confirm deletion", "Confirm", "Cancel");
        if (confirm)
        {
            var result = await _customerService.DeleteCustomerAsync(customer.CustomerId);

            if (result.Success)
            {
                CustomerList.Remove(customer);
                StatusMessage = "Customer deleted successfully.";
                StatusMessageColor = Colors.Black;
            }
            else
            {
                StatusMessage = "Customer exists in a project and cannot be deleted.";
                StatusMessageColor = Colors.Firebrick;
            }
        }
    }

    [RelayCommand]
    private void SortCustomerListByCustomerId()
    {
        if (CustomerList == null)
            return;

        if (IsSortedAscendingByCustomerId)
        {
            var sortedList = CustomerList.OrderByDescending(c => c.CustomerId).ToList();
            CustomerList = new ObservableCollection<CustomerModel>(sortedList);
        }
        else
        {
            var sortedList = CustomerList.OrderBy(p => p.CustomerId).ToList();
            CustomerList = new ObservableCollection<CustomerModel>(sortedList);
        }

        IsSortedAscendingByCustomerId = !IsSortedAscendingByCustomerId;
    }

    [RelayCommand]
    private void SortCustomerListByCustomerName()
    {
        if (CustomerList == null)
            return;

        if (IsSortedAscendingByCustomerName)
        {
            var sortedList = CustomerList.OrderByDescending(c => c.CustomerName).ToList();
            CustomerList = new ObservableCollection<CustomerModel>(sortedList);
        }
        else
        {
            var sortedList = CustomerList.OrderBy(p => p.CustomerName).ToList();
            CustomerList = new ObservableCollection<CustomerModel>(sortedList);
        }

        IsSortedAscendingByCustomerName = !IsSortedAscendingByCustomerName;
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
