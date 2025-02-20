using Business.Interfaces;
using Business.Services;
using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Presentation.MauiMvvm.ViewModels;
using Presentation.MauiMvvm.Views;

namespace Presentation.MauiMvvm
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("FontAwesome6Free-Solid-900.otf", "FontAwesomeSolid");
                });


            builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

            builder.Services.AddScoped<IStatusService, StatusService>();
            builder.Services.AddScoped<IStatusRepository, StatusRepository>();

            builder.Services.AddScoped<IContactPersonService, ContactPersonService>();
            builder.Services.AddScoped<IContactPersonRepository, ContactPersonRepository>();

            builder.Services.AddDbContext<DataContext>(options => options.UseSqlite("Data Source=C:\\Databases\\projectdata.db"));

            builder.Services.AddTransient<AddProjectViewModel>();
            builder.Services.AddTransient<AddProjectView>();
            builder.Services.AddTransient<ShowProjectsViewModel>();
            builder.Services.AddTransient<ShowProjectsView>();
            builder.Services.AddTransient<EditProjectViewModel>();
            builder.Services.AddTransient<EditProjectView>();


            builder.Services.AddTransient<AddProductViewModel>();
            builder.Services.AddTransient<AddProductView>();
            builder.Services.AddTransient<ShowProductsViewModel>();
            builder.Services.AddTransient<ShowProductsView>();
            builder.Services.AddTransient<EditProductViewModel>();
            builder.Services.AddTransient<EditProductView>();

            builder.Services.AddTransient<AddCustomerViewModel>();
            builder.Services.AddTransient<AddCustomerView>();
            builder.Services.AddTransient<ShowCustomersViewModel>();
            builder.Services.AddTransient<ShowCustomersView>();
            builder.Services.AddTransient<EditCustomerViewModel>();
            builder.Services.AddTransient<EditCustomerView>();

            builder.Services.AddTransient<AddUserViewModel>();
            builder.Services.AddTransient<AddUserView>();
            builder.Services.AddTransient<ShowUsersViewModel>();
            builder.Services.AddTransient<ShowUsersView>();
            builder.Services.AddTransient<EditUserViewModel>();
            builder.Services.AddTransient<EditUserView>();




            builder.Logging.AddDebug();


            return builder.Build();
        }
    }
}
