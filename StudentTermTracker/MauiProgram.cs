using Microsoft.Extensions.Logging;
using StudentTermTracker.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Components.Authorization;

#if ANDROID
using Android.App;
#endif

namespace StudentTermTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            //builder.Services.AddSingleton<IAuthService>(serviceProvider =>
            //{
            //#if ANDROID
            //                var currentActivity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;
            //                return new MsalAuthService(currentActivity);
            //#else
            //                return new MsalAuthService();
            //#endif
            //});

            //builder.Services.AddAuthorizationCore();
            builder.Services.AddAuthorizationCore(options =>
            {
                options.AddPolicy("ReadWeather", policy => policy.RequireClaim("roles", "Weather.Read"));
            });
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.TryAddScoped<AuthenticationStateProvider, ExternalAuthStateProvider>();
            builder.Services.AddSingleton<IUserDataService, AzureTableUserService>();
            
            builder.Services.AddScoped<IDialogService, DialogService>();
            builder.Services.AddScoped<ShareService>();

            var host = builder.Build();
            return host;
        }
    }
}
