using Microsoft.Identity.Client;
using Android.App;
using Android.Content;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTermTracker.Platforms.Android
{
    [Activity(Exported = true)]
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault},
        DataHost = "auth",
        DataScheme = "msal1c1861e0-e669-4ecb-8c1e-0fb6fa932d3a")]
    public class MsalActivity : BrowserTabActivity
    {
    }
}
