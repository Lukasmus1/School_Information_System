namespace VUTIS2.App.Services;

public interface IAlertService
{
    Task DisplayAsync(string title, string message);
}
