using Pseven.ViewModels;
using Syncfusion.Maui.Core.Carousel;

namespace Pseven.Views;

public partial class StoricoOrdiniAFornitoriPage : ContentPage
{

    private readonly StoricoOrdiniAFornitoriViewModel _viewModel;
    // 🔹 Costruttore vuoto: serve per Shell, DI, o inizializzazione successiva
    public StoricoOrdiniAFornitoriPage()
    {
        InitializeComponent();
    }

    // 🔹 Costruttore con ViewModel: comodo quando apri la finestra direttamente
    public StoricoOrdiniAFornitoriPage(StoricoOrdiniAFornitoriViewModel viewmodel) : this()//chiamata al costruttore vuoto dove c'e InitializeComponent
    {
        _viewModel = viewmodel;
        BindingContext = _viewModel;

        // L’alert viene visualizzato su QUESTA finestra/pagina
        _viewModel.ShowAlert = (title, msg, ok) => this.DisplayAlert(title, msg, ok);


    }
}