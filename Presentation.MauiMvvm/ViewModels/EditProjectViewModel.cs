using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Presentation.MauiMvvm.ViewModels;

public partial class EditProjectViewModel : ObservableObject, IQueryAttributable
{
    private readonly IProjectService _projectService;
    private readonly IStatusService _statusService;
    private readonly IProductService _productService;
    private readonly ICustomerService _customerService;
    private readonly IUserService _userService;

    public EditProjectViewModel(IProjectService projectService, IStatusService statusService, IProductService productService, ICustomerService customerService, IUserService userService)
    {
        _projectService = projectService;
        _statusService = statusService;
        _productService = productService;
        _customerService = customerService;
        _userService = userService;
    }

    [ObservableProperty]
    private string _statusMessage = string.Empty;

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
    private bool _isEditMode = false;

    public bool IsReadMode => !IsEditMode;



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

            //Debug.WriteLine($"Selected user updated: {SelectedUser.FirstName} {SelectedUser.LastName}");

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


    [RelayCommand]
    private async Task ToggleEditMode()
    {
        IsEditMode = !IsEditMode;
        OnPropertyChanged(nameof(IsReadMode));
        OnPropertyChanged(nameof(DetailedProjectModel));

        //if (IsEditMode)
        //{
            await LoadStatuses();
            await LoadProducts();
            await LoadCustomers();
            await LoadUsers();
        //}
        //if (IsReadMode)
        //{
        //    SetSelectedCustomer();
        //    SetSelectedProducts();
        //    SetSelectedStatus();
        //    SetSelectedUser();
        //}
    }

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


    [RelayCommand]
    public void AddProductToProject()
    {
        if (SelectedProduct == null || Hours <= 0)
        {
            StatusMessage = "Select a service and enter hours.";
            return;
        }

        var existingProduct = SelectedProjectProducts.FirstOrDefault(p => p.ProductId == SelectedProduct.ProductId);
        if (existingProduct != null)
        {
            existingProduct.Hours = Hours;
        }
        else
        {
            ProjectUpdateDto.ProjectProducts.Add(new ProjectProductDto
            {
                ProductId = SelectedProduct.ProductId,
                Product = SelectedProduct,
                Hours = Hours
            });
        }

        SelectedProduct = null;
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
                return;
            }
            Debug.WriteLine($"Updating product: {System.Text.Json.JsonSerializer.Serialize(ProjectUpdateDto, new System.Text.Json.JsonSerializerOptions { WriteIndented = true })}");
            var result = await _projectService.UpdateProjectAsync(ProjectUpdateDto);

            if (result.Success)
            {
                var dbCheck = await _projectService.GetProjectWithDetailsAsync(ProjectUpdateDto.ProjectId);
                Debug.WriteLine($"DB User: {dbCheck.Data.User.FirstName} {dbCheck.Data.User.LastName}");

                Debug.WriteLine("Project updated successfully");
                StatusMessage = result.Message ?? "Project updated successfully";
            }
            else
            {
                Debug.WriteLine($"Project update failed: {result.Message}");
                StatusMessage = result.Message ?? "Failed to update project.";
            }

            var updatedProjectDetails = await _projectService.GetProjectWithDetailsAsync(ProjectUpdateDto.ProjectId);
            if (updatedProjectDetails.Success)
            {
                DetailedProjectModel = updatedProjectDetails.Data;
                ProjectUpdateDto = ProjectFactory.Create(DetailedProjectModel);
            }
            else
            {
                Debug.WriteLine("Failed to load project.");
            }

            //await LoadStatuses();
            //await LoadProducts();
            //await LoadCustomers();
            //await LoadUsers();
            //SetSelectedCustomer();
            //SetSelectedProducts();
            //SetSelectedStatus();
            //SetSelectedUser();

            await ToggleEditMode();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in UpdateProject: {ex.Message}");
        }

    }





    //[RelayCommand]
    //private void CheckProjectData()
    //{
    //    if (_detailedProjectModel == null)
    //    {
    //        Debug.WriteLine("DetailedProjectModel is NULL in EditProjectViewModel.");
    //    }
    //    else
    //    {
    //        Debug.WriteLine($"DetailedProjectModel exists: {DetailedProjectModel.ProjectId}, Title: {DetailedProjectModel?.Title}");
    //    }
    //}

}
