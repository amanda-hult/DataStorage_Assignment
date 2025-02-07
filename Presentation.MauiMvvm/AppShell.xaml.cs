using Presentation.MauiMvvm.Views;

namespace Presentation.MauiMvvm
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ShowProductsView), typeof(ShowProductsView));
            Routing.RegisterRoute(nameof(AddProductView), typeof(AddProductView));

            Routing.RegisterRoute(nameof(ShowCustomersView), typeof(ShowCustomersView));
            Routing.RegisterRoute(nameof(AddCustomerView), typeof(AddCustomerView));

            Routing.RegisterRoute(nameof(ShowUsersView), typeof(ShowUsersView));
            Routing.RegisterRoute(nameof(AddUserView), typeof(AddUserView));
            Routing.RegisterRoute(nameof(EditUserView), typeof(EditUserView));

        }
    }
}
