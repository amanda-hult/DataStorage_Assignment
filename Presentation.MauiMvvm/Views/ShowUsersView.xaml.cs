using Presentation.MauiMvvm.ViewModels;

namespace Presentation.MauiMvvm.Views;

public partial class ShowUsersView : ContentPage
{
	private readonly ShowUsersViewModel _viewModel;
	public ShowUsersView(ShowUsersViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _viewModel.LoadAllUsersCommand.ExecuteAsync(null);
	}
}