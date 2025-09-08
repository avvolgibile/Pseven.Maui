using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Pseven.Models;
using CommunityToolkit.Mvvm.Input;
using Pseven.Views;
using Pseven.Services;
using Pseven.Models;


namespace Pseven.ViewModels
{
    public partial class DocumentiApertiViewModels : ObservableObject
    {

        public ObservableCollection<StoricoDocumento> DocumentiAperti { get; set; } = new();

        private readonly DocumentiApestiService _service = new();

        public Func<string, string, string, Task>? ShowAlert; //serve a far comparire l' allert solo sulla page relativa, altrimenti manda a video la main page, mettere anche nel costruttore della page   _viewModel.ShowAlert = (title, msg, ok) => this.DisplayAlert(title, msg, ok);

        [ObservableProperty]
        private StoricoDocumento? dgwItemselezionatoConTastoDx;

        public DocumentiApertiViewModels()
        {
           
            CaricaDati();
        }

      
       

        private async void CaricaDati()
        {
            var lista = await _service.GetAllAsync();
            foreach (var item in lista)
                DocumentiAperti.Add(item);

        }





        [RelayCommand]
        async Task Elimina(StoricoDocumento? item)
        {
            if (item == null) return;

            // Mostra la message box con l'ID della riga
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                // await App.Current.MainPage.DisplayAlert("Modifica", $"ID: {item.H}", "OK");
                await ShowAlert("Modifica", $"ID: {item.Indirizzo}", "OK");
                
            });
        }

        [RelayCommand]
        async Task Esporta(StoricoDocumento? item)
        {
            if (item == null) return;

            // Mostra la message box con l'ID della riga
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                // await App.Current.MainPage.DisplayAlert("Modifica", $"ID: {item.H}", "OK");
                await ShowAlert("Modifica", $"ID: {item.Indirizzo + "chichichi"}", "OK");
            });

        }





    }







    ////private async Task CaricaDatiAsync()
    ////{
    ////    //try
    ////    //{
    ////    //    using var client = new HttpClient();
    ////    //    var result = await client.GetFromJsonAsync<List<DettaglioDocumento>>("https://localhost:7208/api/DettaglioDocumento");

    ////    //    if (result is not null)
    ////    //        Documenti = new ObservableCollection<DettaglioDocumento>(result);
    ////    //}
    ////    //catch (Exception ex)
    ////    //{
    ////    //    Debug.WriteLine($"Errore caricamento dati: {ex.Message}");
    ////    //}
    ////}
}


