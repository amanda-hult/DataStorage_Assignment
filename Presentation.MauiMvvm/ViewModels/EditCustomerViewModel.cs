using System.ComponentModel.DataAnnotations;
using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Presentation.MauiMvvm.ViewModels;

public partial class EditCustomerViewModel(ICustomerService customerService) : ObservableObject, IQueryAttributable
{
    private readonly ICustomerService _customerService = customerService;

    #region Observable properties
    [ObservableProperty]
    private CustomerModel _customerModel;

    [ObservableProperty]
    private CustomerUpdateDto _customerUpdateDto = new CustomerUpdateDto();

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private Color _statusMessageColor = Colors.Black;
    #endregion

    [RelayCommand]
    public async Task UpdateCustomer()
    {
        if (CustomerUpdateDto == null)
            return;

        var validationContext = new ValidationContext(CustomerUpdateDto);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(CustomerUpdateDto, validationContext, validationResults, true);
        if (!isValid)
        {
            StatusMessage = string.Join("\n", validationResults.Select(x => x.ErrorMessage));
            StatusMessageColor = Colors.Firebrick;
            return;
        }

        var result = await _customerService.UpdateCustomerAsync(CustomerUpdateDto);
        if (result.StatusCode == 409)
        {
            StatusMessage = "A customer with the same name already exists.";
            StatusMessageColor = Colors.Firebrick;
            return;
        }
        if (result.Success)
        {
            StatusMessage = "User updated successfully";
            StatusMessageColor = Colors.Black;
        }
        else
        {
            StatusMessage = result.Message ?? "Failed to update customer";
            StatusMessageColor = Colors.Firebrick;
        }
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("CustomerId", out var customerIdObj) && customerIdObj is string customerIdStr && int.TryParse(customerIdStr, out int customerId))
        {
            var result = await _customerService.GetCustomerByIdAsync(customerId);
            if (result.Success && result.Data != null)
            {
                CustomerModel = result.Data;
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
