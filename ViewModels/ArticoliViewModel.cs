using Pseven.Data;
using Pseven.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pseven.ViewModels
{
    public class ArticoliViewModel : BaseViewModel
    {
        #region Proprietà
        private readonly InternalDataBase _internalDataBase;
        private ObservableCollection<Articolo> _articoli;
        public ObservableCollection<Articolo> Articoli
        {
            get { return _articoli; }
            set { SetProperty(ref _articoli, value); }
        }

        #endregion

        #region Eventi
        #endregion

        #region Comandi
        #endregion

        #region Costruttori
        public ArticoliViewModel(InternalDataBase internaldatabase)
        {
            Title = "Gestione Articoli";
            _internalDataBase = internaldatabase;
            Task.Run(LoadArticoli);
        }
        #endregion

        #region Metodi
        private async Task LoadArticoli()
        {
            Articoli = new ObservableCollection<Articolo>(await _internalDataBase.GetArticoliAsync());
        }
        #endregion
    }
}
