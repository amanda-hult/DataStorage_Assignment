using Presentation.MauiMvvm.ViewModels;

namespace Presentation.MauiMvvm.Views;

public partial class ShowCustomersView : ContentPage
{
	private readonly ShowCustomersViewModel _viewModel;
	public ShowCustomersView(ShowCustomersViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _viewModel.LoadCustomersCommand.ExecuteAsync(null);
	}
}