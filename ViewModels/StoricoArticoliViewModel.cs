
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pseven.Models;
using Pseven.Services;
using Syncfusion.Maui.DataGrid;
using System.Collections.ObjectModel;
using System.Linq;


namespace Pseven.ViewModels;

public partial class StoricoArticoliViewModel : ObservableObject
{
    public Func<string, string, string, Task>? ShowAlert; //serve a far comparire l' allert solo sulla page relativa, altrimenti manda a video la main page, mettere anche nel costruttore della page   _viewModel.ShowAlert = (title, msg, ok) => this.DisplayAlert(title, msg, ok);
    public ObservableCollection<StoricoArticolo> StoricoArticoli { get; set; } = new();

    [ObservableProperty]    
    private StoricoArticolo? dgwItemselezionatoConTastoDx;  

  
    
    private readonly StoricoArticoliService _service = new();


    public StoricoArticoliViewModel()
    {
        
        CaricaDati();

        

    }

    [RelayCommand]
    async Task RistampaEtichetta(StoricoArticolo? item)
    {
        if (item == null) return;

        // Mostra la message box con l'ID della riga
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
           // await App.Current.MainPage.DisplayAlert("Modifica", $"ID: {item.H}", "OK");
            await ShowAlert("Modifica", $"ID: {item.H}", "OK");
            
        });
    }

    [RelayCommand]
    async Task Reinserisci(StoricoArticolo? item)
    {
        if (item == null) return;

        // Mostra la message box con l'ID della riga
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            // await App.Current.MainPage.DisplayAlert("Modifica", $"ID: {item.H}", "OK");
            await ShowAlert("Modifica", $"ID: {item.H+"chichichi"}", "OK");
        });
    }




    private async void CaricaDati()
    {
        var lista = await _service.GetAllAsync();
        foreach (var item in lista)
            StoricoArticoli.Add(item);
        
    }
    [RelayCommand]
    public async Task TestPdfManualeAsync()
    {


        if (StoricoArticoli is null || !StoricoArticoli.Any())
        {
            await Shell.Current.DisplayAlert("Attenzione", "Nessun articolo da stampare", "OK");
            return;
        }

        try
        {
            // 2) Genera il PDF
            var lista = StoricoArticoli.ToList();
            var pdfBytes = PdfArticoliService.GeneraPdfDallaLista(lista);

            // 3) Salva su file temporaneo
            var fileName = $"StoricoArticoli_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
            File.WriteAllBytes(filePath, pdfBytes);

            // 4) Apri con il viewer di sistema
            await Launcher.Default.OpenAsync(
                new OpenFileRequest("Storico articoli", new ReadOnlyFile(filePath))
            );
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Errore", ex.Message, "OK");
        }
    }


}


