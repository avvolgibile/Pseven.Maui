using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pseven.Models;
using Pseven.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pseven.ViewModels
{
    public partial class DocumentoViewModel : BaseViewModel
    {
        public Func<string, string, string, Task>? ShowAlert; //serve a far comparire l' allert solo sulla page relativa, altrimenti manda a video la main page, mettere anche nel costruttore della page   _viewModel.ShowAlert = (title, msg, ok) => this.DisplayAlert(title, msg, ok);
        public ObservableCollection<StoricoArticolo> DocumentoCollection { get;  } = new();


        [ObservableProperty]
        private StoricoArticolo? dgwItemselezionatoConTastoDx;

        private readonly StoricoArticoliService _service = new();//ATTENZIONE
        public DocumentoViewModel()
        {

            CaricaDati();
           
        }



        private async void CaricaDati()
        {
            var lista = await _service.GetAllAsync();
            foreach (var item in lista)
                DocumentoCollection.Add(item);

        }



        [RelayCommand]
        async Task Elimina(StoricoArticolo? item)
        {
            if (item == null) return;

            // Mostra la message box con l'ID della riga
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                // await App.Current.MainPage.DisplayAlert("Modifica", $"ID: {item.H}", "OK");
                await ShowAlert("Elimina", $"ID: {item.Data}", "OK");
            });
        }

      

        [RelayCommand]
        async Task RistampaEtichetta(StoricoArticolo? item)
        {
            if (item == null) return;

            // Mostra la message box con l'ID della riga
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                // await App.Current.MainPage.DisplayAlert("Modifica", $"ID: {item.H}", "OK");
                await ShowAlert("ristampa", $"ID: {item.Data + "chichichi"}", "OK");
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
                await ShowAlert("reinserisci", $"ID: {item.Data + "chichichi"}", "OK");
            });

        }

    }

}
