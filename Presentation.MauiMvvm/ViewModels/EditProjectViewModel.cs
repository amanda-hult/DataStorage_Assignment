using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Presentation.MauiMvvm.ViewModels;

public partial class EditProjectViewModel(IProjectService projectService, IStatusService statusService, IProductService productService, ICustomerService customerService, IUserService userService) : ObservableObject, IQueryAttributable
{
    private readonly IProjectService _projectService = projectService;
    private readonly IStatusService _statusService = statusService;
    private readonly IProductService _productService = productService;
    private readonly ICustomerService _customerService = customerService;
    private readonly IUserService _userService = userService;

    public bool IsReadMode => !IsEditMode;

    #region Observable properties
    [ObservableProperty]
    private bool _isEditMode = false;

    [ObservableProperty]
    private DetailedProjectModel _detailedProjectModel;

    [ObservableProperty]
    private ProjectUpdateDto _projectUpdateDto = new ProjectUpdateDto();


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
    #endregion

    #region Load values

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
            SetSelectedProducts();
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
            SetSelectedStatus();
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
            AvailableCustomers.Clear();
            foreach (var customer in customers.Data)
            {
                AvailableCustomers.Add(customer);
            }
            SetSelectedCustomer();
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
            AvailableUsers.Clear();
            foreach (var user in users.Data)
            {
                AvailableUsers.Add(user);
            }
            SetSelectedUser();
        });
    }
    #endregion

    #region SetSelectedValues
    private void SetSelectedStatus()
    {
        if (DetailedProjectModel != null)
        {
            SelectedStatus = AvailableStatuses.FirstOrDefault(s => s.StatusId == DetailedProjectModel.Status.StatusId);
        }
    }

    private void SetSelectedCustomer()
    {
        if (DetailedProjectModel != null)
        {
            SelectedCustomer = AvailableCustomers.FirstOrDefault(c => c.CustomerId == DetailedProjectModel.Customer.CustomerId);
        }
    }

    private void SetSelectedUser()
    {
        if (DetailedProjectModel != null)
        {
            SelectedUser = AvailableUsers.FirstOrDefault(u => u.UserId == DetailedProjectModel.User.UserId);

            OnPropertyChanged(nameof(SelectedUser));
        }
    }
    private void SetSelectedProducts()
    {
        if (DetailedProjectModel != null)
        {
            SelectedProjectProducts = new ObservableCollection<ProjectProductDto>(DetailedProjectModel.ProjectProducts.Select(pp => new ProjectProductDto
            {
                ProductId = pp.ProductId,
                Product = AvailableProducts.FirstOrDefault(p => p.ProductId == pp.Product.ProductId),
                Hours = pp.Hours,
            }));
        }
    }
    #endregion

    #region OnSelectedChanged
    partial void OnSelectedUserChanged(UserModel value)
    {
        if (value != null)
        {
            ProjectUpdateDto.UserId = value.UserId;
            ProjectUpdateDto.User = value;

            OnPropertyChanged(nameof(ProjectUpdateDto));
        }
    }

    partial void OnSelectedCustomerChanged(CustomerModel value)
    {
        if (value != null)
        {
            ProjectUpdateDto.CustomerId = value.CustomerId;
            ProjectUpdateDto.Customer = value;

            OnPropertyChanged(nameof(ProjectUpdateDto));
        }
    }

    partial void OnSelectedStatusChanged(StatusDto value)
    {
        if (value != null)
        {
            ProjectUpdateDto.StatusId = value.StatusId;

            OnPropertyChanged(nameof(ProjectUpdateDto));
        }
    }
    #endregion

    #region Relay Commands
    [RelayCommand]
    private async Task ToggleEditMode()
    {
        IsEditMode = !IsEditMode;
        OnPropertyChanged(nameof(IsReadMode));
        OnPropertyChanged(nameof(DetailedProjectModel));

        await LoadStatuses();
        await LoadProducts();
        await LoadCustomers();
        await LoadUsers();
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

        var newProduct = new ProjectProductDto
        {
            ProductId = SelectedProduct.ProductId,
            Product = SelectedProduct,
            Hours = Hours
        };

        ProjectUpdateDto.ProjectProducts.Add(newProduct);
        SelectedProjectProducts.Add(newProduct);

        OnPropertyChanged(nameof(SelectedProjectProducts));

        SelectedProduct = null!;
        Hours = 0;
    }

    [RelayCommand]
    private void RemoveProduct(ProjectProductDto product)
    {
        if (product != null)
        {
            var dtoItemToRemove = ProjectUpdateDto.ProjectProducts.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (dtoItemToRemove != null)
            {
                ProjectUpdateDto.ProjectProducts.Remove(dtoItemToRemove);
            }

            var itemToRemove = SelectedProjectProducts.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (itemToRemove != null)
            {
                SelectedProjectProducts.Remove(itemToRemove);
            }

            OnPropertyChanged(nameof(SelectedProjectProducts));
            OnPropertyChanged(nameof(ProjectUpdateDto.ProjectProducts));
        }
    }

    [RelayCommand]
    public async Task UpdateProject()
    {
        try
        {
            if (ProjectUpdateDto == null)
                return;

            var validationContext = new ValidationContext(ProjectUpdateDto);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(ProjectUpdateDto, validationContext, validationResults, true);
            if (ProjectUpdateDto.ContactPerson != null)
            {
                var contactValidationContext = new ValidationContext(ProjectUpdateDto.ContactPerson);
                bool isContactValid = Validator.TryValidateObject(ProjectUpdateDto.ContactPerson, contactValidationContext, validationResults, true);

                isValid = isValid && isContactValid;
            }

            if (!isValid)
            {
                StatusMessage = string.Join("\n", validationResults.Select(x => x.ErrorMessage));
                StatusMessageColor = Colors.Firebrick;
                return;
            }

            if (ProjectUpdateDto.EndDate.HasValue && ProjectUpdateDto.EndDate < ProjectUpdateDto.StartDate)
            {
                StatusMessage = "End date cannot be earlier than start date.";
                StatusMessageColor = Colors.Firebrick;
                return;
            }

            var result = await _projectService.UpdateProjectAsync(ProjectUpdateDto);
            if (result.StatusCode == 409)
            {
                StatusMessage = "Enable to update project. Check if a project with the same title and customerId already exists, or if the email address is used by another contact person.";
                StatusMessageColor = Colors.Firebrick;
                return;
            }

            if (result.Success)
            {
                StatusMessage = result.Message ?? "Project updated successfully";
                StatusMessageColor = Colors.Black;
                DetailedProjectModel = result.Data;
                ProjectUpdateDto = ProjectFactory.Create(DetailedProjectModel);
            }
            else
            {
                StatusMessage = result.Message ?? "Failed to update project.";
                StatusMessageColor = Colors.Firebrick;
            }

            await ToggleEditMode();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in UpdateProject: {ex.Message}");
        }
    }
    #endregion

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("ProjectId") && int.TryParse(query["ProjectId"].ToString(), out int projectId))
        {
            var result = await _projectService.GetProjectWithDetailsAsync(projectId);

            if (result.Success && result.Data != null)
            {
                DetailedProjectModel = result.Data;
                ProjectUpdateDto = ProjectFactory.Create(DetailedProjectModel);
            }
            else
            {
                StatusMessage = "Failed to load project details.";
            }
        }
    }
}
