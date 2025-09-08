using Pseven.ViewModels;
using Syncfusion.Maui.DataGrid;
using Pseven.Models;
using System.Threading.Tasks;

namespace Pseven.Views;

public partial class DocumentoPage : ContentPage
{
   
    
    
    
    private readonly DocumentoViewModel _viewModel;


    // 🔹 Costruttore vuoto: serve per Shell, DI, o inizializzazione successiva
    public DocumentoPage()
    {
        InitializeComponent();


//        if (BindingContext is DocumentoViewModel viewmodel)
//        {
//            _viewModel = viewmodel;

//#if WINDOWS
//            this.Loaded += OnLoadedWin;//1)
//                                       //this.Unloaded += OnUnloadedWin;
//#endif
//        }

//        // L’alert viene visualizzato su QUESTA finestra/pagina
//        _viewModel.ShowAlert = (title, msg, ok) => this.DisplayAlert(title, msg, ok);





    }

    // 🔹 Costruttore con ViewModel: comodo quando apri la finestra direttamente
    public DocumentoPage(DocumentoViewModel viewmodel) : this()//chiamata al costruttore vuoto dove c'e InitializeComponent
    {
        _viewModel = viewmodel;
        BindingContext = _viewModel ;


        //        // L’alert viene visualizzato su QUESTA finestra/pagina
        _viewModel.ShowAlert = (title, msg, ok) => this.DisplayAlert(title, msg, ok);
#if WINDOWS
        this.Loaded += OnLoadedWin;//1)
                                   //this.Unloaded += OnUnloadedWin;
#endif

    }



#if WINDOWS
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

        if (ctrl && shift && e.Key == Windows.System.VirtualKey.D)
        {
            e.Handled = true;
            _ = DisplayAlert("ciao", "Ciao", "chiudi");
        }

        if (ctrl && shift && e.Key == Windows.System.VirtualKey.A)
        {
            e.Handled = true;
            _ = DisplayAlert("ciao2", "Ciao", "chiudi");
        }
    }
#endif
   




    private void DataGrid_SelectionChanged(object sender, DataGridSelectionChangedEventArgs e)
    {
        if (BindingContext is DocumentoViewModel vm)
        {
            var row = e.AddedRows?.FirstOrDefault();
            if (row != null)
            {
                vm.DgwItemselezionatoConTastoDx =
                    row as StoricoArticolo ??
                    (row as DataGridRowInfo)?.RowData as StoricoArticolo;
            }
        }
    }


}