using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using VUTIS2.App.Messages;
using VUTIS2.App.Services;
using VUTIS2.BL.Facades;
using VUTIS2.BL.Models;

namespace VUTIS2.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class SubjectDetailViewModel(ISubjectFacade subjectFacade, INavigationService navigationService, IMessengerService messengerService, IAlertService alertService, IEnrollmentFacade enrollmentFacade, IActivityFacade activityFacade, IStudentFacade studentFacade)
    : ViewModelBase(messengerService), IRecipient<SubjectEditMessage>, IRecipient<SubjectDeleteMessage>, IRecipient<StudentDeleteMessage>, IRecipient<ActivityDeleteMessage>, IRecipient<ActivityEditMessage>
{
    public Guid Id { get; set; }
    public SubjectDetailModel? Subject { get; private set; }
    public IEnumerable<StudentListModel?> Students { get; private set; } = Enumerable.Empty<StudentListModel>();
    public IEnumerable<StudentListModel?> EnrolledStudents { get; private set; } = Enumerable.Empty<StudentListModel>();
    public IEnumerable<ActivityListModel> Activities { get; private set; } = Enumerable.Empty<ActivityListModel>();
    public EnrollmentDetailModel Enrollment { get; set; } = EnrollmentDetailModel.Empty;
    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        Subject = await subjectFacade.GetAsync(Id);
        EnrolledStudents = Enumerable.Empty<StudentListModel>();
        Students = await studentFacade.GetAsync();
        Activities = await activityFacade.GetAsyncFromSubject(Id);
        List<StudentListModel?> studentsList = Students.ToList();
        if (Subject is not null)
        {
            foreach (EnrollmentListModel enrollment in Subject.Enrollments)
            {
                StudentListModel? student = await studentFacade.GetAsyncList(enrollment.StudentId);
                studentsList.Remove(student);
                EnrolledStudents = EnrolledStudents.Append(student);
            }
        }
        Students = studentsList;

    }
    [RelayCommand]
    public async Task GoToActivityDetailAsync(Guid id)
    {
        await navigationService.GoToAsync("activitydetail",
            new Dictionary<string, object?> { [nameof(ActivityDetailViewModel.Id)] = id });
    }

    [RelayCommand]
    public async Task GoToActivityCreateAsync()
    {
        await navigationService.GoToAsync("activityedit",
            new Dictionary<string, object?> { [nameof(ActivityEditViewModel.SubjectId)] = Id });
    }
    [RelayCommand]
    public async Task GoToSubjectEditAsync()
    {
        await navigationService.GoToAsync("/edit",
            new Dictionary<string, object?> { [nameof(SubjectEditViewModel.Subject)] = Subject });
    }

    [RelayCommand]
    public async Task GoToActivityListAsync()
    {
        await navigationService.GoToAsync("/activities",
            new Dictionary<string, object?> { [nameof(ActivityListViewModel.subjectId)] = Id });
    }

    [RelayCommand]
    public async Task AddEnrollmentAsync(Guid StudentId)
    {
        if (Subject is not null)
        {
            Enrollment = EnrollmentDetailModel.Empty with {StudentId = StudentId, SubjectId = Subject.Id};
        }
        await enrollmentFacade.SaveAsync(Enrollment with {Student = default!, Subject = default!});
        await ReloadDataAsync();
        await LoadDataAsync();
    }
    [RelayCommand]
    public async Task RemoveEnrollmentAsync(Guid StudentId)
    {
        EnrollmentListModel? deletedEnrollment = null;
        if (Subject is not null)
        {
            deletedEnrollment = Subject.Enrollments.FirstOrDefault(e => e.StudentId == StudentId);
        }
        if (deletedEnrollment is not null)
        {
            await enrollmentFacade.DeleteAsync(deletedEnrollment.Id);
        }
        await ReloadDataAsync();
        await LoadDataAsync();
    }
    [RelayCommand]
    public async Task RemoveActivityAsync(Guid ActivityId)
    {
        ActivityListModel? deletedActivity = null;
        if (Subject is not null)
        {
            deletedActivity = Subject.Activities.FirstOrDefault(a => a.Id == ActivityId);
        }
        if (deletedActivity is not null)
        {
            await activityFacade.DeleteAsync(deletedActivity.Id);
        }
        await ReloadDataAsync();
        await LoadDataAsync();
    }
    private async Task ReloadDataAsync()
    {
        Subject = await subjectFacade.GetAsync(Id) ?? SubjectDetailModel.Empty;
    }
    [RelayCommand]
    public async Task DeleteAsync()
    {
        if (Subject is not null)
        {
            try
            {
                await subjectFacade.DeleteAsync(Subject.Id);
                MessengerService.Send(new SubjectDeleteMessage());
                navigationService.SendBackButtonPressed();
            }
            catch (InvalidOperationException)
            {
                await alertService.DisplayAsync("filler", "filler");
            }
        }
    }

    public async void Receive(SubjectEditMessage message)
    {
        if (message.SubjectId == Id)
        {
            await LoadDataAsync();
        }
    }

    public void Receive(SubjectDeleteMessage message)
    {
        navigationService.SendBackButtonPressed();
    }

    public async void Receive(StudentDeleteMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(ActivityDeleteMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(ActivityEditMessage message)
    {
       await LoadDataAsync();
    }
}
