using System.Collections.ObjectModel;
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

    #region Observable properties

    [ObservableProperty]
    private ObservableCollection<UserModel> _userList;

    [ObservableProperty]
    private bool _isSortedAscendingByUserId = true;

    [ObservableProperty]
    private bool _isSortedAscendingByFirstName = false;

    [ObservableProperty]
    private bool _isSortedAscendingByLastName = false;

    [ObservableProperty]
    private bool _isSortedAscendingByEmail = false;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private Color _statusMessageColor = Colors.Black;

    #endregion

    #region Sort users

    [RelayCommand]
    private void SortUserListByUserId()
    {
        if (UserList == null)
            return;

        if (IsSortedAscendingByUserId)
        {
            var sortedList = UserList.OrderByDescending(u => u.UserId).ToList();
            UserList = new ObservableCollection<UserModel>(sortedList);
        }
        else
        {
            var sortedList = UserList.OrderBy(u => u.UserId).ToList();
            UserList = new ObservableCollection<UserModel>(sortedList);
        }

        IsSortedAscendingByUserId = !IsSortedAscendingByUserId;
    }

    [RelayCommand]
    private void SortUserListByFirstName()
    {
        if (UserList == null)
            return;

        if (IsSortedAscendingByFirstName)
        {
            var sortedList = UserList.OrderByDescending(u => u.FirstName).ToList();
            UserList = new ObservableCollection<UserModel>(sortedList);
        }
        else
        {
            var sortedList = UserList.OrderBy(u => u.FirstName).ToList();
            UserList = new ObservableCollection<UserModel>(sortedList);
        }

        IsSortedAscendingByFirstName = !IsSortedAscendingByFirstName;
    }

    [RelayCommand]
    private void SortUserListByLastName()
    {
        if (UserList == null)
            return;

        if (IsSortedAscendingByLastName)
        {
            var sortedList = UserList.OrderByDescending(u => u.LastName).ToList();
            UserList = new ObservableCollection<UserModel>(sortedList);
        }
        else
        {
            var sortedList = UserList.OrderBy(u => u.LastName).ToList();
            UserList = new ObservableCollection<UserModel>(sortedList);
        }

        IsSortedAscendingByLastName = !IsSortedAscendingByLastName;
    }

    [RelayCommand]
    private void SortUserListByEmail()
    {
        if (UserList == null)
            return;

        if (IsSortedAscendingByEmail)
        {
            var sortedList = UserList.OrderByDescending(u => u.Email).ToList();
            UserList = new ObservableCollection<UserModel>(sortedList);
        }
        else
        {
            var sortedList = UserList.OrderBy(u => u.Email).ToList();
            UserList = new ObservableCollection<UserModel>(sortedList);
        }

        IsSortedAscendingByEmail = !IsSortedAscendingByEmail;
    }
    #endregion

    [RelayCommand]
    private async Task LoadAllUsers()
    {
        StatusMessage = string.Empty;

        var result = await _userService.GetAllUsersAsync();

        if (result.Success && result.Data != null)
        {
            UserList.Clear();
            foreach (var user in result.Data)
            {
                UserList.Add(user);
            }
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
            StatusMessageColor = Colors.Firebrick;
            return;
        }

        bool confirm = await Shell.Current.DisplayAlert("Confirmation", "Please confirm deletion", "Confirm", "Cancel");
        if (confirm)
        {
            var result = await _userService.DeleteUserAsync(user.UserId);

            if (result.Success)
            {
                UserList.Remove(user);
                StatusMessage = "User deleted successfully.";
                StatusMessageColor = Colors.Black;
            }
            else
            {
                StatusMessage = "Unable to delete user. User cannot be deleted if it exists in a project.";
                StatusMessageColor = Colors.Firebrick;
            }
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
