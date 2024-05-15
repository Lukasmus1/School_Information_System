using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using VUTIS2.App.Messages;
using VUTIS2.App.Services;
using VUTIS2.BL.Facades;
using VUTIS2.BL.Models;

namespace VUTIS2.App.ViewModels;
[QueryProperty(nameof(subjectId), nameof(subjectId))]
public partial class ActivityListViewModel(IActivityFacade activityFacade, INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<ActivityEditMessage>, IRecipient<ActivityDeleteMessage>
{
    public Guid subjectId { get; set; }
    private bool sortedStartTimeAscending = true;
    private bool sortedEndTimeAscending = true;

    public bool FilterStartAscending { get; set; } = true;
    public bool FilterEndAscending { get; set; } = true;

    DateTime startTime = DateTime.Now;
    DateTime endTime = DateTime.Now;
    public IEnumerable<ActivityListModel> Activities { get; set; } = null!;
    public DateTime StartTimeDate
    {
        get => startTime;
        set
        {
            if (startTime.Date != value)
            {
                startTime = new DateTime(value.Year, value.Month, value.Day,
                    StartTimeDate.Hour, StartTimeDate.Minute, StartTimeDate.Second);
                OnPropertyChanged(nameof(startTime));
            }
        }
    }
    public TimeSpan StartTimeTimeSpan
    {
        get => startTime.TimeOfDay;
        set
        {
            if (startTime.TimeOfDay != value)
            {
                startTime = new DateTime(StartTimeDate.Year, StartTimeDate.Month, StartTimeDate.Day,
                    value.Hours, value.Minutes, value.Seconds);
                OnPropertyChanged(nameof(startTime));
            }
        }
    }

    public DateTime EndTimeDate
    {
        get => endTime.Date;
        set
        {
            if (endTime.Date != value.Date)
            {
                endTime = new DateTime(value.Year, value.Month, value.Day,
                    EndTimeTimeSpan.Hours, EndTimeTimeSpan.Minutes, EndTimeTimeSpan.Seconds);
                OnPropertyChanged(nameof(endTime));
            }
        }
    }
    public TimeSpan EndTimeTimeSpan
    {
        get => endTime.TimeOfDay;
        set
        {
            if (endTime.TimeOfDay != value)
            {
                endTime = new DateTime(EndTimeDate.Year, EndTimeDate.Month, EndTimeDate.Day,
                    value.Hours, value.Minutes, value.Seconds);
                OnPropertyChanged(nameof(endTime));
            }
        }
    }
    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        Activities = await activityFacade.GetAsyncBySubject(subjectId);
    }

    [RelayCommand]
    public async Task GoToActivityDetailAsync(Guid id)
    {
        await navigationService.GoToAsync("/activitydetail",
            new Dictionary<string, object?> { [nameof(ActivityDetailViewModel.Id)] = id });
    }

    [RelayCommand]
    public async Task GoToCreateAsync()
    {
        await navigationService.GoToAsync("/activityedit", new Dictionary<string, object?>{ [nameof(ActivityEditViewModel.SubjectId)] = subjectId, [nameof(ActivityEditViewModel.Activity)] = ActivityDetailModel.Empty });
    }

    [RelayCommand]
    public async Task DeleteAsync(Guid id)
    {
        await activityFacade.DeleteAsync(id);
        MessengerService.Send(new ActivityDeleteMessage());
    }
    [RelayCommand]
    public async Task SortByStartTimeAsync()
    {
        if (sortedStartTimeAscending)
        {
            Activities = activityFacade.GetOrderedByStartTimeAsc(Activities.ToList());
            sortedStartTimeAscending = false;
        }
        else
        {
            Activities = activityFacade.GetOrderedByStartTimeDesc(Activities.ToList());
            sortedStartTimeAscending = true;
        }

        await base.LoadDataAsync();
    }

    [RelayCommand]
    public async Task FilterByStartTimeAsync()
    {
        Activities = await activityFacade.GetActivitiesStartTime(startTime, FilterStartAscending, subjectId);
        await base.LoadDataAsync();
    }

    [RelayCommand]
    public async Task FilterByEndTimeAsync()
    {
        Activities = await activityFacade.GetActivitiesEndTime(endTime, FilterEndAscending, subjectId);
        await base.LoadDataAsync();
    }
    [RelayCommand]
    public async Task FilterByBothAsync()
    {
        Activities = await activityFacade.GetActivitiesByBoth(startTime, FilterStartAscending, endTime, FilterEndAscending, subjectId);
        await base.LoadDataAsync();
    }

    [RelayCommand]
    public async Task SortByEndTimeAsync()
    {
        if (sortedEndTimeAscending)
        {
            Activities = activityFacade.GetOrderedByEndTimeAsc(Activities.ToList());
            sortedEndTimeAscending = false;
        }
        else
        {
            Activities = activityFacade.GetOrderedByEndTimeDesc(Activities.ToList());
            sortedEndTimeAscending = true;
        }

        await base.LoadDataAsync();
    }

    public async void Receive(ActivityEditMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(ActivityDeleteMessage message)
    {
        await LoadDataAsync();
    }
}
