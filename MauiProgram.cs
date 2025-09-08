using Microsoft.Maui.LifecycleEvents;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Pseven.Data;
using Pseven.Services;
using Pseven.ViewModels;
using Pseven.Views;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Syncfusion.Maui.Core.Hosting;
using QuestPDF.Infrastructure;

using Pseven.Services.Interfaces;


namespace Pseven
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionCore()
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            QuestPDF.Settings.License = LicenseType.Community;

#if DEBUG
            builder.Logging.AddDebug();
#endif

            //Services
            builder.Services.AddSingleton<InternalDataBase>();
            builder.Services.AddSingleton<ExternalDataBase>();
            //builder.Services.AddSingleton<PrintService>();
            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddSingleton<MainPage>();
            //builder.Services.AddSingleton<DocumentoViewModel>();
            //builder.Services.AddSingleton<DocumentoPage>();

            builder.Services.AddSingleton<MiniFormViewModel>();
            builder.Services.AddSingleton<MiniFormPage>();
            // builder.Services.AddSingleton<DocumentiApertiPage>();
            
            return builder.Build();
        }

    }


}
