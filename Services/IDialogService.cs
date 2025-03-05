
namespace StudentTermTracker.Services
{
    public interface IDialogService
    {
        Task<bool> DisplayAlert(string title,
            string message,
            string accept, 
            string cancel = null);

        Task DisplayAlertAsync(string title,
            string message,
            string cancel);
    }
}
