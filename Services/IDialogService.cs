using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTermTracker.Services
{
    public interface IDialogService
    {
        Task<bool> DisplayAlert(string title,
            string message,
            string accept, 
            string cancel = null);
    }
}
