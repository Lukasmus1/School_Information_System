using CommunityToolkit.Mvvm.Input;
using VUTIS2.App.Messages;
using VUTIS2.App.Services;
using VUTIS2.BL.Facades;
using VUTIS2.BL.Models;

namespace VUTIS2.App.ViewModels;

[QueryProperty(nameof(Subject), nameof(Subject))]
public partial class SubjectEditViewModel(ISubjectFacade subjectFacade, INavigationService navigationService,
    IMessengerService messengerService, IStudentFacade studentFacade, IEnrollmentFacade enrollmentFacade, IActivityFacade activityFacade)
    : ViewModelBase(messengerService)
{
    public Guid Id { get; set; }
    public SubjectDetailModel Subject { get; set; } = SubjectDetailModel.Empty;

    [RelayCommand]
    public async Task SaveAsync()
    {
        await subjectFacade.SaveAsync(Subject with {Enrollments = default!, Activities = default!});
        MessengerService.Send(new SubjectEditMessage { SubjectId = Subject.Id });
        navigationService.SendBackButtonPressed();
    }

    [RelayCommand]
    public void Cancel()
    {
        navigationService.SendBackButtonPressed();
    }


}
