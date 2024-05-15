using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using VUTIS2.App.Messages;
using VUTIS2.App.Services;
using VUTIS2.BL.Facades;
using VUTIS2.BL.Models;

namespace VUTIS2.App.ViewModels;


public partial class StudentListViewModel(IStudentFacade studentFacade,  INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<StudentEditMessage>, IRecipient<StudentDeleteMessage>
{
    public IEnumerable<StudentListModel> Students { get; set; } = null!;

    bool sortedAscendingLastName = true;
    bool sortedAscendingFirstName = true;

    public string SearchText { get; set; } = string.Empty;

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        Students = await studentFacade.GetAsync();
    }
    [RelayCommand]
    public async Task GoToDetailAsync(Guid id)
    {
        await navigationService.GoToAsync("detail",
            new Dictionary<string, object?> { [nameof(StudentDetailViewModel.Id)] = id });
    }

    [RelayCommand]
    public async Task DeleteAsync(Guid id)
    {
        await studentFacade.DeleteAsync(id);
        MessengerService.Send(new StudentDeleteMessage());
    }

    [RelayCommand]
    public async Task SearchAsync()
    {
        Students = await studentFacade.SearchAsync(SearchText);
        await base.LoadDataAsync();
    }

    [RelayCommand]
    public async Task SortByLastNameAsync()
    {
        if(sortedAscendingLastName)
        {
            Students = studentFacade.GetOrderedByLastNameAsc(Students.ToList());
            sortedAscendingLastName = false;
        }
        else
        {
            Students = studentFacade.GetOrderedByLastNameDesc(Students.ToList());
            sortedAscendingLastName = true;
        }
        await base.LoadDataAsync();
    }

    [RelayCommand]
    public async Task SortByFirstNameAsync()
    {
        if (sortedAscendingFirstName)
        {
            Students = studentFacade.GetOrderedByFirstNameAsc(Students.ToList());
            sortedAscendingFirstName = false;
        }
        else
        {
            Students = studentFacade.GetOrderedByFirstNameDesc(Students.ToList());
            sortedAscendingFirstName = true;
        }
        await base.LoadDataAsync();
    }

    [RelayCommand]
    public async Task GoToCreateAsync()
    {
        await navigationService.GoToAsync("/edit");
    }
    public async void Receive(StudentEditMessage message)
    {
        await LoadDataAsync();
    }
    public async void Receive(StudentDeleteMessage message)
    {
        await LoadDataAsync();
    }
}
