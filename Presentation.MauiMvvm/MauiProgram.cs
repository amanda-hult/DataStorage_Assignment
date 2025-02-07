﻿using Business.Interfaces;
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
                });


            builder.Services.AddSingleton<IProductService, ProductService>();
            builder.Services.AddSingleton<IProductRepository, ProductRepository>();
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<IUserRepository, UserRepository>();
            builder.Services.AddSingleton<ICustomerService, CustomerService>();
            builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();

            builder.Services.AddDbContext<DataContext>(options => options.UseSqlite("Data Source=C:\\Databases\\projectdata.db"));

            builder.Services.AddTransient<AddProductViewModel>();
            builder.Services.AddTransient<AddProductView>();
            builder.Services.AddTransient<ShowProductsViewModel>();
            builder.Services.AddTransient<ShowProductsView>();

            builder.Services.AddTransient<AddCustomerViewModel>();
            builder.Services.AddTransient<AddCustomerView>();
            builder.Services.AddTransient<ShowCustomersViewModel>();
            builder.Services.AddTransient<ShowCustomersView>();

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
