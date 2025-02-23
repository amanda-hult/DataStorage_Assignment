using System.Collections.ObjectModel;
using Business.Interfaces;
using Business.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Presentation.MauiMvvm.ViewModels;

public partial class ShowProjectsViewModel : ObservableObject
{
    private readonly IProjectService _projectService;

    public ShowProjectsViewModel(IProjectService projectService)
    {
        _projectService = projectService;
        BasicProjectList = new ObservableCollection<BasicProjectModel>();
    }

    #region Observable properties
    [ObservableProperty]
    private ObservableCollection<BasicProjectModel> _basicProjectList;

    [ObservableProperty]
    private bool _isSortedAscendingByProjectId = true;

    [ObservableProperty]
    private bool _isSortedAscendingByTitle = false;

    [ObservableProperty]
    private bool _isSortedAscendingByStartDate = false;

    [ObservableProperty]
    private bool _isSortedAscendingByEndDate = false;

    [ObservableProperty]
    private bool _isSortedAscendingByStatus = false;

    [ObservableProperty]
    private bool _isSortedAscendingByCustomerName = false;

    [ObservableProperty]
    private string _statusMessage = string.Empty;
    #endregion

    #region Sort projects
    [RelayCommand]
    private void SortProjectListByProjectId()
    {
        if (BasicProjectList == null)
            return;

        if (IsSortedAscendingByProjectId)
        {
            var sortedList = BasicProjectList.OrderByDescending(p => p.ProjectId).ToList();
            BasicProjectList = new ObservableCollection<BasicProjectModel>(sortedList);
        }
        else
        {
            var sortedList = BasicProjectList.OrderBy(p => p.ProjectId).ToList();
            BasicProjectList = new ObservableCollection<BasicProjectModel>(sortedList);
        }

        IsSortedAscendingByProjectId = !IsSortedAscendingByProjectId;
    }

    [RelayCommand]
    private void SortProjectListByTitle()
    {
        if (BasicProjectList == null)
            return;

        if (IsSortedAscendingByTitle)
        {
            var sortedList = BasicProjectList.OrderByDescending(p => p.Title).ToList();
            BasicProjectList = new ObservableCollection<BasicProjectModel>(sortedList);
        }
        else
        {
            var sortedList = BasicProjectList.OrderBy(p => p.Title).ToList();
            BasicProjectList = new ObservableCollection<BasicProjectModel>(sortedList);
        }

        IsSortedAscendingByTitle = !IsSortedAscendingByTitle;
    }

    [RelayCommand]
    private void SortProjectListByStartDate()
    {
        if (BasicProjectList == null)
            return;

        if (IsSortedAscendingByStartDate)
        {
            var sortedList = BasicProjectList.OrderByDescending(p => p.StartDate).ToList();
            BasicProjectList = new ObservableCollection<BasicProjectModel>(sortedList);
        }
        else
        {
            var sortedList = BasicProjectList.OrderBy(p => p.StartDate).ToList();
            BasicProjectList = new ObservableCollection<BasicProjectModel>(sortedList);
        }

        IsSortedAscendingByStartDate = !IsSortedAscendingByStartDate;
    }

    [RelayCommand]
    private void SortProjectListByEndDate()
    {
        if (BasicProjectList == null)
            return;

        if (IsSortedAscendingByEndDate)
        {
            var sortedList = BasicProjectList.OrderByDescending(p => p.EndDate).ToList();
            BasicProjectList = new ObservableCollection<BasicProjectModel>(sortedList);
        }
        else
        {
            var sortedList = BasicProjectList.OrderBy(p => p.EndDate).ToList();
            BasicProjectList = new ObservableCollection<BasicProjectModel>(sortedList);
        }

        IsSortedAscendingByEndDate = !IsSortedAscendingByEndDate;
    }

    [RelayCommand]
    private void SortProjectListByStatus()
    {
        if (BasicProjectList == null)
            return;

        if (IsSortedAscendingByStatus)
        {
            var sortedList = BasicProjectList.OrderByDescending(p => p.Status.StatusId).ToList();
            BasicProjectList = new ObservableCollection<BasicProjectModel>(sortedList);
        }
        else
        {
            var sortedList = BasicProjectList.OrderBy(p => p.Status.StatusId).ToList();
            BasicProjectList = new ObservableCollection<BasicProjectModel>(sortedList);
        }

        IsSortedAscendingByStatus = !IsSortedAscendingByStatus;
    }

    [RelayCommand]
    private void SortProjectListByCustomerName()
    {
        if (BasicProjectList == null)
            return;

        if (IsSortedAscendingByCustomerName)
        {
            var sortedList = BasicProjectList.OrderByDescending(p => p.Customer.CustomerName).ToList();
            BasicProjectList = new ObservableCollection<BasicProjectModel>(sortedList);
        }
        else
        {
            var sortedList = BasicProjectList.OrderBy(p => p.Customer.CustomerName).ToList();
            BasicProjectList = new ObservableCollection<BasicProjectModel>(sortedList);
        }

        IsSortedAscendingByCustomerName = !IsSortedAscendingByCustomerName;
    }
    #endregion


    [RelayCommand]
    private async Task LoadAllProjects()
    {
        var result = await _projectService.GetAllProjectsAsync();
        if (result.Success && result.Data != null)
        {
            BasicProjectList.Clear();
            foreach (var project in result.Data)
            {
                BasicProjectList.Add(project);
            }
        }
        else
        {
            StatusMessage = "Couldn't load projects";
        }
    }

    [RelayCommand]
    private async Task NavigateToEditProject(BasicProjectModel project)
    {
        var parameters = new ShellNavigationQueryParameters
        {
            { "ProjectId", project.ProjectId.ToString() }
        };
            await Shell.Current.GoToAsync("EditProjectView", parameters);
    }

}
