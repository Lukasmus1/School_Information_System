using CommunityToolkit.Mvvm.Input;
using VUTIS2.App.Messages;
using VUTIS2.App.Services;
using VUTIS2.BL.Facades;
using VUTIS2.BL.Models;
using VUTIS2.Common.Enums;

namespace VUTIS2.App.ViewModels;

[QueryProperty(nameof(SubjectId), nameof(SubjectId))]
[QueryProperty(nameof(Activity), nameof(Activity))]
public partial class ActivityEditViewModel(IActivityFacade activityFacade, INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public ActivityDetailModel Activity { get; set; } = ActivityDetailModel.Empty with { StartTime = DateTime.Now, EndTime = DateTime.Now };

    public DateTime StartTimeDate
    {
        get => Activity.StartTime;
        set
        {
            if (Activity.StartTime.Date != value)
            {
                Activity.StartTime = new DateTime(value.Year, value.Month, value.Day,
                    StartTimeDate.Hour, StartTimeDate.Minute, StartTimeDate.Second);
                OnPropertyChanged(nameof(Activity.StartTime));
            }
        }
    }
    public TimeSpan StartTimeTimeSpan
    {
        get => Activity.StartTime.TimeOfDay;
        set
        {
            if (Activity.StartTime.TimeOfDay != value)
            {
                Activity.StartTime = new DateTime(StartTimeDate.Year, StartTimeDate.Month, StartTimeDate.Day,
                    value.Hours, value.Minutes, value.Seconds);
                OnPropertyChanged(nameof(Activity.StartTime));
            }
        }
    }

    public DateTime EndTimeDate
    {
        get => Activity.EndTime.Date;
        set
        {
            if (Activity.EndTime.Date != value.Date)
            {
                Activity.EndTime = new DateTime(value.Year, value.Month, value.Day,
                    EndTimeTimeSpan.Hours, EndTimeTimeSpan.Minutes, EndTimeTimeSpan.Seconds);
                OnPropertyChanged(nameof(Activity.EndTime));
            }
        }
    }
    public TimeSpan EndTimeTimeSpan
    {
        get => Activity.EndTime.TimeOfDay;
        set
        {
            if (Activity.EndTime.TimeOfDay != value)
            {
                Activity.EndTime = new DateTime(EndTimeDate.Year, EndTimeDate.Month, EndTimeDate.Day,
                    value.Hours, value.Minutes, value.Seconds);
                OnPropertyChanged(nameof(Activity.EndTime));
            }
        }
    }

    public List<ActivityType> ActivityTypes { get; } = Enum.GetValues<ActivityType>().ToList();
    public Guid SubjectId { get; set; }


    [RelayCommand]
    public async Task SaveAsync()
    {
        await activityFacade.SaveAsync(Activity with { SubjectId = SubjectId});
        MessengerService.Send(new ActivityEditMessage { ActivityId = Activity.Id });
        navigationService.SendBackButtonPressed();
    }

    [RelayCommand]
    public void Cancel()
    {
        navigationService.SendBackButtonPressed();
    }
}
