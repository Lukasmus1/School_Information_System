using CommunityToolkit.Mvvm.Input;
using VUTIS2.App.Services;

namespace VUTIS2.App.ViewModels;

public partial class MainPageViewModel (INavigationService navigationService, IMessengerService messengerService, IAlertService alertService)
    : ViewModelBase(messengerService)
{
    [RelayCommand]
    private async Task GoToStudentsAsync()
        => await navigationService.GoToAsync<StudentListViewModel>();

    [RelayCommand]
    private async Task GoToSubjectsAsync()
        => await navigationService.GoToAsync<SubjectListViewModel>();
}
