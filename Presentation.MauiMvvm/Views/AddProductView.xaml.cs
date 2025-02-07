using Presentation.MauiMvvm.ViewModels;

namespace Presentation.MauiMvvm.Views;

public partial class AddProductView : ContentPage
{
	public AddProductView(AddProductViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}