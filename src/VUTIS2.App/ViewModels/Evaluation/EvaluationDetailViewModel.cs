using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using VUTIS2.App.Messages;
using VUTIS2.App.Services;
using VUTIS2.BL.Facades;
using VUTIS2.BL.Models;

namespace VUTIS2.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class EvaluationDetailViewModel(IEvaluationFacade evaluationFacade, INavigationService navigationService, IMessengerService messengerService, IAlertService alertService, IActivityFacade activityFacade)
    : ViewModelBase(messengerService), IRecipient<EvaluationEditMessage>
{
    public Guid Id { get; set; }

    public Guid activityId { get; set; }
    public EvaluationDetailModel? Evaluation { get; private set; }

    public ActivityListModel? Activity { get; private set; } = ActivityListModel.Empty;
    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        Evaluation = await evaluationFacade.GetAsync(Id);
        if(Evaluation is not null)
        {
            activityId = Evaluation.ActivityId;
        }
        Activity = await activityFacade.GetAsyncList(activityId);
    }
    [RelayCommand]
    public async Task DeleteAsync()
    {
        if (Evaluation is not null)
        {
            try
            {
                await evaluationFacade.DeleteAsync(Evaluation.Id);
                MessengerService.Send(new EvaluationDeleteMessage());
                navigationService.SendBackButtonPressed();
            }
            catch (InvalidOperationException)
            {
                await alertService.DisplayAsync("filler", "filler");
            }
        }
    }
    [RelayCommand]
    public async Task GoToEditAsync()
    {
        await navigationService.GoToAsync("/evaluationedit",
            new Dictionary<string, object?> { [nameof(EvaluationEditViewModel.Evaluation)] = Evaluation, [nameof(EvaluationEditViewModel.activityId)] = activityId });
    }

    public async void Receive(EvaluationEditMessage message)
    {
        if (message.EvaluationId == Id)
        {
            await LoadDataAsync();
        }
    }
}
