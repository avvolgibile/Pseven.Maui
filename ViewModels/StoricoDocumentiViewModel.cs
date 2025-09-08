using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pseven.Models;
using Pseven.Services;
using Pseven.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Pseven.ViewModels;

    
    public partial class StoricoDocumentiViewModel : ObservableObject
{

    public Func<string, string, string, Task>? ShowAlert; //serve a far comparire l' allert solo sulla page relativa, altrimenti manda a video la main page, mettere anche nel costruttore della page   _viewModel.ShowAlert = (title, msg, ok) => this.DisplayAlert(title, msg, ok);

    public ObservableCollection<StoricoDocumento> StoricoDocumenti { get; set; } = new();

        private readonly StoricoDocumentiService _service = new();
    
    
    [ObservableProperty]
    private StoricoDocumento? dgwItemselezionatoConTastoDx;

   
    public StoricoDocumentiViewModel()
        {

            CaricaDati();
     
        }







      private async void CaricaDati()
      {
            var lista = await _service.GetAllAsync();
            foreach (var item in lista)
            StoricoDocumenti.Add(item);//

      }


    [RelayCommand]
    private static void ApriDocumento()
    {
        Application.Current.OpenWindow(new Window(new DocumentoPage(new DocumentoViewModel())));

    }

    [RelayCommand]
    private void ApriStoricoArticoli()
    {
  
        Application.Current.OpenWindow(new Window(new StoricoArticoliPage(new StoricoArticoliViewModel())));
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
