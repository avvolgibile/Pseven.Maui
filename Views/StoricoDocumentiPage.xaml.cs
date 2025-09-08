using Pseven.Models;
using Pseven.ViewModels;
using Syncfusion.Maui.Core.Carousel;
using Syncfusion.Maui.DataGrid;

namespace Pseven.Views;

public partial class StoricoDocumentiPage : ContentPage
{

   
    private readonly StoricoDocumentiViewModel _viewModel;
    // 🔹 Costruttore vuoto: serve per Shell, DI, o inizializzazione successiva
    public StoricoDocumentiPage()
    {
        InitializeComponent();
    }

    // 🔹 Costruttore con ViewModel: comodo quando apri la finestra direttamente
    public StoricoDocumentiPage(StoricoDocumentiViewModel viewmodel) : this()//chiamata al costruttore vuoto dove c'e InitializeComponent
    {
        _viewModel = viewmodel;
        BindingContext = _viewModel;

        // L’alert viene visualizzato su QUESTA finestra/pagina
        _viewModel.ShowAlert = (title, msg, ok) => this.DisplayAlert(title, msg, ok);


#if WINDOWS
        this.Loaded += OnLoadedWin;//1)
                                   //this.Unloaded += OnUnloadedWin;
#endif
    }



#if WINDOWS// 1)se metto questo nel costruttore, senza onloated non fa in tempo ad agganviare l' evento
    private void OnLoadedWin(object? sender, EventArgs e)
    {
        var mauiWin = this.Window?.Handler?.PlatformView as Microsoft.Maui.MauiWinUIWindow;

        if (mauiWin?.Content is Microsoft.UI.Xaml.FrameworkElement root)
        {
            root.KeyDown += OnKeyDown;
        }
    }

    private void OnKeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        bool ctrl = (Microsoft.UI.Input.InputKeyboardSource
            .GetKeyStateForCurrentThread(Windows.System.VirtualKey.Control)
            & Windows.UI.Core.CoreVirtualKeyStates.Down) == Windows.UI.Core.CoreVirtualKeyStates.Down;

        bool shift = (Microsoft.UI.Input.InputKeyboardSource
            .GetKeyStateForCurrentThread(Windows.System.VirtualKey.Shift)
            & Windows.UI.Core.CoreVirtualKeyStates.Down) == Windows.UI.Core.CoreVirtualKeyStates.Down;

       

        if (ctrl && shift && e.Key == Windows.System.VirtualKey.A)
        {
            e.Handled = true;

            Microsoft.Maui.Controls.Application.Current.OpenWindow(new Microsoft.Maui.Controls.Window(new StoricoArticoliPage(new StoricoArticoliViewModel())));
        }
    }
#endif



    private void DataGrid_SelectionChanged(object sender, DataGridSelectionChangedEventArgs e)
    {
        if (BindingContext is StoricoDocumentiViewModel vm)
        {
            var row = e.AddedRows?.FirstOrDefault();
            if (row != null)
            {
                vm.DgwItemselezionatoConTastoDx =
                    row as StoricoDocumento ??
                    (row as DataGridRowInfo)?.RowData as StoricoDocumento;
            }
        }
    }



}