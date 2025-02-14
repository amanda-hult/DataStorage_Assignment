using System.Collections.ObjectModel;
using System.Diagnostics;
using Business.Interfaces;
using Business.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Presentation.MauiMvvm.ViewModels;

public partial class ShowUsersViewModel : ObservableObject
{
    private readonly IUserService _userService;

    public ShowUsersViewModel(IUserService userService)
    {
        _userService = userService;
        UserList = new ObservableCollection<UserModel>();
    }

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private ObservableCollection<UserModel> _userList;

    [RelayCommand]
    private async Task LoadAllUsers()
    {
        var result = await _userService.GetAllUsersAsync();

        if (result.Success && result.Data != null)
        {
            UserList.Clear();
            foreach (var user in result.Data)
            {
                UserList.Add(user);
            }
            //_userList = new ObservableCollection<UserModel>(result.Data);
        }
        else
        {
            StatusMessage = "Couldn't load users.";
        }
    }

    [RelayCommand]
    private async Task DeleteUser(UserModel user)
    {
        if (user == null)
        {
            StatusMessage = "Invalid user.";
            return;
        }
        var result = await _userService.DeleteUserAsync(user.UserId);

        if (result.Success)
        {
            UserList.Remove(user);
            StatusMessage = "User deleted successfully.";
        }
        else
        {
            StatusMessage = "Failed to delete user.";
        }
    }

    [RelayCommand]
    private async Task NavigateToEditUser(UserModel user)
    {
        var parameters = new ShellNavigationQueryParameters
        {
            { "UserId", user.UserId.ToString() }
        };
        await Shell.Current.GoToAsync("EditUserView", parameters);
    }
}
