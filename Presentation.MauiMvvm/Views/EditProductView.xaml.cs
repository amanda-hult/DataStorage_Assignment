using Presentation.MauiMvvm.ViewModels;

namespace Presentation.MauiMvvm.Views;

public partial class EditProductView : ContentPage
{
	public EditProductView(EditProductViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}