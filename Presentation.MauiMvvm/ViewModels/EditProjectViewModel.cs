using System.Diagnostics;
using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Presentation.MauiMvvm.ViewModels;

public partial class EditProjectViewModel : ObservableObject, IQueryAttributable
{
    private readonly IProjectService _projectService;

    public EditProjectViewModel(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private DetailedProjectModel _detailedProjectModel;

    [ObservableProperty]
    private ProjectUpdateDto _projectUpdateDto = new ProjectUpdateDto();



    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("ProjectId") && int.TryParse(query["ProjectId"].ToString(), out int projectId))
        {
            var result = await _projectService.GetProjectWithDetailsAsync(projectId);

            if (result.Success && result.Data != null)
            {
                DetailedProjectModel = result.Data;
            }
            else
            {
                StatusMessage = "Failed to load project details.";
            }
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

    //[RelayCommand]
    //public async Task UpdateProject()
    //{
    //    if (_projectUpdateDto == null)
    //        return;

    //    var result = await _projectService.UpdateProjectAsync(_projectUpdateDto);

    //    if (result.Success)
    //    {
    //        StatusMessage = result.Message;
    //    }
    //    else
    //    {
    //        StatusMessage = result.Message ?? "Failed to update project.";
    //    }

    //}
}
