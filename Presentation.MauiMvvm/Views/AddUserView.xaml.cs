using Presentation.MauiMvvm.ViewModels;

namespace Presentation.MauiMvvm.Views;

public partial class AddUserView : ContentPage
{
	public AddUserView(AddUserViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}