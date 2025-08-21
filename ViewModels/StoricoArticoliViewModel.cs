using CommunityToolkit.Mvvm.Input;
using System.IO;
using Pdf = Pseven.Services.PdfArticoliService; // cambia con il namespace corretto
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using Pseven.Models;
using Pseven.Services;

namespace Pseven.ViewModels;

public partial class StoricoArticoliViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<StoricoArticolo> articoli;

    public StoricoArticoliViewModel()
    {
        articoli = new ObservableCollection<StoricoArticolo>();
        _ = CaricaDatiAsync();
    }

    private async Task CaricaDatiAsync()
    {
        try
        {
            using var client = new HttpClient();
            var risultato = await client.GetFromJsonAsync<List<StoricoArticolo>>("https://localhost:7107/api/DettaglioDocumento");


           // Console.WriteLine(risultato==null? "risultato null":"trovati risultati");
            
            
            
            if (risultato != null)
                Articoli = new ObservableCollection<StoricoArticolo>(risultato);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Errore nel caricamento: " + ex.Message);
        }
    }
   

    [RelayCommand]
    public async Task TestPdfManualeAsync()
    {


        if (Articoli is null || !Articoli.Any())
        {
            await Shell.Current.DisplayAlert("Attenzione", "Nessun articolo da stampare", "OK");
            return;
        }

        await PdfArticoliService.MostraAnteprimaPdfAsync(Articoli.ToList());

        //var articoloFinto = new StoricoArticolo
        //{
        //    Articoli = "Articolo fittizio per test PDF"
        //};

        //var listaFinta = new List<StoricoArticolo> { articoloFinto };

        //await PdfArticoliService.MostraAnteprimaPdfAsync(listaFinta);
    }

}