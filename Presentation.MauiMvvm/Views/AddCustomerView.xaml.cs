using Presentation.MauiMvvm.ViewModels;

namespace Presentation.MauiMvvm.Views;

public partial class AddCustomerView : ContentPage
{
	public AddCustomerView(AddCustomerViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}