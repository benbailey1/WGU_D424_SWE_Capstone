using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Microsoft.Identity.Client;

namespace StudentTermTracker
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // TODO: Probably remove as this is going to be handled in MauiProgram.cs
            // get the current service provider
            //var services = IPlatformApplication.Current.Services;

            //// get auth service and update it w/ current activity
            //var authService = services.GetService<IAuthService>() as MsalAuthService;
            //if (authService != null)
            //{
            //    authService.SetCurrentActivity(this);
            //}

        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            Platform.OnNewIntent(intent);
        }
    }
}
