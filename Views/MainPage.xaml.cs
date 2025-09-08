using Microsoft.Maui.Controls.Shapes;
using Pseven.Varie;
using Pseven.ViewModels;
using Pseven.Controls;
using Pseven.Services.Interfaces;
using Pseven.Data;


namespace Pseven.Views;

public partial class MainPage : BasePage
{
	private readonly MainPageViewModel _viewModel;
    

    public MainPage(MainPageViewModel viewmodel)
	{
        InitializeComponent();
        _viewModel = viewmodel;
        BindingContext = _viewModel;  

#if WINDOWS
        var window = Application.Current?
            .Windows?.FirstOrDefault()?
            .Handler?.PlatformView as Microsoft.UI.Xaml.Window;

        if (window?.Content is Microsoft.UI.Xaml.FrameworkElement root)
        {
            root.KeyDown += OnKeyDown;   //qui, non CoreWindow
        }
#endif

    }


#if WINDOWS
    private async void OnKeyDown(object? sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        // valido solo se questa è la pagina visibile
       // if (Shell.Current?.CurrentPage != this) return;

        bool ctrl = (Microsoft.UI.Input.InputKeyboardSource
            .GetKeyStateForCurrentThread(Windows.System.VirtualKey.Control)
            & Windows.UI.Core.CoreVirtualKeyStates.Down) == Windows.UI.Core.CoreVirtualKeyStates.Down;

        bool shift = (Microsoft.UI.Input.InputKeyboardSource
            .GetKeyStateForCurrentThread(Windows.System.VirtualKey.Shift)
            & Windows.UI.Core.CoreVirtualKeyStates.Down) == Windows.UI.Core.CoreVirtualKeyStates.Down;

        if (ctrl && shift && e.Key == Windows.System.VirtualKey.D)
        {
            e.Handled = true;
            //var window = new Window(new StoricoDocumentiPage(new StoricoDocumentiViewModel()));
            Application.Current.OpenWindow(new Window(new StoricoDocumentiPage(new StoricoDocumentiViewModel())));

        }

        if (ctrl && !shift && e.Key == Windows.System.VirtualKey.D)
        {
            e.Handled = true;
            Application.Current.OpenWindow(new Window(new DocumentoPage(new DocumentoViewModel())));
            
        }

        if (ctrl && !shift && e.Key == Windows.System.VirtualKey.K)
        {
            e.Handled = true;          

            var vm = BindingContext as MainPageViewModel;

            var window = new Window(new MiniFormPage(vm))
            {
                Width = 650,
                Height = 500
            };
            Application.Current.OpenWindow(window);

        }
        if (ctrl && shift && e.Key == Windows.System.VirtualKey.A)
        {
            e.Handled = true;



            //var window = new Window(new StoricoArticoliPage(new StoricoArticoliViewModel()));

            //var page = new DocumentoPage { BindingContext = new DocumentoViewModel() };
            //var win = new Window(page);
            //Application.Current.OpenWindow(win);




            Application.Current.OpenWindow(new Window(new StoricoArticoliPage(new StoricoArticoliViewModel())));

            //var window = new Window(new DocumentoPage(new DocumentoViewModel()));
            //Application.Current.OpenWindow(new Window(new DocumentoPage(new DocumentoViewModel())));

        }

    }
#endif

}
   
 