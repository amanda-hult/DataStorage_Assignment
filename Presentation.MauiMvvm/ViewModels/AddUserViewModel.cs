using System.ComponentModel.DataAnnotations;
using Business.Dtos;
using Business.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Presentation.MauiMvvm.ViewModels;

public partial class AddUserViewModel(IUserService userService) : ObservableObject
{
    private readonly IUserService _userService = userService;

    #region Observable properties
    [ObservableProperty]
    private UserCreateDto _userCreateDto = new();

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private Color _statusMessageColor = Colors.Black;
    #endregion

    [RelayCommand]
    public async Task AddUser()
    {
        if (UserCreateDto == null)
        {
            StatusMessage = "Invalid data.";
            StatusMessageColor = Colors.Firebrick;
            return;
        }

        var validationContext = new ValidationContext(UserCreateDto);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(UserCreateDto, validationContext, validationResults, true);
        if (!isValid)
        {
            StatusMessage = string.Join("\n", validationResults.Select(x => x.ErrorMessage));
            StatusMessageColor = Colors.Firebrick;
            return;
        }

        var result = await _userService.CreateUserAsync(UserCreateDto);
        if (result.StatusCode == 409)
        {
            StatusMessage = "A user with the same email address already exists.";
            StatusMessageColor = Colors.Firebrick;
            return;
        }
        if (result.Success)
        {
            StatusMessage = "User was added successfully";
            StatusMessageColor = Colors.Black;
            UserCreateDto = new UserCreateDto();
        }
        else
        {
            StatusMessage = result.Message ?? "User could not be added.";
            StatusMessageColor = Colors.Firebrick;
        }
    }
}
