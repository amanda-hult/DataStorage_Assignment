using Business.Dtos;
using Business.Interfaces;
using Business.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Presentation.MauiMvvm.ViewModels;

public partial class EditCustomerViewModel : ObservableObject, IQueryAttributable
{
    private readonly ICustomerService _customerService;

    public EditCustomerViewModel(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private CustomerUpdateDto _customerUpdateDto = new CustomerUpdateDto();

    [RelayCommand]
    public async Task UpdateCustomer()
    {
        if (CustomerUpdateDto == null)
            return;

        var result = await _customerService.UpdateCustomerAsync(_customerUpdateDto);

        if (result.Success)
        {
            StatusMessage = result.Message;
        }
        else
        {
            StatusMessage = result.Message ?? "Failed to update customer";
        }
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("CustomerId", out var customerIdObj) && customerIdObj is string customerIdStr && int.TryParse(customerIdStr, out int customerId))
        {
            var result = await _customerService.GetCustomerByIdAsync(customerId);
            if (result.Success && result.Data != null)
            {
                CustomerUpdateDto = new CustomerUpdateDto
                {
                    CustomerId = result.Data.CustomerId,
                    CustomerName = result.Data.CustomerName,
                };
            }
            else
            {
                StatusMessage = "Customer not found.";
            }
        }
    }
}
