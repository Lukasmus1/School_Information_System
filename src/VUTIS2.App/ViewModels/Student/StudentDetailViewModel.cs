using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using VUTIS2.App.Messages;
using VUTIS2.App.Services;
using VUTIS2.BL.Facades;
using VUTIS2.BL.Models;

namespace VUTIS2.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class StudentDetailViewModel( IStudentFacade studentFacade, INavigationService navigationService, IMessengerService messengerService, IAlertService alertService, IEnrollmentFacade enrollmentFacade, ISubjectFacade subjectFacade)
    : ViewModelBase(messengerService), IRecipient<StudentEditMessage>
{
    public Guid Id { get; set; }
    public StudentDetailModel? Student { get; private set; }

    public IEnumerable<SubjectListModel?> Subjects { get; private set; } = Enumerable.Empty<SubjectListModel>();

    public IEnumerable<SubjectListModel?> EnrolledSubjects { get; private set; } = Enumerable.Empty<SubjectListModel>();
    public EnrollmentDetailModel Enrollment { get; set; } = EnrollmentDetailModel.Empty;
    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        Student = await studentFacade.GetAsync(Id);
        EnrolledSubjects = Enumerable.Empty<SubjectListModel>();
        Subjects = await subjectFacade.GetAsync();
        List<SubjectListModel?> subjectsList = Subjects.ToList();
        if (Student is not null)
        {
            foreach (EnrollmentListModel enrollment in Student.Enrollments)
            {
                SubjectListModel? subject = await subjectFacade.GetAsyncList(enrollment.SubjectId);
                subjectsList.Remove(subject);
                EnrolledSubjects = EnrolledSubjects.Append(subject);
            }
        }
        Subjects = subjectsList;
    }
    [RelayCommand]
    public async Task GoToSubjectDetailAsync(Guid id)
    {
        await navigationService.GoToAsync("/subjectdetail",
            new Dictionary<string, object?> { [nameof(SubjectDetailViewModel.Id)] = id });
    }
    [RelayCommand]

    public async Task GoToStudentEditAsync()
    {
        await navigationService.GoToAsync("/edit",
            new Dictionary<string, object?> { [nameof(StudentEditViewModel.Student)] = Student });
    }
    [RelayCommand]
    public async Task DeleteAsync()
    {
        if (Student is not null)
        {
            try
            {
                await studentFacade.DeleteAsync(Student.Id);
                MessengerService.Send(new StudentDeleteMessage());
                navigationService.SendBackButtonPressed();
            }
            catch (InvalidOperationException)
            {
                await alertService.DisplayAsync("filler", "filler");
            }
        }
    }
    [RelayCommand]
    public async Task AddEnrollmentAsync(Guid SubjectId)
    {
        if (Student is not null)
        {
            Enrollment = EnrollmentDetailModel.Empty with {SubjectId = SubjectId, StudentId = Student.Id};
        }
        await enrollmentFacade.SaveAsync(Enrollment with {Student = default!, Subject = default!});
        await ReloadDataAsync();
        await LoadDataAsync();
    }

    [RelayCommand]
    public async Task RemoveEnrollmentAsync(Guid SubjectId)
    {
        EnrollmentListModel? deletedEnrollment = null;
        if (Student is not null)
        {
            deletedEnrollment = Student.Enrollments.FirstOrDefault(e => e.SubjectId == SubjectId);
        }
        if (deletedEnrollment is not null)
        {
            await enrollmentFacade.DeleteAsync(deletedEnrollment.Id);
        }
        await ReloadDataAsync();
        await LoadDataAsync();
    }

    private async Task ReloadDataAsync()
    {
        if (Student is not null)
        {
            Student = await studentFacade.GetAsync(Student.Id) ?? StudentDetailModel.Empty;
        }
    }
    public async void Receive(StudentEditMessage message)
    {
        if (message.StudentId == Id)
        {
            await LoadDataAsync();
        }
    }
}
