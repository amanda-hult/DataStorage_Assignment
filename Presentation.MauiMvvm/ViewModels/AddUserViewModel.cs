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
    private string _statusMessage = null!;

    [RelayCommand]
    public async Task AddUser()
    {
        var result = await _userService.CreateUserAsync(_userCreateDto);
        if (result.Success)
        {
            _statusMessage = "User was added successfully";
            _userCreateDto = new UserCreateDto();
        }
        else
        {
            _statusMessage = result.Message ?? "User could not be added.";
        }
    }
}
