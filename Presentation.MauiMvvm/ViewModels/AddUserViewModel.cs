using System.ComponentModel.DataAnnotations;
using Business.Dtos;
using Business.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Presentation.MauiMvvm.ViewModels;

public partial class AddUserViewModel : ObservableObject
{
    private readonly IUserService _userService;

    public AddUserViewModel(IUserService userService)
    {
        _userService = userService;
    }

    [ObservableProperty]
    private UserCreateDto _userCreateDto = new();

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private Color _statusMessageColor = Colors.Black;

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
