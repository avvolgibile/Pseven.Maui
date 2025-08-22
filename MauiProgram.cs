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

#if WINDOWS
            //builder.ConfigureLifecycleEvents(events =>
            //{
            //    // Make sure to add "using Microsoft.Maui.LifecycleEvents;" in the top of the file
            //    events.AddWindows(windowsLifecycleBuilder =>
            //    {
            //        windowsLifecycleBuilder.OnWindowCreated(window =>
            //        {
            //            window.ExtendsContentIntoTitleBar = false;
            //            var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
            //            var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
            //            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
            //            switch (appWindow.Presenter)
            //            {
            //                case Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter:
            //                    overlappedPresenter.SetBorderAndTitleBar(true, true);
            //                    overlappedPresenter.Maximize();
            //                    break;
            //            }
            //        });
            //    });
            //});
#endif

#if DEBUG
            builder.Logging.AddDebug();
#endif

            //Services
            builder.Services.AddSingleton<InternalDataBase>();
            builder.Services.AddSingleton<ExternalDataBase>();
            //builder.Services.AddSingleton<PrintService>();
            builder.Services.AddSingleton<OrdineViewModel>();
            builder.Services.AddSingleton<OrdinePage>();
            builder.Services.AddSingleton<DocumentoViewModel>();
            builder.Services.AddSingleton<DocumentoPage>();
            builder.Services.AddSingleton<ArticoliViewModel>();
            builder.Services.AddSingleton<ArticoliPage>();
            builder.Services.AddSingleton<MiniFormViewModel>();
            builder.Services.AddSingleton<MiniFormPage>();
           // builder.Services.AddSingleton<DocumentiApertiPage>();

            return builder.Build();
        }

    }


}
