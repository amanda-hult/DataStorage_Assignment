using Presentation.MauiMvvm.ViewModels;

namespace Presentation.MauiMvvm.Views;

public partial class ShowProjectsView : ContentPage
{
	private readonly ShowProjectsViewModel _viewModel;
	public ShowProjectsView(ShowProjectsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _viewModel.LoadAllProjectsCommand.ExecuteAsync(null);
	}
}