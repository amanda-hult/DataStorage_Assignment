using Presentation.MauiMvvm.ViewModels;

namespace Presentation.MauiMvvm.Views;

public partial class AddProjectView : ContentPage
{
	public AddProjectView(AddProjectViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}