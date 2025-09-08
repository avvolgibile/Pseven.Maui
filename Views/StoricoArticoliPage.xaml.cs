using Pseven.Models;
using Pseven.ViewModels;
using Syncfusion.Maui.Core.Carousel;
using Syncfusion.Maui.DataGrid;
using System.Linq;


namespace Pseven.Views;


public partial class StoricoArticoliPage : ContentPage
{
    private readonly StoricoArticoliViewModel _viewModel;//serve se lo dobbiamo usare anche nel code behind   
    // 🔹 Costruttore vuoto: serve per Shell, DI, o inizializzazione successiva
    public StoricoArticoliPage()
    {
        InitializeComponent();
    }

    // 🔹 Costruttore con ViewModel: comodo quando apri la finestra direttamente
    public StoricoArticoliPage(StoricoArticoliViewModel viewmodel) : this()//chiamata al costruttore vuoto dove c'e InitializeComponent
    {
        _viewModel = viewmodel;
        BindingContext = _viewModel;

        // L’alert viene visualizzato su QUESTA finestra/pagina
        _viewModel.ShowAlert = (title, msg, ok) => this.DisplayAlert(title, msg, ok);


    }















    private void DataGrid_SelectionChanged(object sender, DataGridSelectionChangedEventArgs e)
    {
        if (BindingContext is StoricoArticoliViewModel vm)
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
    private void DataGrid_AutoGeneratingColumn(object? sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        throw new NotImplementedException();
    }

   



}