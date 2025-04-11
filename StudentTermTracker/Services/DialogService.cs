namespace StudentTermTracker.Services
{
    public class DialogService : IDialogService
    {
        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel = null)
        {
            if (cancel == null)
            {
                await Application.Current.MainPage.DisplayAlert(title, message, accept);
                return true;
            }

            return await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }

        public async Task DisplayAlertAsync(string title, string message, string cancel)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }
    }
}
