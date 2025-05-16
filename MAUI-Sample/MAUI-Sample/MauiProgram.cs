using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using MAUI_Sample.Services;
using MAUI_Sample.ViewModels;
using MAUI_Sample.Views;
using WireMock.Server;
using System.Net.Http;

namespace MAUI_Sample
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            // Configuration WireMock
            var mockServer = ApiMockSetup.ConfigureApiMock();
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(mockServer.Urls[0])
            };

            // Enregistrement des services
            builder.Services.AddSingleton<ITaskService>(new MockTaskService(httpClient));
            builder.Services.AddSingleton<WireMockServer>(mockServer);

            // Enregistrement des ViewModels
            builder.Services.AddTransient<TasksViewModel>();
            builder.Services.AddTransient<TaskDetailViewModel>();

            // Enregistrement des Pages
            builder.Services.AddTransient<TasksPage>();
            builder.Services.AddTransient<TaskDetailPage>();

            return builder.Build();
        }
    }
}