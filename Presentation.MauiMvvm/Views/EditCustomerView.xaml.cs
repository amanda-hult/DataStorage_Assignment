using Presentation.MauiMvvm.ViewModels;

namespace Presentation.MauiMvvm.Views;

public partial class EditCustomerView : ContentPage
{
	public EditCustomerView(EditCustomerViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}