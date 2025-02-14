using System.Collections.ObjectModel;
using System.Diagnostics;
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

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private ObservableCollection<BasicProjectModel> _basicProjectList;

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
