using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using VUTIS2.App.Messages;
using VUTIS2.App.Services;
using VUTIS2.BL.Facades;
using VUTIS2.BL.Models;

namespace VUTIS2.App.ViewModels;


public partial class SubjectListViewModel(ISubjectFacade subjectFacade, INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<SubjectEditMessage>, IRecipient<SubjectDeleteMessage>
{
    bool sortedAscendingName = true;
    bool sortedAscendingAbr = true;

    public string SearchText { get; set; } = string.Empty;

    public IEnumerable<SubjectListModel> Subjects { get; set; } = null!;
    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        Subjects = await subjectFacade.GetAsync();
    }
    [RelayCommand]
    public async Task GoToSubjectDetailAsync(Guid id)
    {
        await navigationService.GoToAsync("/detail",
            new Dictionary<string, object?> { [nameof(SubjectDetailViewModel.Id)] = id });
    }
    [RelayCommand]
    public async Task GoToCreateAsync()
    {
        await navigationService.GoToAsync("/edit");
    }
    [RelayCommand]
    public async Task DeleteAsync(Guid id)
    {
        await subjectFacade.DeleteAsync(id);
        MessengerService.Send(new SubjectDeleteMessage());
    }

    [RelayCommand]
    public async Task SearchAsync()
    {
        Subjects = await subjectFacade.SearchAsync(SearchText);
        await base.LoadDataAsync();
    }

    [RelayCommand]
    public async Task SortByNameAsync()
    {
        if (sortedAscendingName)
        {
            Subjects = subjectFacade.GetOrderedByNameAsc(Subjects.ToList());
            sortedAscendingName = false;
        }
        else
        {
            Subjects = subjectFacade.GetOrderedByNameDesc(Subjects.ToList());
            sortedAscendingName = true;
        }

        await base.LoadDataAsync();
    }
    [RelayCommand]
    public async Task SortByAbbreviation()
    {
        if (sortedAscendingAbr)
        {
            Subjects = subjectFacade.GetOrderedByAbbreviationAsc(Subjects.ToList());
            sortedAscendingAbr = false;
        }
        else
        {
            Subjects = subjectFacade.GetOrderedByAbbreviationDesc(Subjects.ToList());
            sortedAscendingAbr = true;
        }
        await base.LoadDataAsync();
    }
    public async void Receive(SubjectEditMessage message)
    {
        await LoadDataAsync();
    }
    public async void Receive(SubjectDeleteMessage message)
    {
        await LoadDataAsync();
    }
}
