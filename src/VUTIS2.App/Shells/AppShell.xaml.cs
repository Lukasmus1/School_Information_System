using CommunityToolkit.Mvvm.Input;
using VUTIS2.App.Services;
using VUTIS2.App.ViewModels;

namespace VUTIS2.App.Shells;

public partial class AppShell
{
    private readonly INavigationService _navigationService;
    public AppShell(INavigationService navigationService)
    {
        _navigationService = navigationService;
        InitializeComponent();
    }

    [RelayCommand]
    private async Task GoToStudentsAsync()
        => await _navigationService.GoToAsync<StudentListViewModel>();

    [RelayCommand]
    private async Task GoToSubjectsAsync()
        => await _navigationService.GoToAsync<SubjectListViewModel>();
}
