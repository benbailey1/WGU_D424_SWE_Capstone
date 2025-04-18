using Microsoft.Extensions.Logging;
using StudentTermTracker.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Http;


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

            builder.Services.AddHttpClient("AppHttpClient", (client) =>
            {
                client.BaseAddress = new Uri(Constants.API_BASE_URL);
            });
            
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddScoped<IDialogService, DialogService>();
            builder.Services.AddScoped<ShareService>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            // add ClearProviders and SetMinimumLogLevel to reduce the insane amount of logs in debug
            builder.Logging.ClearProviders(); 
            builder.Logging.AddDebug();
            builder.Logging.SetMinimumLevel(LogLevel.Error);
#endif      
            var host = builder.Build();
            return host;
        }
    }
}
