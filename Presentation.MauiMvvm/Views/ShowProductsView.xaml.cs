using Presentation.MauiMvvm.ViewModels;

namespace Presentation.MauiMvvm.Views;

public partial class ShowProductsView : ContentPage
{
	public ShowProductsView(ShowProductsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	//protected override async void OnAppearing()
	//{
	//	base.OnAppearing();
	//	await ViewModel.LoadAllProductsCommand.ExecuteAsync(null);
	//}

}