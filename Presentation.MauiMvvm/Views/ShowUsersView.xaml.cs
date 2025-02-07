using Presentation.MauiMvvm.ViewModels;

namespace Presentation.MauiMvvm.Views;

public partial class ShowUsersView : ContentPage
{
	public ShowUsersView(ShowUsersViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}