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

    public partial class StoricoOrdiniAFornitoriViewModel
    {
        public Func<string, string, string, Task>? ShowAlert; //serve a far comparire l' allert solo sulla page relativa, altrimenti manda a video la main page, mettere anche nel costruttore della page   _viewModel.ShowAlert = (title, msg, ok) => this.DisplayAlert(title, msg, ok);
        public ObservableCollection<StoricoOrdineAFornitore> StoricoOrdiniAFornitori { get; set; } = new();

        private readonly StoricoOrdiniAfornitoriService _service = new();
        public StoricoOrdiniAFornitoriViewModel()
        {
         CaricaDati();
        }

        private async void CaricaDati()
        {
            var lista = await _service.GetAllAsync();
            foreach (var item in lista)
                StoricoOrdiniAFornitori.Add(item);//

        }



    }


}
