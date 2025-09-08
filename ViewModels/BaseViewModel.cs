using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pseven.Etichette;
using Pseven.Models;
using Pseven.Services;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ZXing;



namespace Pseven.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
       
      
        public BaseViewModel()
        {
           
        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::Stampa
        public Etichetta Etichetta { get; set; } = new Etichetta
        {
            Alias = "Demo",
            Colore = "Rosso",
            //LuceEtichetta = "ON",
            H = 26,
            Comandi = "TS"
        };

        [RelayCommand]
        public void StampaEtichetta()
        {
            var drawable = new EtichettaVeneziane25mm(Etichetta);
            var image = StampaHelper.RenderDrawableToImage(drawable, 400, 150); // dimensioni adatte all'etichetta
            StampaHelper.StampaImmagine(image);
        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::


       
        #region Proprietà
        private string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        private bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        private bool _isEnabled = false;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { SetProperty(ref _isLoading, value); }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set { SetProperty(ref _isRefreshing, value); }
        }
        #endregion
        #region Eventi
        #endregion
        #region Comandi

        #endregion
        #region Costruttori
       
        #endregion
        #region Metodi
        [RelayCommand]
        protected virtual Task OnAppearing()
        {
            return Task.CompletedTask;
        }
        [RelayCommand]
        protected virtual Task OnDisappearing()
        {
            return Task.CompletedTask;
        }
        #endregion


    }
    
}

