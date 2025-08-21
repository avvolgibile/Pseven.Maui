using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Pseven.Controls
{
    public class CustomContentPage : ContentPage
    {
        public static readonly BindableProperty IsAlwaysOnTopProperty = BindableProperty.Create(nameof(IsAlwaysOnTop), typeof(bool), typeof(CustomContentPage), false, BindingMode.TwoWay, propertyChanged: IsAlwaysOnTopPropertyChanged);
        public bool IsAlwaysOnTop
        {
            get => (bool)GetValue(IsAlwaysOnTopProperty);
            set => SetValue(IsAlwaysOnTopProperty, value);
        }
        private static void IsAlwaysOnTopPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomContentPage)bindable;
            control.ChangeIsOnTop();
        }

        public CustomContentPage()
        {
        }

        //public static Microsoft.UI.Windowing.AppWindow GetAppWindow(MauiWinUIWindow window)
        //{
        //    var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
        //    var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
        //    var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
        //    return appWindow;
        //}
        public void ChangeIsOnTop()
        {
#if WINDOWS
            var window = GetParentWindow().Handler.PlatformView as MauiWinUIWindow;
            var appWindow = Statics.GetAppWindow(window);
            switch (appWindow.Presenter)
            {
                case Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter:
                    overlappedPresenter.IsAlwaysOnTop = IsAlwaysOnTop;
                    //overlappedPresenter.IsMaximizable = false;
                    //overlappedPresenter.IsMinimizable = false;
                    //overlappedPresenter.IsResizable = false;
                    break;
            }
#endif

        }
    }
}
