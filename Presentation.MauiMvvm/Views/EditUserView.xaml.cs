using Presentation.MauiMvvm.ViewModels;

namespace Presentation.MauiMvvm.Views;

public partial class EditUserView : ContentPage
{
	public EditUserView(EditUserViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}