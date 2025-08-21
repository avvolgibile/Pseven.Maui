
using System.Windows.Input;

namespace Pseven.Controls;

public partial class BasePage : ContentPage
{
    public static readonly BindableProperty IsAlwaysOnTopProperty = BindableProperty.Create(nameof(IsAlwaysOnTop), typeof(bool), typeof(BasePage), false, BindingMode.TwoWay);
    public static readonly BindableProperty IsResizableProperty = BindableProperty.Create(nameof(IsResizable), typeof(bool), typeof(BasePage), true, BindingMode.TwoWay);
    public static readonly BindableProperty IsMaximizableProperty = BindableProperty.Create(nameof(IsMaximizable), typeof(bool), typeof(BasePage), true, BindingMode.TwoWay);
    public static readonly BindableProperty IsMinimizableProperty = BindableProperty.Create(nameof(IsMinimizable), typeof(bool), typeof(BasePage), true, BindingMode.TwoWay);
    public static readonly BindableProperty IsMaximizedProperty = BindableProperty.Create(nameof(IsMaximized), typeof(bool), typeof(BasePage), false, BindingMode.TwoWay);

    public bool IsAlwaysOnTop
    {
        get => (bool)GetValue(IsAlwaysOnTopProperty);
        set => SetValue(IsAlwaysOnTopProperty, value);
    }
    public bool IsResizable
    {
        get => (bool)GetValue(IsResizableProperty);
        set => SetValue(IsResizableProperty, value);
    }
    public bool IsMaximizable
    {
        get => (bool)GetValue(IsMaximizableProperty);
        set => SetValue(IsMaximizableProperty, value);
    }
    public bool IsMinimizable
    {
        get => (bool)GetValue(IsMinimizableProperty);
        set => SetValue(IsMinimizableProperty, value);
    }
    public bool IsMaximized
    {
        get => (bool)GetValue(IsMaximizedProperty);
        set => SetValue(IsMaximizedProperty, value);
    }
    public BasePage()
    {
        InitializeComponent();
        Loaded += BasePage_Event;
    }

    private void BasePage_Event(object? sender, EventArgs e)
    {
        ChangeWindowProperty();
    }


    //public static Microsoft.UI.Windowing.AppWindow GetAppWindow(MauiWinUIWindow window)
    //{
    //    var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
    //    var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
    //    var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
    //    return appWindow;
    //}

    private void ChangeWindowProperty()
    {
#if WINDOWS
        var parentWindow = GetParentWindow();
        if (parentWindow != null && parentWindow.Handler != null && parentWindow.Handler.PlatformView != null)
        {
            var window = parentWindow.Handler.PlatformView as MauiWinUIWindow;
            var appWindow = Statics.GetAppWindow(window);
            switch (appWindow.Presenter)
            {
                case Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter:
                    overlappedPresenter.IsAlwaysOnTop = IsAlwaysOnTop;
                    overlappedPresenter.IsMaximizable = IsMaximizable;
                    overlappedPresenter.IsMinimizable = IsMinimizable;
                    overlappedPresenter.IsResizable = IsResizable;
                    if (IsMaximized)
                    {
                        overlappedPresenter.Maximize();
                    }
                    break;
            }
        }
#endif
    }
}