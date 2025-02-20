using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Presentation.MauiMvvm.ViewModels;

public partial class AddProjectViewModel : ObservableObject
{
    private readonly IProjectService _projectService;
    private readonly IProductService _productService;
    private readonly IStatusService _statusService;
    private readonly ICustomerService _customerService;
    private readonly IUserService _userService;

    public AddProjectViewModel(IProjectService projectService, IProductService productService, IStatusService statusService, ICustomerService customerService, IUserService userService)
    {
        _projectService = projectService;
        _productService = productService;
        _statusService = statusService;
        _customerService = customerService;
        _userService = userService;

        Task.Run(async () =>
        {
            await LoadStatuses();
            await LoadProducts();
            await LoadCustomers();
            await LoadUsers();
        });
    }


    [ObservableProperty]
    private ProjectCreateDto _projectCreateDto = new ProjectCreateDto
    {
        StartDate = DateTime.Today,
        EndDate = DateTime.Today.AddDays(1),
    };

    [ObservableProperty]
    private ObservableCollection<ProductModel> _availableProducts = new();

    [ObservableProperty]
    private ObservableCollection<StatusDto> _availableStatuses = new();

    [ObservableProperty]
    private ObservableCollection<CustomerModel> _availableCustomers = new();

    [ObservableProperty]
    private ObservableCollection<UserModel> _availableUsers = new();


    [ObservableProperty]
    private ObservableCollection<ProjectProductDto> _selectedProjectProducts = new();



    [ObservableProperty]
    private ProductModel _selectedProduct;

    [ObservableProperty]
    private StatusDto _selectedStatus;

    [ObservableProperty]
    private CustomerModel _selectedCustomer;

    [ObservableProperty]
    private UserModel _selectedUser;



    [ObservableProperty]
    private int _hours;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private Color _statusMessageColor = Colors.Black;

    partial void OnSelectedStatusChanged(StatusDto value)
    {
        if (value != null)
        {
            ProjectCreateDto.StatusId = value.StatusId;
        }
    }

    partial void OnSelectedCustomerChanged(CustomerModel value)
    {
        if (value != null)
        {
            ProjectCreateDto.Customer = value;
            ProjectCreateDto.CustomerId = value.CustomerId;
        }
    }

    partial void OnSelectedUserChanged(UserModel value)
    {
        if (value != null)
        {
            ProjectCreateDto.User = value;
            ProjectCreateDto.UserId = value.UserId;
        }
    }
    public async Task LoadProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        if (products == null || !products.Success || products.Data == null)
        {
            Debug.WriteLine("Error: ProductService returned null or failed.");
            return;
        }
        MainThread.BeginInvokeOnMainThread(() =>
        {
            AvailableProducts.Clear();
            foreach (var product in products.Data)
            {
                AvailableProducts.Add(product);
            }
        });

    }

    public async Task LoadStatuses()
    {
        var statuses = await _statusService.GetAllStatusesAsync();
        if (statuses == null || !statuses.Success || statuses.Data == null)
        {
            Debug.WriteLine("Error: StatusService returned null or failed.");
            return;
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            AvailableStatuses.Clear();
            foreach (var status in statuses.Data)
            {
                AvailableStatuses.Add(status);
            }
        });
    }

    public async Task LoadCustomers()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        if (customers == null || !customers.Success || customers.Data == null)
        {
            Debug.WriteLine("Error: CustomerService returned null or failed.");
            return;
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            _availableCustomers.Clear();
            foreach (var customer in customers.Data)
            {
                _availableCustomers.Add(customer);
            }
        });
    }

    public async Task LoadUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        if (users == null || !users.Success || users.Data == null)
        {
            Debug.WriteLine("Error: UserService returned null or failed.");
            return;
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            _availableUsers.Clear();
            foreach (var user in users.Data)
            {
                _availableUsers.Add(user);
            }
        });
    }

    [RelayCommand]
    public void AddProductToProject()
    {
        if (SelectedProduct == null || Hours <= 0)
        {
            StatusMessage = "Select a service and enter hours.";
            StatusMessageColor = Colors.Firebrick;
            return;
        }

        var existingProduct = SelectedProjectProducts.FirstOrDefault(p => p.ProductId == SelectedProduct.ProductId);
        if (existingProduct != null)
        {
            StatusMessage = "Service already added.";
            StatusMessageColor = Colors.Firebrick;
            return;
        }

        SelectedProjectProducts.Add(new ProjectProductDto
        {
            ProductId = SelectedProduct.ProductId,
            Product = SelectedProduct,
            Hours = Hours
        });

        SelectedProduct = null;
        Hours = 0;
    }


    [RelayCommand]
    public async Task AddProject()
    {
        if (ProjectCreateDto == null)
        {
            StatusMessage = "Invalid data.";
            StatusMessageColor = Colors.Firebrick;
            return;
        }

        if (SelectedStatus == null)
        {
            StatusMessage = "Please select a status.";
            StatusMessageColor = Colors.Firebrick;
            return;
        }

        var validationContext = new ValidationContext(ProjectCreateDto);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(ProjectCreateDto, validationContext, validationResults, true);

        if (ProjectCreateDto.ContactPerson != null)
        {
            var contactValidationContext = new ValidationContext(ProjectCreateDto.ContactPerson);
            bool isContactValid = Validator.TryValidateObject(ProjectCreateDto.ContactPerson, contactValidationContext, validationResults, true);

            isValid = isValid && isContactValid;
        }


        if (!isValid)
        {
            StatusMessage = string.Join("\n", validationResults.Select(x => x.ErrorMessage));
            StatusMessageColor = Colors.Firebrick;
            return;
        }

        ProjectCreateDto.ProjectProducts = SelectedProjectProducts.ToList();
        var result = await _projectService.CreateProjectAsync(ProjectCreateDto);
        if (result.Success)
        {
            StatusMessage = $"{result.Message}";
            StatusMessageColor = Colors.Black;
        }
        else
        {
            StatusMessage = "Failed to creat project.";
            StatusMessageColor = Colors.Firebrick;
        }
    }
}
