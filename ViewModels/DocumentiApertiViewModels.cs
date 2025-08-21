using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Pseven.Maui.Models;
using CommunityToolkit.Mvvm.Input;
using Pseven.Views;


namespace Pseven.ViewModels
{
    public partial class DocumentiApertiViewModel : ObservableObject
    {

        //[RelayCommand]
        //private async Task ApriDocumentiAperti()
        //{
        //    await Shell.Current.GoToAsync(nameof(DocumentiApertiPage));///////
        //}

        [ObservableProperty]
        private ObservableCollection<DettaglioDocumento> documenti;//////////

        public DocumentiApertiViewModel()
        {
            documenti = new ObservableCollection<DettaglioDocumento>();
            _ = CaricaDatiAsync();
        }

        private async Task CaricaDatiAsync()
        {
            try
            {
                using var client = new HttpClient();
                var result = await client.GetFromJsonAsync<List<DettaglioDocumento>>("https://localhost:7208/api/DettaglioDocumento");

                if (result is not null)
                    Documenti = new ObservableCollection<DettaglioDocumento>(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Errore caricamento dati: {ex.Message}");
            }
        }
    }
}

