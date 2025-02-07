using System.Diagnostics;
using Business.Dtos;
using Business.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Presentation.MauiMvvm.ViewModels;

public partial class EditUserViewModel : ObservableObject, IQueryAttributable
{
    private readonly IUserService _userService;
    public EditUserViewModel(IUserService userService)
    {
        _userService = userService;
    }


    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private UserUpdateDto _userUpdateDto = new UserUpdateDto();


    [RelayCommand]
    public async Task UpdateUser()
    {
        if (UserUpdateDto == null)
            return;

        var result = await _userService.UpdateUserAsync(UserUpdateDto);

        if (result.Success)
        {
            StatusMessage = "User updated successfully.";
        }
        else
        {
            StatusMessage = result.Message ?? "Failed to update user.";
        }
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {

        if (query.TryGetValue("UserId", out var userIdObj) && userIdObj is string userIdStr && int.TryParse(userIdStr, out int userId))
        {
            var result = await _userService.GetUserByIdAsync(userId);
            if (result.Success && result.Data != null)
            {
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
                _statusMessage = "User not found.";
            }
        }

        //if (query.TryGetValue("User", out var userObj) && userObj is UserModel user)
        //{
        //    Debug.WriteLine($"Mottaget UserId : {user.UserId}");

        //    //UserModel = user;

        //    SetProperty(ref _userUpdateDto, new UserUpdateDto
        //    {
        //        UserId = user.UserId,
        //        FirstName = user.FirstName,
        //        LastName = user.LastName,
        //        Email = user.Email
        //    });

        //    Debug.WriteLine($"UserUpdateDto efter mappning: Id = {UserUpdateDto.UserId}");
        //}

        //Debug.WriteLine("Kunde inte hämta user från query");




        //var userId = query["User"] as string;
        //if (!string.IsNullOrEmpty(userId))
        //{
        //    UserUpdateDto = _userService.GetUserAsync(u => u.UserId == userId);
        //}
    }
}
