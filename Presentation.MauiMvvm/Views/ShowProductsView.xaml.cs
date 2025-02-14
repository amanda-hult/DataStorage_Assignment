using Presentation.MauiMvvm.ViewModels;

namespace Presentation.MauiMvvm.Views;

public partial class ShowProductsView : ContentPage
{
	private readonly ShowProductsViewModel _viewModel;
	public ShowProductsView(ShowProductsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _viewModel.LoadAllProductsCommand.ExecuteAsync(null);
	}

}