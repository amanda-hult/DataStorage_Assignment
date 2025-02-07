using Presentation.MauiMvvm.ViewModels;

namespace Presentation.MauiMvvm.Views;

public partial class ShowCustomersView : ContentPage
{
	public ShowCustomersView(ShowCustomersViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}