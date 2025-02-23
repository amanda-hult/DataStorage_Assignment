using System.ComponentModel.DataAnnotations;
using Business.Dtos;
using Business.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Presentation.MauiMvvm.ViewModels;

public partial class AddCustomerViewModel(ICustomerService customerService) : ObservableObject
{
    private readonly ICustomerService _customerService = customerService;

    #region Observable properties
    [ObservableProperty]
    private CustomerCreateDto _customerCreateDto = new();

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private Color _statusMessageColor = Colors.Black;
    #endregion

    [RelayCommand]
    public async Task AddCustomer()
    {
        if (CustomerCreateDto == null)
        {
            StatusMessage = "Invalid data.";
            StatusMessageColor = Colors.Firebrick;
            return;
        }
        
        var validationContext = new ValidationContext(CustomerCreateDto);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(CustomerCreateDto, validationContext, validationResults, true);

        if (!isValid)
        {
            StatusMessage = string.Join("\n", validationResults.Select(x => x.ErrorMessage));
            StatusMessageColor = Colors.Firebrick;
            return;
        }

        var result = await _customerService.CreateCustomerAsync(CustomerCreateDto);
        if (result.StatusCode == 409)
        {
            StatusMessage = "A customer with the same name already exists.";
            StatusMessageColor = Colors.Firebrick;
            return;
        }
        if (result.Success)
        {
            StatusMessage = "Customer was added successfully";
            StatusMessageColor = Colors.Black;
            CustomerCreateDto = new CustomerCreateDto();
        }
        else
        {
            StatusMessage = result.Message ?? "Customer could not be added.";
            StatusMessageColor = Colors.Firebrick;
        }
    }
}
