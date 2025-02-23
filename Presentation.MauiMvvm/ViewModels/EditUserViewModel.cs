using System.ComponentModel.DataAnnotations;
using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Presentation.MauiMvvm.ViewModels;

public partial class EditUserViewModel(IUserService userService) : ObservableObject, IQueryAttributable
{
    private readonly IUserService _userService = userService;

    #region Observable properties
    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private Color _statusMessageColor = Colors.Black;

    [ObservableProperty]
    private UserModel _userModel;

    [ObservableProperty]
    private UserUpdateDto _userUpdateDto = new UserUpdateDto();
    #endregion

    [RelayCommand]
    public async Task UpdateUser()
    {
        if (UserUpdateDto == null)
            return;

        var validationContext = new ValidationContext(UserUpdateDto);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(UserUpdateDto, validationContext, validationResults, true);
        if (!isValid)
        {
            StatusMessage = string.Join("\n", validationResults.Select(x => x.ErrorMessage));
            StatusMessageColor = Colors.Firebrick;
            return;
        }

        var result = await _userService.UpdateUserAsync(UserUpdateDto);
        if (result.StatusCode == 409)
        {
            StatusMessage = "A user with the same email address already exists.";
            StatusMessageColor = Colors.Firebrick;
            return;
        }
        if (result.Success)
        {
            StatusMessage = "User updated successfully.";
            StatusMessageColor = Colors.Black;
        }
        else
        {
            StatusMessage = result.Message ?? "Failed to update user.";
            StatusMessageColor = Colors.Firebrick;
        }
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("UserId", out var userIdObj) && userIdObj is string userIdStr && int.TryParse(userIdStr, out int userId))
        {
            var result = await _userService.GetUserByIdAsync(userId);
            if (result.Success && result.Data != null)
            {
                UserModel = result.Data;
                UserUpdateDto = new UserUpdateDto
                {
                    UserId = result.Data.UserId,
                    FirstName = result.Data.FirstName,
                    LastName = result.Data.LastName,
                    Email = result.Data.Email
                };
            }
            else
            {
                StatusMessage = "User not found.";
            }
        }
    }
}
