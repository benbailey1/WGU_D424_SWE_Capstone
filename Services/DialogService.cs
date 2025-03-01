using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
