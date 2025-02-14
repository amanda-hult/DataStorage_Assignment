using Presentation.MauiMvvm.ViewModels;

namespace Presentation.MauiMvvm.Views;

public partial class EditProjectView : ContentPage
{
	public EditProjectView(EditProjectViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}