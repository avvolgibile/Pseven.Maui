using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics.Skia;
//using Microsoft.UI.Xaml.Media.Imaging;
using Pseven.Data;
using Pseven.Etichette;
using Pseven.Models;
using Pseven.Services;
using Pseven.Varie;
using Pseven.Views;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZXing.QrCode.Internal;

//using Font = Microsoft.Maui.Graphics.Font;

namespace Pseven.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel, IDisposable
    {
        #region Proprietà
        private bool _disposedValue;
        private readonly ExternalDataBase _externalDataBase;
        private readonly InternalDataBase _internalDataBase;
        private Zen.Barcode.Code128BarcodeDraw _barcode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
        //private readonly PrintService _printService;
        //Pen pen1 = new Pen(System.Drawing.Color.Black, 1);

        [ObservableProperty]
        private double _totaleInserimento = 0;

        [ObservableProperty]
        private bool _ivato = true;

        [ObservableProperty]
        private string _user = "io";

        [ObservableProperty]
        private IDrawable? _graphicsDrawable1;

        [ObservableProperty]
        private IDrawable? _graphicsDrawable2;

        [ObservableProperty]
        private IDrawable? _graphicsDrawable3;

        [ObservableProperty]
        private IDrawable? _graphicsDrawable4;

        [ObservableProperty]
        private IDrawable? _graphicsDrawable5;

        [ObservableProperty]
        private IDrawable? _graphicsDrawable6;

        [ObservableProperty]
        private IDrawable? _graphicsDrawable7;

        [ObservableProperty]
        private IDrawable? _graphicsDrawable8;

        [ObservableProperty]
        private IDrawable? _graphicsDrawable9;

        [ObservableProperty]
        private ControlliInterfaccia _controlliInterfaccia = new();

        [ObservableProperty]
        private Graphics _gFX;

        [ObservableProperty]
        private Bitmap _drawingSurface1 = new(330, 135);

        [ObservableProperty]
        private Bitmap _drawingSurface2 = new(330, 135);

        [ObservableProperty]
        private Bitmap _drawingSurface3 = new(330, 135);

        [ObservableProperty]
        private Bitmap _drawingSurface4 = new(330, 135);

        [ObservableProperty]
        private Bitmap _drawingSurface5 = new(330, 135);

        [ObservableProperty]
        private Bitmap _drawingSurface6 = new(330, 135);

        [ObservableProperty]
        private Bitmap _drawingSurface7 = new(330, 135);

        [ObservableProperty]
        private Bitmap _drawingSurface8 = new(330, 135);

        [ObservableProperty]
        private Bitmap _drawingSurface9 = new(330, 135);

        //[ObservableProperty]
        //private BitmapImage _etichetta = new();

        [ObservableProperty]
        private string? _descrizioneArticolo = string.Empty;

        [ObservableProperty]
        private Agente? _agente = new();

        [ObservableProperty]
        private string? _listino = string.Empty;

        [ObservableProperty]
        private Documento _Documento = new();

        [ObservableProperty]
        private MainPageInput? _MainPageInput = new();

        [ObservableProperty]
        private Colore? _colore = new();

        [ObservableProperty]
        private ObservableCollection<Articolo> _elencoArticoli = [];

        [ObservableProperty]
        private ObservableCollection<Articolo> _articoli = [];

        [ObservableProperty]
        private ObservableCollection<Colore> _colori = [];

        [ObservableProperty]
        private ObservableCollection<string> _listaColori = [];

        [ObservableProperty]
        private ObservableCollection<Cliente> _clienti = [];

        [ObservableProperty]
        private ObservableCollection<Agente> _agenti = [];

        [ObservableProperty]
        private ObservableCollection<string> _categorie = [];

        [ObservableProperty]
        private ObservableCollection<string> _sottoCategorie = [];

        [ObservableProperty]
        private ObservableCollection<string> _listini = ["1", "2", "3", "4", "5", "6", "7"];

        [ObservableProperty]
        private ObservableCollection<PrezziCliente> _prezziCliente = [];

        private bool _cercaInElencoDoc = false;
        public bool CercaInElencoDoc
        {
            get => _cercaInElencoDoc;
            set => SetProperty(ref _cercaInElencoDoc, value);
        }

        private object _tipoDocumentoSelezione;
        public object TipoDocumentoSelezione
        {
            get => _tipoDocumentoSelezione;
            set
            {
                SetProperty(ref _tipoDocumentoSelezione, value);
                if (Documento is not null && !string.IsNullOrEmpty((string)value))
                {
                    Documento.TipoDocumento = (TipoDocumento)Enum.Parse(typeof(TipoDocumento), (string)value);
                }
            }
        }

        private object _piuGuideSelezione;
        public object PiuGuideSelezione
        {
            get => _piuGuideSelezione;
            set
            {
                SetProperty(ref _piuGuideSelezione, value);
                if (MainPageInput.Articolo is not null)
                {
                    MainPageInput.PiuGuide = bool.Parse((string)value);
                    EseguiAggiornaEtichetta();
                }
            }
        }

        private object _luceFinitaSelezione;
        public object LuceFinitaSelezione
        {
            get => _luceFinitaSelezione;
            set
            {
                SetProperty(ref _luceFinitaSelezione, value);
                if (MainPageInput.Articolo is not null)
                {
                    MainPageInput.LuceFinita = bool.Parse((string)value);
                    EseguiAggiornaEtichetta();
                }
            }
        }
        private object _ivatoSelezione;
        public object IvatoSelezione
        {
            get => _ivatoSelezione;
            set
            {
                SetProperty(ref _ivatoSelezione, value);
            }
        }
        #endregion

        #region Eventi


        #endregion

        #region Comandi
        public Command CaricaClientiCommand => new(async () => await CaricaClienti());
        public Command CambiaArticoloCommand => new(() => DescrizioneArticolo = "Cambiato");
        #endregion

        #region Costruttori
        public MainPageViewModel(InternalDataBase internaldatabase, ExternalDataBase externaldatabase)
        {
            _internalDataBase = internaldatabase;
            _externalDataBase = externaldatabase;
        }
        #endregion

        #region Metodi
        protected override async Task OnAppearing()
        {
            await Init();
            await CaricaClienti();
            await CaricaArticoli();
            await CaricaAgenti();
            //await CaricaPrezziCliente();
            //GFX = Graphics.FromImage(DrawingSurface);
            //GFX.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

        }
        private Task Init()
        {
            //GFX = Graphics.FromImage(DrawingSurface); GFX.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            return Task.CompletedTask;
        }
        private async Task CaricaClienti()
        {
            await CaricaPrezziCliente();
            var clienti = !CercaInElencoDoc ? await _externalDataBase.GetClienti() : await _internalDataBase.GetClientiAsync();
            if (clienti != null)
            {
                foreach (var cliente in clienti)
                {
                    if (cliente.Listino == "7")
                    {
                        cliente.ListinoPersonale = PrezziCliente.Where(x => x.CodiceCliente == cliente.CodiceCliente).ToList();
                    }
                }
                Clienti = new ObservableCollection<Cliente>(clienti);
                Documento.Cliente = Clienti.First();
            }
        }
        private async Task CaricaArticoli()
        {
            ElencoArticoli = new ObservableCollection<Articolo>(await _externalDataBase.GetArticoli());
            if (await _internalDataBase.CountArticoliAsync() == 0)
            {
                foreach (var articolo in ElencoArticoli)
                {
                    await _internalDataBase.SaveArticoloAsync(articolo);
                }
            }
            List<Articolo>? articoli = await _externalDataBase.GetArticoli(); //TODO Geestione errori db
            if (ElencoArticoli != null)
            {
                foreach (var articolo in articoli.Where(articolo => articolo.Descrizione.Contains('*')))
                {
                    articolo.Descrizione = articolo.Descrizione[..(articolo.Descrizione.IndexOf('*') - 1)];
                }
                var newarticoli = new ObservableCollection<Articolo>();
                foreach (var gruppo in articoli.GroupBy(x => x.Descrizione))
                {
                    var art = articoli.First(x => x.Descrizione == gruppo.Key);
                    art.Colori = [];
                    foreach (var articolo in gruppo)
                    {
                        foreach (var colore in articolo.NoteDb.Split(';').ToList())
                        {
                            if (!string.IsNullOrEmpty(colore))
                            {
                                art.Colori.Add(new Colore { Barcode = articolo.Barcode, CodiceColore = colore });
                            }
                        }

                    }
                    if (!newarticoli.Any(x => x.Descrizione == gruppo.Key))
                    {
                        newarticoli.Add(art);
                    }
                }
                Articoli = new ObservableCollection<Articolo>(newarticoli);
                //Articolo = Articoli.First();
                List<string> categorie = [];
                List<string> sottocategorie = [];
                foreach (var articolo in Articoli)
                {
                    if (!categorie.Any(x => x == articolo.Categoria) && !string.IsNullOrEmpty(articolo.Categoria))
                    {
                        categorie.Add(articolo.Categoria);
                    }
                    if (!sottocategorie.Any(x => x == articolo.SubCateg) && !string.IsNullOrEmpty(articolo.SubCateg))
                    {
                        sottocategorie.Add(articolo.SubCateg);
                    }
                }
                Categorie = new ObservableCollection<string>(categorie.OrderBy(x => x));
                SottoCategorie = new ObservableCollection<string>(sottocategorie.OrderBy(x => x));
            }
        }
        private async Task CaricaAgenti()
        {
            List<Agente> agenti = await _externalDataBase.GetAgenti();
            if (agenti != null)
            {
                Agenti = new ObservableCollection<Agente>(agenti);
            }
        }
        private async Task CaricaPrezziCliente()
        {
            List<PrezziCliente> prezzi = await _externalDataBase.GetPrezziCliente();
            if (prezzi != null)
            {
                PrezziCliente = new ObservableCollection<PrezziCliente>(prezzi);
            }
        }
        private double CalcolaPrezzo(Articolo articolo, string listino)
        {
            double prezzo = 0;
            switch (listino)
            {
                case "1":
                    if (double.TryParse(articolo.Prezzo1, out prezzo)) { prezzo = Math.Round(prezzo, 0); break; }
                    break;
                case "2":
                    if (double.TryParse(articolo.Prezzo2, out prezzo)) { prezzo = Math.Round(prezzo, 0); break; }
                    break;
                case "3":
                    if (double.TryParse(articolo.Prezzo3, out prezzo)) { prezzo = Math.Round(prezzo, 0); break; }
                    break;
                case "4":
                    if (double.TryParse(articolo.Prezzo4, out prezzo)) { prezzo = Math.Round(prezzo, 0); break; }
                    break;
                case "5":
                    if (double.TryParse(articolo.Prezzo5, out prezzo)) { prezzo = Math.Round(prezzo, 0); break; }
                    break;
                case "6":
                    if (double.TryParse(articolo.Prezzo6, out prezzo)) { prezzo = Math.Round(prezzo, 0); break; }
                    break;
                case "7":
                    if (double.TryParse(Documento.Cliente.ListinoPersonale.Single(x => x.CodiceArticolo == articolo.CodiceArticolo).Prezzo, out prezzo)) { prezzo = Math.Round(prezzo, 2); break; }
                    break;
                default:
                    break;
            }
            return prezzo;
        }
        private static double CalcolaFornituraMinima(string mf)
        {
            if (!string.IsNullOrEmpty(mf) && mf.Contains("MF ") && mf.Contains(" mq"))
            {
                return double.Parse(mf.Replace("MF ", "").Replace(" mq", ""));
            }
            return 0;
        }
        [RelayCommand]
        private Task ClienteSelezionato()
        {
            if (Documento.Cliente is not null)
            {
                if (!string.IsNullOrEmpty(Documento.Cliente.Agente))
                {
                    Agente = Agenti.Single(x => x.NomeAgente == Documento.Cliente.Agente);
                }
                if (!string.IsNullOrEmpty(Documento.Cliente.Listino))
                {
                    Listino = Documento.Cliente.Listino;
                    OnPropertyChanged(nameof(Documento));
                }
            }
            EseguiAggiornaEtichetta();
            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task Supplemento1Selezionato()
        {
            if (MainPageInput is not null)
            {
                var supplemento = ElencoArticoli.Single(x => x.Descrizione == MainPageInput.Supplemento1);
                MainPageInput.Supplemento1Prezzo = CalcolaPrezzo(supplemento, Documento.Cliente.Listino);
                MainPageInput.Supplemento1Quantita = 1;
            }
            OnPropertyChanged(nameof(MainPageInput));
            EseguiAggiornaEtichetta();
            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task ArticoloSelezionato()
        {
            if (MainPageInput.Articolo is not null && !string.IsNullOrEmpty(MainPageInput.Articolo.Barcode))
            {
                if (!string.IsNullOrEmpty(MainPageInput.Articolo.Descrizione) && MainPageInput.Articolo.Descrizione.Contains('*'))
                {
                    DescrizioneArticolo = MainPageInput.Articolo.Descrizione;
                }
                if (!string.IsNullOrEmpty(Documento.Cliente.Listino))
                {
                    MainPageInput.Prezzo = CalcolaPrezzo(MainPageInput.Articolo, Documento.Cliente.Listino);

                }
                MainPageInput.PrezzoIvato = Math.Round(MainPageInput.Prezzo + (MainPageInput.Prezzo * int.Parse(MainPageInput.Articolo.Iva) / 100), 0);
                if (!string.IsNullOrEmpty(MainPageInput.Articolo.Generico2))
                {
                    MainPageInput.Quantita = CalcolaFornituraMinima(MainPageInput.Articolo.Generico2);
                }

                OnPropertyChanged(nameof(MainPageInput));
                EseguiAggiornaEtichetta();
            }
            return Task.CompletedTask;
        }
        [RelayCommand]
        private Task ColoreSelezionato()
        {
            if (MainPageInput is not null && MainPageInput.Colore is not null && Colore is not null && !string.IsNullOrEmpty(Colore.Barcode))
            {
                MainPageInput.Colore = Colore.CodiceColore;
                var articolo = ElencoArticoli.Single(x => x.Barcode == Colore.Barcode);
                if (articolo is not null)
                {
                    if (!string.IsNullOrEmpty(articolo.Descrizione) && articolo.Descrizione.Contains('*'))
                    {
                        DescrizioneArticolo = articolo.Descrizione;
                    }
                    if (!string.IsNullOrEmpty(Documento.Cliente.Listino))
                    {
                        MainPageInput.Prezzo = CalcolaPrezzo(articolo, Documento.Cliente.Listino);
                    }
                    if (!string.IsNullOrEmpty(articolo.Iva))
                    {
                        MainPageInput.PrezzoIvato = Math.Round(MainPageInput.Prezzo + (MainPageInput.Prezzo * int.Parse(articolo.Iva) / 100), 0);
                    }
                    if (!string.IsNullOrEmpty(articolo.Generico2))
                    {
                        MainPageInput.Quantita = CalcolaFornituraMinima(articolo.Generico2);
                    }
                    OnPropertyChanged(nameof(MainPageInput));
                }
                EseguiAggiornaEtichetta();
            }
            return Task.CompletedTask;
        }
        [RelayCommand]
        private void EseguiAggiornaEtichetta()
        {

            if (MainPageInput is not null)
            {
                if (Documento is not null && Documento.Cliente is not null)
                {
                    MainPageInput.RagioneSociale = Documento.Cliente.RagioneSociale;
                }
                AggiornaEtichetta();
                //if (MainPageInput.GraphicsDrawable1 is not null)
                //{
                //    GraphicsDrawable1 = MainPageInput.GraphicsDrawable1;
                //}
                //if (MainPageInput.GraphicsDrawable2 is not null)
                //{
                //    GraphicsDrawable2 = MainPageInput.GraphicsDrawable2;
                //}
                //if (MainPageInput.GraphicsDrawable3 is not null)
                //{
                //    GraphicsDrawable3 = MainPageInput.GraphicsDrawable3;
                //}
                if (Documento is not null && Documento.Cliente is not null)
                {
                    if (string.IsNullOrEmpty(Documento.Cliente.Sconto)) { Documento.Cliente.Sconto = "0"; }
                    double Sconto = (100 - double.Parse(Documento.Cliente.Sconto)) / 100;
                    if (MainPageInput.Articolo is not null)
                    {
                        try
                        {
                            if (Ivato)
                            {
                                TotaleInserimento = Math.Round((MainPageInput.Quantita * MainPageInput.PrezzoIvato) + (MainPageInput.Supplemento1PrezzoTotale + MainPageInput.Supplemento2PrezzoTotale) * (double.Parse($"1,{MainPageInput.Articolo.Iva}") * Sconto));
                            }
                            else
                            {
                                TotaleInserimento = Math.Round((MainPageInput.Quantita * MainPageInput.PrezzoIvato) + (MainPageInput.Supplemento1PrezzoTotale + MainPageInput.Supplemento2PrezzoTotale) * Sconto);
                            }
                        }
                        catch (FormatException)
                        {

                            throw;
                        }
                    }
                }

            }
            if (!string.IsNullOrEmpty(ControlliInterfaccia.MessaggioErrore))
            {
                Application.Current.MainPage.DisplayAlert("Errore", ControlliInterfaccia.MessaggioErrore, "Ok");
                ControlliInterfaccia.MessaggioErrore = string.Empty;
            }
        }

        #region Apertura views Page
        [RelayCommand]
        private static void ApriDocumento()
        {
            Application.Current.OpenWindow(new Window(new DocumentoPage(new DocumentoViewModel())));

        }
        [RelayCommand]
        private static void ApriAntemprima()
        {

        }


        [RelayCommand]
        private static async Task ApriDocumentiAperti()
        {

            //var vm2 = new DocumentiApertiViewModels();
            //var page2 = new DocumentiApertiPage { BindingContext = vm2 };

            //Application.Current.OpenWindow(new Window(page2));

            Application.Current.OpenWindow(new Window(new DocumentiApertiPage(new DocumentiApertiViewModels())));
        }


      
        [RelayCommand]
        private void ApriMiniForm()
        {
            var window = new Window(new MiniFormPage(this))
            {
                Width = 650,
                Height = 500
            };
            Application.Current.OpenWindow(window);
        }
        [RelayCommand]
        private async Task SalvaDocumento() //TODO gestione errori
        {
            //TODO provvisiorio
            if (Documento != null)
            {
                Documento.DataDocumento = DateTime.Now;
                //Documento.Cliente = Cliente;
                if (Agente is not null)
                {
                    Documento.NomeAgente = Agente.NomeAgente;
                }
                if (await _internalDataBase.GetClienteByCodiceAsync(Documento.Cliente.CodiceCliente) is null)
                {
                    var r = await _internalDataBase.SaveClienteAsync(Documento.Cliente);
                }
                var result = await _internalDataBase.SaveDocumentoAsync(Documento);
                if (result != 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Salvataggio", "Documento salvato correttamente", "Ok");
                }
            }
        }
        [RelayCommand]
        private static void SalvaStampaEtichetta()
        {

        }

        private void Printetichetta_Page1(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //Bitmap myBitmap1 = new Bitmap(pictureBox1.Width, pictureBox1.Height);//pictureBox1.Width, pictureBox1.Height
            //pictureBox1.DrawToBitmap(myBitmap1, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));//le prime due cifre spostano l'immaginecompleta di scritte all' interno dell' etichetta
            //e.Graphics.DrawImage(myBitmap1, -6, -2);//sposta l'immaginecompleta di scritte all' interno dell' etichetta----9, -4
            //myBitmap1.Dispose();//will free the bitmap ressources.
            SkiaBitmapExportContext skiaBitmapExportContext = new(400, 400, 72);
            Bitmap bitmap = new Bitmap(400, 400);
            ICanvas canvas = skiaBitmapExportContext.Canvas;

        }

        [RelayCommand]
        private void ApriStoricoOrdiniAFornitori()
        {
            Application.Current.OpenWindow(new Window(new StoricoOrdiniAFornitoriPage()));

        }

        #endregion







        [RelayCommand]
        private void NuovaEtichetta()
        {
            Documento = new Documento
            {
                Cliente = new Cliente()
            };
            MainPageInput = new();
            NuovoProdotto();
        }
        [RelayCommand]
        private void NuovoProdotto()
        {
            MainPageInput.Articolo = new Articolo();
            Colore = new Colore();
            MainPageInput.Articolo.Colori = [];
            DescrizioneArticolo = "";
            Colori = [];
            GraphicsDrawable1 = new EtichettaVuota();
            GraphicsDrawable2 = new EtichettaVuota();
            GraphicsDrawable3 = new EtichettaVuota();
            OnPropertyChanged(nameof(MainPageInput));
        }
        [RelayCommand]
        private static void Cp()
        {

        }
        [RelayCommand]
        private static void Ap()
        {

        }
        [RelayCommand]
        private static void Keep()
        {

        }
        [RelayCommand]
        private void Diviso2()
        {
            if (MainPageInput is not null)
            {
                MainPageInput.Diviso2 = !MainPageInput.Diviso2;
                //TODO: gestire pulsante /2
            }
        }
        [RelayCommand]
        private void Diviso2CU()
        {
            if (MainPageInput is not null)
            {
                MainPageInput.Diviso2CU = !MainPageInput.Diviso2CU;
                //TODO: gestire pulsante /2CU
            }
        }

        //ArticoliGestiti_____________________________________________________________________________________

        private double Fatt_min_Tot = 0, Fatt_min_L = 0, Fatt_min_H = 0, fattMinTotSuppl1 = 0;
        private double luceLperEtich = 0, luceLperEtich_Page1 = 0, luceLperEtich_Page2 = 0;
        private double luceHperEtich = 0;

        private string Supplemento1_NomeSuppDaSalvare = "";
        private string Supplemento2_NomeSuppDaSalvare = "";

        private string mixTaglioXetich = "";//usata nei binari
        private double mixBandaFinita = 0;
        private double calcoloFloat = 0;//anche pieghe plisse??
        private string calcoloString = "", calcoloString_Page2 = "";///pieghe plisse
        private string pzSupplXetichetta = "";
        private string cordino1String = "", cordino1String_Page2 = "";//usato anche per mix spazzolino
        private string cordino2String = "", cordino2String_Page2 = "";
        private string nAgganciCingoliPannelli = "", nAgganciCingoliPannelli_Page2 = "";//agganci intermedi,cingoli

        private string mixTeloFinita = "0", mixTeloFinita_Page2 = "0";//anche rete plisse//

        private readonly int PartenzaBarcodeDX = 200;
        private double mixTaglioCassonetto = 0;
        private double mixTaglioBinario = 0;
        private double mixTaglioTubo = 0;
        private double mixTaglioTubo2 = 0;
        private double mixTaglioGuida = 0;
        private double mixTaglioGuida_Page2 = 0;
        private double mixTaglioProfiloSuperiore = 0;
        private double mixTaglioProfiloInferiore = 0;
        private double mixTaglioProfilo = 0;
        private double mixTagliocompensatore = 0;
        private double mixTagliocompensatore_Page2 = 0;
        private double mixTaglioFondaleManiglia = 0;
        private double mixTaglioFondaleManiglia_Page2 = 0;
        private double mixTaglioProfiloSupplemento1 = 0;

        string StringaPerTexboxAVV = "";
        private string annotazioniVerieSuEtichiette = "";
        private string annotazioniVerieSuEtichiette2 = "";

        public void AggiornaEtichetta()
        {
            if (MainPageInput.Articolo != null && MainPageInput.Articolo.Descrizione != null)
            {
                switch (MainPageInput.Articolo.Descrizione.ToLower().TrimStart())
                {
                    //Veneziane
                    case "venez.15mm":
                        Veneziane15();
                        break;
                    case "venez.25mm":
                        Veneziane25();
                        break;
                    case "venez.35mm":
                        Veneziane35();
                        break;
                    case "venez.50mm":
                        Veneziane50();
                        break;
                    //Verticali
                    case "verticale":
                        Verticali();
                        break;
                    case "bande vert.":
                        BandeVerticali();
                        break;
                    case "bande vert. complete":
                        BandeVerticaliComplete();
                        break;
                    //Binario Pannello
                    case "binario verticale":
                        BinarioVerticale();
                        break;
                    case "binario pannello 2 vie a corda":
                        BinarioPannelloCorda(2);
                        break;
                    case "binario pannello 3 vie a corda":
                        BinarioPannelloCorda(3);
                        break;
                    case "binario pannello 4 vie a corda":
                        BinarioPannelloCorda(4);
                        break;
                    case "binario pannello 5 vie a corda":
                        BinarioPannelloCorda(5);
                        break;
                    case "binario pannello 6 vie a corda":
                        BinarioPannelloCorda(6);
                        break;
                    case "binario pannello 7 vie a corda":
                        BinarioPannelloCorda(7);
                        break;
                    case "binario pannello 8 vie a corda":
                        BinarioPannelloCorda(8);
                        break;
                    case "binario pannello 2 vie con asta":
                        BinarioPannelloAsta(2);
                        break;
                    case "binario pannello 3 vie con asta":
                        BinarioPannelloAsta(3);
                        break;
                    case "binario pannello 4 vie con asta":
                        BinarioPannelloAsta(4);
                        break;
                    case "binario pannello 5 vie con asta":
                        BinarioPannelloAsta(5);
                        break;
                    case "binario pannello 6 vie con asta":
                        BinarioPannelloAsta(6);
                        break;
                    case "binario pannello 7 vie con asta":
                        BinarioPannelloAsta(7);
                        break;
                    case "binario pannello 8 vie con asta":
                        BinarioPannelloAsta(8);
                        break;
                    case "binario pannello 2 vie manuale":
                        BinarioPannelloManuale(2);
                        break;
                    case "binario pannello 3 vie manuale":
                        BinarioPannelloManuale(3);
                        break;
                    case "binario pannello 4 vie manuale":
                        BinarioPannelloManuale(4);
                        break;
                    case "binario pannello 5 vie manuale":
                        BinarioPannelloManuale(5);
                        break;
                    case "binario pannello 6 vie manuale":
                        BinarioPannelloManuale(6);
                        break;
                    case "binario pannello 7 vie manuale":
                        BinarioPannelloManuale(7);
                        break;
                    case "binario pannello 8 vie manuale":
                        BinarioPannelloManuale(8);
                        break;
                    case "binario pannello 3 vie a corda ap.centr. 4pannelli":
                        BinarioPannelloXVieACordaApCentraleXPannelli(3, 4);
                        break;
                    case "binario pannello 3 vie a corda ap.centr. 6pannelli":
                        BinarioPannelloXVieACordaApCentraleXPannelli(3, 6);
                        break;
                    case "binario pannello 3 vie con asta ap.centr. 4pannelli":
                        BinarioPannelloXVieConAstaApCentraleXPannelli(3, 4);
                        break;
                    case "binario pannello 3 vie con asta ap.centr. 6pannelli":
                        BinarioPannelloXVieConAstaApCentraleXPannelli(3, 6);
                        break;
                    case "binario pannello 3 vie manuale ap.centr. 4pannelli":
                        BinarioPannelloXVieManualeApCentraleXPannelli(3, 4);
                        break;
                    case "binario pannello 3 vie manuale ap.centr. 6pannelli":
                        BinarioPannelloXVieManualeApCentraleXPannelli(3, 6);
                        break;
                    case "binario pannello 4 vie a corda ap.centr. 6pannelli":
                        BinarioPannelloXVieACordaApCentraleXPannelli(4, 6);
                        break;
                    case "binario pannello 4 vie con asta ap.centr. 6pannelli":
                        BinarioPannelloXVieConAstaApCentraleXPannelli(4, 6);
                        break;
                    case "binario pannello 4 vie manuale ap.centr. 6pannelli":
                        BinarioPannelloXVieManualeApCentraleXPannelli(4, 6);
                        break;
                    case "binario pannello 5 vie a corda ap.centr. 8pannelli":
                        BinarioPannelloXVieACordaApCentraleXPannelli(5, 8);
                        break;
                    case "binario pannello 5 vie con asta ap.centr. 8pannelli":
                        BinarioPannelloXVieConAstaApCentraleXPannelli(5, 8);
                        break;
                    case "binario pannello 5 vie manuale ap.centr. 8pannelli":
                        BinarioPannelloXVieManualeApCentraleXPannelli(5, 8);
                        break;
                    case "binario pannello 6 vie a corda ap.centr. 10pannelli":
                        BinarioPannelloXVieACordaApCentraleXPannelli(6, 10);
                        break;
                    case "binario pannello 6 vie con asta ap.centr. 10pannelli":
                        BinarioPannelloXVieConAstaApCentraleXPannelli(6, 10);
                        break;
                    case "binario pannello 6 vie manuale ap.centr. 10pannelli":
                        BinarioPannelloXVieManualeApCentraleXPannelli(6, 10);
                        break;
                    //Zanzariere
                    case "rete saldata":
                        ReteSaldata();
                        break;
                    case "rete saldata zebrata":
                        ReteSaldataZebrata();
                        break;
                    case "zanzariera fissa":
                        ZanzarieraFissa();
                        break;
                    case "unika 45":
                        Unika45();
                        break;
                    case "incassata 45 + unika 45":
                        Incassata45PiUnika45();
                        break;
                    case "zanzariera a cricchetto":
                        ZanzariereAMolla("Z.Cricchetto");
                        break;
                    case "zanzariera a molla":
                        ZanzariereAMolla("Z.Molla");
                        break;
                    case "zanzariera a molla doppia g.":
                        ZanzariereAMollaDoppiaG();
                        break;
                    case "zanzariera a catena":
                        ZanzariereACatena(0, 11.5f, "Z.Catena");
                        break;
                    case "zanzariera molla caty":
                        ZanzariereACatena(Quotacassone: 2.4f, Quotaguide: 7.2f, "Z.Molla/Caty");
                        break;
                    case "zanzariera a catena doppia g.":
                        ZanzariereACatenaDoppiaG();
                        break;
                    case "zanzariera laterale":
                        ZanzariereLaterale(3.3f, "Z. laterale");
                        break;
                    case "zanzariera laterale miniguida":
                        ZanzariereLaterale(2.2f, "Z.miniguida");
                        break;
                    case "zanzariera gioconda laterale 22mm":
                        ZanzarieraGiocondaLaterale22mm();
                        break;
                    case "zanzariera gioconda reversibile 25mm":
                        ZanzarieraGiocondaReversibile22mm();
                        break;
                    case "flexa":
                        Flexa();
                        break;
                    case "jolly":
                        Jolly();
                        break;
                    case "scenica" or "miniscenica" or "picoscenica" or "scenipro":
                        SceniPro();
                        break;
                    case "plissè":
                        Plisse(true);
                        break;
                    case "plissè mq":
                        Plisse(false);
                        break;
                    case "zanzariera anta":
                        ZanzarieraAnta();
                        break;
                    case "zanzariera scorrevole":
                        ZanzariereScorrevoli();
                        break;
                    //Binario
                    case "binario arricc.":
                        BinarioArricc();
                        break;
                    case "binario arricc. a strappo":
                        BinarioStrappo();
                        break;
                    //Porta a soffietto
                    case "porta a soffietto":
                        PortaASoffietto();
                        break;
                    //Cubo
                    case "cubo 80":
                        Cubo80();
                        break;
                    case "cubo 100":
                        Cubo100();
                        break;
                    case "cubo 130":
                        Cubo130();
                        break;
                    case "cubo 80 arg":
                        Cubo80Arg();
                        break;
                    case "cubo 100 arg":
                        Cubo100Arg();
                        break;
                    case "cubo 130 arg":
                        Cubo130Arg();
                        break;
                    //Rullo
                    case "rullo standard":
                        RulloStandardCristineMiniCat(Quotatubo: 3.3f, Quotatessuto: 3.8f, "Rullo standard");
                        break;
                    case "rullo standard portarullo":
                        RulloStandardMedioPortarullo(QuotaprofiloSuperiore: 0.7f, Quotatubo: 3.1f, "Rullo standard-portar");
                        break;
                    case "rullo medio":
                        RulloMedio();
                        break;
                    case "rullo angolar":
                        RulloAngolar();
                        break;
                    case "rullo medio portarullo":
                        RulloStandardMedioPortarullo(QuotaprofiloSuperiore: 1, Quotatubo: 3.6f, "Rullo medio-portar");
                        break;
                    case "rullo angolar portarullo":
                        RulloAngolarPortarullo();
                        break;
                    case "rullo medio motore":
                        RulloMedioMotore();
                        break;
                    case "rullo angolar motore":
                        RulloAngolarMotore();
                        break;
                    case "rullo medio motore portarullo":
                        RulloMedioMotorePortarullo();
                        break;
                    case "rullo angolar motore portarullo":
                        RulloAngolarMotorePortarullo();
                        break;
                    case "rullo grande catena":
                        RulloGrandeDemoltEInox(Quotatubo: 4.9f, Quotafondale: 5.3f, Quotatessuto: 5.6f, "Rullo Grande");
                        break;
                    case "rullo angolar grande catena":
                        RulloAngolarGrandeDemoltiplicato();
                        break;
                    case "rullo grande catena cavi inox":
                        RulloGrandeDemoltEInox(Quotatubo: 4.9f, Quotafondale: 6.1f, Quotatessuto: 5.8f, "R.Grande cat C.inox");
                        break;
                    case "rullo grande motore":
                        RulloGrandeMotoreEInox(Quotatubo: 3.1f, Quotafondale: 4/*mm*/ , "Rullo Grande motor");
                        break;
                    case "rullo angolar grande motore":
                        RulloAngolarGrandeMotore();
                        break;
                    case "rullo grande motore cavi inox":
                        RulloGrandeMotoreEInox(Quotatubo: 3.1f, Quotafondale: 9/*mm*/ , "R.Grande mot C.inox");
                        break;
                    case "rullo maxi":
                        RulloMaxiMotorCaviInox(Quotafondale: 0, "Rullo Maxi");
                        break;
                    case "rullo maxi cavi inox":
                        RulloMaxiMotorCaviInox(Quotafondale: 4, "R.Maxi C.inox");
                        break;
                    case "rullo mega":
                        RulloMega();
                        break;
                    //------------------------------------------------------------------------------------------------
                    case "rullo cass 50 molla":
                        RulloCass50Molla();
                        break;
                    case "rullo cass 50 cat":
                        RulloCass50Cat();
                        break;
                    case "rullo cass 50 motor":
                        RulloCass50Motor();
                        break;
                    //------------------------------------------------------------------------------------------------
                    case "rullo cass 63 catena":
                        Rullo_Cass_63_83_cat(2.8f, 5.1f, 5.7f, "Cass 63 cat");
                        break;
                    case "rullo cass 63 motore":
                        Rullo_Cass_63_83_110_mot(Quotacassone: 0.7f, Quotatubo: 2f, "Tubo43", "Cass 63 mot");
                        break;
                    case "rullo cass 83 catena":
                        Rullo_Cass_63_83_cat(Quotacassone: 2.2f, Quotatubo: 3.1f, Quotatessuto: 3.7f, "Cass 83 cat");
                        break;
                    case "rullo cass 83 motore":
                        Rullo_Cass_63_83_110_mot(Quotacassone: 0.7f, Quotatubo: 2.4f, "Tubo43", "Cass 83 mot");
                        break;
                    case "rullo cass 83 mot cavi inox":
                        Rullo_Cass_83_mot_caviInox(0.7f, 2.2f);
                        break;
                    case "rullo cass 110":
                        Rullo_Cass_63_83_110_mot(Quotacassone: 0.9f, Quotatubo: 3.7f, "Tubo43", "Cass 110");
                        break;
                    case "rullo cass 110 cavi inox":
                        Rullo_Cass_110_mot_caviInox(0.9f, 3.7f);
                        break;
                    case "rullo cass 63 catena con guida":
                        Rullo_Cass_63_83_cat_con_guida_ZIP(2.8f, 5.1f, 12.2f, 7.3f, 5.3f, "Cass 63 cat guida");
                        break;
                    case "rullo cass 63 mot con guida":
                        Rullo_Cass_63_83_mot_con_guida(0.7f, 2, 6.8f, 4.4f, "Cass63 mot guida");
                        break;
                    case "rullo cass 83 catena con guida":
                        Rullo_Cass_63_83_cat_con_guida_ZIP(2.2f, 3.1f, 12.2f, 8.8f, 3.5f, "Cass 83 cat guida");
                        break;
                    case "rullo cass 83 cat zip":
                        Rullo_Cass_63_83_cat_con_guida_ZIP(2.2f, 3.1f, 10.8f, 0, 7f, "Cass 83 ZIP");
                        break;
                    case "rullo cass 83 mot con guida":
                        Rullo_Cass_63_83_mot_con_guida(Quotacassone: 0.7f, Quotatubo: 2.4f, Quotaguide: 8.8f, Quotatessuto: 5.2f, "Cass83 mot guida");
                        break;
                    case "lucernaio 63":
                        Lucernaio63();
                        break;
                    case "lucernaio 83":
                        Lucernaio83_110(0.7f, 3.7f, 3.9f, 17.2f, "Lucernaio 83");
                        break;
                    case "lucernaio 110":
                        Lucernaio83_110(0.7f, 2.9f, 4.1f, Quotaguide: 24, "Lucernaio 110");
                        break;
                    case "rullo a molla":
                        RulloAMolla();
                        break;
                    case "rullo a molla grande portarullo":
                        RulloAMollaGrandePortarullo();
                        break;
                    case "rullo cass n&d catena":
                        RulloNedCassonetto();
                        break;
                    case "rullo cass n&d motore":
                        RulloNedCassonettoMotore();
                        break;
                    case "rullo cristine":
                        RulloStandardCristineMiniCat(Quotatubo: 3.8f, Quotatessuto: 4.3f, "Rullo Cristine");
                        break;
                    case "rullo cristine2":
                        RulloDecorativoCat(3, 3.5f, "Cristin2 cat", "");
                        break;
                    case "rullo cristine2 motore":
                        RulloDecorativoMot(2.3f, "Cristin2 motore", "");
                        break;
                    case "rullo triangolar":
                        RulloDecorativoCat(3, 3.5f, "Triangolar cat", "");
                        break;
                    case "rullo triangolar motore":
                        RulloDecorativoMot(2.3f, "Triangolar motore", "");
                        break;
                    case "rullo square":
                        RulloDecorativoCat(4.1f, 4.6f, "Square cat", "Fondale rettangolare");
                        break;
                    case "rullo square motore":
                        RulloDecorativoMot(3.2f, "Square motore", "Fondale rettangolare");
                        break;
                    case "rullo curved catena":
                        RulloDecorativoCat(6, 6.4f, "Curved cat", "");
                        break;
                    case "rullo curved motore":
                        RulloDecorativoMot(5.3f, "Curved motore", "");
                        break;
                    case "mantovana+laterali+accessori":
                        MantovanaLateraliAccessori();
                        break;
                    case "rullo mantovana catena":
                        RulloMantovanaCatena();
                        break;
                    case "rullo mantovana motore":
                        RulloMantovanaMotore();
                        break;
                    case "rullo mini cat":
                        RulloStandardCristineMiniCat(2.6f, 3.1f, "Rullo mini cat");
                        break;
                 
                    case "rullo mini molla":
                        RulloMiniMolla();
                        break;
                    case "sole 33 cat":
                        Sole_33_41_Cat(5.3f, "Sole 33 catena");
                        break;
                    case "sole 33 cat front":
                        Sole_33_41_Cat_Front(Quotaguide: 6.9f, "Sole 33 cat Front");
                        break;
                    case "sole 33 molla":
                        Sole_33_41_Molla(5.3f, "Sole 33 molla");
                        break;
                    case "sole 33 molla front":
                        Sole_33_41_Molla_Front(Quotaguide: 6.9f, "Sole 33 molla Front");
                        break;
                    case "sole 41 cat":
                        Sole_33_41_Cat(6.1f, "Sole 41 catena");
                        break;
                    case "sole 41 cat front":
                        Sole_33_41_Cat_Front(Quotaguide: 7.7f, "Sole 41 cat Front");
                        break;
                    case "sole 41 molla":
                        Sole_33_41_Molla(6.1f, "Sole 41 molla");
                        break;
                    case "sole 41 molla front":
                        Sole_33_41_Molla_Front(Quotaguide: 7.7f, "Sole 41 molla Front");
                        break;
                    case "sole 33 motor":
                        Sole_33_Motor();
                        break;
                    case "sole 33 motor front":
                        Sole_33_Motor_Front();
                        break;
                    case "sole 41 motor":
                        Sole_41_Motor();
                        break;
                    case "sole 41 motor front":
                        Sole_41_Motor_Front();
                        break;
                    case "telo squadrato":
                        Telo_squadrato();
                        break;
                    case "telo pannello confezionato":
                        Telo_pannello_confezionato();
                        break;
                    case "avvolgibile all" or "avvolgibile arialuce" or "avvolgibile duero":
                        Avvolgibili_varie(0.8f, MainPageInput.Articolo.Descrizione);
                        break;
                    case "avvolgibile pvc" or "avvolgibile pvc mod sole":
                        Avvolgibili_varie(0.6f, "Avvolgibile PVC");
                        break;
                    case "avvolgibile new solar" or "avvolgibile new solar mini" or "avvolgibile new solar elephant":
                        Avvolgibili_varie(2.4f, MainPageInput.Articolo.Descrizione);
                        break;
                    case "girasole" or "orienta":
                        Girasole(MainPageInput.Articolo.Descrizione);
                        break;

                    default: TuttGliAltri(); break;
                };
            }
        }

        //Veneziane
        private void Veneziane15()
        {
            double luce = MainPageInput.LuceFinita ? -0.5 : 0;
            double[] cordiniVeneziane = new double[3];
            ControlliInterfaccia = new ControlliInterfaccia
            {
                GuideVisible = true,
                HcVisible = true,
                ComandiVisible = true,
                AttacchiVisible = true,
                ListaComandi = ["TD", "DX", "SX", "TS"],
                ListaNote = ["Term.Forato+ganci met", "Term.Forato+ganci plas", "Cass. unico xxx_)6mm(_", "!!NOTE!!NOTE!!"],
                ListaSupplemento1 = ["Monoc.asta-corda", "Monoc.catena", "Motorizzata", "Motorizzata Radio"],
                ListaAttacchi = ["Soft"]
            };
            OnPropertyChanged(nameof(ControlliInterfaccia));

            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + luce, 1); //per etichetta
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber, 1); //per etichetta
                if (MainPageInput.LNumber > 350 && !MainPageInput.Diviso2 && !MainPageInput.Diviso2CU) { ControlliInterfaccia.MessaggioErrore = "Misura presumibilmente sbagliata"; } //TODO: Manca verifica pulsante /2
                if (MainPageInput.HNumber > 450) { ControlliInterfaccia.MessaggioErrore = "Misura presumibilmente sbagliata"; }
                //888888888888888888888888888888888888888888888888888888888888888888888888  2 fori 88888888888888888888888888888888888888888888888888888888888888888888888888
                if (MainPageInput.LNumber <= 66.9)
                {
                    calcoloFloat = 0;

                    if (MainPageInput.LNumber <= 27)
                    {
                        if (MainPageInput.LNumber <= 15)
                        {
                            CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, 0);
                        }
                        else
                        {
                            CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, MainPageInput.LNumber - 7);
                        }
                    }
                    else
                    {
                        CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, MainPageInput.LNumber - 15);
                    }
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888 3 fori 2/3 cordini 8888888888888888888888888888888888888888888888888888888888888888
                else if (MainPageInput.LNumber <= 112.9)
                {
                    calcoloFloat = (MainPageInput.LNumber + luce - 29) / 2;
                    if (MainPageInput.LNumber <= 100)
                    {
                        CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, MainPageInput.LNumber - 15);
                    }
                    else
                    {
                        CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, MainPageInput.LNumber - 15, 1);
                    }
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888  4 fori 4 cordini 8888888888888888888888888888888888888888888888888888888888888888
                else if (MainPageInput.LNumber <= 152.9)
                {
                    calcoloFloat = (MainPageInput.LNumber + luce - 29) / 3;
                    CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, MainPageInput.LNumber - 15, 2);
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888  5 fori 4 cordini  8888888888888888888888888888888888888888888888888888888888888888
                else if (MainPageInput.LNumber <= 193.9)
                {
                    calcoloFloat = (MainPageInput.LNumber + luce - 29) / 4;
                    CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, MainPageInput.LNumber - 15, 2);
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888  6 fori 4 cordini  8888888888888888888888888888888888888888888888888888888888888888
                else if (MainPageInput.LNumber <= 233.9)
                {
                    calcoloFloat = (MainPageInput.LNumber + luce - 29) / 5;
                    CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, MainPageInput.LNumber - 15, 2);
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888  7 fori 4 cordini  8888888888888888888888888888888888888888888888888888888888888888
                else
                {
                    calcoloFloat = (MainPageInput.LNumber + luce - 29) / 6;
                    CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, MainPageInput.LNumber - 15, 2);
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                double Fatt_min_Tot_temp = Fatt_min_Tot;
                if (User == "io" && Documento.Cliente.Listino == "1") Fatt_min_Tot_temp = 1.5;
                CalcoloQuantitaMQ(MainPageInput.LNumber, MainPageInput.HNumber, MainPageInput.Pezzi, Fatt_min_Tot_temp);
                CalcoloSupplemento1(MainPageInput.Supplemento1Quantita, MainPageInput.Supplemento1Prezzo);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
            }
            else
            {
                luceLperEtich = 0;
            }
            var etichetta = new Etichetta { Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = MainPageInput.LuceLperEtich, LuceHEtichetta = MainPageInput.LuceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, CordiniVeneziane = cordiniVeneziane, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var etichettaVeneziane15 = new EtichettaVeneziane15mm(etichetta))
            {
                GraphicsDrawable1 = etichettaVeneziane15;
                DrawingSurface1 = etichettaVeneziane15.ToBitmap();
            }
            Aggiorna();
        }
        private void Veneziane25()
        {
            double luce = MainPageInput.LuceFinita ? -0.5 : 0;
            double[] cordiniVeneziane = new double[3];
            ControlliInterfaccia = new ControlliInterfaccia
            {
                GuideVisible = true,
                HcVisible = true,
                ComandiVisible = true,
                AttacchiVisible = true,
                ListaComandi = ["TD", "DX", "SX", "TS"],
                ListaNote = ["Term.Forato+ganci met", "Term.Forato+ganci plas", "Cass. unico xxx_)6mm(_", "!!NOTE!!NOTE!!"],
                ListaSupplemento1 = ["Monoc.asta-corda", "Monoc.catena", "Motorizzata", "Motorizzata Radio"],
                ListaAttacchi = ["Soft"]
            };
            OnPropertyChanged(nameof(ControlliInterfaccia));

            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                luceLperEtich = Math.Round(MainPageInput.LNumber + luce, 1); //per etichetta
                luceHperEtich = Math.Round(MainPageInput.HNumber, 1); //per etichetta
                if (MainPageInput.LNumber > 350 && !MainPageInput.Diviso2 && !MainPageInput.Diviso2CU) { ControlliInterfaccia.MessaggioErrore = "Misura presumibilmente sbagliata"; } //TODO: Manca verifica pulsante /2
                if (MainPageInput.HNumber > 450) { ControlliInterfaccia.MessaggioErrore = "Misura presumibilmente sbagliata"; }
                //888888888888888888888888888888888888888888888888888888888888888888888888  2 fori 88888888888888888888888888888888888888888888888888888888888888888888888888
                if (MainPageInput.LNumber <= 77.9)
                {
                    calcoloFloat = 0;

                    if (MainPageInput.LNumber <= 27)
                    {
                        if (MainPageInput.LNumber <= 15)
                        {
                            CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, 0);
                        }
                        else
                        {
                            CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, MainPageInput.LNumber - 7);
                        }
                    }
                    else
                    {
                        CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, MainPageInput.LNumber - 15);
                    }
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888 3 fori 2/3 cordini 8888888888888888888888888888888888888888888888888888888888888888
                else if (MainPageInput.LNumber <= 132.9)
                {
                    calcoloFloat = (MainPageInput.LNumber + luce - 29) / 2;
                    if (MainPageInput.LNumber <= 120)
                    {
                        CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, MainPageInput.LNumber - 15);
                    }
                    else
                    {
                        CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, MainPageInput.LNumber - 15, 1);
                    }
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888  4 fori 4 cordini 8888888888888888888888888888888888888888888888888888888888888888
                else if (MainPageInput.LNumber <= 190.9)
                {
                    calcoloFloat = (MainPageInput.LNumber + luce - 29) / 3;
                    CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, MainPageInput.LNumber - 15, 2);
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888  5 fori 4 cordini  8888888888888888888888888888888888888888888888888888888888888888
                else if (MainPageInput.LNumber <= 209.9)
                {
                    calcoloFloat = (MainPageInput.LNumber + luce - 29) / 4;
                    CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, MainPageInput.LNumber - 15, 2);
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888  6 fori 4 cordini  8888888888888888888888888888888888888888888888888888888888888888
                else if (MainPageInput.LNumber <= 289.9)
                {
                    calcoloFloat = (MainPageInput.LNumber + luce - 29) / 5;
                    CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, MainPageInput.LNumber - 15, 2);
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888  7 fori 4 cordini  8888888888888888888888888888888888888888888888888888888888888888
                else
                {
                    calcoloFloat = (MainPageInput.LNumber + luce - 29) / 6;
                    CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, MainPageInput.LNumber - 15, 2);
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                double Fatt_min_Tot_temp = Fatt_min_Tot;
                if (User == "io" && Documento.Cliente.Listino == "1") Fatt_min_Tot_temp = 1.5;
                CalcoloQuantitaMQ(MainPageInput.LNumber, MainPageInput.HNumber, MainPageInput.Pezzi, Fatt_min_Tot_temp);
                CalcoloSupplemento1(MainPageInput.Supplemento1Quantita, MainPageInput.Supplemento1Prezzo);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
            }
            else
            {
                luceLperEtich = 0;
            }
            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, CordiniVeneziane = cordiniVeneziane, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var etichettaVeneziane25 = new EtichettaVeneziane25mm(etichetta))
            {
                GraphicsDrawable1 = etichettaVeneziane25;
                DrawingSurface1 = etichettaVeneziane25.ToBitmap();
            }

            Aggiorna();
        }
        private void Veneziane35()
        {
            double luce = MainPageInput.LuceFinita ? -1 : 0;
            double[] cordiniVeneziane = new double[3];

            ControlliInterfaccia = new ControlliInterfaccia
            {
                GuideVisible = true,
                HcVisible = true,
                ComandiVisible = true,
                AttacchiVisible = true,
                ListaComandi = ["TD", "DX", "SX", "TS"],
                ListaNote = ["Lamella - 1cm centrale", "Cass. unico xxx_)1cm(_", "Attacco guida a L", "Attacco guida a Z", "!!NOTE!!NOTE!!"],
                ListaAttacchi = ["Cerniera", "-U-"]
            };

            OnPropertyChanged(nameof(ControlliInterfaccia));

            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                luceLperEtich = Math.Round(MainPageInput.LNumber + luce, 1); //per etichetta
                luceHperEtich = Math.Round(MainPageInput.HNumber, 1); //per etichetta
                if (MainPageInput.LNumber > 350 && !MainPageInput.Diviso2 && !MainPageInput.Diviso2CU) { ControlliInterfaccia.MessaggioErrore = "Misura presumibilmente sbagliata"; } //TODO: Manca verifica pulsante /2
                if (MainPageInput.HNumber > 450) { ControlliInterfaccia.MessaggioErrore = "Misura presumibilmente sbagliata"; }
                //888888888888888888888888888888888888888888888888888888888888888888888888  2 fori 88888888888888888888888888888888888888888888888888888888888888888888888888
                if (MainPageInput.LNumber <= 90)
                {
                    calcoloFloat = 0;

                    if (MainPageInput.LNumber <= 58)
                    {
                        CalcoliVari.CalcoloTiraggioOrientatore_35_50(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, +5, 40);
                    }
                    else
                    {
                        CalcoliVari.CalcoloTiraggioOrientatore_35_50(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, -10, 40);
                    }
                }
                //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                else if (MainPageInput.LNumber <= 154.9)
                {
                    calcoloFloat = (MainPageInput.LNumber + luce - 42) / 2;
                    CalcoliVari.CalcoloTiraggioOrientatore_35_50(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, -10, 40);
                }
                //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                else if (MainPageInput.LNumber <= 205.9)
                {
                    calcoloFloat = (MainPageInput.LNumber + luce - 42) / 3;
                    CalcoliVari.CalcoloTiraggioOrientatore_35_50(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, -10, 40);
                }
                //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                else
                {
                    calcoloFloat = (MainPageInput.LNumber + luce - 42) / 4;
                    CalcoliVari.CalcoloTiraggioOrientatore_35_50(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, -10, 40, 1);
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                CalcoloQuantitaMQ(MainPageInput.LNumber, MainPageInput.HNumber, MainPageInput.Pezzi, Fatt_min_Tot);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
            }
            else
            {
                luceLperEtich = 0;
                luceHperEtich = 0;
            }
            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, CordiniVeneziane = cordiniVeneziane, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var etichettaVeneziane35 = new EtichettaVeneziane35mm(etichetta))
            {
                GraphicsDrawable1 = etichettaVeneziane35;
                DrawingSurface1 = etichettaVeneziane35.ToBitmap();
            }

            Aggiorna();
        }
        private void Veneziane50()
        {
            double luce = MainPageInput.LuceFinita ? -1 : 0;
            double[] cordiniVeneziane = new double[3];

            ControlliInterfaccia = new ControlliInterfaccia
            {
                GuideVisible = true,


                HcVisible = true,
                ComandiVisible = true,
                AttacchiVisible = true,
                ListaComandi = ["TD", "DX", "SX", "TS"],
                ListaNote = ["Lamella - 1cm centrale", "Cass. unico xxx_)1cm(_", "Attacco guida a L", "Attacco guida a Z", "!!NOTE!!NOTE!!"],
                ListaAttacchi = ["Cerniera", "-U-"]
            };

            OnPropertyChanged(nameof(ControlliInterfaccia));

            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                luceLperEtich = Math.Round(MainPageInput.LNumber + luce, 1); //per etichetta
                luceHperEtich = Math.Round(MainPageInput.HNumber, 1); //per etichetta
                if (MainPageInput.LNumber > 350 && !MainPageInput.Diviso2 && !MainPageInput.Diviso2CU) { ControlliInterfaccia.MessaggioErrore = "Misura presumibilmente sbagliata"; }
                if (MainPageInput.HNumber > 450) { ControlliInterfaccia.MessaggioErrore = "Misura presumibilmente sbagliata"; }
                //888888888888888888888888888888888888888888888888888888888888888888888888  2 fori 88888888888888888888888888888888888888888888888888888888888888888888888888
                if (MainPageInput.LNumber <= 99)
                {
                    calcoloFloat = 0;

                    if (MainPageInput.LNumber <= 58)
                    {
                        CalcoliVari.CalcoloTiraggioOrientatore_35_50(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, +5, 70);
                    }
                    else
                    {
                        CalcoliVari.CalcoloTiraggioOrientatore_35_50(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, -10, 70);
                    }
                }
                //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                else if (MainPageInput.LNumber <= 168.9)
                {
                    calcoloFloat = (MainPageInput.LNumber + luce - 42) / 2;
                    CalcoliVari.CalcoloTiraggioOrientatore_35_50(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, -10, 70);
                }
                //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                else if (MainPageInput.LNumber <= 223.9)
                {
                    calcoloFloat = (MainPageInput.LNumber + luce - 42) / 3;
                    CalcoliVari.CalcoloTiraggioOrientatore_35_50(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, -10, 70);
                }
                //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                else
                {
                    calcoloFloat = (MainPageInput.LNumber + luce - 42) / 4;
                    CalcoliVari.CalcoloTiraggioOrientatore_35_50(ref cordiniVeneziane, MainPageInput.Hc, MainPageInput.HNumber, -10, 70, 1);
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                CalcoloQuantitaMQ(MainPageInput.LNumber, MainPageInput.HNumber, MainPageInput.Pezzi, Fatt_min_Tot);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
            }
            else
            {
                luceLperEtich = 0;
                luceHperEtich = 0;
            }
            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, CordiniVeneziane = cordiniVeneziane, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var etichettaVeneziane50 = new EtichettaVeneziane50mm(etichetta))
            {
                GraphicsDrawable1 = etichettaVeneziane50;
                DrawingSurface1 = etichettaVeneziane50.ToBitmap();
            }

            Aggiorna();
        }
        //Verticali
        private void Verticali()
        {
            string varie2 = MainPageInput.AperturaCentrale ? "Apert. Centrale" : "";
            ControlliInterfaccia = new ControlliInterfaccia
            {
                HcVisible = true,
                AperturaCentraleVisible = true,
                ListaNote = ["Cass. col Argento", "Cass. col Avorio", "Comando invertito", "INCLINATA", "!!NOTE!!NOTE!!"],
            };
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }
            if (MainPageInput.AperturaCentrale)
            {
                ControlliInterfaccia.ListaSupplemento1 = ["Apertura centrale su Verticale"];
                LoadDatiSupplemento1("Apertura centrale su Verticale");
                CalcoloSupplemento1(MainPageInput.Pezzi, MainPageInput.Pezzi);
            }
            else
            {
                ControlliInterfaccia.ListaSupplemento1.Clear();
                MainPageInput.Supplemento1Pezzi = 0;
                MainPageInput.Supplemento1Prezzo = 0;
                MainPageInput.Supplemento1PrezzoTotale = 0;
                MainPageInput.Supplemento1Quantita = 0;
            }
            var etichetta = new Etichetta { Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = MainPageInput.LuceLperEtich, LuceHEtichetta = MainPageInput.LuceHperEtich, H = MainPageInput.HNumber, L = MainPageInput.LNumber, AperturaCentrale = MainPageInput.AperturaCentrale, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif, MixBandaFinita = MainPageInput.MixBandaFinita, MixTaglioCassonetto = MainPageInput.MixTaglioCassonetto, MixTaglioTubo = MainPageInput.MixTaglioTubo };
            using (var etichettaVenezianeVerticale1 = new EtichettaVerticale1(etichetta))
            {
                GraphicsDrawable1 = etichettaVenezianeVerticale1;
                DrawingSurface1 = etichettaVenezianeVerticale1.ToBitmap();
            }
            using (var etichettaVenezianeVerticale2 = new EtichettaVerticale2(etichetta))
            {
                GraphicsDrawable2 = etichettaVenezianeVerticale2;
                DrawingSurface2 = etichettaVenezianeVerticale2.ToBitmap();
            }
            using (var etichettaVenezianeVerticale3 = new EtichettaVerticale3(etichetta))
            {
                GraphicsDrawable3 = etichettaVenezianeVerticale3;
                DrawingSurface3 = etichettaVenezianeVerticale3.ToBitmap();
            }
            Aggiorna();
        }
        private void BandeVerticali()
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }
            //ControlliInterfaccia = new ControlliInterfaccia
            //{
            //    HcVisible = true,
            //    AperturaCentraleVisible = true,
            //    ListaNote = ["Cass. col Argento", "Cass. col Avorio", "Comando invertito", "INCLINATA", "!!NOTE!!NOTE!!"],
            //};
            //if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            //     private void Bande_vert(string Nome, string Annotazione_su_etichetta)
            //{
            //    articoloDB = articoliCB.Text.Replace("'", "''");

            //    if (flagForCMBXitems == false)
            //    {
            //        Email_A_fornitori_Intestazione("");
            //        L_label.Text = "H(mm)";
            //        H_label.Visible = false;
            //        H_TXBX.Visible = false;

            //        LuceRBN.Text = "telo-telo";
            //        FinitaRBN.Text = "telo-portatelo";

            //        flagForCMBXitems = true;
            //        flagForEtich_unica = true;
            //    }

            //    if (LuceRBN.Checked == true) luceH = 0;
            //    else luceH = +4;

            //    if (L_TXBX.Text != "") //Calcolo quantita'
            //    {
            //        //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //        Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);
            //        Calcolo_QuantitaMQ(int.Parse(L_TXBX.Text), 1000, Fatt_min_Tot);//verificare 1000 era 100
            //        //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //    }
            //    else QuantitaTXBX.Text = "";

            //    GFX.Clear(Color.White);
            //    GFX.DrawString(Alias.Substring(0, Alias.Length > 35 ? 35 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 3);
            //    GFX.DrawString(Nome, new Font("thaoma", 8), Brushes.Black, 220, 3);
            //    GFX.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
            //    GFX.DrawString(Annotazione_su_etichetta, new Font("thaoma", 8), Brushes.Black, 105, 35);
            //    GFX.DrawString("H " + (float.TryParse(L_TXBX.Text, out float f) ? f - luceH : 0).ToString(), new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
            //    GFX.DrawString("PZ " + PezziTXBX.Text, new Font("thaoma", 8), Brushes.Black, 70, 58);

            //    GFX.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 80);
            //    GFX.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 80);
            //    pictureBox1.Image = drawingsurface;
            //}

            Aggiorna();
        }
        private void BandeVerticaliComplete()
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }
            Aggiorna();
        }
        //Binario Pannello
        private void BinarioVerticale()
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }
            //articoloDB = articoliCB.Text.Replace("'", "''");

            //if (AperturaCentraleCKBX.Checked == true) varie2 = "Apert. centrale";
            //else varie2 = "";

            //if (flagForCMBXitems == false)
            //{
            //    Email_A_fornitori_Intestazione("");
            //    H_TXBX.Visible = false;
            //    H_label.Visible = false;
            //    Hc_label.Visible = true;
            //    HcTXBX.Visible = true;
            //    AperturaCentraleCKBX.Visible = true;

            //    NoteCMBBX.Items.Add("Argento");
            //    NoteCMBBX.Items.Add("Avorio");
            //    NoteCMBBX.Items.Add("Apertura Invertita");

            //    flagForCMBXitems = true;

            //}
            //if (LuceRBN.Checked == true) luceL = -3;
            //else luceL = 0;

            //if (L_TXBX.Text != "")//Calcolo quantita'
            //{
            //    luceLperEtich = int.Parse(L_TXBX.Text) + luceL;
            //    //--------------------------------------------------------------------------------------------------------------------        
            //    mixTaglioCassonetto = luceLperEtich - 15;
            //    mixTaglioTubo = luceLperEtich - 31;

            //    //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //    Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);
            //    Calcolo_QuantitaMQ(int.Parse(L_TXBX.Text), 1000, Fatt_min_Tot);//verificare 1000 prima era 100
            //    //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //}
            //else
            //{
            //    luceLperEtich = 0;
            //    QuantitaTXBX.Text = "";
            //    mixTaglioCassonetto = 0;
            //    mixTaglioTubo = 0;
            //}

            //if (AperturaCentraleCKBX.Checked)
            //{
            //    Supplemento1CMBX.Text = "Apertura centrale su Verticale";
            //    LoadDatiSupplemento1CMBX();
            //    Calcolo_supplemento1(int.Parse(PezziTXBX.Text), PezziTXBX.Text);
            //}
            //else
            //{
            //    Supplemento1CMBX.Text = "";
            //    Pz_supp1_txb.Text = "";
            //    Tot_SupplementoTXBX.Text = "";
            //    Qnt_suppTXBX.Text = "0";
            //    Costo_supplementoTXBX.Text = "0";
            //}

            //GFX.Clear(Color.White);
            //GFX.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 3);
            //GFX.DrawString("Binario Verticale", new Font("thaoma", 8), Brushes.Black, 220, 3);
            //GFX.DrawString("Col " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 20);
            //GFX.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 35);
            //GFX.DrawString(Hc.ToString(), new Font("thaoma", 8), Brushes.Black, 135, 35);
            //GFX.DrawRectangle(pen1, 7, 50, 177, 31);
            //GFX.DrawString("(" + CalcoliVari.CalcoloCordaVerticale_Pannello(HcTXBX.Text, L_TXBX.Text) + ")corda", new Font("thaoma", 8), Brushes.Black, 121, 53);
            //GFX.DrawString(CalcoliVari.N_bande(L_TXBX, AperturaCentraleCKBX.Checked), new Font("thaoma", 8), Brushes.Black, 5, 53);
            //GFX.DrawString("(" + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "", uno: 0, due: 1100, tre: 1900, quattro: 3000, cinque: 4000, sei: 6001) + ")N°clip", new Font("thaoma", 8), Brushes.Black, 7, 67);

            //GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioCassonetto.ToString("0000")), 11), 190, 23);
            //GFX.DrawString(mixTaglioCassonetto.ToString(), new Font("thaoma", 7), Brushes.Black, 250, 32);
            //GFX.DrawString("Binario", new Font("thaoma", 6), Brushes.Black, 200, 34);
            //GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioTubo.ToString("0000")), 11), 190, 45);
            //GFX.DrawString(mixTaglioTubo.ToString(), new Font("thaoma", 7), Brushes.Black, 250, 56);
            //GFX.DrawString("Alberino", new Font("thaoma", 6), Brushes.Black, 200, 57);

            //GFX.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
            //GFX.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);

            //pictureBox1.Image = drawingsurface;
            Aggiorna();
        }
        private void BinarioPannelloCorda(int n)
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }
            //articoloDB = articoliCB.Text.Replace("'", "''");

            //if (LuceRBN.Checked == true) luceL = -3;
            //else luceL = 0;

            //if (flagForCMBXitems == false)
            //{
            //    Email_A_fornitori_Intestazione("");
            //    nAgganciCingoliPannelli = NumeroVie.ToString();
            //    H_label.Visible = false;
            //    H_TXBX.Visible = false;
            //    L2_label.Visible = true;
            //    L2_TXBX.Visible = true;
            //    L2_TXBX.Text = "0";
            //    NoteArtVarieLabel.Text = "con L2=0 -> calcolo mix pannello automatico";
            //    Comandi_label.Visible = true;
            //    ComandiCMBX.Visible = true;
            //    Hc_label.Visible = true;
            //    HcTXBX.Visible = true;

            //    flagForCMBXitems = true;

            //    if (ColoreCMBX.Items.Count >= 1) ColoreCMBX.SelectedIndex = 0;
            //    ComandiCMBX.Items.Add("DX");
            //    ComandiCMBX.Items.Add("SX");
            //    ComandiCMBX.Text = "DX";//questa riga va' lasciata qui, come ultima riga, altrimenti quando cambia la texbox attiva changed                
            //}
            ////--------------------------------------------------------------------------------------------------------------------
            //if (L_TXBX.Text != "" && L_TXBX.Text != "0")
            //{
            //    luceLperEtich = int.Parse(L_TXBX.Text) + luceL;
            //    mixTaglioBinario = luceLperEtich - 24;
            //    if (L2_TXBX.Text == "0") mixTaglioProfiloInferiore = (int)Math.Round((luceLperEtich + (60d * (NumeroVie - 1))) / NumeroVie);
            //    else
            //    {
            //        if (L2_TXBX.Text == "")
            //        {
            //            L2_TXBX.Text = "0";
            //            return;
            //        }
            //        mixTaglioProfiloInferiore = int.Parse(L2_TXBX.Text);
            //    }
            //    calcoloFLoat = (float)mixTaglioProfiloInferiore * NumeroVie / 1000;//strappo

            //    //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //    Email_A_fornitori_Pannello(mixTaglioProfiloInferiore.ToString());
            //    Calcolo_QuantitaMQ(int.Parse(L_TXBX.Text), 1000, Fatt_min_Tot);
            //    //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888

            //}
            //else
            //{
            //    QuantitaTXBX.Text = "";
            //    luceLperEtich = 0;
            //    calcoloFLoat = 0;

            //    mixTaglioBinario = 0;
            //    mixTaglioProfiloInferiore = 0;
            //}

            //GFX.Clear(Color.White);
            //GFX.DrawString(Alias.Substring(0, Alias.Length > 35 ? 35 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 0);
            //GFX.DrawString(NumeroVie.ToString() + " vie corda", new Font("thaoma", 8), Brushes.Black, 220, 0);
            //GFX.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 22);
            //GFX.DrawString("COM " + ComandiCMBX.Text, new Font("thaoma", 8), Brushes.Black, 63, 22);
            //GFX.DrawString(Hc.ToString(), new Font("thaoma", 8), Brushes.Black, 130, 22);

            //GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioBinario.ToString("0000")), 8), PartenzaBarcodeDX, 20);
            //GFX.DrawString(mixTaglioBinario.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 26);
            //GFX.DrawString("Binario", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 26);

            //GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioProfiloInferiore.ToString("0000")), 8), PartenzaBarcodeDX, 39);
            //GFX.DrawString(mixTaglioProfiloInferiore.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 46);
            //GFX.DrawString(NumeroVie + " Portateli", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 46);


            //GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, (mixTaglioProfiloInferiore - 12).ToString("0000")), 8), PartenzaBarcodeDX, 61);
            //GFX.DrawString((mixTaglioProfiloInferiore - 12).ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 67);
            //GFX.DrawString(NumeroVie + " Pesi", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 67);

            //GFX.DrawRectangle(pen1, 7, 40, 190, 37);
            //GFX.DrawString("(" + CalcoliVari.CalcoloCordaVerticale_Pannello(HcTXBX.Text, L_TXBX.Text) + ")mix corda", new Font("thaoma", 8), Brushes.Black, 10, 43);
            //GFX.DrawString("(" + Math.Round(calcoloFLoat, 2) + ")mix strappo", new Font("thaoma", 8), Brushes.Black, 10, 60);
            //GFX.DrawString("(" + (NumeroVie > 5 ? (int.Parse(CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "", uno: 0, due: 1100, tre: 1900, quattro: 2600, cinque: 3400, sei: 5000, sette: 6000)) * 2).ToString() : CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "", uno: 0, due: 1100, tre: 1900, quattro: 2600, cinque: 3400, sei: 5000, sette: 6000)) + ")N°clip", new Font("thaoma", 8), Brushes.Black, 140, 43);
            //GFX.DrawString("Sag. punte Portateli", new Font("thaoma", 7, FontStyle.Bold), Brushes.Black, 100, 60);

            //GFX.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 80);
            //GFX.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 250, 80);

            //pictureBox1.Image = drawingsurface;
            Aggiorna();
        }
        private void BinarioPannelloAsta(int n)
        {
            Aggiorna();
        }
        private void BinarioPannelloManuale(int n)
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBinarioPannelloManuale = new EtichettaBinarioPannelloManuale(etichetta))
            {
                GraphicsDrawable1 = EtichettaBinarioPannelloManuale;
                DrawingSurface1 = EtichettaBinarioPannelloManuale.ToBitmap();
            }
            //articoloDB = articoliCB.Text.Replace("'", "''");

            //if (LuceRBN.Checked == true) luceL = -3;
            //else luceL = 0;

            //if (flagForCMBXitems == false)
            //{
            //    Email_A_fornitori_Intestazione("");
            //    nAgganciCingoliPannelli = NumeroVie.ToString();
            //    H_label.Visible = false;
            //    H_TXBX.Visible = false;
            //    L_label.Visible = true;
            //    L_TXBX.Visible = true;
            //    L_label.Text = "L(mm)";
            //    L2_label.Visible = true;
            //    L2_TXBX.Visible = true;
            //    L2_TXBX.Text = "0";
            //    NoteArtVarieLabel.Text = "con L2=0 -> calcolo mix pannello automatico";

            //    flagForCMBXitems = true;

            //    if (ColoreCMBX.Items.Count >= 1) ColoreCMBX.SelectedIndex = 0;
            //}
            ////--------------------------------------------------------------------------------------------------------------------
            //if (L_TXBX.Text != "" && L_TXBX.Text != "0")
            //{
            //    luceLperEtich = int.Parse(L_TXBX.Text) + luceL;

            //    mixTaglioBinario = luceLperEtich - 5;
            //    if (L2_TXBX.Text == "0") mixTaglioProfiloInferiore = (int)Math.Round((luceLperEtich + (60d * (NumeroVie - 1))) / NumeroVie);//verificare
            //    else
            //    {
            //        if (L2_TXBX.Text == "")
            //        {
            //            L2_TXBX.Text = "0";
            //            return;
            //        }
            //        mixTaglioProfiloInferiore = int.Parse(L2_TXBX.Text);
            //    }
            //    calcoloFLoat = (float)mixTaglioProfiloInferiore * NumeroVie / 1000;//strappo//verificare

            //    //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //    Email_A_fornitori_Pannello(mixTaglioProfiloInferiore.ToString());
            //    Calcolo_QuantitaMQ(int.Parse(L_TXBX.Text), 1000, Fatt_min_Tot);//verificare
            //    //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //}
            //else
            //{
            //    luceLperEtich = 0;
            //    calcoloFLoat = 0;
            //    QuantitaTXBX.Text = "";
            //    mixTaglioBinario = 0;
            //    mixTaglioProfiloInferiore = 0;
            //}

            //GFX.Clear(Color.White);
            //GFX.DrawString(Alias.Substring(0, Alias.Length > 35 ? 35 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 0);
            //GFX.DrawString(NumeroVie + " vie manuale", new Font("thaoma", 8), Brushes.Black, 220, 0);
            //GFX.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 22);

            //GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioBinario.ToString("0000")), 8), PartenzaBarcodeDX, 20);
            //GFX.DrawString(mixTaglioBinario.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 26);
            //GFX.DrawString("Binario", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 26);
            //GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioProfiloInferiore.ToString("0000")), 8), PartenzaBarcodeDX, 39);
            //GFX.DrawString(mixTaglioProfiloInferiore.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 46);
            //GFX.DrawString(NumeroVie + " Portateli", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 46);
            //GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, (mixTaglioProfiloInferiore - 12).ToString("0000")), 8), PartenzaBarcodeDX, 61);
            //GFX.DrawString((mixTaglioProfiloInferiore - 12).ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 67);
            //GFX.DrawString(NumeroVie + " Pesi", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 67);

            //GFX.DrawRectangle(pen1, 7, 40, 190, 37);
            //GFX.DrawString("(" + Math.Round(calcoloFLoat, 2) + ")mix strappo", new Font("thaoma", 8), Brushes.Black, 10, 60);
            //GFX.DrawString("(" + (NumeroVie > 5 ? (int.Parse(CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "", uno: 0, due: 1100, tre: 1900, quattro: 2600, cinque: 3400, sei: 5000, sette: 6000)) * 2).ToString() : CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "", uno: 0, due: 1100, tre: 1900, quattro: 2600, cinque: 3400, sei: 5000, sette: 6000)) + ")N°clip", new Font("thaoma", 8), Brushes.Black, 140, 43);

            //GFX.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 80);
            //GFX.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 250, 80);
            //pictureBox1.Image = drawingsurface;
            Aggiorna();
        }
        private void BinarioPannelloXVieACordaApCentraleXPannelli(int numeroVie, int numeroPannelli)
        {
            Aggiorna();
        }
        private void BinarioPannelloXVieConAstaApCentraleXPannelli(int numeroVie, int numeroPannelli)
        {
            Aggiorna();
        }
        private void BinarioPannelloXVieManualeApCentraleXPannelli(int numeroVie, int numeroPannelli)
        {
            Aggiorna();
        }
        //Zanzariere
        private void ReteSaldata()
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }
            //articoloDB = articoliCB.Text.Replace("'", "''");
            //if (flagForCMBXitems == false)
            //{
            //    L_label.Text = "L(mm)"; L_label.Visible = true; L_TXBX.Visible = true;
            //    H_label.Visible = true; H_TXBX.Visible = true;
            //    Varie1TCMBX.Visible = false;
            //    ColoreCMBX.Visible = false;
            //    Varie1_label.Visible = false;
            //    Colore_label.Visible = false;
            //    Guide_Label.Visible = false;
            //    LuceRBN.Visible = false;
            //    FinitaRBN.Visible = false;

            //    NoteCMBBX.Items.Clear();
            //    NoteCMBBX.Items.Add("Senza bottoncini"); NoteCMBBX.Items.Add("Doppia Saldatura");
            //    L_TXBX.Focus();

            //    flagForCMBXitems = true;
            //}

            //if ((L_TXBX.Text != "") & (H_TXBX.Text != "")) //Calcolo quantita'
            //{
            //    if (int.Parse(H_TXBX.Text) <= 1200 && H_TXBX.ContainsFocus == false) H_TXBX.Text = "1200";
            //    else if (int.Parse(H_TXBX.Text) <= 1600 & H_TXBX.ContainsFocus == false) H_TXBX.Text = "1600";
            //    else if (int.Parse(H_TXBX.Text) <= 2000 & H_TXBX.ContainsFocus == false) H_TXBX.Text = "2000";
            //    else if (int.Parse(H_TXBX.Text) <= 2600 & H_TXBX.ContainsFocus == false) H_TXBX.Text = "2600";
            //    else if (int.Parse(H_TXBX.Text) <= 2800 & H_TXBX.ContainsFocus == false) H_TXBX.Text = "2800";

            //    //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //    Calcolo_QuantitaMQ(int.Parse(L_TXBX.Text), int.Parse(H_TXBX.Text), Fatt_min_Tot);
            //    //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //}

            //GFX.Clear(Color.White);
            //GFX.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 3);
            //GFX.DrawString("Rete Saldata", new Font("thaoma", 8), Brushes.Black, 210, 3);
            //GFX.DrawString("L " + L_TXBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 22);
            //GFX.DrawString("H " + H_TXBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 75, 22);

            //GFX.DrawString("NOTE: " + NoteCMBBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 83);
            //GFX.DrawString("Rif " + RifTXBX.Text, new Font("thaoma", 8), Brushes.Black, 220, 83);

            //pictureBox1.Image = drawingsurface;
            Aggiorna();
        }
        private void ReteSaldataZebrata()
        {
            Aggiorna();
        }
        private void ZanzarieraFissa()
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }
            //articoloDB = articoliCB.Text.Replace("'", "''");
            //if (LuceRBN.Checked == true)
            //{
            //    luceL = -3;
            //    luceH = -3;
            //}
            //else
            //{
            //    luceL = 0;
            //    luceH = 0;
            //}
            //if (flagForCMBXitems == false)
            //{
            //    NoteArtVarieLabel.Text = "-vista frontale-";

            //    ImponRB.TabStop = false;
            //    Supplemento1CMBX.Items.Add("Rete Acciaio");//
            //    Supplemento1CMBX.Items.Add("Rete pet-screen  h 122");
            //    Supplemento2CMBX.Items.Add("Giunto angolo magnete+piastra+vite");
            //    NoteCMBBX.Items.Add("Rete Alluminio");
            //    flagForCMBXitems = true;
            //}

            //if ((L_TXBX.Text != "") && (H_TXBX.Text != ""))
            //{
            //    luceLperEtich = int.Parse(L_TXBX.Text) + luceL;
            //    luceHperEtich = int.Parse(H_TXBX.Text) + luceH;
            //    //-------------------------------------------------------------------------------------------------------------------- 

            //    if (User == "Alessio") { mixTaglioProfiloSuperiore = luceLperEtich - 61; mixTaglioProfilo = luceHperEtich - 61; }
            //    else { mixTaglioProfiloSuperiore = luceLperEtich - 60; mixTaglioProfilo = luceHperEtich - 60; }

            //    //---------------------------------------------------------------------------------------------------------------------
            //    //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //    Calcolo_QuantitaMQ(int.Parse(L_TXBX.Text), int.Parse(H_TXBX.Text), Fatt_min_Tot);
            //    Calcolo_supplemento1(float.Parse(QuantitaTXBX.Text), PezziTXBX.Text);
            //    Calcolo_supplemento2(4 * int.Parse(PezziTXBX.Text), PezziTXBX.Text);
            //    //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888m
            //}
            //else
            //{
            //    luceLperEtich = 0;
            //    luceHperEtich = 0;
            //    mixTaglioProfiloSuperiore = 0;
            //    mixTaglioProfilo = 0;
            //}

            //GFX.Clear(Color.White);
            //GFX.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 0);
            //GFX.DrawString("Z.Fissa", new Font("thaoma", 8), Brushes.Black, 210, 0);

            //GFX.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
            //GFX.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
            //GFX.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 75, 40);

            //if (Supplemento1CMBX.Text != "")
            //{
            //    GFX.FillRectangle(Brushes.Gray, 5, 67, 125, 16);
            //    GFX.DrawString(Supplemento1CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.White, 5, 68);
            //}

            //if (Supplemento2CMBX.Text != "")
            //{
            //    GFX.FillRectangle(Brushes.Gray, 155, 67, 170, 16);
            //    GFX.DrawString(Supplemento2CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.White, 155, 68);
            //}

            //GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioProfiloSuperiore.ToString("0000")), 10), PartenzaBarcodeDX, 13);
            //GFX.DrawString(mixTaglioProfiloSuperiore.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 21);
            //GFX.DrawString("L", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 21);
            //GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioProfilo.ToString("0000")), 10), PartenzaBarcodeDX, 33);
            //GFX.DrawString(mixTaglioProfilo.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 41);
            //GFX.DrawString("H", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 41);
            //GFX.DrawString("-Vista frontale-", new Font("thaoma", 8), Brushes.Black, 218, 53);

            //GFX.DrawString("NOTE: " + NoteCMBBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 83);
            //GFX.DrawString("Rif " + RifTXBX.Text, new Font("thaoma", 8), Brushes.Black, 220, 83);

            //pictureBox1.Image = drawingsurface;
            Aggiorna();
        }
        private void Unika45()
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }
            Aggiorna();
        }
        private void Incassata45PiUnika45()
        {
            Aggiorna();
        }
        private void ZanzariereAMolla(string nomezanzariera)
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaZanzariereAMolla = new EtichettaZanzariereAMolla(etichetta))
            {
                GraphicsDrawable1 = EtichettaZanzariereAMolla;
                DrawingSurface1 = EtichettaZanzariereAMolla.ToBitmap();
            }
            //articoloDB = articoliCB.Text.Replace("'", "''");

            //if (LuceRBN.Checked == true)
            //{
            //    if (User == "Alessio") { luceL = -3; luceH = -2; }
            //    else { luceL = -2; luceH = -2; }
            //}
            //else { luceL = 0; luceH = 0; }

            //if (flagForCMBXitems == false)
            //{
            //    Email_A_fornitori_Intestazione("");

            //    NoteCMBBX.Items.Add("maniglia -Xmm");
            //    Supplemento1CMBX.Items.Add("Freno adagio");
            //    Supplemento2CMBX.Items.Add("Rete Zebrata");

            //    flagForCMBXitems = true;
            //}
            ////--------------------------------------------------------------------------------------------------------------------
            //if ((L_TXBX.Text != "") && (H_TXBX.Text != ""))
            //{
            //    int Hrete;
            //    luceLperEtich = int.Parse(L_TXBX.Text) + luceL;
            //    luceHperEtich = int.Parse(H_TXBX.Text) + luceH;

            //    if (int.Parse(H_TXBX.Text) <= 1100) Hrete = 1200;
            //    else if (int.Parse(H_TXBX.Text) <= 1500) Hrete = 1600;
            //    else if (int.Parse(H_TXBX.Text) <= 1900) Hrete = 2000;
            //    else if (int.Parse(H_TXBX.Text) <= 2500) Hrete = 2600;
            //    else if (int.Parse(H_TXBX.Text) <= 2700) Hrete = 2800;
            //    else Hrete = 3000;

            //    //------------------------------------------------------------------

            //    mixTaglioCassonetto = (luceLperEtich - 18);
            //    mixTaglioTubo = luceLperEtich - 72;
            //    mixTaglioFondaleManiglia = luceLperEtich - 86;

            //    if (User == "io") { mixTeloFinita = (luceLperEtich - 33) + "x" + Hrete; mixTaglioGuida = luceHperEtich - 115; }
            //    else if (User == "Alessio") { mixTeloFinita = (luceLperEtich - 37) + "x" + Hrete; mixTaglioGuida = int.Parse(H_TXBX.Text) - 110; }//Alessio
            //    else mixTaglioGuida = luceHperEtich - 105;//Gugliersi

            //    if (nomezanzariera.Contains("Cricchetto")) mixTaglioGuida = luceHperEtich - 71;

            //    //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //    Calcolo_QuantitaMQ(int.Parse(L_TXBX.Text), int.Parse(H_TXBX.Text), Fatt_min_Tot);
            //    Calcolo_supplemento1(int.Parse(PezziTXBX.Text), PezziTXBX.Text);
            //    Calcolo_supplemento2(float.Parse(QuantitaTXBX.Text), PezziTXBX.Text);//verificare
            //    Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);
            //    //88888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //}
            //else
            //{
            //    mixTeloFinita = "0";
            //    QuantitaTXBX.Text = "";
            //    mixTaglioCassonetto = 0;
            //    mixTaglioTubo = 0;
            //    mixTaglioFondaleManiglia = 0;
            //    mixTaglioGuida = 0;
            //}
            ////---------------------------------------------------------------------------------------------------------------------           

            //GFX.Clear(Color.White);
            //GFX.DrawString(Alias.Substring(0, Alias.Length > 31 ? 31 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 2);
            //GFX.DrawString(nomezanzariera, new Font("thaoma", 8), Brushes.Black, 215, 2);
            //GFX.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
            //GFX.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 37);
            //GFX.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 75, 37);
            //GFX.DrawString(DateTime.Now.ToString("dd-MM-yyyy"), new Font("thaoma", 7), Brushes.Black, 5, 51);

            //if (Supplemento1CMBX.Text != "")
            //{
            //    GFX.FillRectangle(Brushes.Gray, 5, 65, 90, 16);
            //    GFX.DrawString(Supplemento1CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.White, 5, 65);
            //}
            //else GFX.FillRectangle(Brushes.White, 0, 0, 0, 0);

            //if (User == "io")
            //{
            //    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioCassonetto.ToString("0000")), 9), PartenzaBarcodeDX, 21);
            //    GFX.DrawString(mixTaglioCassonetto.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 29);
            //    GFX.DrawString("cass", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 28);
            //    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioTubo.ToString("0000")), 9), PartenzaBarcodeDX, 41);
            //    GFX.DrawString(mixTaglioTubo.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 48);
            //    GFX.DrawString("tubo", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 47);
            //    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioFondaleManiglia.ToString("0000")), 9), PartenzaBarcodeDX, 61);
            //    GFX.DrawString(mixTaglioFondaleManiglia.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 68);
            //    GFX.DrawString("maniglia", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 67);

            //}
            //else//Alessio
            //{
            //    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, (luceLperEtich - 22).ToString("0000")), 10), PartenzaBarcodeDX, 21);
            //    GFX.DrawString((luceLperEtich - 22).ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 31);
            //    GFX.DrawString("L", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 30);
            //}

            //GFX.DrawString("NOTE: " + NoteCMBBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 83);
            //GFX.DrawString("Rif " + RifTXBX.Text, new Font("thaoma", 8), Brushes.Black, 220, 83);

            //pictureBox1.Image = drawingsurface;

            //GFX2.Clear(Color.White);
            //GFX2.DrawString(Alias.Substring(0, Alias.Length > 31 ? 31 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 2);
            //GFX2.DrawString(nomezanzariera, new Font("thaoma", 8), Brushes.Black, 215, 2);
            //GFX2.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 20);
            //GFX2.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 35);
            //GFX2.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 75, 35);

            //GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioGuida.ToString("0000")), 10), PartenzaBarcodeDX, 21);
            //GFX2.DrawString(mixTaglioGuida.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 29);
            //GFX2.DrawString("guide", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 29);

            //GFX2.DrawString("NOTE: " + NoteCMBBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 49);
            //GFX2.DrawString("Rif " + RifTXBX.Text, new Font("thaoma", 8), Brushes.Black, 220, 49);

            //pictureBox2.Image = drawingsurface2;

            //GFX3.Clear(Color.White);
            //GFX3.DrawString(Alias.Substring(0, Alias.Length > 31 ? 31 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 2);
            //GFX3.DrawString(nomezanzariera, new Font("thaoma", 8), Brushes.Black, 215, 2);
            //GFX3.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 20);
            //GFX3.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 35);
            //GFX3.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 75, 35);

            //GFX3.DrawImage(barcode.Draw(string.Format(barcodeStruct, luceLperEtich.ToString("0000")), 10), PartenzaBarcodeDX, 21);
            //GFX3.DrawString("(" + mixTeloFinita + ")rete", new Font("thaoma", 8), Brushes.Black, 213, 35);
            //if (Supplemento2CMBX.Text != "")
            //{
            //    GFX3.FillRectangle(Brushes.Gray, 210, 50, 85, 16);
            //    GFX3.DrawString(Supplemento2CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.White, 213, 50);
            //}
            //else GFX3.FillRectangle(Brushes.White, 0, 0, 0, 0);

            //GFX3.DrawString("NOTE: " + NoteCMBBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 83);
            //GFX3.DrawString("Rif " + RifTXBX.Text, new Font("thaoma", 8), Brushes.Black, 220, 83);

            //pictureBox3.Image = drawingsurface3;
            Aggiorna();
        }
        private void ZanzariereAMollaDoppiaG()
        {
            Aggiorna();
        }
        private void ZanzariereACatena(float Quotacassone, float Quotaguide, string Nomezanzariera)
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaZanzariereACatena = new EtichettaZanzariereACatena(etichetta))
            {
                GraphicsDrawable1 = EtichettaZanzariereACatena;
                DrawingSurface1 = EtichettaZanzariereACatena.ToBitmap();
            }
            //     private void Zanzariere_a_catena(int Quotacassone, int Quotaguide, string Nomezanzariera)
            //{
            //    articoloDB = articoliCB.Text.Replace("'", "''");
            //    if (LuceRBN.Checked == true)
            //    {
            //        if (User == "Alessio") { luceL = -3; luceH = -2; }
            //        else { luceL = -2; luceH = -2; }
            //    }
            //    else { luceL = 0; luceH = 0; }

            //    if (flagForCMBXitems == false)
            //    {
            //        Email_A_fornitori_Intestazione("");

            //        Hc_label.Visible = true;
            //        HcTXBX.Visible = true;

            //        Comandi_label.Visible = true;
            //        ComandiCMBX.Visible = true;
            //        ComandiCMBX.Enabled = true;
            //        Comandi_label.Text = "Catena";
            //        ComandiCMBX.Items.Add("DX"); ComandiCMBX.Items.Add("SX"); ComandiCMBX.Items.Add("Doppia DX"); ComandiCMBX.Items.Add("Doppia SX"); ComandiCMBX.Text = "DX";
            //        Supplemento2CMBX.Items.Add("Rete Zebrata");
            //        NoteCMBBX.Items.Add("maniglia -XXmm");

            //        flagForCMBXitems = true;
            //    }

            //    if (L_TXBX.Text != "" && H_TXBX.Text != "")
            //    {
            //        int Hrete;
            //        luceLperEtich = int.Parse(L_TXBX.Text) + luceL;
            //        luceHperEtich = int.Parse(H_TXBX.Text) + luceH;

            //        if (int.Parse(H_TXBX.Text) <= 1100) Hrete = 1200;
            //        else if (int.Parse(H_TXBX.Text) <= 1500) Hrete = 1600;
            //        else if (int.Parse(H_TXBX.Text) <= 1900) Hrete = 2000;
            //        else if (int.Parse(H_TXBX.Text) <= 2500) Hrete = 2600;
            //        else if (int.Parse(H_TXBX.Text) <= 2700) Hrete = 2800;
            //        else Hrete = 3000;
            //        //--------------------------------------------------------------------------------------------------------------------               
            //        mixTeloFinita = luceLperEtich - 37 + "x" + Hrete;
            //        mixTaglioCassonetto = luceLperEtich - 28;
            //        mixTaglioTubo = luceLperEtich - 72;
            //        mixTaglioFondaleManiglia = luceLperEtich - 88;


            //        if (User == "io") { mixTeloFinita = (luceLperEtich - 33) + "x" + Hrete; mixTaglioGuida = luceHperEtich - Quotaguide; }
            //        else if (User == "Alessio") { mixTeloFinita = (luceLperEtich - 40) + "x" + Hrete; mixTaglioGuida = int.Parse(H_TXBX.Text) - 75; }//Alessio
            //        else mixTaglioGuida = luceHperEtich - 115;//Gugliersi

            //        //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //        Calcolo_QuantitaMQ(int.Parse(L_TXBX.Text), int.Parse(H_TXBX.Text), Fatt_min_Tot);
            //        Calcolo_supplemento1(int.Parse(PezziTXBX.Text), PezziTXBX.Text);
            //        Calcolo_supplemento2(float.Parse(QuantitaTXBX.Text), PezziTXBX.Text);//verificare float
            //        Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);
            //        //88888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //    }
            //    else
            //    {
            //        mixTeloFinita = "";
            //        mixTaglioCassonetto = 0;
            //        mixTaglioTubo = 0;
            //        mixTaglioFondaleManiglia = 0;
            //        mixTaglioGuida = 0;
            //    }

            //    GFX.Clear(Color.White);
            //    GFX.DrawString(Alias.Substring(0, Alias.Length > 35 ? 35 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 2);
            //    GFX.DrawString(Nomezanzariera, new Font("thaoma", 8), Brushes.Black, 215, 2);
            //    GFX.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
            //    GFX.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 37);
            //    GFX.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 75, 37);
            //    GFX.DrawString(DateTime.Now.ToString("dd-MM-yyyy"), new Font("thaoma", 7), Brushes.Black, 130, 37);
            //    GFX.DrawRectangle(pen1, 7, 51, 95, 30);
            //    GFX.DrawString("Catena " + ComandiCMBX.Text, new Font("thaoma", 8), Brushes.Black, 7, 52);
            //    GFX.DrawString(Hc.ToString(), new Font("thaoma", 8), Brushes.Black, 7, 67);


            //    if (User == "io")
            //    {
            //        GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioCassonetto.ToString("0000")), 9), PartenzaBarcodeDX, 21);
            //        GFX.DrawString(mixTaglioCassonetto.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 29);
            //        GFX.DrawString("cass", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 28);
            //        GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioTubo.ToString("0000")), 9), PartenzaBarcodeDX, 41);
            //        GFX.DrawString(mixTaglioTubo.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 48);
            //        GFX.DrawString("tubo", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 47);
            //        GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioFondaleManiglia.ToString("0000")), 9), PartenzaBarcodeDX, 61);
            //        GFX.DrawString(mixTaglioFondaleManiglia.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 68);
            //        GFX.DrawString("maniglia", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 67);

            //    }
            //    else//Alessio
            //    {
            //        GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, (luceLperEtich - 30).ToString("0000")), 10), PartenzaBarcodeDX, 21);
            //        GFX.DrawString((luceLperEtich - 30).ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 31);
            //        GFX.DrawString("L", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 30);
            //    }

            //    GFX.DrawString("NOTE: " + NoteCMBBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 83);
            //    GFX.DrawString("Rif " + RifTXBX.Text, new Font("thaoma", 8), Brushes.Black, 220, 83);

            //    pictureBox1.Image = drawingsurface;

            //    GFX2.Clear(Color.White);
            //    GFX2.DrawString(Alias.Substring(0, Alias.Length > 35 ? 35 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 2);
            //    GFX2.DrawString(Nomezanzariera, new Font("thaoma", 8), Brushes.Black, 215, 2);
            //    GFX2.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
            //    GFX2.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 35);
            //    GFX2.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 75, 35);

            //    GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioGuida.ToString("0000")), 10), PartenzaBarcodeDX, 21);
            //    GFX2.DrawString(mixTaglioGuida.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 29);
            //    GFX2.DrawString("guide", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 29);

            //    GFX2.DrawString("NOTE: " + NoteCMBBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 49);
            //    GFX2.DrawString("Rif " + RifTXBX.Text, new Font("thaoma", 8), Brushes.Black, 220, 49);

            //    pictureBox2.Image = drawingsurface2;

            //    GFX3.Clear(Color.White);
            //    GFX3.DrawString(Alias.Substring(0, Alias.Length > 31 ? 31 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 2);
            //    GFX3.DrawString(Nomezanzariera, new Font("thaoma", 8), Brushes.Black, 215, 2);
            //    GFX3.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 20);
            //    GFX3.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 35);
            //    GFX3.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 75, 35);

            //    GFX3.DrawImage(barcode.Draw(string.Format(barcodeStruct, luceLperEtich.ToString("0000")), 10), PartenzaBarcodeDX, 21);
            //    GFX3.DrawString("(" + mixTeloFinita + ")rete", new Font("thaoma", 8), Brushes.Black, 213, 35);
            //    if (Supplemento2CMBX.Text != "")
            //    {
            //        GFX3.FillRectangle(Brushes.Gray, 210, 50, 85, 16);
            //        GFX3.DrawString(Supplemento2CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.White, 213, 50);
            //    }
            //    else GFX3.FillRectangle(Brushes.White, 0, 0, 0, 0);

            //    GFX3.DrawString("NOTE: " + NoteCMBBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 83);
            //    GFX3.DrawString("Rif " + RifTXBX.Text, new Font("thaoma", 8), Brushes.Black, 220, 83);

            //    pictureBox3.Image = drawingsurface3;
            Aggiorna();
        }
        private void ZanzariereACatenaDoppiaG()
        {
            Aggiorna();
        }
        private void ZanzariereLaterale(float Quotarete, string Nomezanzariera)
        {
            Aggiorna();
        }
        private void Flexa()
        {
            Aggiorna();
        }
        private void Jolly()
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }
            //if (flagForCMBXitems == false)
            //{
            //    Email_A_fornitori_Intestazione("");

            //    VersioneCMBX.Visible = true;
            //    VersioneLBL.Visible = true;
            //    VersioneCMBX.Items.Clear();
            //    VersioneCMBX.Items.Add("Apertura asimmetrica");
            //    VersioneCMBX.Items.Add("Apertura centrale");

            //    LuceRBN.Visible = false;
            //    FinitaRBN.Visible = false;
            //    Varie1TCMBX.Visible = true;
            //    Varie1_label.Visible = true;
            //    Varie1TCMBX.DropDownStyle = ComboBoxStyle.DropDownList;
            //    Varie1_label.Text = "Mix";
            //    Varie1TCMBX.Items.Add("Luce");
            //    Varie1TCMBX.Items.Add("Finita");
            //    Varie1TCMBX.Items.Add("Interno guida (-2mm)");
            //    Varie1TCMBX.Items.Add("Esterno guida (+65mm)");
            //    Varie1TCMBX.Items.Add("Guida sovrapposta (-15mm)");
            //    Varie1TCMBX.Items.Add("Double Face 50 (-28mm)");
            //    Varie1TCMBX.Items.Add("Double Face 40x48 (-15mm)");
            //    Varie1TCMBX.Text = "Finita";

            //    flagForCMBXitems = true;
            //}

            //Elabora_luceLH_ZanzariereIncassate();
            //NoteArtVarieLabel.Text = "";

            //if (VersioneCMBX.Text == "Apertura asimmetrica")
            //{
            //    L2_label.Visible = true;
            //    L2_TXBX.Visible = true;
            //}
            //else
            //{
            //    L2_label.Visible = false;
            //    L2_TXBX.Visible = false;
            //    L2_TXBX.Text = "";
            //}

            //if (L_TXBX.Text != "" && H_TXBX.Text != "")
            //{
            //    luceLperEtich = int.Parse(L_TXBX.Text) + luceL;
            //    luceHperEtich = int.Parse(H_TXBX.Text) + luceH;

            //    mixTaglioProfiloInferiore = luceLperEtich - 12;//guida a terra
            //                                                   //mixTaglioFondaleManiglia = Math.Round((luceHperEtich - 11) * 10, 1);
            //    mixTagliocompensatore = luceLperEtich - 5;


            //    cordino1String = ((luceHperEtich * 2) + 600).ToString();//Filo 8//da verificare
            //    cordino1String_Page2 = cordino1String;//Filo 8


            //    if (VersioneCMBX.Text == "")//Normale a 1 anta
            //    {
            //        articoloDB = articoliCB.Text.Replace("'", "''");

            //        if (luceHperEtich > 2800) NoteArtVarieLabel.Text = "Altezza non realizzabile!";
            //        if (luceLperEtich > 1600) NoteArtVarieLabel.Text = "Larghezza non realizzabile!";
            //        if (luceHperEtich < luceLperEtich + 150) NoteArtVarieLabel.Text = "JOLLY non realizzabile!";

            //        cordino1String = ((luceHperEtich * 2) + 600).ToString();//Filo 8
            //        nAgganciCingoliPannelli = Math.Round((luceLperEtich * 71) / 1550d).ToString();//cingoli
            //        //sostituzioneCingoliAntivento =;
            //        cordino2String = (luceLperEtich + 80).ToString();//lamina //0.00 serve a non far mangiarsi l'ultimo zero

            //        if (int.Parse(L_TXBX.Text) <= 800) calcoloString = "34";
            //        else if (int.Parse(L_TXBX.Text) <= 1200) calcoloString = "35";
            //        else calcoloString = "36";

            //        //888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //        Calcolo_QuantitaMQ(Varie1TCMBX.Text.Contains("+") ? (int)luceLperEtich : int.Parse(L_TXBX.Text), int.Parse(H_TXBX.Text), Fatt_min_Tot);
            //        //888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //    }
            //    else if (VersioneCMBX.Text == "Apertura asimmetrica")
            //    {
            //        articoloDB = articoliCB.Text.Replace("'", "''")
            //        + Environment.NewLine + "  2A Ap.Asimmetrica ";// + luceLperEtich + "x" + luceHperEtich ;//L: questa combinazione serve a quando si converte da preventivo a ordine 



            //        if (L2_TXBX.Text != "")
            //        {
            //            luceLperEtich_Page1 = (int)Math.Round(int.Parse(L2_TXBX.Text) + (luceL / 2d));//verificare
            //            luceLperEtich_Page2 = (int)Math.Round(int.Parse(L_TXBX.Text) + (luceL / 2d) - int.Parse(L2_TXBX.Text));
            //            if ((int.Parse(L2_TXBX.Text) + (luceL / 2)) >= (int.Parse(L_TXBX.Text) + (luceL / 2)))
            //            {
            //                MessageBox.Show("La larghezza di 1 Anta non può essere uguale o maggiore della larghezza totale!", "P_Sun", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                L2_TXBX.Text = "";
            //                L2_TXBX.Focus();
            //                return;
            //            }

            //            if (luceHperEtich > 2800) NoteArtVarieLabel.Text = "Altezza non realizzabile!";
            //            if (luceLperEtich_Page1 > 1600 || luceLperEtich_Page2 > 1600) NoteArtVarieLabel.Text = "Larghezza non realizzabile!";
            //            if (luceHperEtich < luceLperEtich_Page1 + 150 || luceHperEtich < luceLperEtich_Page2 + 150) NoteArtVarieLabel.Text = "jolly non realizzabile!";

            //            nAgganciCingoliPannelli = Math.Round(luceLperEtich_Page1 * 71 / 1550d).ToString();//cingoli
            //            nAgganciCingoliPannelli_Page2 = Math.Round(luceLperEtich_Page2 * 71 / 1550d).ToString();//cingoli
            //            cordino2String = (luceLperEtich_Page1 + 80).ToString();//lamine //0.00 serve a non far mangiarsi l'ultimo zero
            //            cordino2String_Page2 = (luceLperEtich_Page2 + 80).ToString();

            //            if (luceLperEtich_Page1 <= 800) calcoloString = "34";
            //            else if (luceLperEtich_Page1 <= 1200) calcoloString = "35";
            //            else calcoloString = "36";

            //            if (luceLperEtich_Page2 <= 800) calcoloString_Page2 = "34";
            //            else if (luceLperEtich_Page2 <= 1200) calcoloString_Page2 = "35";
            //            else calcoloString_Page2 = "36";

            //            //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //            Calcolo_QuantitaMQ(Varie1TCMBX.Text.Contains("+") ? (int)luceLperEtich : int.Parse(L_TXBX.Text), int.Parse(H_TXBX.Text), Fatt_min_Tot * 2);
            //            //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888

            //        }
            //        else
            //        {
            //            mixTaglioCassonetto = 0;
            //            mixTaglioProfiloInferiore = 0;
            //            mixTagliocompensatore = 0;
            //            mixTaglioFondaleManiglia = 0;
            //            cordino1String = "0";//mix FILO 8
            //            cordino2String = "0";//Lamina
            //            cordino1String_Page2 = "0";//mix FILO 8
            //            cordino2String_Page2 = "0";//Lamina
            //            nAgganciCingoliPannelli = "0";//cingoli
            //            nAgganciCingoliPannelli_Page2 = "0";
            //            calcoloString = "";//giri Molla
            //            calcoloString_Page2 = "";//giri Molla
            //            luceLperEtich_Page1 = 0;
            //            luceLperEtich_Page2 = 0;
            //            luceLperEtich = 0;
            //            luceHperEtich = 0;
            //            QuantitaTXBX.Text = "";
            //        }
            //    }
            //    else if (VersioneCMBX.Text == "Apertura centrale")
            //    {

            //        int resto = (int.Parse(L_TXBX.Text) + luceL) % 2;

            //        luceLperEtich_Page1 = (int)Math.Round((int.Parse(L_TXBX.Text) + luceL) / 2d);
            //        luceLperEtich_Page2 = luceLperEtich_Page1 - resto;

            //        articoloDB = articoliCB.Text.Replace("'", "''")
            //        + Environment.NewLine + "  2A Ap.Centrale ";// + luceLperEtich + "x" + luceHperEtich ;//L: questa combinazione serve a quando si converte da preventivo a ordine 

            //        if (luceHperEtich > 2800) NoteArtVarieLabel.Text = "Altezza non realizzabile!";
            //        if (luceLperEtich_Page1 > 1600) NoteArtVarieLabel.Text = "Larghezza non realizzabile!";
            //        if (luceHperEtich < luceLperEtich_Page1 + 150) NoteArtVarieLabel.Text = "JOLLY non realizzabile!";

            //        nAgganciCingoliPannelli = Math.Round(luceLperEtich_Page1 * 71 / 1550d).ToString();//cingoli
            //        nAgganciCingoliPannelli_Page2 = nAgganciCingoliPannelli;
            //        cordino2String = (luceLperEtich_Page1 + 80).ToString();//lamine //0.00 serve a non far mangiarsi l'ultimo zero
            //        cordino2String_Page2 = cordino2String;

            //        if (luceLperEtich_Page1 <= 800) calcoloString = "34";
            //        else if (luceLperEtich_Page1 <= 1200) calcoloString = "35";
            //        else calcoloString = "36";
            //        calcoloString_Page2 = calcoloString;

            //        //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //        Calcolo_QuantitaMQ(Varie1TCMBX.Text.Contains("+") ? (int)luceLperEtich : int.Parse(L_TXBX.Text), int.Parse(H_TXBX.Text), Fatt_min_Tot * 2);
            //        //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888

            //    }
            //    else
            //    {
            //        MessageBox.Show("Stringa in versione non corretta", "P_Sun", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        VersioneCMBX.Text = "";
            //        VersioneCMBX.Focus();
            //    }
            //    //88888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //    Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);
            //    //88888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //}
            //else
            //{
            //    mixTaglioCassonetto = 0;
            //    mixTaglioProfiloInferiore = 0;
            //    mixTagliocompensatore = 0;
            //    mixTaglioFondaleManiglia = 0;
            //    cordino1String = "0";//mix FILO 8
            //    cordino2String = "0";//Lamina
            //    cordino1String_Page2 = "0";//mix FILO 8
            //    cordino2String_Page2 = "0";//Lamina
            //    nAgganciCingoliPannelli = "0";//cingoli
            //    nAgganciCingoliPannelli_Page2 = "0";
            //    calcoloString = "";//giri Molla
            //    calcoloString_Page2 = "";//giri Molla
            //    luceLperEtich_Page1 = 0;
            //    luceLperEtich_Page2 = 0;
            //    luceLperEtich = 0;
            //    luceHperEtich = 0;
            //    QuantitaTXBX.Text = "";
            //}

            //if (VersioneCMBX.Text != "")
            //{
            //    GFX.Clear(Color.White);
            //    GFX.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 3);
            //    GFX.DrawString(DD1[0] + " 2A", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 215, 3);
            //    GFX.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
            //    GFX.DrawString("L " + luceLperEtich_Page1 + "   (Ltot" + luceLperEtich + ")", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
            //    GFX.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 125, 40);
            //    GFX.DrawString(DateTime.Now.ToString("dd-MM-yyyy"), new Font("thaoma", 7), Brushes.Black, 5, 60);

            //    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, luceHperEtich.ToString("0000")), 10), PartenzaBarcodeDX, 21);
            //    GFX.DrawString(luceHperEtich.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 32);//26
            //    GFX.DrawString("H", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 31);    //  25 
            //    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioProfiloInferiore.ToString("0000")), 10), PartenzaBarcodeDX, 55);
            //    GFX.DrawString(mixTaglioProfiloInferiore.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 67);
            //    GFX.DrawString("G. a terra", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 66);

            //    GFX.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
            //    GFX.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);

            //    pictureBox1.Image = drawingsurface;
            //    GFX2.Clear(Color.White);
            //    GFX2.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 0);
            //    GFX2.DrawString(DD1[0] + " 2A", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 215, 0);
            //    GFX2.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
            //    GFX2.DrawString("L " + luceLperEtich_Page1 + "   (Ltot" + luceLperEtich + ")", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
            //    GFX2.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 125, 40);

            //    GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTagliocompensatore.ToString("0000")), 10), PartenzaBarcodeDX, 21);
            //    GFX2.DrawString(mixTagliocompensatore.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 31);
            //    GFX2.DrawString("compens.", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 30);
            //    //GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioFondaleManiglia.ToString("0000")), 10), PartenzaBarcodeDX, 55);
            //    //GFX2.DrawString(mixTaglioFondaleManiglia.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX+70, 67);
            //    //GFX2.DrawString("M.manig", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX+14, 66);

            //    GFX2.DrawRectangle(pen1, 7, 54, 305, 25);
            //    GFX2.DrawString("(" + cordino1String + ")Filo 8", new Font("thaoma", 8), Brushes.Black, 9, 54);
            //    GFX2.DrawString("(" + cordino2String + ")Lamina", new Font("thaoma", 8), Brushes.Black, 9, 66);
            //    GFX2.DrawString("(" + nAgganciCingoliPannelli + ")cingoli", new Font("thaoma", 8), Brushes.Black, 123, 54);
            //    GFX2.DrawString("(2)antivento sostit. ogni" + " " + Math.Round(double.Parse(nAgganciCingoliPannelli) / 3), new Font("thaoma", 8), Brushes.Black, 180, 54);
            //    GFX2.DrawString("(" + calcoloString + ")giri Molla", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 123, 66);//

            //    GFX2.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
            //    GFX2.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);

            //    pictureBox2.Image = drawingsurface2;
            //    GFX4.Clear(Color.White);
            //    GFX4.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 3);
            //    GFX4.DrawString(DD1[0] + " 2A", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 215, 3);
            //    GFX4.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
            //    GFX4.DrawString("L " + luceLperEtich_Page2 + "   (Ltot" + luceLperEtich + ")", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
            //    GFX4.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 125, 40);
            //    GFX4.DrawString(DateTime.Now.ToString("dd-MM-yyyy"), new Font("thaoma", 7), Brushes.Black, 5, 60);

            //    GFX4.DrawImage(barcode.Draw(string.Format(barcodeStruct, luceHperEtich.ToString("0000")), 10), PartenzaBarcodeDX, 21);
            //    GFX4.DrawString(luceHperEtich.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 32);//26
            //    GFX4.DrawString("H", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 31);    //  25 
            //    //GFX4.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioProfiloInferiore.ToString("0000")), 10), PartenzaBarcodeDX, 55);
            //    //GFX4.DrawString(mixTaglioProfiloInferiore.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 67);
            //    //GFX4.DrawString("G. a terra", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 66);

            //    GFX4.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
            //    GFX4.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);

            //    pictureBox4.Image = drawingsurface4;
            //    GFX5.Clear(Color.White);
            //    GFX5.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 0);
            //    GFX5.DrawString(DD1[0] + " 2A", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 215, 0);
            //    GFX5.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
            //    GFX5.DrawString("L " + luceLperEtich_Page2 + "   (Ltot" + luceLperEtich + ")", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
            //    GFX5.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 125, 40);

            //    GFX5.DrawRectangle(pen1, 7, 54, 305, 25);
            //    GFX5.DrawString("(" + cordino1String_Page2 + ")Filo 8", new Font("thaoma", 8), Brushes.Black, 9, 54);
            //    GFX5.DrawString("(" + cordino2String_Page2 + ")Lamina", new Font("thaoma", 8), Brushes.Black, 9, 66);
            //    GFX5.DrawString("(" + nAgganciCingoliPannelli_Page2 + ")cingoli", new Font("thaoma", 8), Brushes.Black, 123, 54);
            //    GFX5.DrawString("(2)antivento sostit. ogni" + " " + Math.Round(double.Parse(nAgganciCingoliPannelli_Page2) / 3), new Font("thaoma", 8), Brushes.Black, 180, 54);
            //    GFX5.DrawString("(" + calcoloString_Page2 + ")giri Molla", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 123, 66);

            //    GFX5.DrawString("NOTE " + NoteCMBBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 83);
            //    GFX5.DrawString("Rif " + RifTXBX.Text, new Font("thaoma", 8), Brushes.Black, 220, 83);

            //    pictureBox5.Image = drawingsurface5;
            //}
            //else
            //{
            //    GFX.Clear(Color.White);
            //    GFX.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 3);
            //    GFX.DrawString(DD1[0], new Font("thaoma", 8), Brushes.Black, 215, 3);
            //    GFX.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
            //    GFX.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
            //    GFX.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 75, 40);
            //    GFX.DrawString(DateTime.Now.ToString("dd-MM-yyyy"), new Font("thaoma", 7), Brushes.Black, 5, 60);

            //    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, luceHperEtich.ToString("0000")), 10), PartenzaBarcodeDX, 21);
            //    GFX.DrawString(luceHperEtich.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 32);//26
            //    GFX.DrawString("H", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 31);//  25 
            //    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioProfiloInferiore.ToString("0000")), 10), PartenzaBarcodeDX, 55);
            //    GFX.DrawString(mixTaglioProfiloInferiore.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 67);
            //    GFX.DrawString("G. a terra", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 66);

            //    GFX.DrawString("NOTE " + NoteCMBBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 83);
            //    GFX.DrawString("Rif " + RifTXBX.Text, new Font("thaoma", 8), Brushes.Black, 220, 83);

            //    pictureBox1.Image = drawingsurface;
            //    GFX2.Clear(Color.White);
            //    GFX2.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 0);
            //    GFX2.DrawString(DD1[0], new Font("thaoma", 8), Brushes.Black, 215, 0);
            //    GFX2.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
            //    GFX2.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
            //    GFX2.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 75, 40);

            //    GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTagliocompensatore.ToString("0000")), 10), PartenzaBarcodeDX, 21);
            //    GFX2.DrawString(mixTagliocompensatore.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 32);
            //    GFX2.DrawString("compens.", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 31);
            //    // GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioFondaleManiglia.ToString("0000")), 10), PartenzaBarcodeDX, 55);
            //    //GFX2.DrawString(mixTaglioFondaleManiglia.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX+70, 67);
            //    //GFX2.DrawString("B.manig", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX+14, 66);
            //    GFX2.DrawRectangle(pen1, 7, 54, 305, 25);

            //    GFX2.DrawString("(" + cordino1String + ")Filo 8", new Font("thaoma", 8), Brushes.Black, 9, 54);
            //    GFX2.DrawString("(" + cordino2String + ")Lamine", new Font("thaoma", 8), Brushes.Black, 9, 66);
            //    GFX2.DrawString("(" + nAgganciCingoliPannelli + ")cingoli", new Font("thaoma", 8), Brushes.Black, 123, 54);
            //    GFX2.DrawString("(2)antivento sostit. ogni" + " " + Math.Round(double.Parse(nAgganciCingoliPannelli) / 3), new Font("thaoma", 8), Brushes.Black, 180, 54);
            //    GFX2.DrawString("(" + calcoloString + ")giri Molla", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 123, 66);
            //    GFX2.DrawString("NOTE " + NoteCMBBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 83);
            //    GFX2.DrawString("Rif " + RifTXBX.Text, new Font("thaoma", 8), Brushes.Black, 220, 83);

            //    pictureBox2.Image = drawingsurface2;

            //    pictureBox4.Image = null;
            //    pictureBox5.Image = null;
            //}
            Aggiorna();
        }
        private void SceniPro()
        {
            Aggiorna();
        }
        private void Plisse(bool listinoAmodulo)
        {
            Aggiorna();
        }
        private void ZanzarieraAnta()
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaZanzarieraAnta = new EtichettaZanzarieraAnta(etichetta))
            {
                GraphicsDrawable1 = EtichettaZanzarieraAnta;
                DrawingSurface1 = EtichettaZanzarieraAnta.ToBitmap();
            }
            //if (LuceRBN.Checked == true)
            //{
            //    luceL = -3;
            //    luceH = -3;
            //}
            //else
            //{
            //    luceL = 0;
            //    luceH = 0;
            //}
            //articoloDB = articoliCB.Text.Replace("'", "''");
            //if (flagForCMBXitems == false)
            //{
            //    Email_A_fornitori_Intestazione("");

            //    Colore_label.Visible = true;
            //    ColoreCMBX.Visible = true;

            //    ColoreCMBX.Focus();
            //    L_label.Visible = true;
            //    L_TXBX.Visible = true;
            //    H_label.Visible = true;
            //    H_TXBX.Visible = true;

            //    VersioneCMBX.Visible = true;
            //    VersioneLBL.Visible = true;
            //    VersioneCMBX.Items.Clear();

            //    VersioneCMBX.Items.Add("1 anta");
            //    VersioneCMBX.Items.Add("2 ante");
            //    VersioneCMBX.Items.Add("3 ante");
            //    VersioneCMBX.Items.Add("4 ante");
            //    VersioneCMBX.Items.Add("2 ante Apertura asimmetrica");
            //    VersioneCMBX.Text = "1 anta";
            //    VersioneCMBX.DropDownStyle = ComboBoxStyle.DropDownList;

            //    Comandi_label.Visible = true;
            //    ComandiCMBX.Visible = true;
            //    Comandi_label.Text = "Com";
            //    ComandiCMBX.Items.Add("Spingere DX");
            //    ComandiCMBX.Items.Add("Spingere SX");
            //    ComandiCMBX.Text = "Spingere DX";
            //    ComandiCMBX.DropDownStyle = ComboBoxStyle.DropDownList;

            //    Supplemento1CMBX.Items.Add("Rete acciaio Sotto");
            //    Supplemento1CMBX.Items.Add("Rete acciaio Sopra e Sotto");
            //    Supplemento1CMBX.Items.Add("Rete pet screen Sotto");
            //    Supplemento1CMBX.Items.Add("Rete pet screen Sopra e Sotto");
            //    Supplemento1CMBX.Items.Add("Bachelite Sotto");
            //    Supplemento1CMBX.Items.Add("Bachelite Sopra e Sotto");
            //    Supplemento1CMBX.Items.Add("Plexiglas Sotto");
            //    Supplemento1CMBX.Items.Add("Plexiglas Sopra e Sotto");
            //    NoteCMBBX.Text = "TEL Z ";
            //    NoteCMBBX.Items.Add("TEL Z ");
            //    NoteCMBBX.Items.Add("TEL L ");
            //    flagForCMBXitems = true;

            //}

            //if (VersioneCMBX.Text == "1 anta") //*********************************************************1 Anta************************1****************
            //{
            //    Fatt_min_Tot = 2;
            //    L2_label.Visible = false;
            //    L2_TXBX.Visible = false;
            //    L2_TXBX.Text = "";
            //    NoteArtVarieLabel.Text = "";
            //}
            //else if (VersioneCMBX.Text == "2 ante") //*********************************************************2 Ante************************2****************
            //{
            //    Fatt_min_Tot = 3;
            //    L2_label.Visible = false;
            //    L2_TXBX.Visible = false;
            //    NoteArtVarieLabel.Text = "";
            //}
            //else if (VersioneCMBX.Text == "2 ante Apertura asimmetrica") //*********************************************************2 Ante Asimmetrica************************2****************
            //{
            //    Fatt_min_Tot = 3;
            //    L2_label.Visible = true;
            //    L2_TXBX.Visible = true;
            //    NoteArtVarieLabel.Text = "L2 = anta piu' piccola";
            //}
            //else if (VersioneCMBX.Text == "3 ante") //*********************************************************3 ante************************3****************
            //{
            //    Fatt_min_Tot = 4;
            //    L2_label.Visible = false;
            //    L2_TXBX.Visible = false;
            //    L2_TXBX.Text = "";
            //    NoteArtVarieLabel.Text = "";
            //}
            //else if (VersioneCMBX.Text == "4 ante") //*********************************************************4 Ante************************4****************
            //{
            //    Fatt_min_Tot = 5;
            //    L2_label.Visible = false;
            //    L2_TXBX.Visible = false;
            //    L2_TXBX.Text = "";
            //    NoteArtVarieLabel.Text = "";
            //}
            //if (L_TXBX.Text != "" && H_TXBX.Text != "")
            //{
            //    luceLperEtich = int.Parse(L_TXBX.Text) + luceL;
            //    luceHperEtich = int.Parse(H_TXBX.Text) + luceH;
            //    //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //    Calcolo_QuantitaMQ(int.Parse(L_TXBX.Text), int.Parse(H_TXBX.Text), Fatt_min_Tot);
            //    Calcolo_supplemento1(int.Parse(VersioneCMBX.Text.Replace(" anta", "").Replace(" ante", "").Replace(" Apertura asimmetrica", "")) * int.Parse(PezziTXBX.Text), PezziTXBX.Text);
            //    Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);
            //    //88888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //}

            //GFX.Clear(Color.White);
            //GFX.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 3);
            //GFX.DrawString("Z.Anta", new Font("thaoma", 8), Brushes.Black, 215, 3);
            //GFX.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 2, 22);
            //GFX.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
            //GFX.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 75, 40);
            //GFX.DrawString(ComandiCMBX.Text, new Font("thaoma", 8), Brushes.Black, 200, 35);
            //GFX.DrawString("Versione " + VersioneCMBX.Text, new Font("thaoma", 8), Brushes.Black, 130, 52);

            //if (Supplemento1CMBX.Text != "")
            //{
            //    GFX.FillRectangle(Brushes.Gray, 5, 65, 160, 16);
            //    GFX.DrawString(Supplemento1CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.White, 5, 65);
            //}

            //GFX.DrawString("NOTE " + NoteCMBBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 83);
            //GFX.DrawString("Rif " + RifTXBX.Text, new Font("thaoma", 8), Brushes.Black, 220, 83);

            //pictureBox1.Image = drawingsurface;
            Aggiorna();
        }
        private void ZanzariereScorrevoli()
        {
            Aggiorna();
        }
        private void ZanzarieraGiocondaLaterale22mm()
        {
            Aggiorna();
        }
        private void ZanzarieraGiocondaReversibile22mm()
        {
            Aggiorna();
        }
        //Binario
        private void BinarioArricc()
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }
            //articoloDB = articoliCB.Text.Replace("'", "''");//curva minima compreso guide 18

            //if (AperturaCentraleCKBX.Checked == true) varie2 = "Apert. centrale";
            //else varie2 = "";

            //if (flagForCMBXitems == false)
            //{
            //    ColoreCMBX.Visible = false;
            //    Colore_label.Visible = false;
            //    VersioneCMBX.Visible = true;
            //    VersioneCMBX.Items.Clear();
            //    VersioneCMBX.Items.Add("___________");
            //    VersioneCMBX.Items.Add("(________)");
            //    VersioneCMBX.Items.Add("(________");
            //    VersioneCMBX.Items.Add(" _________)");
            //    VersioneCMBX.Items.Add("|_______|");
            //    VersioneCMBX.Items.Add("________|");
            //    VersioneCMBX.Items.Add("|________");
            //    VersioneLBL.Visible = true;
            //    // Varie1TCMBX.DropDownStyle = ComboBoxStyle.DropDownList;

            //    Hc_label.Visible = true;
            //    HcTXBX.Visible = true;
            //    AperturaCentraleCKBX.Visible = true;
            //    Comandi_label.Visible = true;
            //    ComandiCMBX.Visible = true;
            //    Comandi_label.Text = "Comando";
            //    ComandiCMBX.Items.Add("DX");
            //    ComandiCMBX.Items.Add("SX");
            //    ComandiCMBX.Text = "DX";

            //    flagForCMBXitems = true;
            //    VersioneCMBX.Text = "___________";
            //    VersioneCMBX.Focus();
            //}

            //switch (VersioneCMBX.Text)
            //{
            //    case "___________":

            //        H_label.Visible = false;
            //        H_TXBX.Visible = false;
            //        H_TXBX.Text = "";
            //        H2_TXBX.Text = "";
            //        H2_TXBX.Visible = false;
            //        H2_label.Visible = false;
            //        if (!NoteCMBBX.ContainsFocus)
            //        {
            //            NoteCMBBX.Items.Remove("accoppiata (+35mm)");
            //            NoteCMBBX.Items.Add("accoppiata (+35mm)");
            //        }
            //        NoteArtVarieLabel.Text = "";

            //        if (L_TXBX.Text != "")//Calcolo quantita'
            //        {
            //            QuantitaTXBX.Text = Math.Round(Math.Max(int.Parse(L_TXBX.Text) / 1000f, Fatt_min_Tot) * int.Parse(PezziTXBX.Text), 2).ToString();

            //            if (AperturaCentraleCKBX.Checked == true) calcoloString = "*" + Math.Ceiling(int.Parse(L_TXBX.Text) / 1000d * 11 / 2) + "**" + Math.Ceiling(int.Parse(L_TXBX.Text) / 1000d * 11 / 2) + "*";//verificare
            //            else calcoloString = "*" + Math.Ceiling(int.Parse(L_TXBX.Text) / 1000d * 11) + "*";

            //            mixTaglioXetich = (int.Parse(L_TXBX.Text) - 122).ToString();
            //            mixTaglioBinario = int.Parse(L_TXBX.Text) - 122;
            //        }
            //        else { QuantitaTXBX.Text = ""; calcoloString = ""; mixTaglioBinario = 0; }

            //        if ((HcTXBX.Text != "") && (HcTXBX.Text != "0") && (L_TXBX.Text != "") && int.TryParse(HcTXBX.Text, out _))
            //        {
            //            cordino1String = (((int.Parse(HcTXBX.Text) * 2) + (int.Parse(L_TXBX.Text) * 2) - 80) / 1000f).ToString();
            //        }
            //        else cordino1String = "???";

            //        break;

            //    case "(________)":


            //        if (VersioneCMBX.TextHasChanged == true)
            //        {
            //            H_label.Visible = true;
            //            H_TXBX.Visible = true;
            //            H2_label.Visible = true;
            //            H2_TXBX.Visible = true;
            //            H2_TXBX.Text = "225";
            //            H_TXBX.Text = "225";
            //            H_label.Text = "SpSX";
            //            NoteArtVarieLabel.Text = "curva minima compreso guide 180mm//con mensole max 210mm";
            //            NoteCMBBX.Items.Clear();
            //            if (NoteCMBBX.Text == "accoppiata (+35mm)") NoteCMBBX.Text = "";
            //        }

            //        if (L_TXBX.Text != "" && H_TXBX.Text != "" && H2_TXBX.Text != "")//////////////Calcolo quantita'//////////////////////
            //        {
            //            QuantitaTXBX.Text = Math.Round(Math.Max((int.Parse(L_TXBX.Text) + int.Parse(H_TXBX.Text) + int.Parse(H2_TXBX.Text)) / 1000f, Fatt_min_Tot) * int.Parse(PezziTXBX.Text), 2).ToString();

            //            int Hmin = 225, H2min = 225;
            //            string MixTaglioCurvaSxXetich = "", MixTaglioCurvaDxXetich = "";

            //            if (int.Parse(H_TXBX.Text) > 225)
            //            {
            //                Hmin = int.Parse(H_TXBX.Text);
            //                if (int.Parse(H_TXBX.Text) != 225) MixTaglioCurvaSxXetich = "[Usc." + (int.Parse(H_TXBX.Text) - 225) + "]";
            //            }
            //            else if (int.Parse(H_TXBX.Text) != 225) MixTaglioCurvaSxXetich = "[Tagl." + (int.Parse(H_TXBX.Text) - 225) + "]";

            //            if (float.Parse(H2_TXBX.Text) > 225)
            //            {
            //                H2min = int.Parse(H2_TXBX.Text);
            //                if (int.Parse(H2_TXBX.Text) != 225) MixTaglioCurvaDxXetich = "[Usc." + (int.Parse(H2_TXBX.Text) - 225) + "]";
            //            }
            //            else if (int.Parse(H2_TXBX.Text) != 225) MixTaglioCurvaDxXetich = "[Tagl." + (int.Parse(H2_TXBX.Text) - 225) + "]";

            //            calcoloInt = Hmin + H2min - 260;//19
            //            mixTaglioXetich = MixTaglioCurvaSxXetich + (int.Parse(L_TXBX.Text) + calcoloInt).ToString() + MixTaglioCurvaDxXetich;//verificare
            //            mixTaglioBinario = int.Parse(L_TXBX.Text) + calcoloInt;//verificare
            //            if (AperturaCentraleCKBX.Checked)
            //            {
            //                calcoloString = "*" + Math.Ceiling((int.Parse(L_TXBX.Text) + int.Parse(H_TXBX.Text) + int.Parse(H2_TXBX.Text) - 190) / 1000f * 11 / 2) + "**" + Math.Ceiling((int.Parse(L_TXBX.Text) + int.Parse(H_TXBX.Text) + int.Parse(H2_TXBX.Text) - 190) / 1000f * 11 / 2) + "*";
            //            }
            //            else calcoloString = "*" + Math.Round((int.Parse(L_TXBX.Text) + int.Parse(H_TXBX.Text) + int.Parse(H2_TXBX.Text) - 190) / 1000f * 11) + "*";

            //            if ((HcTXBX.Text != "") & (HcTXBX.Text != "0") & (int.TryParse(HcTXBX.Text, out _)))
            //            {
            //                cordino1String = Math.Round(((int.Parse(HcTXBX.Text) * 2) + (int.Parse(L_TXBX.Text) * 2) + (int.Parse(H_TXBX.Text) * 2) + (int.Parse(H2_TXBX.Text) * 2) - 140) / 1000f, 2).ToString();
            //            }
            //            else cordino1String = "???";
            //        }
            //        else { QuantitaTXBX.Text = ""; calcoloString = ""; mixTaglioXetich = ""; cordino1String = "???"; mixTaglioBinario = 0; }

            //        break;

            //    case "|_______|":

            //        if (VersioneCMBX.TextHasChanged == true)
            //        {
            //            H_label.Visible = true;
            //            H_TXBX.Visible = true;
            //            H2_TXBX.Visible = true;
            //            H2_label.Visible = true;
            //            H_label.Text = "SpSX";
            //            H_TXBX.Text = "170";
            //            H2_TXBX.Text = "170";
            //            NoteArtVarieLabel.Text = "";
            //            NoteCMBBX.Items.Clear();
            //            if (NoteCMBBX.Text == "accoppiata (+35mm)") NoteCMBBX.Text = "";

            //        }
            //        if (L_TXBX.Text != "" && H_TXBX.Text != "" && H2_TXBX.Text != "")//////////////Calcolo quantita'//////////////////////
            //        {
            //            QuantitaTXBX.Text = Math.Round(Math.Max((int.Parse(L_TXBX.Text) + int.Parse(H_TXBX.Text) + int.Parse(H2_TXBX.Text)) / 1000f, Fatt_min_Tot) * int.Parse(PezziTXBX.Text), 2).ToString();

            //            if (AperturaCentraleCKBX.Checked) calcoloString = "*" + Math.Round(int.Parse(H_TXBX.Text) / 1000d * 11) + "+*" + Math.Ceiling(int.Parse(L_TXBX.Text) / 1000d * 11 / 2) + "**" + Math.Ceiling(int.Parse(L_TXBX.Text) / 1000d * 11 / 2) + "*+" + Math.Round(int.Parse(H2_TXBX.Text) / 1000d * 11) + "*";
            //            else calcoloString = "*" + Math.Round(int.Parse(H_TXBX.Text) / 1000d * 11) + "+" + Math.Round(int.Parse(L_TXBX.Text) / 1000f * 11) + "+" + Math.Round(int.Parse(H2_TXBX.Text) / 1000d * 11) + "*";

            //            mixTaglioXetich = (int.Parse(H_TXBX.Text) - 22) + "+" + (int.Parse(L_TXBX.Text) - 122) + "+" + (int.Parse(H2_TXBX.Text) - 22);
            //            mixTaglioBinario = int.Parse(L_TXBX.Text) - 122;
            //        }
            //        else
            //        {
            //            QuantitaTXBX.Text = "";
            //            calcoloString = "";
            //            mixTaglioXetich = "";
            //            mixTaglioBinario = 0;
            //        }

            //        if ((HcTXBX.Text != "") && (HcTXBX.Text != "0") && (L_TXBX.Text != "") && int.TryParse(HcTXBX.Text, out _))
            //        {
            //            cordino1String = Math.Round(((int.Parse(HcTXBX.Text) * 2) + (int.Parse(L_TXBX.Text) * 2) - 80) / 1000f, 2).ToString();//verificare
            //        }
            //        else cordino1String = "???";

            //        break;

            //    case "(________"://-----------------------------------------------------------------------------------------------------
            //        if (VersioneCMBX.TextHasChanged == true)
            //        {
            //            H_label.Visible = true;
            //            H_TXBX.Visible = true;
            //            H2_TXBX.Text = "";
            //            H2_label.Visible = false;
            //            H2_TXBX.Visible = false;
            //            H_TXBX.Text = "225";
            //            NoteArtVarieLabel.Text = "curva minima compreso guide 180//con mensole max 210";
            //            NoteCMBBX.Items.Clear();
            //            NoteCMBBX.Items.Add("accoppiata (+35mm)");
            //        }

            //        if (L_TXBX.Text != "" && H_TXBX.Text != "")//Calcolo quantita'
            //        {
            //            QuantitaTXBX.Text = Math.Round(Math.Max((int.Parse(L_TXBX.Text) + int.Parse(H_TXBX.Text)) / 1000f, Fatt_min_Tot) * int.Parse(PezziTXBX.Text), 2).ToString();

            //            int Hmin = 225;
            //            string MixTaglioCurvaSxXetich = "";

            //            if (int.Parse(H_TXBX.Text) > 225)
            //            {
            //                Hmin = int.Parse(H_TXBX.Text);
            //                if (int.Parse(H_TXBX.Text) != 225) MixTaglioCurvaSxXetich = "[Usc." + (int.Parse(H_TXBX.Text) - 225) + "]";
            //            }
            //            else if (int.Parse(H_TXBX.Text) != 225) MixTaglioCurvaSxXetich = "[Tagl." + (int.Parse(H_TXBX.Text) - 225) + "]";

            //            calcoloInt = Hmin - 185;//4
            //            mixTaglioXetich = MixTaglioCurvaSxXetich + (int.Parse(L_TXBX.Text) + calcoloInt).ToString();//verificare

            //            if (AperturaCentraleCKBX.Checked) calcoloString = "*" + Math.Ceiling((int.Parse(L_TXBX.Text) + int.Parse(H_TXBX.Text) - 160) / 1000f * 11 / 2) + "**" + Math.Ceiling((((int.Parse(L_TXBX.Text) + int.Parse(H_TXBX.Text) - 160) / 1000f) * 11) / 2) + "*";
            //            else calcoloString = "*" + Math.Round((int.Parse(L_TXBX.Text) + int.Parse(H_TXBX.Text) - 160) / 1000f * 11) + "*";

            //            mixTaglioBinario = int.Parse(L_TXBX.Text) + calcoloInt;

            //            if ((HcTXBX.Text != "") & (HcTXBX.Text != "0") & (int.TryParse(HcTXBX.Text, out _)))
            //            {
            //                cordino1String = Math.Round(((int.Parse(HcTXBX.Text) * 2) + (int.Parse(L_TXBX.Text) * 2) + (int.Parse(H_TXBX.Text) * 2) - 100) / 1000f, 2).ToString();
            //            }
            //            else cordino1String = "???";

            //        }
            //        else
            //        {
            //            QuantitaTXBX.Text = "";
            //            calcoloString = "";
            //            mixTaglioXetich = "";
            //            cordino1String = "???";
            //            mixTaglioBinario = 0;
            //        }

            //        break;
            //    case "|________"://-----------------------------------------------------------------------------------------------------

            //        if (VersioneCMBX.TextHasChanged == true)
            //        {
            //            H_label.Visible = true;
            //            H_TXBX.Visible = true;
            //            H2_TXBX.Text = "";
            //            H2_label.Visible = false;
            //            H2_TXBX.Visible = false;
            //            H_TXBX.Text = "170";
            //            NoteArtVarieLabel.Text = "";
            //            NoteCMBBX.Items.Clear();
            //            NoteCMBBX.Items.Add("accoppiata (+35mm)");
            //        }
            //        if (L_TXBX.Text != "" && H_TXBX.Text != "")//Calcolo quantita'
            //        {
            //            QuantitaTXBX.Text = Math.Round(Math.Max((int.Parse(L_TXBX.Text) + int.Parse(H_TXBX.Text)) / 1000f, Fatt_min_Tot) * int.Parse(PezziTXBX.Text), 2).ToString();

            //            if (AperturaCentraleCKBX.Checked) calcoloString = "*" + Math.Round(int.Parse(H_TXBX.Text) / 1000d * 11d) + "+*" + Math.Ceiling(int.Parse(L_TXBX.Text) / 1000d * 11 / 2) + "**" + Math.Ceiling(int.Parse(L_TXBX.Text) / 1000d * 11 / 2) + "*";
            //            else calcoloString = "*" + Math.Round(int.Parse(H_TXBX.Text) / 1000d * 11) + "+" + Math.Round(int.Parse(L_TXBX.Text) / 1000d * 11);

            //            mixTaglioXetich = (int.Parse(H_TXBX.Text) - 22) + "+" + (int.Parse(L_TXBX.Text) - 122);
            //            mixTaglioBinario = int.Parse(L_TXBX.Text) - 122;

            //        }
            //        else { QuantitaTXBX.Text = ""; calcoloString = ""; mixTaglioXetich = ""; mixTaglioBinario = 0; }

            //        if ((HcTXBX.Text != "") && (HcTXBX.Text != "0") && (L_TXBX.Text != "") && float.TryParse(HcTXBX.Text, out _))
            //        {
            //            cordino1String = Math.Round(((int.Parse(HcTXBX.Text) * 2) + (int.Parse(L_TXBX.Text) * 2) - 80) / 1000f, 2).ToString();
            //        }
            //        else cordino1String = "???";

            //        break;
            //    case "________|"://--------------------------------------------------------------------------------------------------------
            //        if (VersioneCMBX.TextHasChanged == true)
            //        {
            //            H_label.Visible = false;
            //            H_TXBX.Visible = false;
            //            H_TXBX.Text = "";
            //            H2_label.Visible = true;
            //            H2_TXBX.Visible = true;
            //            H2_TXBX.Text = "170";
            //            NoteArtVarieLabel.Text = "";
            //            NoteCMBBX.Items.Clear();
            //            NoteCMBBX.Items.Add("accoppiata (+35mm)");
            //        }


            //        if (L_TXBX.Text != "" && H2_TXBX.Text != "")//Calcolo quantita'
            //        {
            //            QuantitaTXBX.Text = Math.Round(Math.Max((int.Parse(L_TXBX.Text) + int.Parse(H2_TXBX.Text)) / 1000f, Fatt_min_Tot) * int.Parse(PezziTXBX.Text), 2).ToString();

            //            if (AperturaCentraleCKBX.Checked) calcoloString = "*" + Math.Ceiling(int.Parse(L_TXBX.Text) / 1000d * 11 / 2) + "**" + Math.Ceiling(int.Parse(L_TXBX.Text) / 1000d * 11 / 2) + "*+" + Math.Round(int.Parse(H2_TXBX.Text) / 1000d * 11) + "*";
            //            else calcoloString = "*" + Math.Round(int.Parse(L_TXBX.Text) / 1000d * 11) + "+" + Math.Round(int.Parse(H2_TXBX.Text) / 1000d * 11) + "*";

            //            mixTaglioXetich = (int.Parse(L_TXBX.Text) - 122) + "+" + (int.Parse(H2_TXBX.Text) - 22);
            //            mixTaglioBinario = int.Parse(L_TXBX.Text) - 122;

            //        }
            //        else { QuantitaTXBX.Text = ""; calcoloString = ""; mixTaglioXetich = ""; mixTaglioBinario = 0; }

            //        if ((HcTXBX.Text != "") && (HcTXBX.Text != "0") && (L_TXBX.Text != "") && int.TryParse(HcTXBX.Text, out _))
            //        {
            //            cordino1String = Math.Round(((int.Parse(HcTXBX.Text) * 2) + (int.Parse(L_TXBX.Text) * 2) - 80) / 1000f, 2).ToString();
            //        }
            //        else cordino1String = "???";

            //        break;
            //    case " _________)"://----------------------------------------------------------------------------------------------------------

            //        if (VersioneCMBX.TextHasChanged == true)
            //        {
            //            H_label.Visible = false;
            //            H_TXBX.Visible = false;
            //            H_TXBX.Text = "";
            //            H2_label.Visible = true;
            //            H2_TXBX.Visible = true;
            //            H2_TXBX.Text = "225";
            //            NoteArtVarieLabel.Text = "curva minima compreso guide 180//con mensole max 210";
            //            NoteCMBBX.Items.Remove("accoppiata (+35mm)");
            //            NoteCMBBX.Items.Add("accoppiata (+35mm)");
            //        }

            //        if (L_TXBX.Text != "" && H2_TXBX.Text != "")//Calcolo quantita'
            //        {
            //            QuantitaTXBX.Text = Math.Round(Math.Max((int.Parse(L_TXBX.Text) + int.Parse(H2_TXBX.Text)) / 1000f, Fatt_min_Tot) * int.Parse(PezziTXBX.Text), 2).ToString();

            //            int H2min = 225;
            //            string MixTaglioCurvaDxXetich = "";

            //            if (int.Parse(H2_TXBX.Text) > 225)
            //            {
            //                if (int.Parse(H2_TXBX.Text) != 225) MixTaglioCurvaDxXetich = "[Usc." + (int.Parse(H2_TXBX.Text) - 225) + "]";
            //                H2min = int.Parse(H2_TXBX.Text);
            //            }
            //            else if (int.Parse(H2_TXBX.Text) != 225) MixTaglioCurvaDxXetich = "[Tagl." + (int.Parse(H2_TXBX.Text) - 225) + "]";

            //            calcoloInt = H2min - 185;
            //            mixTaglioXetich = (int.Parse(L_TXBX.Text) + calcoloInt).ToString() + MixTaglioCurvaDxXetich;
            //            mixTaglioBinario = (int.Parse(L_TXBX.Text) + calcoloInt);

            //            if (AperturaCentraleCKBX.Checked) calcoloString = "*" + Math.Ceiling((int.Parse(L_TXBX.Text) + int.Parse(H2_TXBX.Text) - 160) / 1000d * 11 / 2) + "**" + Math.Ceiling((int.Parse(L_TXBX.Text) + int.Parse(H2_TXBX.Text) - 160) / 1000d * 11 / 2) + "*";

            //            else calcoloString = "*" + Math.Round((int.Parse(L_TXBX.Text) + int.Parse(H2_TXBX.Text) - 160) / 1000d * 11) + "*";

            //            if ((HcTXBX.Text != "") & (HcTXBX.Text != "0") & (int.TryParse(HcTXBX.Text, out _)))
            //            {
            //                cordino1String = Math.Round(((int.Parse(HcTXBX.Text) * 2) + (int.Parse(L_TXBX.Text) * 2) + (int.Parse(H2_TXBX.Text) * 2) - 100) / 1000f, 2).ToString();
            //            }
            //            else cordino1String = "???";

            //        }
            //        else
            //        {
            //            QuantitaTXBX.Text = "";
            //            calcoloString = "";
            //            mixTaglioXetich = "";
            //            cordino1String = "???";
            //            mixTaglioBinario = 0;
            //        }
            //        break;
            //}
            //Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);

            //GFX.Clear(Color.White);
            //GFX.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 3);
            //GFX.DrawString("Binario Arr.a corda", new Font("thaoma", 8), Brushes.Black, 210, 3);
            //GFX.DrawString("L " + L_TXBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 16, 40);
            //GFX.DrawString(AperturaCentraleCKBX.Checked == true ? "Apertura Centrale" : "", new Font("thaoma", 8), Brushes.Black, 114, 22);
            //GFX.DrawString("COM " + ComandiCMBX.Text, new Font("thaoma", 8), Brushes.Black, 135, 40);
            //GFX.DrawString(Hc.ToString(), new Font("thaoma", 8), Brushes.Black, 80, 40);
            //GFX.DrawString(H_TXBX.Text + " " + VersioneCMBX.Text + " " + H2_TXBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);//   versione

            //GFX.DrawRectangle(pen1, 8, 55, 120, 26);
            //GFX.DrawString(calcoloString, new Font("thaoma", 8), Brushes.Black, 10, 57);
            //GFX.DrawString("(" + cordino1String + ")mix corda", new Font("thaoma", 8), Brushes.Black, 10, 67);

            //GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioBinario.ToString("0000")), 13), 200, 27);
            //GFX.DrawString(mixTaglioXetich, new Font("thaoma", 8), Brushes.Black, 196, 56);
            //// GFX.DrawString("Binario", new Font("thaoma", 7), Brushes.Black, 214, 41);

            //GFX.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
            //GFX.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);

            //pictureBox1.Image = drawingsurface;
            Aggiorna();
        }
        private void BinarioStrappo()
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }
            //articoloDB = articoliCB.Text.Replace("'", "''");//minima curva 12 compreso tappi

            //if (flagForCMBXitems == false)
            //{
            //    VersioneCMBX.Visible = true;
            //    VersioneCMBX.Items.Clear();
            //    VersioneCMBX.Items.Add("___________");
            //    VersioneCMBX.Items.Add("(________)");
            //    VersioneCMBX.Items.Add("(________");
            //    VersioneCMBX.Items.Add("________)");
            //    VersioneCMBX.Items.Add("|_______|");
            //    VersioneCMBX.Items.Add("________|");
            //    VersioneCMBX.Items.Add("|________");
            //    Colore_label.Visible = false;
            //    ColoreCMBX.Visible = false;
            //    VersioneLBL.Visible = true;

            //    flagForCMBXitems = true;
            //    VersioneCMBX.Text = "___________";
            //    VersioneCMBX.Focus();
            //}

            //switch (VersioneCMBX.Text)
            //{
            //    case "___________":

            //        if (VersioneCMBX.TextHasChanged == true)
            //        {
            //            H_label.Visible = false;
            //            H_TXBX.Visible = false;
            //            H2_TXBX.Visible = false;
            //            H2_label.Visible = false;
            //            Pz_supp1_txb.Text = "";
            //            Tot_SupplementoTXBX.Text = "";
            //            Qnt_suppTXBX.Text = "0";
            //            Costo_supplementoTXBX.Text = "0";
            //            Supplemento1CMBX.Text = "";
            //            Supplemento1CMBX.Items.Clear();
            //            NoteArtVarieLabel.Text = "";
            //        }

            //        if (L_TXBX.Text != "")//Calcolo quantita'
            //        {
            //            QuantitaTXBX.Text = Math.Round(Math.Max(int.Parse(L_TXBX.Text) / 1000f, Fatt_min_Tot) * int.Parse(PezziTXBX.Text), 2).ToString();

            //            calcoloString = "*" + Math.Round(int.Parse(L_TXBX.Text) / 1000d * 11) + "*";
            //            mixTaglioXetich = (int.Parse(L_TXBX.Text) - 5).ToString();

            //            mixTaglioBinario = int.Parse(L_TXBX.Text) - 5;
            //        }
            //        else
            //        {
            //            QuantitaTXBX.Text = "";
            //            calcoloString = "";
            //            mixTaglioXetich = "";
            //            mixTaglioBinario = 0;
            //        }

            //        break;

            //    case "(________)":

            //        if (VersioneCMBX.TextHasChanged == true)
            //        {
            //            H_label.Visible = true;
            //            H_TXBX.Visible = true;
            //            H2_TXBX.Visible = true;
            //            H2_label.Visible = true;
            //            H_label.Text = "SpSX";
            //            H_TXBX.Text = "170";
            //            H2_TXBX.Text = "170";
            //            Pz_supp1_txb.Text = "";
            //            Tot_SupplementoTXBX.Text = "";
            //            Qnt_suppTXBX.Text = "0";
            //            Costo_supplementoTXBX.Text = "0";
            //            Supplemento1CMBX.Text = "";
            //            Supplemento1CMBX.Items.Clear();
            //            // NoteArtVarieLabel.Text = "curva minima compreso guide 18//con mensole max 21";//?????????????????????????????????????????
            //        }


            //        if (L_TXBX.Text != "" && H_TXBX.Text != "" && H2_TXBX.Text != "")//////////////Calcolo quantita'//////////////////////
            //        {
            //            QuantitaTXBX.Text = Math.Round(Math.Max((int.Parse(L_TXBX.Text) + int.Parse(H_TXBX.Text) + int.Parse(H2_TXBX.Text)) / 1000f, Fatt_min_Tot) * int.Parse(PezziTXBX.Text), 2).ToString();

            //            int Hmin = 170, H2min = 170;
            //            string MixTaglioCurvaSxXetich = "", MixTaglioCurvaDxXetich = "";

            //            if (int.Parse(H_TXBX.Text) > 170)
            //            {
            //                Hmin = int.Parse(H_TXBX.Text);
            //                if (int.Parse(H_TXBX.Text) != 170) MixTaglioCurvaSxXetich = "[Usc." + (int.Parse(H_TXBX.Text) - 170) + "]";//verificare
            //            }
            //            else if (int.Parse(H_TXBX.Text) != 170) MixTaglioCurvaSxXetich = "[Tagl." + (int.Parse(H_TXBX.Text) - 170) + "]";//verificare

            //            if (int.Parse(H2_TXBX.Text) > 170)
            //            {
            //                H2min = int.Parse(H2_TXBX.Text);
            //                if (int.Parse(H2_TXBX.Text) != 170) MixTaglioCurvaDxXetich = "[Usc." + (int.Parse(H2_TXBX.Text) - 170) + "]";
            //            }
            //            else if (int.Parse(H2_TXBX.Text) != 170) MixTaglioCurvaDxXetich = "[Tagl." + (int.Parse(H2_TXBX.Text) - 170) + "]";

            //            calcoloInt = Hmin + H2min - 150;
            //            mixTaglioXetich = MixTaglioCurvaSxXetich + (int.Parse(L_TXBX.Text) + calcoloInt) + MixTaglioCurvaDxXetich;
            //            calcoloString = "*" + Math.Round((int.Parse(L_TXBX.Text) + int.Parse(H_TXBX.Text) + int.Parse(H2_TXBX.Text) - 70) / 1000d * 11) + "*";
            //            mixTaglioBinario = int.Parse(L_TXBX.Text) + calcoloInt;

            //        }
            //        else
            //        {
            //            QuantitaTXBX.Text = "";
            //            calcoloString = "";
            //            mixTaglioXetich = "";
            //            mixTaglioBinario = 0;
            //        }
            //        break;

            //    case "|_______|":

            //        if (VersioneCMBX.TextHasChanged)
            //        {
            //            H_label.Visible = true;
            //            H_TXBX.Visible = true;
            //            H2_TXBX.Visible = true;
            //            H2_label.Visible = true;
            //            H_label.Text = "SpSX";
            //            H_TXBX.Text = "170";
            //            H2_TXBX.Text = "170";
            //            NoteArtVarieLabel.Text = "";
            //            Supplemento1CMBX.Items.Clear();
            //            Supplemento1CMBX.Items.Add("Guida ad angolo (coppia)");
            //            Supplemento1CMBX.Text = "Guida ad angolo (coppia)";
            //            Supplemento1_NomeSuppDaSalvare = "Guida ad angolo (coppia) (Suppl.)";
            //            LoadDatiSupplemento1CMBX();
            //        }
            //        if (L_TXBX.Text != "" && H_TXBX.Text != "" && H2_TXBX.Text != "")//////////////Calcolo quantita'//////////////////////
            //        {
            //            QuantitaTXBX.Text = Math.Round(Math.Max((int.Parse(L_TXBX.Text) + int.Parse(H_TXBX.Text) + int.Parse(H2_TXBX.Text)) / 1000f, Fatt_min_Tot) * int.Parse(PezziTXBX.Text), 2).ToString();

            //            calcoloString = "*" + (Math.Round(int.Parse(H_TXBX.Text) / 1000d * 11) + "+" + Math.Round(int.Parse(L_TXBX.Text) / 1000d * 11) + "+" + Math.Round(int.Parse(H2_TXBX.Text) / 1000d * 11)) + "*";
            //            mixTaglioXetich = (int.Parse(H_TXBX.Text) - 22) + "+" + (int.Parse(L_TXBX.Text) - 122) + "+" + (int.Parse(H2_TXBX.Text) - 22);
            //            mixTaglioBinario = int.Parse(L_TXBX.Text) - 122;
            //        }
            //        else
            //        {
            //            QuantitaTXBX.Text = "";
            //            calcoloString = "";
            //            mixTaglioXetich = "";
            //            mixTaglioBinario = 0;
            //        }


            //        Pz_supp1_txb.Text = PezziTXBX.Text;
            //        Qnt_suppTXBX.Text = PezziTXBX.Text;
            //        if (Supplemento1CMBX.Text != "") Tot_SupplementoTXBX.Text = Math.Round(float.Parse(Costo_supplementoTXBX.Text) * int.Parse(PezziTXBX.Text), 2).ToString();
            //        else MessageBox.Show("Il nome supplemento in maestro deve essere esattamente: Guida ad angolo (coppia)", "P_Sun            Inserisci Supplemento guida ad angolo", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //        break;

            //    case "(________"://-----------------------------------------------------------------------------------------------------
            //        if (VersioneCMBX.TextHasChanged == true)
            //        {
            //            H_label.Visible = true;
            //            H_TXBX.Visible = true;
            //            H2_TXBX.Text = "";
            //            H2_label.Visible = false;
            //            H2_TXBX.Visible = false;
            //            H_TXBX.Text = "170";
            //            Pz_supp1_txb.Text = "";
            //            Tot_SupplementoTXBX.Text = "";
            //            Qnt_suppTXBX.Text = "0";
            //            Costo_supplementoTXBX.Text = "0";
            //            Supplemento1CMBX.Text = "";
            //            Supplemento1CMBX.Items.Clear();
            //            // NoteArtVarieLabel.Text = "curva minima compreso guide 18//con mensole max 21";
            //        }
            //        if (L_TXBX.Text != "" && H_TXBX.Text != "")//Calcolo quantita'
            //        {
            //            QuantitaTXBX.Text = Math.Round(Math.Max((int.Parse(L_TXBX.Text) + int.Parse(H_TXBX.Text)) / 1000d, Fatt_min_Tot) * int.Parse(PezziTXBX.Text), 2).ToString();

            //            int Hmin = 170;
            //            string MixTaglioCurvaSxXetich = "";

            //            if (int.Parse(H_TXBX.Text) > 170)
            //            {
            //                Hmin = int.Parse(H_TXBX.Text);
            //                if (int.Parse(H_TXBX.Text) != 170) MixTaglioCurvaSxXetich = "[Usc." + (int.Parse(H_TXBX.Text) - 170) + "]";
            //            }
            //            else if (float.Parse(H_TXBX.Text) != 170) MixTaglioCurvaSxXetich = "[Tagl." + (int.Parse(H_TXBX.Text) - 170) + "]";

            //            calcoloInt = Hmin - 65;
            //            mixTaglioXetich = MixTaglioCurvaSxXetich + (int.Parse(L_TXBX.Text) + calcoloInt).ToString();
            //            calcoloString = "*" + Math.Round((int.Parse(L_TXBX.Text) + int.Parse(H_TXBX.Text) - 40) / 1000d * 11) + "*";

            //            mixTaglioBinario = int.Parse(L_TXBX.Text) + calcoloInt;

            //        }
            //        else
            //        {
            //            QuantitaTXBX.Text = "";
            //            calcoloString = "";
            //            mixTaglioXetich = "";
            //            mixTaglioBinario = 0;
            //        }

            //        break;
            //    case "|________"://-----------------------------------------------------------------------------------------------------

            //        if (VersioneCMBX.TextHasChanged == true)
            //        {
            //            H_label.Visible = true;
            //            H_TXBX.Visible = true;
            //            H2_TXBX.Text = "";
            //            H2_label.Visible = false;
            //            H2_TXBX.Visible = false;
            //            H_TXBX.Text = "170";
            //            NoteArtVarieLabel.Text = "";
            //            Supplemento1CMBX.Items.Clear();
            //            Supplemento1CMBX.Items.Add("Guida ad angolo singola");
            //            Supplemento1CMBX.Text = "Guida ad angolo singola";
            //            Supplemento1_NomeSuppDaSalvare = "Guida ad angolo singola (Suppl.)";
            //            LoadDatiSupplemento1CMBX();

            //        }
            //        if (L_TXBX.Text != "" && H_TXBX.Text != "")//Calcolo quantita'
            //        {
            //            QuantitaTXBX.Text = Math.Round(Math.Max((int.Parse(L_TXBX.Text) + int.Parse(H_TXBX.Text)) / 1000f, Fatt_min_Tot) * int.Parse(PezziTXBX.Text), 2).ToString();

            //            calcoloString = "*" + Math.Round(int.Parse(H_TXBX.Text) / 1000d * 11) + "+" + Math.Round(int.Parse(L_TXBX.Text) / 1000d * 11);
            //            mixTaglioXetich = (int.Parse(H_TXBX.Text) - 22) + "+" + (int.Parse(L_TXBX.Text) - 65);

            //            mixTaglioBinario = int.Parse(L_TXBX.Text) - 65;
            //        }
            //        else
            //        {
            //            QuantitaTXBX.Text = "";
            //            calcoloString = "";
            //            mixTaglioXetich = "";
            //            mixTaglioBinario = 0;
            //        }

            //        Pz_supp1_txb.Text = PezziTXBX.Text;
            //        Qnt_suppTXBX.Text = PezziTXBX.Text;
            //        if (Supplemento1CMBX.Text != "") Tot_SupplementoTXBX.Text = Math.Round(float.Parse(Costo_supplementoTXBX.Text) * int.Parse(PezziTXBX.Text), 2).ToString();
            //        else MessageBox.Show("Il nome supplemento in maestro deve essere esattamente: Guida ad angolo singola", "P_Sun            Inserisci Supplemento guida ad angolo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        break;

            //    case "________|"://--------------------------------------------------------------------------------------------------------

            //        if (VersioneCMBX.TextHasChanged == true)
            //        {
            //            H_label.Visible = false;
            //            H_TXBX.Visible = false;
            //            H_TXBX.Text = "";
            //            H2_label.Visible = true;
            //            H2_TXBX.Visible = true;
            //            H2_TXBX.Text = "170";
            //            NoteArtVarieLabel.Text = "";
            //            Supplemento1CMBX.Items.Clear();
            //            Supplemento1CMBX.Items.Add("Guida ad angolo singola");
            //            Supplemento1CMBX.Text = "Guida ad angolo singola";
            //            Supplemento1_NomeSuppDaSalvare = "Guida ad angolo singola (Suppl.)";
            //            LoadDatiSupplemento1CMBX();
            //        }

            //        if (L_TXBX.Text != "" && H2_TXBX.Text != "")//Calcolo quantita'
            //        {
            //            QuantitaTXBX.Text = Math.Round(Math.Max((int.Parse(L_TXBX.Text) + int.Parse(H2_TXBX.Text)) / 1000f, Fatt_min_Tot) * int.Parse(PezziTXBX.Text), 2).ToString();

            //            calcoloString = "*" + Math.Round(int.Parse(L_TXBX.Text) / 1000d * 11) + "+" + Math.Round(int.Parse(H2_TXBX.Text) / 1000f * 11) + "*";
            //            mixTaglioXetich = (int.Parse(L_TXBX.Text) - 65) + "+" + (int.Parse(H2_TXBX.Text) - 22);

            //            mixTaglioBinario = int.Parse(L_TXBX.Text) - 65;
            //        }
            //        else
            //        {
            //            QuantitaTXBX.Text = "";
            //            calcoloString = "";
            //            mixTaglioXetich = "";
            //            mixTaglioBinario = 0;
            //        }
            //        Supplemento1_NomeSuppDaSalvare = "Guida ad angolo singola";
            //        Pz_supp1_txb.Text = PezziTXBX.Text;
            //        Qnt_suppTXBX.Text = PezziTXBX.Text;

            //        if (Supplemento1CMBX.Text != "") Tot_SupplementoTXBX.Text = Math.Round(float.Parse(Costo_supplementoTXBX.Text) * float.Parse(PezziTXBX.Text), 2).ToString();
            //        else MessageBox.Show("Il nome supplemento in maestro deve essere esattamente: Guida ad angolo singola", "P_Sun            Inserisci Supplemento guida ad angolo", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //        break;
            //    case "________)"://----------------------------------------------------------------------------------------------------------

            //        if (VersioneCMBX.TextHasChanged == true)
            //        {
            //            H_label.Visible = false;
            //            H_TXBX.Visible = false;
            //            H_TXBX.Text = "";
            //            H2_label.Visible = true;
            //            H2_TXBX.Visible = true;
            //            H2_TXBX.Text = "170";
            //            Pz_supp1_txb.Text = "";
            //            Tot_SupplementoTXBX.Text = "";
            //            Qnt_suppTXBX.Text = "0";
            //            Costo_supplementoTXBX.Text = "0";
            //            Supplemento1CMBX.Text = "";
            //            Supplemento1CMBX.Items.Clear();
            //            // NoteArtVarieLabel.Text = "curva minima compreso guide 18//con mensole max 21";
            //        }
            //        if (L_TXBX.Text != "" && H2_TXBX.Text != "")//Calcolo quantita'
            //        {
            //            QuantitaTXBX.Text = Math.Round(Math.Max((int.Parse(L_TXBX.Text) + int.Parse(H2_TXBX.Text)) / 1000d, Fatt_min_Tot) * int.Parse(PezziTXBX.Text), 2).ToString();

            //            int H2min = 170;
            //            string MixTaglioCurvaDxXetich = "";

            //            if (int.Parse(H2_TXBX.Text) > 170)
            //            {
            //                H2min = int.Parse(H2_TXBX.Text);
            //                if (int.Parse(H2_TXBX.Text) != 170) MixTaglioCurvaDxXetich = "[Usc." + (int.Parse(H2_TXBX.Text) - 170) + "]";

            //            }
            //            else if (int.Parse(H2_TXBX.Text) != 170) MixTaglioCurvaDxXetich = "[Tagl." + (int.Parse(H2_TXBX.Text) - 170) + "]";

            //            calcoloInt = H2min - 65;
            //            mixTaglioXetich = int.Parse(L_TXBX.Text) + calcoloInt + MixTaglioCurvaDxXetich;
            //            calcoloString = "*" + Math.Round((int.Parse(L_TXBX.Text) + int.Parse(H2_TXBX.Text) - 40) / 1000d * 11) + "*";

            //            mixTaglioBinario = int.Parse(L_TXBX.Text) + calcoloInt;
            //        }
            //        else
            //        {
            //            QuantitaTXBX.Text = "";
            //            calcoloString = "";
            //            mixTaglioXetich = "";
            //            mixTaglioBinario = 0;
            //        }
            //        break;
            //}
            //Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);

            //GFX.Clear(Color.White);
            //GFX.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 3);
            //GFX.DrawString("Binario strappo", new Font("thaoma", 8), Brushes.Black, 200, 3);
            //GFX.DrawString("L " + L_TXBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 16, 40);
            //GFX.DrawString(H_TXBX.Text + " " + VersioneCMBX.Text + " " + H2_TXBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);//   versione

            //GFX.DrawRectangle(pen1, 8, 55, 120, 20);
            //GFX.DrawString(calcoloString, new Font("thaoma", 8), Brushes.Black, 10, 57);

            //GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioBinario.ToString("0000")), 13), 200, 27);
            //GFX.DrawString(mixTaglioXetich, new Font("thaoma", 8), Brushes.Black, 196, 56);
            ////  GFX.DrawString("Binario", new Font("thaoma", 7), Brushes.Black, 214, 41);

            //GFX.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
            //GFX.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);
            //pictureBox1.Image = drawingsurface;
            Aggiorna();
        }
        //Porta a soffietto
        private void PortaASoffietto()
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }
            //articoloDB = articoliCB.Text.Replace("'", "''");

            //if (LuceRBN.Checked == true) luceH = -8;
            //else luceH = 0;

            //if (flagForCMBXitems == false)
            //{
            //    Email_A_fornitori_Intestazione("");
            //    Supplemento1CMBX.Items.Clear();
            //    Supplemento1CMBX.Items.Add("Serratura");

            //    NoteCMBBX.Items.Add("vetri sbucc. fume' a scalare");
            //    NoteCMBBX.Items.Add("vetri sbucc. bianchi a scalare");

            //    flagForCMBXitems = true;
            //}

            //if (L_TXBX.Text != "" && H_TXBX.Text != "")
            //{
            //    if (L_TXBX.Text != "" && L_TXBX.TextHasChanged) N_pannelli();
            //    luceHperEtich = int.Parse(H_TXBX.Text) + luceH;

            //    if ((luceHperEtich - 610) > 0) mixTaglioProfilo = luceHperEtich - 643;//64.3=61.1+3.2//65=61.1+4
            //    else mixTaglioProfilo = 0;

            //    if (H_TXBX.TextHasChanged || ColoreCMBX.TextHasChanged) Fatt_min_e_disponib_soffietti();
            //}
            //else
            //{
            //    mixTaglioProfilo = 0;
            //    luceLperEtich = 0;
            //    luceHperEtich = 0;
            //}
            ////888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //Calcolo_supplemento1(int.Parse(PezziTXBX.Text), PezziTXBX.Text);
            //Calcolo_QuantitaMQ(Fatt_min_L, Fatt_min_H, Fatt_min_Tot);
            //Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);
            ////888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888

            //GFX.Clear(Color.White);
            //GFX.DrawString(Alias.Substring(0, Alias.Length > 20 ? 20 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 0);
            //GFX.DrawString("P.Soff", new Font("thaoma", 8), Brushes.Black, 180, 0);
            //GFX.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 225, 0);
            //GFX.DrawString("L " + L_TXBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 22);
            //GFX.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 75, 22);
            //GFX.DrawString("(" + (luceHperEtich - 32) + ") mix pannello", new Font("thaoma", 8), Brushes.Black, 17, 44);
            //GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioProfilo.ToString("0000")), 12), PartenzaBarcodeDX, 22);
            //GFX.DrawString(mixTaglioProfilo.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 36);
            //if (Supplemento1CMBX.Text != "") GFX.DrawString("Con Serratura", new Font("thaoma", 10, FontStyle.Bold), Brushes.Black, 117, 40);
            //GFX.DrawString(calcoloString.ToString(), new Font("thaoma", 8), Brushes.Black, 5, 60);

            //GFX.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
            //GFX.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);
            //pictureBox1.Image = drawingsurface;
            Aggiorna();
        }
        //Rullo
        private void Cubo80()
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }



            //if (LuceRBN.Checked == true) { luceL = -3; luceH = -1; }
            //else { luceL = 0; luceH = 0; }

            //if (flagForCMBXitems == false)
            //{
            //    ConfigurazioneRulloMotore("Tubo55");
            //    AttacchiCBX.Visible = false;
            //    Attacchi_label.Visible = false;
            //    Supplemento2CMBX.Items.Add("GUIDA AD INCASSO");
            //    Supplemento2CMBX.Items.Add("GUIDA MAGGIORATA 70 mm");
            //    Supplemento2CMBX.Items.Add("COMPENSATORE FLAT");
            //    flagForCMBXitems = true;
            //    if (ColoreCMBX.Items.Count >= 3) ColoreCMBX.SelectedIndex = 2;
            //}

            //string mixDima = "";
            //string MixZavorra = "";
            //string Tipo_tubo = "";

            //if (H_TXBX.Text != "" && L_TXBX.Text != "" && Varie1TCMBX.Text != "")
            //{
            //    articoloDB = articoliCB.Text.Replace("'", "''")
            //   + Environment.NewLine + "   " + luceLperEtich + "x" + H_TXBX.Text + " " + Varie1TCMBX.Text;
            //    luceLperEtich = int.Parse(L_TXBX.Text) + luceL;
            //    luceHperEtich = int.Parse(H_TXBX.Text);
            //    //--------------------------------------------------------------------------------------------------------------------                 
            //    mixTaglioCassonetto = luceLperEtich - 9;
            //    mixTaglioTubo = luceLperEtich - 136;
            //    mixTaglioFondaleManiglia = luceLperEtich - 98;
            //    mixTaglioGuida = luceHperEtich - 89;
            //    mixTeloFinita = (luceLperEtich - 72) + "x" + (int.Parse(H_TXBX.Text) + 300);

            //    if (luceLperEtich <= 1500) MixZavorra = (luceLperEtich - 160) + " pz1";
            //    if (luceLperEtich <= 1000) MixZavorra = (luceLperEtich - 160) + " pz2";
            //    if (luceLperEtich > 1500) MixZavorra = " 600 2pz";

            //    if (luceHperEtich <= 2000) mixDima = (luceHperEtich + 100) + "dx";
            //    else mixDima = (luceHperEtich - 1000) + "sx";

            //    //if (luceLperEtich <= 200) Tipo_tubo = "47";
            //    //else Tipo_tubo = "55";


            //    annotazioniVerieSuEtichiette = "-Viti per guida pz:" + (Math.Max(Math.Round(int.Parse(L_TXBX.Text) / 1000d), 2) * 2);//N° viti per guide

            //    //---------------------------------------------------------------------------------------------------------------------                 
            //    if (Varie1TCMBX.Text == "Solo meccanica" | Varie1TCMBX.Text.Contains("meccanica"))//solo meccanica
            //    {
            //        CostoTessutoTXBX.Text = "0";
            //        CostomeccanicaTabella(int.Parse(L_TXBX.Text), int.Parse(H_TXBX.Text));
            //    }
            //    else
            //    {
            //        L_TXBX.AutoResetTextHasChanged = false; H_TXBX.AutoResetTextHasChanged = false; Varie1TCMBX.AutoResetTextHasChanged = false;
            //        CostomeccanicaTabella(int.Parse(L_TXBX.Text), int.Parse(H_TXBX.Text));
            //        L_TXBX.AutoResetTextHasChanged = true; Varie1TCMBX.AutoResetTextHasChanged = true; H_TXBX.AutoResetTextHasChanged = true;
            //        CostoTessuto(int.Parse(L_TXBX.Text));
            //    }
            //    //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //    Calcolo_QuantitaRullo();
            //    Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);
            //    Calcolo_supplemento1(int.Parse(PezziTXBX.Text), PezziTXBX.Text);
            //    Calcolo_supplemento2_Cubo();
            //    //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //}
            //else
            //{
            //    QuantitaTXBX.Text = "";
            //    luceLperEtich = 0;
            //    luceHperEtich = 0;
            //}
            Aggiorna();
        }
        private void Cubo100()
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }
            Aggiorna();


            //if (LuceRBN.Checked == true) { luceL = -3; luceH = -1; }
            //else { luceL = 0; luceH = 0; }

            //if (flagForCMBXitems == false)
            //{
            //    ConfigurazioneRulloMotore("Tubo61");
            //    AttacchiCBX.Visible = false;
            //    Attacchi_label.Visible = false;
            //    Supplemento1CMBX.Items.Remove("Motore a batteria");
            //    Supplemento2CMBX.Items.Add("GUIDA AD INCASSO");
            //    Supplemento2CMBX.Items.Add("GUIDA MAGGIORATA 70 mm");
            //    Supplemento2CMBX.Items.Add("COMPENSATORE FLAT");
            //    flagForCMBXitems = true;
            //    if (ColoreCMBX.Items.Count >= 3) ColoreCMBX.SelectedIndex = 2;
            //}

            //string mixDima = "";
            //string MixZavorra = "";
            //string Tipo_tubo = "";

            //if (H_TXBX.Text != "" && L_TXBX.Text != "" && Varie1TCMBX.Text != "")
            //{
            //    articoloDB = articoliCB.Text.Replace("'", "''")
            //   + Environment.NewLine + "   " + luceLperEtich + "x" + H_TXBX.Text + " " + Varie1TCMBX.Text;
            //    luceLperEtich = int.Parse(L_TXBX.Text) + luceL;
            //    luceHperEtich = int.Parse(H_TXBX.Text);
            //    //--------------------------------------------------------------------------------------------------------------------                 
            //    mixTaglioCassonetto = luceLperEtich - 9;
            //    mixTaglioTubo = luceLperEtich - 112;
            //    mixTaglioFondaleManiglia = luceLperEtich - 98;
            //    mixTaglioGuida = luceHperEtich - 109;
            //    mixTeloFinita = (luceLperEtich - 72) + "x" + (int.Parse(H_TXBX.Text) + 300);

            //    if (luceLperEtich <= 3000) MixZavorra = " 800 pz2";
            //    if (luceLperEtich <= 1800) MixZavorra = (luceLperEtich - 160) + " pz1";
            //    if (luceLperEtich <= 1200) MixZavorra = (luceLperEtich - 160) + " pz2";
            //    if (luceLperEtich > 3000) MixZavorra = " 600 pz2";

            //    if (luceHperEtich <= 2000) mixDima = luceHperEtich + 100 + "dx";
            //    else mixDima = luceHperEtich - 100 + "sx";

            //    if (luceLperEtich <= 4000) Tipo_tubo = "78";
            //    else Tipo_tubo = "85";

            //    annotazioniVerieSuEtichiette = "-Viti per guida pz:" + (Math.Max(Math.Round(int.Parse(L_TXBX.Text) / 1000d), 2) * 2);//N° viti per guide

            //    //---------------------------------------------------------------------------------------------------------------------                 
            //    if (Varie1TCMBX.Text == "Solo meccanica" | Varie1TCMBX.Text.Contains("meccanica"))//solo meccanica
            //    {
            //        CostoTessutoTXBX.Text = "0";
            //        CostomeccanicaTabella(int.Parse(L_TXBX.Text), int.Parse(H_TXBX.Text));
            //    }
            //    else
            //    {
            //        L_TXBX.AutoResetTextHasChanged = false; H_TXBX.AutoResetTextHasChanged = false; Varie1TCMBX.AutoResetTextHasChanged = false;
            //        CostomeccanicaTabella(int.Parse(L_TXBX.Text), int.Parse(H_TXBX.Text));
            //        L_TXBX.AutoResetTextHasChanged = true; Varie1TCMBX.AutoResetTextHasChanged = true; H_TXBX.AutoResetTextHasChanged = true;
            //        CostoTessuto(int.Parse(L_TXBX.Text));
            //    }
            //    //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //    Calcolo_QuantitaRullo();
            //    Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);
            //    Calcolo_supplemento1(int.Parse(PezziTXBX.Text), PezziTXBX.Text);
            //    Calcolo_supplemento2_Cubo();
            //    //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //}
            //else
            //{
            //    QuantitaTXBX.Text = "";
            //    luceLperEtich = 0;
            //    luceHperEtich = 0;
            //}


        }
        private void Cubo130()
        {
            Aggiorna();
        }
        private void Cubo80Arg()
        {
            Aggiorna();
        }
        private void Cubo100Arg()
        {
            Aggiorna();
        }
        private void Cubo130Arg()
        {
            Aggiorna();
        }
        private void RulloStandardCristineMiniCat(float Quotatubo, float Quotatessuto, string Nomerullo)
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }
            Aggiorna();


            //string[] accessoriCavettate = { "", "", "", "" };// 

            //if (LuceRBN.Checked == true) luceL = -1;
            //else luceL = 0;

            //if (flagForCMBXitems == false)
            //{
            //    ConfigurazioneRulloCatena();
            //    NoteCMBBX.Items.Clear();
            //    NoteCMBBX.Items.Add("Discesa interno casa");
            //    NoteCMBBX.Items.Add("Fondale rettangolare");
            //    NoteCMBBX.Items.Add("Disc. interno casa//Fond. rettangolare");
            //    NoteCMBBX.Items.Add("Fond.in tasca");
            //    NoteCMBBX.Items.Add("Disc.interno casa//Fond.in tasca");
            //    Supplemento2CMBX.Items.Clear();
            //    Supplemento2CMBX.Items.Add("Guida Perlon");
            //    Supplemento2CMBX.Items.Add("Guida Perlon front");
            //    Supplemento2CMBX.Items.Add("Guida Inox");
            //    flagForCMBXitems = true;
            //    if (ColoreCMBX.Items.Count >= 2) ColoreCMBX.SelectedIndex = 1;
            //}
            //string[] CC;
            //CC = Varie1TCMBX.Text.Split(' ');

            //if (H_TXBX.Text != "" && L_TXBX.Text != "")//&& Varie1TCMBX.Text != ""
            //{
            //    luceLperEtich = int.Parse(L_TXBX.Text) + luceL;
            //    luceHperEtich = int.Parse(H_TXBX.Text) + luceH;
            //    //--------------------------------------------------------------------------------------------------------------------               
            //    mixTaglioTubo = luceLperEtich - Quotatubo;
            //    //---------------------------------------------------------------------------------------------------------------------
            //    //----------------------------------------------------------------------------------------------------------- 
            //    //                                    gestione cavettate
            //    //-----------------------------------------------------------------------------------------------------------                   

            //    if (Supplemento2CMBX.Text.ToLower().Contains("inox"))
            //    {
            //        accessoriCavettate[1] = "- 2 Fissaggio a muro guid. cavo (35300005)";
            //        accessoriCavettate[2] = "- 2 Cavi inox 1,5mm da: " + Math.Round(int.Parse(H_TXBX.Text) + 150d) + " mm + 4 manicotti 1,5 (04maniall015)";
            //    }
            //    else if (Supplemento2CMBX.Text.ToLower().Contains("perlon")) //guida perlon
            //    {
            //        if (Supplemento2CMBX.Text.ToLower().Contains("front")) accessoriCavettate[1] = "- 2 Piastrine trasparenti angolo (ZAW0084)";
            //        else accessoriCavettate[1] = "- 2 Piastrine trasparenti (ZAW0085)";
            //        accessoriCavettate[2] = "- 2 Cavi Nylon 2mm da: " + Math.Round(int.Parse(H_TXBX.Text) + 150d) + " mm + 4 morsetti (ZAT0063-0011-006-0)";
            //    }
            //    else
            //    {
            //        accessoriCavettate[0] = "";
            //        accessoriCavettate[1] = "";
            //        accessoriCavettate[2] = "";
            //        accessoriCavettate[3] = "";
            //        goto Salta;
            //    }

            //    if (AttacchiCBX.Text.ToLower().Contains("frontale")) accessoriCavettate[0] = "- Cp Staffe guide standard front";
            //    else accessoriCavettate[0] = "- Cp Staffe guide standard soff";//AttacchiCBX soffitto // oppue a combobox vuota

            //    Salta:
            //    //----------------------------------------------------------------------------------------------------------- 
            //    //                                  fine gestione cavettate
            //    //-----------------------------------------------------------------------------------------------------------
            //    if (Varie1TCMBX.Text == "Solo meccanica" | Varie1TCMBX.Text.Contains("meccanica"))//solo meccanica
            //    {
            //        articoloDB = articoliCB.Text.Replace("'", "''")
            //      + Environment.NewLine + " L  " + luceLperEtich + " " + Varie1TCMBX.Text;
            //        mixTeloFinita = (luceLperEtich - Quotatessuto) + "x ??";
            //        // H_TXBX.Enabled = false;
            //        // H_TXBX.Text = "0";
            //        CostomeccanicaSemplice(int.Parse(L_TXBX.Text));
            //        CostoTessutoTXBX.Text = "0";
            //    }
            //    else
            //    {
            //        articoloDB = articoliCB.Text.Replace("'", "''")
            //       + Environment.NewLine + "   " + luceLperEtich + "x" + H_TXBX.Text + " " + Varie1TCMBX.Text;

            //        mixTeloFinita = (luceLperEtich - Quotatessuto) + "x" + (int.Parse(H_TXBX.Text) + Tessuti.GetTessutoByName(CC[0]).Arrotolamento_su_tubo * Tessuti.GetTessutoByName(CC[0]).Tessuto_NeD);

            //        L_TXBX.AutoResetTextHasChanged = false; Varie1TCMBX.AutoResetTextHasChanged = false;
            //        CostomeccanicaSemplice(int.Parse(L_TXBX.Text));
            //        L_TXBX.AutoResetTextHasChanged = true; Varie1TCMBX.AutoResetTextHasChanged = true;
            //        CostoTessuto(int.Parse(L_TXBX.Text));
            //    }
            //}
            //else
            //{
            //    luceLperEtich = 0;
            //    luceHperEtich = 0;
            //    mixTeloFinita = "0";

            //    mixTaglioTubo = 0;
            //    mixTaglioFondaleManiglia = 0;
            //}
            ////8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //Calcolo_QuantitaRullo();
            //Calcolo_supplemento1_CatenaMetallo(PezziTXBX.Text);
            //Calcolo_supplemento2(float.Parse(QuantitaTXBX.Text), PezziTXBX.Text);
            //Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);
            ////8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888


        }
        private void RulloMedio()
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }
            Aggiorna();
        }
        private void RulloAngolar()
        {
            Aggiorna();
        }
        private void RulloStandardMedioPortarullo(float QuotaprofiloSuperiore, float Quotatubo, string NomeRullo)
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }
            Aggiorna();

            //if (LuceRBN.Checked == true) luceL = -2;
            //else luceL = 0;

            //if (flagForCMBXitems == false)
            //{
            //    ConfigurazioneRulloCatena();
            //    NoteCMBBX.Items.Clear();
            //    NoteCMBBX.Items.Add("Discesa interno casa");
            //    NoteCMBBX.Items.Add("Fondale rettangolare");
            //    NoteCMBBX.Items.Add("Disc. interno casa//Fond. rettangolare");
            //    NoteCMBBX.Items.Add("Fond.in tasca");
            //    NoteCMBBX.Items.Add("Disc.interno casa//Fond.in tasca");
            //    Supplemento2CMBX.Items.Clear();
            //    Supplemento2CMBX.Items.Add("Guida Perlon");
            //    Supplemento2CMBX.Items.Add("Guida Perlon front");
            //    flagForCMBXitems = true;
            //    if (ColoreCMBX.Items.Count >= 1) ColoreCMBX.SelectedIndex = 0;
            //}


            //string[] CC;
            //CC = Varie1TCMBX.Text.Split(' ');

            //if (H_TXBX.Text != "" && L_TXBX.Text != "")//&& Varie1TCMBX.Text != ""
            //{
            //    luceLperEtich = int.Parse(L_TXBX.Text) + luceL;
            //    luceHperEtich = int.Parse(H_TXBX.Text) + luceH;
            //    //--------------------------------------------------------------------------------------------------------------------                 
            //    mixTaglioProfiloSuperiore = luceLperEtich - QuotaprofiloSuperiore;
            //    mixTaglioTubo = luceLperEtich - Quotatubo;

            //    //---------------------------------------------------------------------------------------------------------------------                
            //    if (Varie1TCMBX.Text == "Solo meccanica" | Varie1TCMBX.Text.Contains("meccanica"))//solo meccanica
            //    {
            //        articoloDB = articoliCB.Text.Replace("'", "''")
            //      + Environment.NewLine + " L  " + luceLperEtich + " " + Varie1TCMBX.Text;
            //        mixTeloFinita = mixTaglioTubo - 6 + "x ??";

            //        CostoTessutoTXBX.Text = "0";
            //        CostomeccanicaSemplice(int.Parse(L_TXBX.Text));
            //    }
            //    else
            //    {
            //        articoloDB = articoliCB.Text.Replace("'", "''")
            //       + Environment.NewLine + "   " + luceLperEtich + "x" + H_TXBX.Text + " " + Varie1TCMBX.Text;

            //        mixTeloFinita = mixTaglioTubo - 6 + "x" + ((int.Parse(H_TXBX.Text) + Tessuti.GetTessutoByName(CC[0]).Arrotolamento_su_tubo) * Tessuti.GetTessutoByName(CC[0]).Tessuto_NeD);

            //        L_TXBX.AutoResetTextHasChanged = false; Varie1TCMBX.AutoResetTextHasChanged = false;
            //        CostomeccanicaSemplice(int.Parse(L_TXBX.Text));
            //        L_TXBX.AutoResetTextHasChanged = true; Varie1TCMBX.AutoResetTextHasChanged = true;
            //        CostoTessuto(int.Parse(L_TXBX.Text));
            //    }
            //}
            //else
            //{
            //    luceLperEtich = 0;
            //    luceHperEtich = 0;
            //    mixTeloFinita = "0";

            //    mixTaglioProfiloSuperiore = 0;
            //    mixTaglioTubo = 0;
            //}


        }
        private void RulloAngolarPortarullo()
        {
            Aggiorna();
        }
        private void RulloMedioMotore()
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }
            Aggiorna();


            //int quantita_supplemento = 1;//per calcolare il raddoppio dei supplementi quando sono affiancate
            //string[] accessoriCavettate = { "", "", "", "" };// 

            //if (LuceRBN.Checked == true) luceL = -3;
            //else luceL = 0;

            //if (flagForCMBXitems == false)
            //{
            //    ConfigurazioneRulloMotore("Tubo43");

            //    ComandiCMBX.Items.Clear();
            //    ComandiCMBX.Items.Add("DX");
            //    ComandiCMBX.Items.Add("SX");
            //    ComandiCMBX.Text = "DX";
            //    NoteCMBBX.Items.Clear();
            //    NoteCMBBX.Items.Add("Discesa interno casa");
            //    NoteCMBBX.Items.Add("Fondale rettangolare");
            //    NoteCMBBX.Items.Add("Fond.in tasca");
            //    NoteCMBBX.Items.Add("Disc.interno casa//Fond.rettangolare");
            //    NoteCMBBX.Items.Add("Disc.interno casa//Fond.in tasca");

            //    Supplemento2CMBX.Items.Clear();
            //    Supplemento2CMBX.Items.Add("Guida Perlon");
            //    Supplemento2CMBX.Items.Add("Guida Perlon front");
            //    Supplemento2CMBX.Items.Add("Guida Inox");
            //    VersioneCMBX.Visible = true;
            //    VersioneLBL.Visible = true;
            //    VersioneCMBX.Items.Clear();
            //    VersioneCMBX.Items.Add("Affiancato"); VersioneCMBX.Items.Add("Affiancato asimmetrico");
            //    VersioneCMBX.Items.Add("Doppio"); VersioneCMBX.Items.Add("Affiancato Doppio"); VersioneCMBX.Items.Add("Affiancato asimmetrico Doppio");

            //    flagForCMBXitems = true;

            //    if (ColoreCMBX.Items.Count >= 4) ColoreCMBX.SelectedIndex = 3;
            //}
            ////000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
            //if (VersioneCMBX.Text == "Affiancato asimmetrico" || VersioneCMBX.Text == "Affiancato asimmetrico Doppio") { L2_label.Visible = true; L2_TXBX.Visible = true; }
            //else { L2_label.Visible = false; L2_TXBX.Visible = false; L2_TXBX.Text = ""; }
            //string Doppiorullo; int QuotaDoppiorullo;
            //if (VersioneCMBX.Text.Contains("Doppio")) { Doppiorullo = "Doppio"; QuotaDoppiorullo = 4; }
            //else { Doppiorullo = ""; QuotaDoppiorullo = 0; }

            //if ((VersioneCMBX.Text.Contains("Affiancato") | VersioneCMBX.Text.Contains("Doppio")) && Supplemento2CMBX.Text.Contains("Guida"))
            //{
            //    accessoriCavettate[0] = "";
            //    accessoriCavettate[1] = "";
            //    accessoriCavettate[2] = "";
            //    accessoriCavettate[3] = "";
            //    MessageBox.Show("Non e' possibile avere guide!", "P_sun", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    Supplemento2CMBX.Text = "";
            //}

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //if (H_TXBX.Text != "" && L_TXBX.Text != "")//&& Varie1TCMBX.Text != ""
            //{

            //    //0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
            //    luceLperEtich = int.Parse(L_TXBX.Text) + luceL;
            //    luceHperEtich = int.Parse(H_TXBX.Text) + luceH;
            //    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            //    if (VersioneCMBX.Text == "" || VersioneCMBX.Text == "Doppio")//1 telo
            //    {
            //        //----------------------------------------------------------------------------------------------------------- 
            //        //                                    gestione cavettate
            //        //-----------------------------------------------------------------------------------------------------------                   

            //        if (Supplemento2CMBX.Text.ToLower().Contains("inox"))
            //        {
            //            accessoriCavettate[1] = "- 2 Fissaggio a muro guid. cavo (35300005)";
            //            accessoriCavettate[2] = "- 2 Cavi inox 1,5mm da: " + (int.Parse(H_TXBX.Text) + 150) + " mm + 4 manicotti 1,5 (04maniall015)";
            //        }
            //        else if (Supplemento2CMBX.Text.ToLower().Contains("perlon")) //guida perlon
            //        {
            //            if (Supplemento2CMBX.Text.ToLower().Contains("front")) accessoriCavettate[1] = "- 2 Piastrine trasparenti angolo (ZAW0084)";
            //            else accessoriCavettate[1] = "- 2 Piastrine trasparenti (ZAW0085)";
            //            accessoriCavettate[2] = "- 2 Cavi Nylon 2mm da: " + (int.Parse(H_TXBX.Text) + 150) + " mm + 4 morsetti (ZAT0063-0011-006-0)";
            //        }
            //        else
            //        {
            //            accessoriCavettate[0] = "";
            //            accessoriCavettate[1] = "";
            //            accessoriCavettate[2] = "";
            //            accessoriCavettate[3] = "";
            //            goto Salta;
            //        }

            //        if (AttacchiCBX.Text.ToLower().Contains("frontale"))
            //        {
            //            if (NoteCMBBX.Text.ToLower().Contains("interno casa")) accessoriCavettate[0] = "- Cp Staffe M front int casa";
            //            else accessoriCavettate[0] = "- Cp Staffe M front";
            //        }
            //        else accessoriCavettate[0] = "- Cp Staffe M soff";//AttacchiCBX soffitto // oppue a combobox vuota

            //        Salta:
            //        //----------------------------------------------------------------------------------------------------------- 
            //        //                                  fine gestione cavettate
            //        //-----------------------------------------------------------------------------------------------------------
            //        quantita_supplemento = 1;
            //        //--------------------------------------------------------------------------------------------------------------------                                                
            //        mixTaglioTubo = luceLperEtich - 29 + QuotaDoppiorullo - Quotamotore("Tubo43");//QuotaMotore è in mm
            //        //---------------------------------------------------------------------------------------------------------------------      
            //        if (Varie1TCMBX.Text == "Solo meccanica" | Varie1TCMBX.Text.Contains("meccanica"))//solo meccanica
            //        {
            //            articoloDB = articoliCB.Text.Replace("'", "''")
            //          + Environment.NewLine + Doppiorullo + " L  " + luceLperEtich + " " + Varie1TCMBX.Text;
            //            mixTeloFinita = mixTaglioTubo - 6 + "x ??";
            //            //  H_TXBX.Enabled = false;
            //            //  H_TXBX.Text = "0";
            //            CostoTessutoTXBX.Text = "0";
            //            CostomeccanicaSemplice(int.Parse(L_TXBX.Text));
            //        }
            //        else
            //        {
            //            articoloDB = articoliCB.Text.Replace("'", "''")
            //            + Environment.NewLine + Doppiorullo + "  " + luceLperEtich + "x" + H_TXBX.Text + " " + Varie1TCMBX.Text;

            //            string[] CC;
            //            CC = Varie1TCMBX.Text.Split(' ');
            //            mixTeloFinita = mixTaglioTubo - 6 + "x" + (((int.Parse(H_TXBX.Text) + Tessuti.GetTessutoByName(CC[0]).Arrotolamento_su_tubo)) * Tessuti.GetTessutoByName(CC[0]).Tessuto_NeD);

            //            // H_TXBX.Enabled = true;

            //            L_TXBX.AutoResetTextHasChanged = false; VersioneCMBX.AutoResetTextHasChanged = false; Varie1TCMBX.AutoResetTextHasChanged = false;
            //            CostomeccanicaSemplice(int.Parse(L_TXBX.Text));
            //            L_TXBX.AutoResetTextHasChanged = true; VersioneCMBX.AutoResetTextHasChanged = false; Varie1TCMBX.AutoResetTextHasChanged = true;
            //            CostoTessuto(int.Parse(L_TXBX.Text));
            //        }
            //    }
            //    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            //    else if (VersioneCMBX.Text == "Affiancato asimmetrico" || VersioneCMBX.Text == "Affiancato asimmetrico Doppio")
            //    {
            //        quantita_supplemento = 2;
            //        if (L2_TXBX.Text != "")
            //        {
            //            luceLperEtich_Page1 = (int)Math.Round(int.Parse(L2_TXBX.Text) + (luceL / 2d), 1);
            //            luceLperEtich_Page2 = (int)Math.Round(int.Parse(L_TXBX.Text) + (luceL / 2d) - int.Parse(L2_TXBX.Text), 1);

            //            if ((int.Parse(L2_TXBX.Text) + (luceL / 2)) >= (int.Parse(L_TXBX.Text) + (luceL / 2)))
            //            {
            //                MessageBox.Show("La larghezza di 1 Anta non può essere uguale o maggiore della larghezza totale!", "P_Sun", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                L2_TXBX.Text = "";
            //                L2_TXBX.Focus();
            //                return;
            //            }
            //            //--------------------------------------------------------------------------------------------------------------------                    
            //            mixTaglioTubo = (int)Math.Round(luceLperEtich_Page1 - 20 + (QuotaDoppiorullo / 2d), 1) - Quotamotore("Tubo43");//verificare
            //            mixTaglioTubo2 = (int)Math.Round(luceLperEtich_Page2 - 20 + (QuotaDoppiorullo / 2d), 1) - Quotamotore("Tubo43");
            //            //---------------------------------------------------------------------------------------------------------------------                                   
            //            if (Varie1TCMBX.Text == "Solo meccanica" | Varie1TCMBX.Text.Contains("meccanica"))//solo meccanica
            //            {
            //                articoloDB = articoliCB.Text.Replace("'", "''")
            //            + Environment.NewLine + Doppiorullo + " Affianc. asimm. L " + luceLperEtich + "  " + Varie1TCMBX.Text;
            //                //  H_TXBX.Enabled = false;
            //                // H_TXBX.Text = "0";
            //                CostoTessutoTXBX.Text = "0";
            //                mixTeloFinita = mixTaglioTubo - 6 + "x ??";
            //                mixTeloFinita_Page2 = mixTaglioTubo2 - 6 + "x ??";

            //                float Costo_provvisorio;
            //                L_TXBX.AutoResetTextHasChanged = false; VersioneCMBX.AutoResetTextHasChanged = false; Varie1TCMBX.AutoResetTextHasChanged = false; L2_TXBX.AutoResetTextHasChanged = false;
            //                CostomeccanicaSemplice((int)luceLperEtich_Page1);
            //                L_TXBX.AutoResetTextHasChanged = true; VersioneCMBX.AutoResetTextHasChanged = true; Varie1TCMBX.AutoResetTextHasChanged = true; L2_TXBX.AutoResetTextHasChanged = true;
            //                Costo_provvisorio = float.Parse(CostoMeccanicaTXBX.Text);
            //                if (CostomeccanicaSemplice((int)luceLperEtich_Page2) && Costo_provvisorio != 0) CostoMeccanicaTXBX.Text = (float.Parse(CostoMeccanicaTXBX.Text) + Costo_provvisorio).ToString();
            //                if (Costo_provvisorio == 0) CostoMeccanicaTXBX.Text = "0";
            //            }
            //            else
            //            {
            //                articoloDB = articoliCB.Text.Replace("'", "''")
            //                + Environment.NewLine + Doppiorullo + " Affianc. asimm. " + luceLperEtich + "x" + H_TXBX.Text + "  " + Varie1TCMBX.Text;
            //                // H_TXBX.Enabled = true;
            //                string[] CC;
            //                CC = Varie1TCMBX.Text.Split(' ');

            //                mixTeloFinita = mixTaglioTubo - 6 + "x" + ((int.Parse(H_TXBX.Text) + Tessuti.GetTessutoByName(CC[0]).Arrotolamento_su_tubo) * Tessuti.GetTessutoByName(CC[0]).Tessuto_NeD);
            //                mixTeloFinita_Page2 = mixTaglioTubo2 - 6 + "x" + ((int.Parse(H_TXBX.Text) + Tessuti.GetTessutoByName(CC[0]).Arrotolamento_su_tubo) * Tessuti.GetTessutoByName(CC[0]).Tessuto_NeD);

            //                float Costo_provvisorio;
            //                L_TXBX.AutoResetTextHasChanged = false; H_TXBX.AutoResetTextHasChanged = false; VersioneCMBX.AutoResetTextHasChanged = false; Varie1TCMBX.AutoResetTextHasChanged = false; L2_TXBX.AutoResetTextHasChanged = false;
            //                CostomeccanicaSemplice((int)luceLperEtich_Page1);
            //                Costo_provvisorio = float.Parse(CostoMeccanicaTXBX.Text);
            //                if (CostomeccanicaSemplice((int)luceLperEtich_Page2) && Costo_provvisorio != 0) CostoMeccanicaTXBX.Text = (float.Parse(CostoMeccanicaTXBX.Text) + Costo_provvisorio).ToString();
            //                if (Costo_provvisorio == 0) CostoMeccanicaTXBX.Text = "0";

            //                CostoTessuto((int)luceLperEtich_Page1);
            //                Costo_provvisorio = float.Parse(CostoTessutoTXBX.Text);
            //                L_TXBX.AutoResetTextHasChanged = true; H_TXBX.AutoResetTextHasChanged = true; VersioneCMBX.AutoResetTextHasChanged = true; Varie1TCMBX.AutoResetTextHasChanged = true; L2_TXBX.AutoResetTextHasChanged = true;
            //                if (CostoTessuto((int)luceLperEtich_Page2) && Costo_provvisorio != 0)
            //                    CostoTessutoTXBX.Text = (float.Parse(CostoTessutoTXBX.Text) + Costo_provvisorio).ToString();
            //                if (Costo_provvisorio == 0) CostoTessutoTXBX.Text = "0";

            //            }
            //        }
            //        else
            //        {
            //            luceLperEtich = 0; luceHperEtich = 0;
            //            mixTeloFinita = "0"; mixTeloFinita_Page2 = "0";
            //            mixTaglioTubo = 0;
            //            mixTaglioTubo2 = 0;

            //            luceLperEtich_Page1 = 0; luceLperEtich_Page2 = 0; luceLperEtich = 0; luceHperEtich = 0;
            //            ImponibTXBX.Text = ""; IvatoTXBX.Text = "";
            //        }
            //    }
            //    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            //    else if (VersioneCMBX.Text == "Affiancato" || VersioneCMBX.Text == "Affiancato Doppio")
            //    {
            //        quantita_supplemento = 2;
            //        luceLperEtich_Page1 = (int)Math.Round((int.Parse(L_TXBX.Text) + luceL) / 2d, 1);
            //        luceLperEtich_Page2 = (int)Math.Round((int.Parse(L_TXBX.Text) + luceL) / 2d, 1);
            //        //--------------------------------------------------------------------------------------------------------------------                   
            //        mixTaglioTubo = (int)Math.Round(luceLperEtich_Page1 - 20 + (QuotaDoppiorullo / 2f), 1) - Quotamotore("Tubo43");
            //        mixTaglioTubo2 = (int)Math.Round(luceLperEtich_Page1 - 20 + (QuotaDoppiorullo / 2f), 1) - Quotamotore("Tubo43");


            //        //mixTaglioTubo = Math.Round((luceLperEtich_Page1 - 2 + (QuotaDoppiorullo / 2)) * 10, 1) - Quotamotore("Tubo43");
            //        //mixTaglioTubo2 = Math.Round((luceLperEtich_Page1 - 2 + (QuotaDoppiorullo / 2)) * 10, 1) - Quotamotore("Tubo43");


            //        //---------------------------------------------------------------------------------------------------------------------                                   
            //        if (Varie1TCMBX.Text == "Solo meccanica" | Varie1TCMBX.Text.Contains("meccanica"))//solo meccanica
            //        {
            //            articoloDB = articoliCB.Text.Replace("'", "''")
            //        + Environment.NewLine + Doppiorullo + " Affianc. L " + luceLperEtich + "  " + Varie1TCMBX.Text;
            //            // H_TXBX.Enabled = false;
            //            // H_TXBX.Text = "0";
            //            CostoTessutoTXBX.Text = "0";
            //            mixTeloFinita = mixTaglioTubo - 6 + "x ??";
            //            mixTeloFinita_Page2 = mixTeloFinita;

            //            if (CostomeccanicaSemplice((int)luceLperEtich_Page1)) CostoMeccanicaTXBX.Text = (float.Parse(CostoMeccanicaTXBX.Text) * 2).ToString();
            //        }
            //        else
            //        {
            //            articoloDB = articoliCB.Text.Replace("'", "''")
            //            + Environment.NewLine + Doppiorullo + " Affianc. " + luceLperEtich + "x" + H_TXBX.Text + "  " + Varie1TCMBX.Text;
            //            //  H_TXBX.Enabled = true;
            //            string[] CC;
            //            CC = Varie1TCMBX.Text.Split(' ');

            //            mixTeloFinita = mixTaglioTubo - 6 + "x" + ((int.Parse(H_TXBX.Text) + Tessuti.GetTessutoByName(CC[0]).Arrotolamento_su_tubo) * Tessuti.GetTessutoByName(CC[0]).Tessuto_NeD);
            //            mixTeloFinita_Page2 = mixTaglioTubo - 6 + "x" + ((int.Parse(H_TXBX.Text) + Tessuti.GetTessutoByName(CC[0]).Arrotolamento_su_tubo) * Tessuti.GetTessutoByName(CC[0]).Tessuto_NeD);

            //            L_TXBX.AutoResetTextHasChanged = false; VersioneCMBX.AutoResetTextHasChanged = false; Varie1TCMBX.AutoResetTextHasChanged = false; L2_TXBX.AutoResetTextHasChanged = false;
            //            if (CostomeccanicaSemplice((int)luceLperEtich_Page1)) CostoMeccanicaTXBX.Text = (float.Parse(CostoMeccanicaTXBX.Text) * 2).ToString();

            //            L_TXBX.AutoResetTextHasChanged = true; VersioneCMBX.AutoResetTextHasChanged = true; Varie1TCMBX.AutoResetTextHasChanged = true;
            //            if (CostoTessuto((int)luceLperEtich_Page1)) CostoTessutoTXBX.Text = (float.Parse(CostoTessutoTXBX.Text) * 2).ToString();
            //        }







            //        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                                    
            //    }
            //    else
            //    {
            //        MessageBox.Show("Stringa in versione non corretta", "P_Sun", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        VersioneCMBX.Text = ""; VersioneCMBX.Focus();
            //    }
            //    //8888888888888888888888888888888888888888888888888888888888888888888888888 Supplementi 888888888888888888888888888888888888888888888888888888888888888888
            //    Calcolo_QuantitaRullo();
            //    if (Supplemento1CMBX.Text != "") Calcolo_supplemento1(int.Parse(PezziTXBX.Text) * quantita_supplemento, (int.Parse(PezziTXBX.Text) * quantita_supplemento).ToString());
            //    Calcolo_supplemento2(int.Parse(PezziTXBX.Text) * quantita_supplemento, (int.Parse(PezziTXBX.Text) * quantita_supplemento).ToString());
            //    Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);
            //    //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //}
            //else
            //{
            //    luceLperEtich = 0; luceHperEtich = 0;
            //    mixTeloFinita = "0";
            //    mixTaglioTubo = 0;
            //    mixTaglioTubo2 = 0;
            //    luceLperEtich_Page1 = 0; luceLperEtich_Page2 = 0;
            //    luceLperEtich = 0; luceHperEtich = 0;
            //    ImponibTXBX.Text = ""; IvatoTXBX.Text = "";
           // }
        }
        private void RulloAngolarMotore()
        {
            Aggiorna();
        }
        private void RulloMedioMotorePortarullo()
        {
            Aggiorna();
        }
        private void RulloAngolarMotorePortarullo()
        {
            Aggiorna();
        }
        private void RulloAngolarGrandeDemoltiplicato()
        {
            Aggiorna();
        }
        private void RulloGrandeDemoltEInox(float Quotatubo, float Quotafondale, float Quotatessuto, string Nomerullo)
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }
            Aggiorna();


            //string[] accessoriCavettate = { "", "", "" };

            //if (LuceRBN.Checked == true) luceL = -2;
            //else luceL = 0;

            //if (flagForCMBXitems == false)
            //{
            //    ConfigurazioneRulloCatena();
            //    NoteCMBBX.Items.Clear();
            //    NoteCMBBX.Items.Add("Discesa interno casa");

            //    if (!Nomerullo.ToLower().Contains("inox"))
            //    {
            //        NoteCMBBX.Items.Add("Fondale rettangolare");
            //        NoteCMBBX.Items.Add("Fond.in tasca");
            //        NoteCMBBX.Items.Add("Disc.interno casa//Fond.rettangolare");
            //        NoteCMBBX.Items.Add("Disc.interno casa//Fond.in tasca");
            //    }
            //    else
            //    {
            //        Supplemento2CMBX.Items.Clear();
            //        Supplemento2CMBX.Items.Add("Attacco frontale guida inox");
            //    }

            //    AttacchiCBX.Text = "Soffitto";
            //    flagForCMBXitems = true;
            //    if (ColoreCMBX.Items.Count >= 2) ColoreCMBX.SelectedIndex = 1;
            //}

            //if (H_TXBX.Text != "" && L_TXBX.Text != "")//&& Varie1TCMBX.Text != ""
            //{
            //    luceLperEtich = int.Parse(L_TXBX.Text) + luceL;
            //    luceHperEtich = int.Parse(H_TXBX.Text) + luceH;
            //    //--------------------------------------------------------------------------------------------------------------------               
            //    mixTaglioTubo = luceLperEtich - Quotatubo;
            //    mixTaglioFondaleManiglia = luceLperEtich - Quotafondale;

            //    //----------------------------------------------------------------------------------------------------------- 
            //    //                                    gestione cavettate
            //    //-----------------------------------------------------------------------------------------------------------

            //    if (AttacchiCBX.Text.Contains("front") & NoteCMBBX.Text.Contains("interno casa"))
            //    {
            //        accessoriCavettate[0] = "- cp staffe R.grande front/int casa (dx-sx)";
            //        accessoriCavettate[1] = "- crimpare cavo con terminale testa batt. Ø 3 inox";
            //    }
            //    else if (AttacchiCBX.Text.Contains("front") & !NoteCMBBX.Text.Contains("interno casa"))
            //    {
            //        accessoriCavettate[0] = "- cp staffe R.grande front (dx-sx)";
            //        accessoriCavettate[1] = "- crimpare cavo con terminale testa battente Ø 3 inox";
            //    }
            //    else
            //    {
            //        accessoriCavettate[0] = "- cp staffe R.grande soffitto (dx-sx)";
            //        accessoriCavettate[1] = "- 2 Fermi cilintr. con grano 3mm inox";
            //    }
            //    if (Supplemento2CMBX.Text.Contains("Attacco frontale guida inox")) accessoriCavettate[2] = "- cp Attacco frontale guida inox??";
            //    else accessoriCavettate[2] = "- 2 Tenditori a pavimento(101750) ";

            //    //----------------------------------------------------------------------------------------------------------- 
            //    //                                  fine gestione cavettate
            //    //-----------------------------------------------------------------------------------------------------------

            //    if (Varie1TCMBX.Text == "Solo meccanica" | Varie1TCMBX.Text.Contains("meccanica"))//solo meccanica
            //    {
            //        articoloDB = articoliCB.Text.Replace("'", "''")
            //      + Environment.NewLine + " L  " + luceLperEtich + " " + Varie1TCMBX.Text;
            //        mixTeloFinita = luceLperEtich - Quotatessuto + "x ??";

            //        CostoTessutoTXBX.Text = "0";
            //        CostomeccanicaSemplice(int.Parse(L_TXBX.Text));
            //    }
            //    else
            //    {
            //        articoloDB = articoliCB.Text.Replace("'", "''")
            //       + Environment.NewLine + "   " + luceLperEtich + "x" + H_TXBX.Text + " " + Varie1TCMBX.Text;
            //        mixTeloFinita = luceLperEtich - Quotatessuto + "x" + (int.Parse(H_TXBX.Text) + 400);

            //        L_TXBX.AutoResetTextHasChanged = false; Varie1TCMBX.AutoResetTextHasChanged = false;
            //        CostomeccanicaSemplice(int.Parse(L_TXBX.Text));
            //        L_TXBX.AutoResetTextHasChanged = true; Varie1TCMBX.AutoResetTextHasChanged = true;
            //        CostoTessuto(int.Parse(L_TXBX.Text));
            //    }
            //}
            //else
            //{
            //    luceLperEtich = 0;
            //    luceHperEtich = 0;
            //    mixTeloFinita = "0";

            //    mixTaglioTubo = 0;
            //    mixTaglioFondaleManiglia = 0;
            //}
            ////8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //Calcolo_QuantitaRullo();
            //Calcolo_supplemento1_CatenaMetallo(PezziTXBX.Text);
            //Calcolo_supplemento2(float.Parse(QuantitaTXBX.Text), PezziTXBX.Text);
            //Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);
            ////8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888



        }
        private void RulloGrandeMotoreEInox(float Quotatubo, int Quotafondale, string Nomerullo)
        {
            Aggiorna();
        }
        private void RulloAngolarGrandeMotore()
        {
            Aggiorna();
        }
        private void RulloMaxiMotorCaviInox(int Quotafondale, string v)
        {
            Aggiorna();
        }
        private void RulloMega()
        {
            Aggiorna();
        }
        private void RulloCass50Molla()
        {
            Aggiorna();
        }
        private void RulloCass50Cat()
        {
            Aggiorna();
        }
        private void RulloCass50Motor()
        {
            Aggiorna();
        }
        private void Rullo_Cass_63_83_cat(float Quotacassone, float Quotatubo, float Quotatessuto, string Nomerullo)
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }
            Aggiorna();



            //string[] accessoriCavettate = { "", "", "", "" };// 

            //if (LuceRBN.Checked == true) luceL = -3;
            //else luceL = 0;

            //if (flagForCMBXitems == false)
            //{
            //    ConfigurazioneRulloCatena();
            //    NoteCMBBX.Items.Clear();
            //    NoteCMBBX.Items.Add("Fondale rettangolare");
            //    NoteCMBBX.Items.Add("Fond.in tasca");
            //    Supplemento2CMBX.Items.Clear();
            //    Supplemento2CMBX.Items.Add("Guida Perlon");
            //    Supplemento2CMBX.Items.Add("Guida Perlon front");
            //    Supplemento2CMBX.Items.Add("Guida Inox");
            //    flagForCMBXitems = true;
            //    if (ColoreCMBX.Items.Count >= 3) ColoreCMBX.SelectedIndex = 2;
            //}

            //if (H_TXBX.Text != "" && L_TXBX.Text != "")//&& Varie1TCMBX.Text != ""
            //{
            //    #region gestione cavetti
            //    //----------------------------------------------------------------------------------------------------------- 
            //    //                                    gestione cavettate
            //    //-----------------------------------------------------------------------------------------------------------                   

            //    if (Supplemento2CMBX.Text.ToLower().Contains("inox"))
            //    {
            //        accessoriCavettate[1] = "- 2 Fissaggio a muro guid. cavo (35300005)";
            //        accessoriCavettate[2] = "- 2 Cavi inox 1,5mm da: " + (int.Parse(H_TXBX.Text) + 150) + " mm + 4 manicotti 1,5 (04maniall015)";
            //        accessoriCavettate[0] = "- Cp Tappi guide???? escono 2 mm per lato rispetto al cassone";
            //    }
            //    else if (Supplemento2CMBX.Text.ToLower().Contains("perlon")) //guida perlon
            //    {
            //        if (Supplemento2CMBX.Text.ToLower().Contains("front")) accessoriCavettate[1] = "- 2 Piastrine trasparenti angolo (ZAW0084)";
            //        else accessoriCavettate[1] = "- 2 Piastrine trasparenti (ZAW0085)";
            //        accessoriCavettate[2] = "- 2 Cavi Nylon 2mm da: " + (int.Parse(H_TXBX.Text) + 150) + " mm + 4 morsetti (ZAT0063-0011-006-0)";
            //        accessoriCavettate[0] = "- Cp Tappi guide???? escono 2 mm per lato rispetto al cassone";
            //    }
            //    else
            //    {
            //        accessoriCavettate[0] = "";
            //        accessoriCavettate[1] = "";
            //        accessoriCavettate[2] = "";
            //        accessoriCavettate[3] = "";
            //    }

            //    //----------------------------------------------------------------------------------------------------------- 
            //    //                                  fine gestione cavettate
            //    //-----------------------------------------------------------------------------------------------------------
            //    #endregion


            //    luceLperEtich = int.Parse(L_TXBX.Text) + luceL;
            //    luceHperEtich = int.Parse(H_TXBX.Text) + luceH;
            //    //--------------------------------------------------------------------------------------------------------------------               
            //    mixTaglioCassonetto = luceLperEtich - Quotacassone;
            //    mixTaglioTubo = luceLperEtich - Quotatubo;
            //    //---------------------------------------------------------------------------------------------------------------------      
            //    if (Varie1TCMBX.Text == "Solo meccanica" | Varie1TCMBX.Text.Contains("meccanica"))//solo meccanica
            //    {
            //        articoloDB = articoliCB.Text.Replace("'", "''")
            //      + Environment.NewLine + " L  " + luceLperEtich + " " + Varie1TCMBX.Text;
            //        mixTeloFinita = mixTaglioTubo - 6 + "x ??";

            //        CostoTessutoTXBX.Text = "0";
            //        CostomeccanicaSemplice(int.Parse(L_TXBX.Text));
            //    }
            //    else
            //    {
            //        articoloDB = articoliCB.Text.Replace("'", "''")
            //       + Environment.NewLine + "   " + luceLperEtich + "x" + H_TXBX.Text + " " + Varie1TCMBX.Text;
            //        mixTeloFinita = mixTaglioTubo - 6 + "x" + (int.Parse(H_TXBX.Text) + 200);
            //        //   H_TXBX.Enabled = true;

            //        L_TXBX.AutoResetTextHasChanged = false; Varie1TCMBX.AutoResetTextHasChanged = false;
            //        CostomeccanicaSemplice(int.Parse(L_TXBX.Text));
            //        L_TXBX.AutoResetTextHasChanged = true; Varie1TCMBX.AutoResetTextHasChanged = true;
            //        CostoTessuto(int.Parse(L_TXBX.Text));
            //    }
            //}
            //else
            //{
            //    luceLperEtich = 0;
            //    luceHperEtich = 0;
            //    mixTeloFinita = "0";

            //    mixTaglioCassonetto = 0;
            //    mixTaglioTubo = 0;
            //}

            ////8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);
            //Calcolo_QuantitaRullo();
            //Calcolo_supplemento1_CatenaMetallo(PezziTXBX.Text);
            //Calcolo_supplemento2(float.Parse(QuantitaTXBX.Text), PezziTXBX.Text);
            ////8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888


        }
        private void Rullo_Cass_63_83_110_mot(float Quotacassone, float Quotatubo, string Tiporullo, string Nomerullo)//aggiungere tipo tubo
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }
            Aggiorna();
        }
        private void Rullo_Cass_83_mot_caviInox(float Quotacassone, float Quotatubo)
        {
            Aggiorna();
        }
        private void Rullo_Cass_110_mot_caviInox(float Quotacassone, float Quotatubo)
        {
            Aggiorna();
        }
        private void Rullo_Cass_63_83_cat_con_guida_ZIP(float Quotacassone, float Quotatubo, float Quotafondale, float Quotaguide, float Quotatessuto, string Nomerullo)
        {
            Aggiorna();
        }
        private void Rullo_Cass_63_83_mot_con_guida(float Quotacassone, float Quotatubo, float Quotaguide, float Quotatessuto, string Nomerullo)
        {
            //if (LuceRBN.Checked == true) { luceL = -3; luceH = -1; }
            //else { luceL = 0; luceH = 0; }

            //if (flagForCMBXitems == false)
            //{
            //    ConfigurazioneRulloMotore("Tubo43");
            //    flagForCMBXitems = true;
            //    if (ColoreCMBX.Items.Count >= 3) ColoreCMBX.SelectedIndex = 2;
            //}

            //if (H_TXBX.Text != "" && L_TXBX.Text != "")//&& Varie1TCMBX.Text != ""
            //{
            //    luceLperEtich = int.Parse(L_TXBX.Text) + luceL;
            //    luceHperEtich = int.Parse(H_TXBX.Text) + luceH;
            //    articoloDB = articoliCB.Text.Replace("'", "''")
            // + Environment.NewLine + "   " + L_TXBX.Text + "x" + H_TXBX.Text + " " + Varie1TCMBX.Text;

            //    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////                                            
            //    mixTaglioCassonetto = luceLperEtich - Quotacassone;
            //    mixTaglioTubo = luceLperEtich - Quotatubo - Quotamotore("Tubo43");//
            //    mixTeloFinita = (luceLperEtich - Quotatessuto) + "x" + (int.Parse(H_TXBX.Text) + 200); // mixTaglioTubo - 12 + "x" + Math.Round((float.Parse(H_TXBX.Text.Replace(".", ",")) + 20) * 10);
            //    mixTaglioFondaleManiglia = luceLperEtich - 123;
            //    if (AttacchiCBX.Text != "" && AttacchiCBX.Text != "laterale") Quotaguide += 3;
            //    mixTaglioGuida = luceHperEtich - Quotaguide;
            //    //---------------------------------------------------------------------------------------------------------------------                 
            //    if (Varie1TCMBX.Text == "Solo meccanica" | Varie1TCMBX.Text.Contains("meccanica"))//solo meccanica
            //    {
            //        CostoTessutoTXBX.Text = "0";
            //        CostomeccanicaTabella(int.Parse(L_TXBX.Text), int.Parse(H_TXBX.Text));
            //    }
            //    else
            //    {
            //        L_TXBX.AutoResetTextHasChanged = false; H_TXBX.AutoResetTextHasChanged = false; Varie1TCMBX.AutoResetTextHasChanged = false;
            //        CostomeccanicaTabella(int.Parse(L_TXBX.Text), int.Parse(H_TXBX.Text));
            //        L_TXBX.AutoResetTextHasChanged = true; Varie1TCMBX.AutoResetTextHasChanged = true; H_TXBX.AutoResetTextHasChanged = true;
            //        CostoTessuto(int.Parse(L_TXBX.Text));
            //    }
            //}
            //else
            //{
            //    luceLperEtich = 0;
            //    luceHperEtich = 0;
            //    mixTeloFinita = "0";

            //    mixTaglioCassonetto = 0;
            //    mixTaglioTubo = 0;
            //    mixTaglioGuida = 0;

            //    CostoTessutoTXBX.Text = "0";
            //    CostoMeccanicaTXBX.Text = "0";
            //}
            ////8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //Calcolo_QuantitaRullo();
            //Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);
            //Calcolo_supplemento1(int.Parse(PezziTXBX.Text), PezziTXBX.Text);
            ////8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888

            Aggiorna();
        }
        private void Lucernaio63()
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }
            Aggiorna();

            //if (LuceRBN.Checked == true) { luceL = -2; luceH = -2; }
            //else { luceL = 0; luceH = 0; }

            //if (flagForCMBXitems == false)
            //{
            //    ConfigurazioneRulloMotore("Tubo43");
            //    if (ColoreCMBX.Items.Count >= 3) ColoreCMBX.SelectedIndex = 2;
            //    flagForCMBXitems = true;
            //}

            //if (H_TXBX.Text != "" && L_TXBX.Text != "")
            //{
            //    luceLperEtich = int.Parse(L_TXBX.Text) + luceL;
            //    luceHperEtich = int.Parse(H_TXBX.Text) + luceH;
            //    articoloDB = articoliCB.Text.Replace("'", "''")
            //+ Environment.NewLine + "   " + L_TXBX.Text + "x" + H_TXBX.Text + " " + Varie1TCMBX.Text;
            //    ////////////////////////////////////////////////////////////////////////////////////////////                                         
            //    mixTeloFinita = (luceLperEtich - 44) + "x" + (int.Parse(H_TXBX.Text) + 200);
            //    //--------------------------------------------------------------------------------------------------------------------                
            //    mixTaglioCassonetto = luceLperEtich - 6;
            //    mixTaglioTubo = luceLperEtich - 16 - Quotamotore("Tubo43");//motore avvitato direttamente sulla testata senza dischetto

            //    mixTaglioTubo2 = luceLperEtich - 38;
            //    int Quotaguide = 0;
            //    if (AttacchiCBX.Text != "" && AttacchiCBX.Text != "laterale") Quotaguide += 5;
            //    mixTaglioGuida = luceHperEtich - 135 - Quotaguide;
            //    //---------------------------------------------------------------------------------------------------------------------                 
            //    if (Varie1TCMBX.Text == "Solo meccanica" | Varie1TCMBX.Text.Contains("meccanica"))//solo meccanica
            //    {
            //        CostoTessutoTXBX.Text = "0";
            //        CostomeccanicaTabella(int.Parse(L_TXBX.Text), int.Parse(H_TXBX.Text));
            //    }
            //    else
            //    {
            //        L_TXBX.AutoResetTextHasChanged = false; H_TXBX.AutoResetTextHasChanged = false; Varie1TCMBX.AutoResetTextHasChanged = false;
            //        CostomeccanicaTabella(int.Parse(L_TXBX.Text), int.Parse(H_TXBX.Text));
            //        L_TXBX.AutoResetTextHasChanged = true; Varie1TCMBX.AutoResetTextHasChanged = true; H_TXBX.AutoResetTextHasChanged = true;
            //        CostoTessuto(int.Parse(L_TXBX.Text));
            //    }
            //}
            //else
            //{
            //    luceLperEtich = 0;
            //    luceHperEtich = 0;
            //    mixTeloFinita = "0";

            //    mixTaglioCassonetto = 0;
            //    mixTaglioTubo = 0;
            //    mixTaglioTubo2 = 0;
            //    mixTaglioFondaleManiglia = 0;
            //    mixTaglioGuida = 0;

            //    CostoTessutoTXBX.Text = "0";
            //    CostoMeccanicaTXBX.Text = "0";
            //}
            ////8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);
            //Calcolo_supplemento1(int.Parse(PezziTXBX.Text), PezziTXBX.Text);
            //Calcolo_QuantitaRullo();
            ////8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
        }
        private void Lucernaio83_110(float Quotacassone, float Quotatubo, float Quotatubo2, float Quotaguide, string Nomerullo)
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }
            Aggiorna();


            //if (LuceRBN.Checked == true) { luceL = -3; luceH = -2; }
            //else { luceL = 0; luceH = 0; }

            //if (flagForCMBXitems == false)
            //{
            //    ConfigurazioneRulloMotore("Tubo43");
            //    Attacchi_label.Visible = false;
            //    AttacchiCBX.Visible = false;
            //    AttacchiCBX.Items.Clear();
            //    if (ColoreCMBX.Items.Count >= 3) ColoreCMBX.SelectedIndex = 2;
            //    flagForCMBXitems = true;
            //}

            //string[] steccatura = { "", "", "", "" };// la terza e' il numero di molle

            //if (H_TXBX.Text != "" && L_TXBX.Text != "")//&& Varie1TCMBX.Text != ""
            //{
            //    luceLperEtich = int.Parse(L_TXBX.Text) + luceL;
            //    luceHperEtich = int.Parse(H_TXBX.Text) + luceH;
            //    articoloDB = articoliCB.Text.Replace("'", "''")
            //  + Environment.NewLine + "   " + L_TXBX.Text + "x" + H_TXBX.Text + " " + Varie1TCMBX.Text;

            //    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////                                            
            //    mixTeloFinita = (luceLperEtich - 55) + "x" + (int.Parse(H_TXBX.Text) + 200);
            //    //--------------------------------------------------------------------------------------------------------------------
            //    mixTaglioCassonetto = luceLperEtich - Quotacassone;
            //    mixTaglioTubo = luceLperEtich - Quotatubo - Quotamotore("Tubo43");//
            //    mixTaglioTubo2 = luceLperEtich - Quotatubo2;//tubo molla
            //    mixTaglioFondaleManiglia = luceLperEtich - 45;
            //    //if (AttacchiCBX.Text != "" && AttacchiCBX.Text != "laterale") Quotaguide += 0.5f;Come si fanno a mettere gli attacchi soffitto/parete?
            //    mixTaglioGuida = luceHperEtich - Quotaguide;

            //    mixBandaFinita = (int)(luceHperEtich + 100);//cintino
            //                                                //----------------------------------------------------------------------------------------------------------------------------
            //                                                //-----------------------------------------------------------------------------------------------------------
            //                                                //                                 Gestione steccature e molle
            //                                                //-----------------------------------------------------------------------------------------------------------
            //    int mixtelo_x_Steccatura = (int)luceHperEtich - (int.Parse(Regex.Match(Nomerullo, @"\d+").Value) * 2);
            //    NoteArtVarieLabel.Text = "";
            //    if (luceLperEtich > 1000)//in generale (sotto 1ml di larghezza, no stecche) sotto i 200 una steccatura, sopra 2 steccature, quindi (200-8,3 -8,3=183) (200-11 -11 = 178) non considero il fondale perche la tasca deve stare in mezzo ai cassoni
            //    {
            //        if (luceHperEtich > 2000)// 2 steccatura
            //        {
            //            steccatura[0] = Math.Round((mixtelo_x_Steccatura / 3) - 20d) /*mix terminale*/ + " da tasca fatta";
            //            steccatura[1] = Math.Round((mixtelo_x_Steccatura / 3 * 2) - 20d) + " da tasca fatta, verificare accavallamento stecca";
            //            steccatura[2] = "+2 strisce da 50 mm";
            //        }
            //        else if (luceHperEtich > 1300)//  1 steccatura
            //        {
            //            steccatura[0] = Math.Round(mixtelo_x_Steccatura / 2d) - 20 /*mix terminale*/ + " da tasca fatta";
            //            steccatura[1] = "";
            //            steccatura[2] = "+1 striscia da 50 mm";
            //        }
            //        else if (luceHperEtich > 1200 && luceLperEtich > 1400)//  1 steccatura
            //        {
            //            steccatura[0] = Math.Round(mixtelo_x_Steccatura / 2d) - 20 /*mix terminale*/ + " da tasca fatta";
            //            steccatura[1] = "";
            //            steccatura[2] = "+1 striscia da 5 cm";
            //        }
            //        else// no steccatura
            //        {
            //            steccatura[0] = "no";
            //            steccatura[1] = "";
            //            steccatura[2] = "";
            //        }
            //    }
            //    else
            //    {
            //        steccatura[0] = "no";
            //        steccatura[1] = "";
            //        steccatura[2] = "";
            //    }
            //    if (luceLperEtich > 1450 && luceHperEtich > 800)
            //    {
            //        if (luceLperEtich < 1530) steccatura[3] = "2 molle, Tagliare cannucce di 40 mm";
            //        else steccatura[3] = "2 molle";
            //        mixTaglioTubo2 -= 130;
            //    }
            //    else
            //    {
            //        if (luceLperEtich < 800)
            //        {
            //            steccatura[3] = "1 molla, Tagliare cannuccia di " + (800 - luceLperEtich) + "mm";
            //            if (luceHperEtich > 1700) NoteArtVarieLabel.Text = "non realizzabile, molla corta per questa H";
            //            else NoteArtVarieLabel.Text = "";
            //            if (luceLperEtich < 700) NoteArtVarieLabel.Text = "non realizzabile!";

            //        }
            //        else steccatura[3] = "1 molla";
            //    }
            //    //-----------------------------------------------------------------------------------------------------------
            //    //                             Fine Gestione steccature e molle
            //    //-----------------------------------------------------------------------------------------------------------                
            //    if (Varie1TCMBX.Text == "Solo meccanica" | Varie1TCMBX.Text.Contains("meccanica"))//solo meccanica
            //    {
            //        CostoTessutoTXBX.Text = "0";
            //        CostomeccanicaTabella(int.Parse(L_TXBX.Text), int.Parse(H_TXBX.Text));
            //    }
            //    else
            //    {
            //        L_TXBX.AutoResetTextHasChanged = false; H_TXBX.AutoResetTextHasChanged = false; Varie1TCMBX.AutoResetTextHasChanged = false;
            //        CostomeccanicaTabella(int.Parse(L_TXBX.Text), int.Parse(H_TXBX.Text));
            //        L_TXBX.AutoResetTextHasChanged = true; Varie1TCMBX.AutoResetTextHasChanged = true; H_TXBX.AutoResetTextHasChanged = true;
            //        CostoTessuto(int.Parse(L_TXBX.Text));
            //    }
            //}
            //else
            //{
            //    luceLperEtich = 0;
            //    luceHperEtich = 0;
            //    mixTeloFinita = "0";

            //    mixTaglioCassonetto = 0;
            //    mixTaglioTubo = 0;
            //    mixTaglioTubo2 = 0;
            //    mixTaglioFondaleManiglia = 0;
            //    mixTaglioGuida = 0;

            //    CostoTessutoTXBX.Text = "0";
            //    CostoMeccanicaTXBX.Text = "0";
            //}
            ////8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);
            //Calcolo_supplemento1(int.Parse(PezziTXBX.Text), PezziTXBX.Text);
            //Calcolo_QuantitaRullo();
            ////8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
        }
        private void RulloAMolla()
        {
            Aggiorna();
        }
        private void RulloAMollaGrandePortarullo()
        {
            Aggiorna();
        }
        private void RulloNedCassonettoMotore()
        {
            Aggiorna();
        }
        private void RulloNedCassonetto()
        {
            Aggiorna();
        }
        private void RulloDecorativoCat(float Quotatubo, float Quotatessuto, string Nomerullo, string Annotazione_su_etichetta)
        {
            Aggiorna();
        }
        private void RulloDecorativoMot(float Quotatubo, string Nomerullo, string Annotazione_su_etichetta)
        {
            Aggiorna();
        }
        private void MantovanaLateraliAccessori()
        {
            Aggiorna();
        }
        private void RulloMantovanaCatena()
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta { Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaRulloMantovanaCatena = new EtichettaRulloMantovanaCatena(etichetta))
            {
                GraphicsDrawable1 = EtichettaRulloMantovanaCatena;
                DrawingSurface1 = EtichettaRulloMantovanaCatena.ToBitmap();
            }
            Aggiorna();


            //string tappiFresati = "Tappi fresati in alluminio";
            //string tappiFresatiangolari = "Tappi angolari fresati in alluminio";
            //int quantita_supplemento = 1;//per calcolare il raddoppio dei supplementi quando sono affiancate
            //if (LuceRBN.Checked == true) luceL = -4;
            //else luceL = 0;

            //if (flagForCMBXitems == false)
            //{
            //    ConfigurazioneRulloCatena();
            //    NoteCMBBX.Items.Clear();
            //    NoteCMBBX.Items.Add("Discesa interno casa");
            //    NoteCMBBX.Items.Add("Fond.in tasca");
            //    NoteCMBBX.Items.Add("Disc.interno casa//Fond.in tasca");
            //    Attacchi_label.Visible = true;
            //    AttacchiCBX.Visible = true;
            //    AttacchiCBX.Items.Clear();
            //    AttacchiCBX.Items.Add("Soffitto");
            //    AttacchiCBX.Items.Add("Frontale");
            //    AttacchiCBX.Text = "Frontale";

            //    VersioneCMBX.Visible = true; VersioneLBL.Visible = true;
            //    VersioneCMBX.Items.Clear();
            //    VersioneCMBX.Items.Add("angolo _|____|_ (30)");
            //    VersioneCMBX.Items.Add("angolo singolo ____|_ (30)");
            //    VersioneCMBX.Items.Add("angolo singolo _|____ (30)");
            //    VersioneCMBX.Items.Add("");
            //    VersioneCMBX.Items.Add("angolo _|__'__|_ (30) Affiancato");
            //    VersioneCMBX.Items.Add("angolo singolo ___'__|_ (30) Affiancato");
            //    VersioneCMBX.Items.Add("angolo singolo _|__'___ (30) Affiancato");
            //    VersioneCMBX.Items.Add("angolo _|__'__|_ (30) Affiancato asimmetrico");
            //    VersioneCMBX.Items.Add("angolo singolo ___'__|_ (30) Affiancato asimmetrico");
            //    VersioneCMBX.Items.Add("angolo singolo _|__'___ (30) Affiancato asimmetrico");
            //    VersioneCMBX.Items.Add("");
            //    VersioneCMBX.Items.Add("angolo _|__|__|_ (30) Affiancato con spall.");
            //    VersioneCMBX.Items.Add("angolo singolo ___|__|_ (30) Affiancato con spall.");
            //    VersioneCMBX.Items.Add("angolo singolo _|__|___ (30) Affiancato con spall.");
            //    VersioneCMBX.Items.Add("angolo _|__|__|_ (30) Affiancato asimmetrico con spall.");
            //    VersioneCMBX.Items.Add("angolo singolo ___|__|_ (30) Affiancato asimmetrico con spall.");
            //    VersioneCMBX.Items.Add("angolo singolo _|__|___ (30) Affiancato asimmetrico con spall.");
            //    VersioneCMBX.Items.Add("");
            //    VersioneCMBX.Items.Add("angolo 90° |____|");
            //    VersioneCMBX.Items.Add("angolo singolo 90° ____|");
            //    VersioneCMBX.Items.Add("angolo singolo 90° |____");
            //    VersioneCMBX.Items.Add("");
            //    VersioneCMBX.Items.Add("angolo 90° |__'__| Affiancato");
            //    VersioneCMBX.Items.Add("angolo singolo 90° __'__| Affiancato");
            //    VersioneCMBX.Items.Add("angolo singolo 90° |__'__ Affiancato");
            //    VersioneCMBX.Items.Add("angolo 90° |__'__| Affiancato asimmetrico");
            //    VersioneCMBX.Items.Add("angolo singolo 90° __'__| Affiancato asimmetrico");
            //    VersioneCMBX.Items.Add("angolo singolo 90° |__'__ Affiancato asimmetrico");
            //    VersioneCMBX.Items.Add("");
            //    VersioneCMBX.Items.Add("angolo 90° |__|__| Affiancato con spall.");
            //    VersioneCMBX.Items.Add("angolo singolo 90° __|__| Affiancato con spall.");
            //    VersioneCMBX.Items.Add("angolo singolo 90° |__|__ Affiancato con spall.");
            //    VersioneCMBX.Items.Add("angolo 90° |__|__| Affiancato asimmetrico con spall.");
            //    VersioneCMBX.Items.Add("angolo singolo 90° __|__| Affiancato asimmetrico con spall.");
            //    VersioneCMBX.Items.Add("angolo singolo 90° |__|__ Affiancato asimmetrico con spall.");

            //    VersioneCMBX.Text = "angolo _|____|_ (30)";


            //    Supplemento2CMBX.Items.Add(tappiFresati);

            //    H2_label.Visible = true; H2_TXBX.Visible = true; H2_TXBX.Text = "100"; H2_label.Text = "Laterali";
            //    NoteArtVarieLabel.Text = "mix esterno laterali-";

            //    // UM = "PZ";//Perche' manca caricadati                
            //    flagForCMBXitems = true;
            //    if (ColoreCMBX.Items.Count >= 1) ColoreCMBX.SelectedIndex = 0;

            //    flagForCMBXitems = true;
            //}
            //////PPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPP
            //if (VersioneCMBX.Text.Contains("asimmetrico")) { L2_label.Visible = true; L2_TXBX.Visible = true; }
            //else { L2_label.Visible = false; L2_TXBX.Visible = false; L2_TXBX.Text = ""; }


            //string[] CC;
            //CC = Varie1TCMBX.Text.Split(' ');//tessuti

            //Elementivari elementivari;
            //elementivari.copriviti = "";
            //elementivari.coverstaffa = "";
            //elementivari.staffaDX = "";
            //elementivari.staffaSX = "";
            //elementivari.staffaCentrale = "staffa centrale";
            //elementivari.squadrette = "";
            //elementivari.attacchiSoffitto = "";
            //elementivari.fermacatena = "";
            //elementivari.coestruso = "";
            //elementivari.tappoMantovana = "";
            //elementivari.lateraleCentrale = "";
            //elementivari.tipoStaffa = "";
            //elementivari.spessori = "";
            //elementivari.tipostaffaEspessore = "";
            //elementivari.numeroLaterali = "";

            //int quotaattaccosoffito_spall = 0;
            //int quotaportarullo = 0;
            //#endregion

            //if (H_TXBX.Text != "" && L_TXBX.Text != "" && H2_TXBX.Text != "")//&& Varie1TCMBX.Text != ""
            //{
            //    luceLperEtich = int.Parse(L_TXBX.Text) + luceL;
            //    luceHperEtich = int.Parse(H_TXBX.Text) + luceH;
            //    //**************************************************** Laterali *****************************************************************
            //    mixTaglioProfilo = int.Parse(H2_TXBX.Text);

            //    if (VersioneCMBX.Text.Contains("asimmetrico")) { L2_label.Visible = true; L2_TXBX.Visible = true; }
            //    else { L2_label.Visible = false; L2_TXBX.Visible = false; L2_TXBX.Text = ""; }

            //    if (AttacchiCBX.Text == "Frontale")//spessori
            //    {
            //        elementivari.tipostaffaEspessore = "staffa";
            //        if (H2_TXBX.Text == "") { MessageBox.Show("Ci deve essere una cifra in laterali!", "P_Sun", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); H2_TXBX.Text = "10"; H2_TXBX.Focus(); }
            //        else if (int.Parse(H2_TXBX.Text) > 100 && int.Parse(H2_TXBX.Text) < 130)
            //        {
            //            elementivari.spessori = "Spessori per mantovana da " + (int.Parse(H2_TXBX.Text) - 100) + "mm";
            //            elementivari.tipostaffaEspessore = "staffa + spessore";//MOD.nuovo
            //            elementivari.staffaCentrale = "staffa centrale";
            //        }
            //        else if (int.Parse(H2_TXBX.Text) == 130)
            //        {
            //            elementivari.spessori = "";
            //            elementivari.tipoStaffa = "Staffe prolungate";
            //            elementivari.tipostaffaEspessore = "staffa prolungata";
            //            elementivari.staffaCentrale = "staffa centrale prolung.";
            //        }
            //        else if (int.Parse(H2_TXBX.Text) > 130 && int.Parse(H2_TXBX.Text) <= 150)
            //        {
            //            elementivari.spessori = "Spess. per mantov. da " + (int.Parse(H2_TXBX.Text) - 130) + "mm ";//+ staffe prolungate";
            //            elementivari.tipoStaffa = "Staffe prolungate";
            //            elementivari.tipostaffaEspessore = "st. prolung. + spess.";
            //            elementivari.staffaCentrale = "staf.centrale prolung.";
            //        }
            //        else if (int.Parse(H2_TXBX.Text) > 150)
            //        {
            //            elementivari.spessori = "- Tagliare le staffe prolung.";
            //            elementivari.tipoStaffa = "Staffe prolungate";
            //            elementivari.tipostaffaEspessore = "staffa tagliata";
            //            elementivari.staffaCentrale = "staffa centrale prolung. tagliata";

            //        }
            //        else if (int.Parse(H2_TXBX.Text) < 100) { MessageBox.Show("Impossibile fare laterali meno di 10cm!", "P_Sun", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); H2_TXBX.Text = "100"; H2_TXBX.Focus(); }
            //        else elementivari.spessori = "";
            //    }

            //    if (VersioneCMBX.Text.Contains("("))//mixTaglioProfiloSuperiore MANTOVANA
            //    {
            //        if (int.TryParse(VersioneCMBX.Text.Split('(', ')')[1], out int output)) mixTaglioProfiloSuperiore = int.Parse(L_TXBX.Text) + luceL + ((output - (Supplemento2CMBX.Text == tappiFresati ? 13 : 0)) * (VersioneCMBX.Text.Contains("singolo") ? 1 : 2));
            //        else
            //        {
            //            MessageBox.Show("Stringa in versione non corretta!" + "\n" + "Inserisci numero tra le parentesi", "P_Sun", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            mixTaglioProfiloSuperiore = 0;
            //            VersioneCMBX.Select();
            //        }
            //    }
            //    else //90°
            //    {
            //        mixTaglioProfiloSuperiore = int.Parse(L_TXBX.Text) + luceL + (VersioneCMBX.Text.Contains("singolo") ? -13 : -26);
            //        if (Supplemento2CMBX.Text != tappiFresati)
            //        {
            //            Supplemento2CMBX.Text = tappiFresati;
            //            Supplemento2CMBX.Focus();//questo focus serve per convalidare
            //        }
            //        if (Supplemento2CMBX.Text != "" && Costo_supplemento2TXBX.Text == "0")//se si toglie anualmente "Tappi fresati in alluminio" non viene aggiornato ilvo di questo if prezzo, moti
            //        {
            //            Supplemento2CMBX.Focus();//questo focus serve per convalidare
            //        }

            //    }


            //    #region gestione_mantovana
            //    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //    //**************************************************** INIZIO gestione Mantovana  *********************************************************             
            //    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////         

            //    // spessore mantovana parte centrale: 9.6mm(19+1°), parte grossa 13.4mm(27), cover 1.8mm(3.6)
            //    //°poi c'è un millimetro? che il comando non tocca la staffa in quanto c'è il dentino del profilo della mantovana 
            //    // - 3.6 per rullo medio    Quindi se la mix tenda è 100 il tubo 96,4 senza cover 96.8

            //    // affiancate si toglie 3cm ogni tenda  quindi se la tenda è 100 il tubo 47 senza cover 47,2
            //    // soffitto: 2.7f - 0.4
            //    // parete : 2.1 -0.4 (2.1= 1.9 + 2mm di comendo che non tocca la staffa)
            //    //  soffitto con laterale solo ad un lato = 1.3f - 0.2f;(/2=0.6)

            //    switch (VersioneCMBX.Text)
            //    {
            //        case string a when a.Contains("angolo _|____|_"):

            //            elementivari.copriviti = "-2 Copriviti"; elementivari.squadrette = "-2 Squadrette con grani";
            //            elementivari.numeroLaterali = "2";

            //            if (Supplemento2CMBX.Text == tappiFresati) elementivari.tappoMantovana = "-2 Tappi fresati in alluminio";
            //            else elementivari.tappoMantovana = "-2 Tappi mantovana in PVC";

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 27 - 4;//
            //                elementivari.attacchiSoffitto = "-" + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "attacchi a soffitto", uno: 1000, due: 2100, tre: 2800, quattro: 3500, cinque: 4200, sei: 4900, sette: 5800);
            //                if (ComandiCMBX.Text.Contains("dx") || ComandiCMBX.Text.Contains("DX"))
            //                {
            //                    elementivari.staffaDX = "- montare staffa lato comando a DX a soffitto";
            //                    elementivari.staffaSX = "- montare staffa lato calotta a SX a soffitto";
            //                }
            //                else
            //                {
            //                    elementivari.staffaDX = "- montare staffa lato calotta a DX a soffitto";
            //                    elementivari.staffaSX = "- montare staffa lato comando a SX a soffitto";
            //                }

            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 20 - 4;

            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-2 " + elementivari.spessori /*+ " MOD.nuovo"*/;
            //                }
            //                if (ComandiCMBX.Text.Contains("dx") || ComandiCMBX.Text.Contains("DX"))
            //                {
            //                    elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato comando a DX a parete";
            //                    elementivari.staffaSX = "- montare " + elementivari.tipostaffaEspessore + " lato calotta a SX a parete";
            //                }
            //                else
            //                {
            //                    elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato calotta a DX a parete";
            //                    elementivari.staffaSX = "- montare " + elementivari.tipostaffaEspessore + " lato comando a SX a parete";
            //                }
            //            }

            //            break;
            //        case string a when a.Contains("angolo singolo ____|_"):

            //            elementivari.copriviti = "-2 Copriviti";
            //            elementivari.numeroLaterali = "1";

            //            if (Supplemento2CMBX.Text == tappiFresati) elementivari.tappoMantovana = "-2 Tappi fresati in alluminio";
            //            else elementivari.tappoMantovana = "-2 Tappi mantovana in PVC";

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 13 - 2;
            //                elementivari.attacchiSoffitto = "-" + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "attacchi a soffitto", uno: 0, due: 650, tre: 1350, quattro: 2450, cinque: 3750, sei: 4650, sette: 5550);
            //                elementivari.squadrette = "-1 Squadretta con grani";
            //                if (ComandiCMBX.Text.Contains("dx") || ComandiCMBX.Text.Contains("DX"))
            //                {
            //                    elementivari.staffaDX = "- montare staffa lato comando a DX a soffitto";
            //                    elementivari.staffaSX = "- staffa lato calotta con cover";
            //                }
            //                else
            //                {
            //                    elementivari.staffaDX = "- montare staffa lato calotta a DX a soffitto";
            //                    elementivari.staffaSX = "- staffa lato comando con cover";
            //                }

            //            }
            //            else//parete
            //            {
            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa senza laterale più di 15cm");
            //                elementivari.squadrette = "-1 Squadrette con grani e 1 modificata?";

            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-1 " + elementivari.spessori + " + 1 spessore medio";
            //                }
            //                if (ComandiCMBX.Text.Contains("dx") || ComandiCMBX.Text.Contains("DX"))
            //                {
            //                    quotaattaccosoffito_spall = 11 - 2;
            //                    elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato comando a DX a parete";
            //                    elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato calotta con cover";
            //                }
            //                else
            //                {
            //                    quotaattaccosoffito_spall = 10 - 2;
            //                    elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato calotta a DX a parete";
            //                    elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato comando con cover";
            //                }
            //            }

            //            break;
            //        case string a when a.Contains("angolo singolo _|____"):

            //            elementivari.copriviti = "-2 Copriviti";
            //            elementivari.numeroLaterali = "1";

            //            if (Supplemento2CMBX.Text == tappiFresati) elementivari.tappoMantovana = "-2 Tappi fresati in alluminio";
            //            else elementivari.tappoMantovana = "-2 Tappi mantovana in PVC";

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 13 - 2;
            //                elementivari.attacchiSoffitto = "-" + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "attacchi a soffitto", uno: 0, due: 650, tre: 1350, quattro: 2450, cinque: 3750, sei: 4650, sette: 5550);
            //                elementivari.squadrette = "-1 Squadretta con grani";
            //                if (ComandiCMBX.Text.Contains("dx") || ComandiCMBX.Text.Contains("DX"))
            //                {
            //                    elementivari.staffaDX = "- staffa lato comando con cover";
            //                    elementivari.staffaSX = "- montare staffa lato calotta a SX a soffitto";
            //                }
            //                else
            //                {
            //                    elementivari.staffaDX = "- staffa lato calotta con cover";
            //                    elementivari.staffaSX = "- montare staffa lato comando a SX a soffitto";
            //                }

            //            }
            //            else//parete
            //            {

            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa senza laterale più di 15cm");
            //                elementivari.squadrette = "-1 Squadrette con grani e 1 modificata?";

            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-1 " + elementivari.spessori + " + 1 spessore medio";
            //                }
            //                if (ComandiCMBX.Text.Contains("dx") || ComandiCMBX.Text.Contains("DX"))
            //                {
            //                    quotaattaccosoffito_spall = 10 - 2;
            //                    elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato calotta a SX a parete";
            //                    elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato comando con cover";
            //                }
            //                else
            //                {
            //                    quotaattaccosoffito_spall = 11 - 2;
            //                    elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato comando a SX a parete";
            //                    elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato calotta con cover";
            //                }
            //            }

            //            break;
            //        case string a when a.Contains("angolo _|__'__|_")://OK verificato 04/2025   

            //            elementivari.copriviti = "-3 Copriviti";
            //            elementivari.squadrette = "-2 Squadrette con grani";
            //            elementivari.numeroLaterali = "2";

            //            if (Supplemento2CMBX.Text == tappiFresati) elementivari.tappoMantovana = "-2 " + tappiFresati;
            //            else elementivari.tappoMantovana = "-2 Tappi mantovana in PVC";

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {

            //                quotaattaccosoffito_spall = 12;//( 2.7f - 0.4f)/2;
            //                elementivari.attacchiSoffitto = "-" + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "attacchi a soffitto", uno: 1000, due: 2100, tre: 2800, quattro: 3500, cinque: 4200, sei: 4900, sette: 5800);
            //                elementivari.staffaDX = "- montare le staffe lato comando ai laterali a soffitto";
            //                elementivari.staffaSX = "- 1 staffa cenrale";

            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 9;// (2.1f - 0.4f)/2;

            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa centrale più di 15cm");
            //                if (int.Parse(L_TXBX.Text) > 4000) MessageBox.Show("Larghezza troppo ampia, la mantovana potrebbe imbarcarsi, è consigliata la versione con spalletta centrale.");
            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-2 " + elementivari.spessori + " + 1 spessore medio";
            //                }


            //                elementivari.staffaSX = "- " + elementivari.staffaCentrale;

            //                elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato comando ai laterali a parete";

            //            }

            //            break;
            //        case string a when a.Contains("angolo singolo ___'__|_"):

            //            elementivari.copriviti = "-3 Copriviti";
            //            elementivari.numeroLaterali = "1";

            //            if (Supplemento2CMBX.Text == tappiFresati) elementivari.tappoMantovana = "-2 Tappi fresati in alluminio";
            //            else elementivari.tappoMantovana = "-2 Tappi mantovana in PVC";

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 6;
            //                elementivari.attacchiSoffitto = "-" + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "attacchi a soffitto", uno: 0, due: 650, tre: 1350, quattro: 2450, cinque: 3750, sei: 4650, sette: 5550);
            //                elementivari.squadrette = "-1 Squadretta con grani";

            //                elementivari.staffaDX = "- staffa lato comando con cover + " + elementivari.staffaCentrale;
            //                elementivari.staffaSX = "- montare staffa lato comando a DX a soffitto";

            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 5;

            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa centrale o esterna senza laterale più di 15cm");
            //                if (int.Parse(L_TXBX.Text) > 4000) MessageBox.Show("Larghezza troppo ampia, la mantovana potrebbe imbarcarsi, è consigliata la versione con spalletta centrale.");
            //                elementivari.squadrette = "-1 Squadrette con grani e 1 modificata?";

            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-1 " + elementivari.spessori + " + 2 spessori medio";
            //                }

            //                elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato comando a DX a parete";
            //                elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato comando con cover + " + elementivari.staffaCentrale;


            //            }

            //            break;
            //        case string a when a.Contains("angolo singolo _|__'___"):

            //            elementivari.copriviti = "-3 Copriviti";
            //            elementivari.numeroLaterali = "1";

            //            if (Supplemento2CMBX.Text == tappiFresati) elementivari.tappoMantovana = "-2 Tappi fresati in alluminio";
            //            else elementivari.tappoMantovana = "-2 Tappi mantovana in PVC";

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 6;
            //                elementivari.attacchiSoffitto = "-" + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "attacchi a soffitto", uno: 0, due: 650, tre: 1350, quattro: 2450, cinque: 3750, sei: 4650, sette: 5550);
            //                elementivari.squadrette = "-1 Squadretta con grani";

            //                elementivari.staffaDX = "- staffa lato comando con cover + " + elementivari.staffaCentrale;
            //                elementivari.staffaSX = "- montare staffa lato comando a SX a soffitto";

            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 5;

            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa centrale o esterna senza laterale più di 15cm");
            //                if (int.Parse(L_TXBX.Text) > 4000) MessageBox.Show("Larghezza troppo ampia, la mantovana potrebbe imbarcarsi, è consigliata la versione con spalletta centrale.");
            //                elementivari.squadrette = "-1 Squadrette con grani e 1 modificata?";

            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-1 " + elementivari.spessori + " + 2 spessori medio";
            //                }

            //                elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato comando a SX a parete";
            //                elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato comando con cover + " + elementivari.staffaCentrale;

            //            }

            //            break;
            //        case string a when a.Contains("angolo _|__|__|_"):

            //            elementivari.copriviti = "-3 Copriviti";
            //            elementivari.squadrette = "-3 Squadrette con grani";
            //            elementivari.numeroLaterali = "3";

            //            if (Supplemento2CMBX.Text == tappiFresati) elementivari.tappoMantovana = "-2 " + tappiFresati;
            //            else elementivari.tappoMantovana = "-2 Tappi mantovana in PVC";

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 0;

            //                elementivari.staffaDX = "";
            //                elementivari.staffaSX = "";
            //                MessageBox.Show("non e' possibile mettere il laterale centrale quando è a soffitto");
            //                AttacchiCBX.Text = "Frontale";

            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 0;

            //                elementivari.lateraleCentrale = "- staffa centrale vedi campione";
            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-3 " + elementivari.spessori;
            //                    elementivari.lateraleCentrale = "- staffa centr. con spess vedi campione";
            //                }
            //                elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato comando ai laterali a parete";
            //                elementivari.staffaSX = elementivari.lateraleCentrale;

            //            }

            //            break;
            //        case string a when a.Contains("angolo singolo ___|__|_"):

            //            elementivari.copriviti = "-3 Copriviti";
            //            elementivari.squadrette = "-2 Squadrette con grani + 1 modificata?";
            //            elementivari.numeroLaterali = "2";

            //            if (Supplemento2CMBX.Text == tappiFresati) elementivari.tappoMantovana = "-2 " + tappiFresati;
            //            else elementivari.tappoMantovana = "-2 Tappi mantovana in PVC";

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {

            //                quotaattaccosoffito_spall = 0;

            //                elementivari.staffaDX = "";
            //                elementivari.staffaSX = "";
            //                MessageBox.Show("non e' possibile mettere il laterale centrale quando è a soffitto");
            //                AttacchiCBX.Text = "Frontale";

            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 0;

            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa senza laterale più di 15cm");
            //                elementivari.squadrette = "-2 Squadrette con grani e 1 modificata?";

            //                elementivari.lateraleCentrale = "- staffa centrale vedi campione";
            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-2 " + elementivari.spessori + " + 1 spessore medio";
            //                    elementivari.lateraleCentrale = "- staffa centrale con spessore vedi campione";
            //                }
            //                elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato comando al laterale DX a parete";
            //                elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato com. e cover + " + elementivari.staffaCentrale + " vedi campione";

            //            }

            //            break;
            //        case string a when a.Contains("angolo singolo _|__|___"):

            //            elementivari.copriviti = "-3 Copriviti";
            //            elementivari.squadrette = "-2 Squadrette con grani + 1 modificata?";
            //            elementivari.numeroLaterali = "2";

            //            if (Supplemento2CMBX.Text == tappiFresati) elementivari.tappoMantovana = "-2 " + tappiFresati;
            //            else elementivari.tappoMantovana = "-2 Tappi mantovana in PVC";


            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 0;

            //                elementivari.staffaDX = "";
            //                elementivari.staffaSX = "";
            //                MessageBox.Show("non e' possibile mettere il laterale centrale quando è a soffitto");
            //                AttacchiCBX.Text = "Frontale";

            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 0;

            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa senza laterale più di 15cm");
            //                elementivari.squadrette = "-2 Squadrette con grani e 1 modificata?";

            //                elementivari.lateraleCentrale = "- staffa centrale vedi campione";
            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-2 " + elementivari.spessori + " + 1 spessore medio";
            //                    elementivari.lateraleCentrale = "- staffa centrale con spessore vedi campione";
            //                }
            //                elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato comando al laterale SX a parete";
            //                elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato com. e cover + " + elementivari.staffaCentrale + " vedi campione";

            //            }

            //            break;
            //        case string a when a.Contains("angolo 90° |____|"):

            //            elementivari.copriviti = "-2 Copriviti"; elementivari.numeroLaterali = "2";

            //            if (Supplemento2CMBX.Text == tappiFresati) elementivari.tappoMantovana = "-2 tappi angolari fresati";
            //            else
            //            {
            //                MessageBox.Show("Questa versione deve essere con i tappi fresati angolari!");
            //                Supplemento2CMBX.Focus();
            //            }

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 27 - 4;//
            //                elementivari.attacchiSoffitto = "-" + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "attacchi a soffitto", uno: 1000, due: 2100, tre: 2800, quattro: 3500, cinque: 4200, sei: 4900, sette: 5800);
            //                if (ComandiCMBX.Text.Contains("dx") || ComandiCMBX.Text.Contains("DX"))
            //                {
            //                    elementivari.staffaDX = "- montare staffa lato comando a DX a soffitto";
            //                    elementivari.staffaSX = "- montare staffa lato calotta a SX a soffitto";
            //                }
            //                else
            //                {
            //                    elementivari.staffaDX = "- montare staffa lato calotta a DX a soffitto";
            //                    elementivari.staffaSX = "- montare staffa lato comando a SX a soffitto";
            //                }
            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 20 - 4;//

            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-2 " + elementivari.spessori /*+ " MOD.nuovo"*/;
            //                }
            //                if (ComandiCMBX.Text.Contains("dx") || ComandiCMBX.Text.Contains("DX"))
            //                {
            //                    elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato comando a DX a parete";
            //                    elementivari.staffaSX = "- montare " + elementivari.tipostaffaEspessore + " lato calotta a SX a parete";
            //                }
            //                else
            //                {
            //                    elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato calotta a DX a parete";
            //                    elementivari.staffaSX = "- montare " + elementivari.tipostaffaEspessore + " lato comando a SX a parete";
            //                }
            //            }

            //            break;
            //        case string a when a.Contains("angolo singolo 90° ____|"):

            //            elementivari.copriviti = "-2 Copriviti";
            //            elementivari.numeroLaterali = "1";

            //            if (Supplemento2CMBX.Text == tappiFresati) elementivari.tappoMantovana = "-1 tappi angolare fresato e 1 laterale";
            //            else
            //            {
            //                MessageBox.Show("Questa versione deve essere con i tappi fresati angolari!");
            //                Supplemento2CMBX.Focus();
            //            }

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 13 - 2;
            //                elementivari.attacchiSoffitto = "-" + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "attacchi a soffitto", uno: 0, due: 650, tre: 1350, quattro: 2450, cinque: 3750, sei: 4650, sette: 5550);
            //                if (ComandiCMBX.Text.Contains("dx") || ComandiCMBX.Text.Contains("DX"))
            //                {
            //                    elementivari.staffaDX = "- montare staffa lato comando a DX a soffitto";
            //                    elementivari.staffaSX = "- staffa lato calotta con cover";
            //                }
            //                else
            //                {
            //                    elementivari.staffaDX = "- montare staffa lato calotta a DX a soffitto";
            //                    elementivari.staffaSX = "- staffa lato comando con cover";
            //                }

            //            }
            //            else//parete
            //            {

            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa senza laterale più di 150mm");
            //                elementivari.squadrette = "-1 squadetta modificata?";

            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-1 " + elementivari.spessori + " + 1 spessore medio";
            //                }
            //                if (ComandiCMBX.Text.Contains("dx") || ComandiCMBX.Text.Contains("DX"))
            //                {
            //                    quotaattaccosoffito_spall = 11 - 2;
            //                    elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato comando a DX a parete";
            //                    elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato calotta con cover";
            //                }
            //                else
            //                {
            //                    quotaattaccosoffito_spall = 10 - 2;
            //                    elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato calotta a DX a parete";
            //                    elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato comando con cover";
            //                }
            //            }

            //            break;
            //        case string a when a.Contains("angolo singolo 90° |____"):

            //            elementivari.copriviti = "-2 Copriviti";
            //            elementivari.numeroLaterali = "1";

            //            if (Supplemento2CMBX.Text == tappiFresati) elementivari.tappoMantovana = "-1 tappi angolare fresato e 1 laterale";
            //            else
            //            {
            //                MessageBox.Show("Questa versione deve essere con i tappi fresati angolari!");
            //                Supplemento2CMBX.Focus();
            //            }

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 13 - 2;
            //                elementivari.attacchiSoffitto = "-" + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "attacchi a soffitto", uno: 0, due: 650, tre: 1350, quattro: 2450, cinque: 3750, sei: 4650, sette: 5550);
            //                if (ComandiCMBX.Text.Contains("dx") || ComandiCMBX.Text.Contains("DX"))
            //                {
            //                    elementivari.staffaDX = "- montare staffa lato calotta a SX a soffitto";
            //                    elementivari.staffaSX = "- staffa lato comando con cover";
            //                }
            //                else
            //                {
            //                    elementivari.staffaDX = "- montare staffa lato comando a SX a soffitto";
            //                    elementivari.staffaSX = "- staffa lato calotta con cover";
            //                }

            //            }
            //            else//parete
            //            {

            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa senza laterale più di 150mm");
            //                elementivari.squadrette = "-1 squadetta modificata?";

            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-1 " + elementivari.spessori + " + 1 spessore medio";
            //                }
            //                if (ComandiCMBX.Text.Contains("dx") || ComandiCMBX.Text.Contains("DX"))
            //                {
            //                    quotaattaccosoffito_spall = 10 - 2;
            //                    elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato calotta a SX a parete";
            //                    elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato comando con cover";
            //                }
            //                else
            //                {
            //                    quotaattaccosoffito_spall = 11 - 2;
            //                    elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato comando a SX a parete";
            //                    elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato calotta con cover";
            //                }
            //            }

            //            break;
            //        case string a when a.Contains("angolo 90° |__'__|"):

            //            elementivari.copriviti = "-3 Copriviti";
            //            elementivari.numeroLaterali = "2";

            //            if (Supplemento2CMBX.Text == tappiFresati) elementivari.tappoMantovana = "-2 tappi angolari fresati";
            //            else
            //            {
            //                MessageBox.Show("Questa versione deve essere con i tappi fresati angolari!");
            //                Supplemento2CMBX.Focus();
            //            }

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 12;//( 2.7f - 0.4f)/2;
            //                elementivari.attacchiSoffitto = "-" + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "attacchi a soffitto", uno: 1000, due: 2100, tre: 2800, quattro: 3500, cinque: 4200, sei: 4900, sette: 5800);
            //                elementivari.staffaDX = "- montare le staffe lato comando ai laterali a soffitto";
            //                elementivari.staffaSX = "- 1 staffa cenrale";

            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 9;// (2.1f - 0.4f)/2;

            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa centrale più di 150mm");
            //                if (int.Parse(L_TXBX.Text) > 4000) MessageBox.Show("Larghezza troppo ampia, la mantovana potrebbe imbarcarsi, è consigliata la versione con spalletta centrale.");
            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-2 " + elementivari.spessori + " + 1 spessore medio";
            //                }


            //                elementivari.staffaSX = "- " + elementivari.staffaCentrale;

            //                elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato comando ai laterali a parete";

            //            }

            //            break;
            //        case string a when a.Contains("angolo singolo 90° __'__|"):

            //            elementivari.copriviti = "-3 Copriviti";
            //            elementivari.numeroLaterali = "1";

            //            if (Supplemento2CMBX.Text == tappiFresati) elementivari.tappoMantovana = "-1 tappi angolare fresato e 1 laterale";
            //            else
            //            {
            //                MessageBox.Show("Questa versione deve essere con i tappi fresati angolari!");
            //                Supplemento2CMBX.Focus();
            //            }

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 6;
            //                elementivari.attacchiSoffitto = "-" + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "attacchi a soffitto", uno: 0, due: 650, tre: 1350, quattro: 2450, cinque: 3750, sei: 4650, sette: 5550);
            //                elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato comando a DX a soffitto";
            //                elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato comando con cover + " + elementivari.staffaCentrale;
            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 5;

            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa centrale o esterna senza laterale più di 15cm");
            //                if (int.Parse(L_TXBX.Text) > 4000) MessageBox.Show("Larghezza troppo ampia, la mantovana potrebbe imbarcarsi, è consigliata la versione con spalletta centrale.");
            //                elementivari.squadrette = "-1 squadetta modificata?";

            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-1 " + elementivari.spessori + " + 2 spessori medio";
            //                }


            //                elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato comando a DX a parete";
            //                elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato comando con cover + " + elementivari.staffaCentrale;


            //            }

            //            break;
            //        case string a when a.Contains("angolo singolo 90° |__'__"):

            //            elementivari.copriviti = "-3 Copriviti";
            //            elementivari.numeroLaterali = "1";

            //            if (Supplemento2CMBX.Text == tappiFresati) elementivari.tappoMantovana = "-1 tappi angolare fresato e 1 laterale";
            //            else
            //            {
            //                MessageBox.Show("Questa versione deve essere con i tappi fresati angolari!");
            //                Supplemento2CMBX.Focus();
            //            }

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 6;
            //                elementivari.attacchiSoffitto = "-" + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "attacchi a soffitto", uno: 0, due: 650, tre: 1350, quattro: 2450, cinque: 3750, sei: 4650, sette: 5550);
            //                elementivari.staffaSX = "- montare staffa lato comando a SX a soffitto";
            //                elementivari.staffaDX = "- staffa lato comando con cover + " + elementivari.staffaCentrale;


            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 5;

            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa centrale o esterna senza laterale più di 150mm");
            //                if (int.Parse(L_TXBX.Text) > 4000) MessageBox.Show("Larghezza troppo ampia, la mantovana potrebbe imbarcarsi, è consigliata la versione con spalletta centrale.");
            //                elementivari.squadrette = "-1 squadetta modificata?";

            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-1 " + elementivari.spessori + " + 2 spessori medio";
            //                }

            //                elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato comando a DX a parete";
            //                elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato comando con cover + " + elementivari.staffaCentrale;


            //            }

            //            break;
            //        case string a when a.Contains("angolo 90° |__|__|"):

            //            elementivari.copriviti = "-3 Copriviti";
            //            elementivari.squadrette = "-1 Squadretta con grani";
            //            elementivari.numeroLaterali = "3";

            //            if (Supplemento2CMBX.Text == tappiFresati) elementivari.tappoMantovana = "-2 tappi angolari fresati";
            //            else
            //            {
            //                MessageBox.Show("Questa versione deve essere con i tappi fresati angolari!");
            //                Supplemento2CMBX.Focus();
            //            }

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 0;

            //                elementivari.staffaDX = "";
            //                elementivari.staffaSX = "";
            //                MessageBox.Show("non e' possibile mettere il laterale centrale quando è a soffitto");
            //                AttacchiCBX.Text = "Frontale";

            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 0;

            //                elementivari.lateraleCentrale = "- staffa centrale vedi campione";
            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolung"))
            //                {
            //                    elementivari.spessori = "-3 " + elementivari.spessori;
            //                    elementivari.lateraleCentrale = "- staffa centrale con spessore vedi campione";

            //                }

            //                elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato comando ai laterali a parete";
            //                elementivari.staffaSX = elementivari.lateraleCentrale;

            //            }

            //            break;
            //        case string a when a.Contains("angolo singolo 90° __|__|"):

            //            elementivari.copriviti = "-3 Copriviti";
            //            elementivari.squadrette = "-1 Squadretta con grani + 1 modificata?";
            //            elementivari.numeroLaterali = "2";

            //            if (Supplemento2CMBX.Text == tappiFresati) elementivari.tappoMantovana = "-1 tappi angolare fresato e 1 laterale";
            //            else
            //            {
            //                MessageBox.Show("Questa versione deve essere con i tappi fresati angolari!");
            //                Supplemento2CMBX.Focus();
            //            }

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 0;

            //                elementivari.staffaDX = "";
            //                elementivari.staffaSX = "";
            //                MessageBox.Show("non e' possibile mettere il laterale centrale quando è a soffitto");
            //                AttacchiCBX.Text = "Frontale";

            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 0;

            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa senza laterale più di 15cm");
            //                elementivari.squadrette = "-2 Squadrette con grani e 1 modificata?";

            //                elementivari.lateraleCentrale = "- staffa centrale vedi campione";
            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-2 " + elementivari.spessori + " + 1 spessore medio";
            //                    elementivari.lateraleCentrale = "- staffa centrale con spessore vedi campione";
            //                }
            //                elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato comando al laterale DX a parete";
            //                elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato com. e cover + " + elementivari.staffaCentrale + " vedi campione";
            //            }
            //            break;
            //        case string a when a.Contains("angolo singolo 90° |__|__")://solo frontal

            //            elementivari.copriviti = "-3 Copriviti";
            //            elementivari.squadrette = "-1 Squadretta con grani + 1 modificata?";
            //            elementivari.numeroLaterali = "2";

            //            if (Supplemento2CMBX.Text == tappiFresati) elementivari.tappoMantovana = "-1 tappi angolare fresato e 1 laterale";
            //            else
            //            {
            //                MessageBox.Show("Questa versione deve essere con i tappi fresati angolari!");
            //                Supplemento2CMBX.Focus();
            //            }

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 0;

            //                elementivari.staffaDX = "";
            //                elementivari.staffaSX = "";
            //                MessageBox.Show("non e' possibile mettere il laterale centrale quando è a soffitto");
            //                AttacchiCBX.Text = "Frontale";

            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 0;

            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa senza laterale più di 15cm");
            //                elementivari.squadrette = "-2 Squadrette con grani e 1 modificata?";

            //                elementivari.lateraleCentrale = "- staffa centrale vedi campione";
            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-2 " + elementivari.spessori + " + 1 spessore medio";
            //                    elementivari.lateraleCentrale = "- staffa centrale con spessore vedi campione";
            //                }
            //                elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato comando al laterale SX a parete";
            //                elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato com. e cover + " + elementivari.staffaCentrale + " vedi campione";
            //            }
            //            break;
            //        default:
            //            MessageBox.Show("Stringa in versione non corretta!", "P_Sun", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            VersioneCMBX.Text = "";
            //            VersioneCMBX.Focus();
            //            break;
            //    }
            //    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //    //**************************************************** FINE gestione Mantovana Frontale *******************************************            
            //    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////         
            //    #endregion

            //    if (HcTXBX.Text != "")
            //    {
            //        if (int.Parse(HcTXBX.Text) < (int.Parse(H_TXBX.Text) - 200) || Tessuti.GetTessutoByName(CC[0]).Tessuto_NeD == 2)
            //        {
            //            elementivari.coestruso = " con Coestruso piccolo";
            //            elementivari.fermacatena = "";
            //        }
            //        else
            //        {
            //            elementivari.coestruso = "";
            //            elementivari.fermacatena = (string.Concat(VersioneCMBX.Text.Contains("Affiancato") ? "-4" : "-2") + " fermacatena trasparenti");
            //        }
            //    }
            //    else if (H_TXBX.Text != "0") HcTXBX.Text = (int.Parse(H_TXBX.Text) - 200).ToString();

            //    //___________________________________________________________________________________________________________________________________________________

            //    switch (VersioneCMBX.Text)
            //    {
            //        #region 1 telo                   

            //        case string a when !a.Contains("Affiancato") && a.Contains("angolo"): //1 telo

            //            quantita_supplemento = 1;

            //            mixTaglioTubo = luceLperEtich - 36 - quotaattaccosoffito_spall;
            //            mixTaglioFondaleManiglia = mixTaglioTubo;


            //            //---------------------------------------------------------------------------------------------------------------------

            //            if (Varie1TCMBX.Text == "Solo meccanica" | Varie1TCMBX.Text.Contains("meccanica"))//solo meccanica
            //            {
            //                articoloDB = articoliCB.Text.Replace("'", "''")
            //              + Environment.NewLine + " L  " + luceLperEtich + " " + Varie1TCMBX.Text;
            //                mixTeloFinita = mixTaglioTubo - 6 + "x ??";
            //                //  H_TXBX.Enabled = false;
            //                // H_TXBX.Text = "0";
            //                CostoTessutoTXBX.Text = "0";
            //                CostomeccanicaSemplice(int.Parse(L_TXBX.Text));
            //            }
            //            else
            //            {
            //                articoloDB = articoliCB.Text.Replace("'", "''")
            //               + Environment.NewLine + "   " + luceLperEtich + "x" + H_TXBX.Text + " " + Varie1TCMBX.Text;

            //                mixTeloFinita = mixTaglioTubo - 6 + "x" + ((int.Parse(H_TXBX.Text) + Tessuti.GetTessutoByName(CC[0]).Arrotolamento_su_tubo) * Tessuti.GetTessutoByName(CC[0]).Tessuto_NeD);
            //                //  H_TXBX.Enabled = true;

            //                L_TXBX.AutoResetTextHasChanged = false; Varie1TCMBX.AutoResetTextHasChanged = false;
            //                CostomeccanicaSemplice(int.Parse(L_TXBX.Text));
            //                L_TXBX.AutoResetTextHasChanged = true; Varie1TCMBX.AutoResetTextHasChanged = true;
            //                CostoTessuto(int.Parse(L_TXBX.Text));
            //            }

            //            break;

            //        #endregion
            //        #region centrale

            //        case string a when a.Contains("Affiancato") & !a.Contains("asimmetrico"): // Centrale


            //            quantita_supplemento = 2;
            //            luceLperEtich_Page1 = (int)Math.Round((int.Parse(L_TXBX.Text) + luceL) / 2d, 1);
            //            luceLperEtich_Page2 = luceLperEtich_Page1;
            //            //--------------------------------------------------------------------------------------------------------------------                   
            //            mixTaglioTubo = (luceLperEtich_Page1 - 30 - quotaattaccosoffito_spall);
            //            mixTaglioTubo2 = mixTaglioTubo;
            //            mixTaglioFondaleManiglia = mixTaglioTubo;
            //            mixTaglioFondaleManiglia_Page2 = mixTaglioFondaleManiglia;//mixTaglioFondaleManiglia;
            //            mixTaglioPortarullo_page1 = luceLperEtich_Page1 - 36 - quotaportarullo;
            //            mixTaglioPortarullo_page2 = mixTaglioPortarullo_page1;
            //            //---------------------------------------------------------------------------------------------------------------------                                   
            //            if (Varie1TCMBX.Text == "Solo meccanica" | Varie1TCMBX.Text.Contains("meccanica"))//solo meccanica
            //            {
            //                articoloDB = articoliCB.Text.Replace("'", "''")
            //            + Environment.NewLine + " Affianc. L " + luceLperEtich + "  " + Varie1TCMBX.Text;

            //                CostoTessutoTXBX.Text = "0";
            //                mixTeloFinita = mixTaglioTubo - 6 + "x ??";
            //                mixTeloFinita_Page2 = mixTeloFinita;

            //                if (CostomeccanicaSemplice((int)luceLperEtich_Page1)) CostoMeccanicaTXBX.Text = (float.Parse(CostoMeccanicaTXBX.Text) * 2).ToString();
            //            }
            //            else
            //            {
            //                articoloDB = articoliCB.Text.Replace("'", "''")
            //                + Environment.NewLine + " Affianc. " + luceLperEtich + "x" + H_TXBX.Text + "  " + Varie1TCMBX.Text;

            //                mixTeloFinita = mixTaglioTubo - 6 + "x" + ((int.Parse(H_TXBX.Text) + Tessuti.GetTessutoByName(CC[0]).Arrotolamento_su_tubo) * Tessuti.GetTessutoByName(CC[0]).Tessuto_NeD);
            //                mixTeloFinita_Page2 = mixTeloFinita;

            //                L_TXBX.AutoResetTextHasChanged = false; VersioneCMBX.AutoResetTextHasChanged = false; Varie1TCMBX.AutoResetTextHasChanged = false; L2_TXBX.AutoResetTextHasChanged = false;
            //                if (CostomeccanicaSemplice((int)luceLperEtich_Page1)) CostoMeccanicaTXBX.Text = (float.Parse(CostoMeccanicaTXBX.Text) * 2).ToString();

            //                L_TXBX.AutoResetTextHasChanged = true; VersioneCMBX.AutoResetTextHasChanged = true; Varie1TCMBX.AutoResetTextHasChanged = true;
            //                if (CostoTessuto((int)luceLperEtich_Page1)) CostoTessutoTXBX.Text = (float.Parse(CostoTessutoTXBX.Text) * 2).ToString();
            //            }
            //            break;
            //        #endregion
            //        #region asimmetrico

            //        case string a when a.Contains("asimmetrico"): // asimmetrico

            //            quantita_supplemento = 2;
            //            if (L2_TXBX.Text != "")
            //            {

            //                luceLperEtich_Page1 = (int)Math.Round(int.Parse(L2_TXBX.Text) + (luceL / 2d), 1);
            //                luceLperEtich_Page2 = (int)Math.Round(int.Parse(L_TXBX.Text) + (luceL / 2d) - int.Parse(L2_TXBX.Text), 1);

            //                if ((int.Parse(L2_TXBX.Text) + (luceL / 2)) >= (int.Parse(L_TXBX.Text) + (luceL / 2)))
            //                {
            //                    MessageBox.Show("La larghezza di 1 Anta non può essere uguale o maggiore della larghezza totale!", "P_Sun", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                    L2_TXBX.Text = ""; L2_TXBX.Focus(); return;
            //                }
            //                //--------------------------------------------------------------------------------------------------------------------                   

            //                mixTaglioTubo = luceLperEtich_Page1 - 30 - quotaattaccosoffito_spall;
            //                mixTaglioTubo2 = luceLperEtich_Page2 - 30 - quotaattaccosoffito_spall;
            //                mixTaglioFondaleManiglia = mixTaglioTubo;
            //                mixTaglioFondaleManiglia_Page2 = mixTaglioTubo2;


            //                //---------------------------------------------------------------------------------------------------------------------                                   
            //                if (Varie1TCMBX.Text == "Solo meccanica" | Varie1TCMBX.Text.Contains("meccanica"))//solo meccanica
            //                {
            //                    articoloDB = articoliCB.Text.Replace("'", "''")
            //                + Environment.NewLine + " Affianc. asimm. L " + luceLperEtich + "  " + Varie1TCMBX.Text;

            //                    CostoTessutoTXBX.Text = "0";
            //                    mixTeloFinita = mixTaglioTubo - 6 + "x ??";
            //                    mixTeloFinita_Page2 = mixTaglioFondaleManiglia_Page2 - 6 + "x ??";

            //                    float Costo_provvisorio;
            //                    L_TXBX.AutoResetTextHasChanged = false; VersioneCMBX.AutoResetTextHasChanged = false; Varie1TCMBX.AutoResetTextHasChanged = false; L2_TXBX.AutoResetTextHasChanged = false;
            //                    CostomeccanicaSemplice((int)luceLperEtich_Page1);
            //                    L_TXBX.AutoResetTextHasChanged = true; VersioneCMBX.AutoResetTextHasChanged = true; Varie1TCMBX.AutoResetTextHasChanged = true; L2_TXBX.AutoResetTextHasChanged = true;
            //                    Costo_provvisorio = float.Parse(CostoMeccanicaTXBX.Text);
            //                    if (CostomeccanicaSemplice((int)luceLperEtich_Page2) && Costo_provvisorio != 0) CostoMeccanicaTXBX.Text = (float.Parse(CostoMeccanicaTXBX.Text) + Costo_provvisorio).ToString();
            //                    if (Costo_provvisorio == 0) CostoMeccanicaTXBX.Text = "0";
            //                }
            //                else
            //                {
            //                    articoloDB = articoliCB.Text.Replace("'", "''")
            //                    + Environment.NewLine + " Affianc. asimm. " + luceLperEtich + "x" + H_TXBX.Text + "  " + Varie1TCMBX.Text;

            //                    mixTeloFinita = mixTaglioTubo - 6 + "x" + ((int.Parse(H_TXBX.Text) + Tessuti.GetTessutoByName(CC[0]).Arrotolamento_su_tubo) * Tessuti.GetTessutoByName(CC[0]).Tessuto_NeD);
            //                    mixTeloFinita_Page2 = mixTaglioFondaleManiglia_Page2 - 6 + "x" + ((int.Parse(H_TXBX.Text) + Tessuti.GetTessutoByName(CC[0]).Arrotolamento_su_tubo) * Tessuti.GetTessutoByName(CC[0]).Tessuto_NeD);

            //                    float Costo_provvisorio;
            //                    L_TXBX.AutoResetTextHasChanged = false; H_TXBX.AutoResetTextHasChanged = false; VersioneCMBX.AutoResetTextHasChanged = false; Varie1TCMBX.AutoResetTextHasChanged = false; L2_TXBX.AutoResetTextHasChanged = false;
            //                    CostomeccanicaSemplice((int)luceLperEtich_Page1);
            //                    Costo_provvisorio = float.Parse(CostoMeccanicaTXBX.Text);
            //                    if (CostomeccanicaSemplice((int)luceLperEtich_Page2) && Costo_provvisorio != 0) CostoMeccanicaTXBX.Text = (float.Parse(CostoMeccanicaTXBX.Text) + Costo_provvisorio).ToString();
            //                    if (Costo_provvisorio == 0) CostoMeccanicaTXBX.Text = "0";

            //                    CostoTessuto((int)luceLperEtich_Page1);
            //                    Costo_provvisorio = float.Parse(CostoTessutoTXBX.Text);
            //                    L_TXBX.AutoResetTextHasChanged = true; H_TXBX.AutoResetTextHasChanged = true; VersioneCMBX.AutoResetTextHasChanged = true; Varie1TCMBX.AutoResetTextHasChanged = true; L2_TXBX.AutoResetTextHasChanged = true;
            //                    if (CostoTessuto((int)luceLperEtich_Page2) && Costo_provvisorio != 0)
            //                        CostoTessutoTXBX.Text = (float.Parse(CostoTessutoTXBX.Text) + Costo_provvisorio).ToString();
            //                    if (Costo_provvisorio == 0) CostoTessutoTXBX.Text = "0";
            //                }


            //            }
            //            else
            //            {
            //                luceLperEtich = 0; luceHperEtich = 0;
            //                mixTeloFinita = "0";
            //                mixTeloFinita_Page2 = "0";
            //                mixTaglioTubo = 0;
            //                mixTaglioTubo2 = 0;
            //                mixTaglioFondaleManiglia = 0;
            //                mixTaglioFondaleManiglia_Page2 = 0;
            //                luceLperEtich_Page1 = 0; luceLperEtich_Page2 = 0;
            //                luceLperEtich = 0; luceHperEtich = 0;
            //                ImponibTXBX.Text = ""; IvatoTXBX.Text = "";
            //            }
            //            break;
            //            #endregion
            //    }
            //    ////PPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPP
            //}
            //else
            //{
            //    luceLperEtich = 0;
            //    luceHperEtich = 0; mixTeloFinita = "0";
            //    mixTaglioProfiloSuperiore = 0;
            //    mixTaglioProfilo = 0;
            //    mixTaglioTubo = 0;
            //    mixTaglioFondaleManiglia = 0;
            //    mixTaglioFondaleManiglia_Page2 = 0;
            //    ImponibTXBX.Text = ""; IvatoTXBX.Text = "";
            //}
            ////8888888888888888888888888888888888888888888888888888888888888888888888 Supplementi 88888888888888888888888888888888888888888888888888
            //Calcolo_QuantitaRullo();
            //Calcolo_supplemento1_CatenaMetallo((int.Parse(PezziTXBX.Text) * quantita_supplemento).ToString());
            //Calcolo_supplemento2(float.Parse(QuantitaTXBX.Text), PezziTXBX.Text);
            //Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);
            ////8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
        }
        private void RulloMantovanaMotore()
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }
            Aggiorna();




            //#region assegnazione variabili

            //string tappiFresati = "Tappi fresati in alluminio";
            //string tappiFresatiangolari = "Tappi angolari fresati in alluminio";

            //int quantita_supplemento = 1;//per calcolare il raddoppio dei supplementi quando sono affiancate
            //if (LuceRBN.Checked == true) luceL = -4;
            //else luceL = 0;

            //if (flagForCMBXitems == false)
            //{
            //    ConfigurazioneRulloMotore("Tubo43");
            //    NoteCMBBX.Items.Clear();
            //    NoteCMBBX.Items.Add("Discesa interno casa");
            //    NoteCMBBX.Items.Add("Fond.in tasca");
            //    NoteCMBBX.Items.Add("Disc.interno casa//Fond.in tasca");
            //    Attacchi_label.Visible = true;
            //    AttacchiCBX.Visible = true;
            //    AttacchiCBX.Items.Clear();
            //    AttacchiCBX.Items.Add("Soffitto");
            //    AttacchiCBX.Items.Add("Frontale");
            //    AttacchiCBX.Text = "Frontale";
            //    VersioneLBL.Visible = true; VersioneCMBX.Visible = true;
            //    VersioneCMBX.Visible = true; VersioneLBL.Visible = true;
            //    VersioneCMBX.Items.Clear();
            //    VersioneCMBX.Items.Add("angolo _|____|_ (30)");
            //    VersioneCMBX.Items.Add("angolo singolo ____|_ (30)");
            //    VersioneCMBX.Items.Add("angolo singolo _|____ (30)");
            //    VersioneCMBX.Items.Add("");
            //    VersioneCMBX.Items.Add("angolo _|__'__|_ (30) Affiancato");
            //    VersioneCMBX.Items.Add("angolo singolo ___'__|_ (30) Affiancato");
            //    VersioneCMBX.Items.Add("angolo singolo _|__'___ (30) Affiancato");
            //    VersioneCMBX.Items.Add("angolo _|__'__|_ (30) Affiancato asimmetrico");
            //    VersioneCMBX.Items.Add("angolo singolo ___'__|_ (30) Affiancato asimmetrico");
            //    VersioneCMBX.Items.Add("angolo singolo _|__'___ (30) Affiancato asimmetrico");
            //    VersioneCMBX.Items.Add("");
            //    VersioneCMBX.Items.Add("angolo _|__|__|_ (30) Affiancato con spall.");
            //    VersioneCMBX.Items.Add("angolo singolo ___|__|_ (30) Affiancato con spall.");
            //    VersioneCMBX.Items.Add("angolo singolo _|__|___ (30) Affiancato con spall.");
            //    VersioneCMBX.Items.Add("angolo _|__|__|_ (30) Affiancato asimmetrico con spall.");
            //    VersioneCMBX.Items.Add("angolo singolo ___|__|_ (30) Affiancato asimmetrico con spall.");
            //    VersioneCMBX.Items.Add("angolo singolo _|__|___ (30) Affiancato asimmetrico con spall.");
            //    VersioneCMBX.Items.Add("");
            //    VersioneCMBX.Items.Add("angolo 90° |____|");
            //    VersioneCMBX.Items.Add("angolo singolo 90° ____|");
            //    VersioneCMBX.Items.Add("angolo singolo 90° |____");
            //    VersioneCMBX.Items.Add("");
            //    VersioneCMBX.Items.Add("angolo 90° |__'__| Affiancato");
            //    VersioneCMBX.Items.Add("angolo singolo 90° __'__| Affiancato");
            //    VersioneCMBX.Items.Add("angolo singolo 90° |__'__ Affiancato");
            //    VersioneCMBX.Items.Add("angolo 90° |__'__| Affiancato asimmetrico");
            //    VersioneCMBX.Items.Add("angolo singolo 90° __'__| Affiancato asimmetrico");
            //    VersioneCMBX.Items.Add("angolo singolo 90° |__'__ Affiancato asimmetrico");
            //    VersioneCMBX.Items.Add("");
            //    VersioneCMBX.Items.Add("angolo 90° |__|__| Affiancato con spall.");
            //    VersioneCMBX.Items.Add("angolo singolo 90° __|__| Affiancato con spall.");
            //    VersioneCMBX.Items.Add("angolo singolo 90° |__|__ Affiancato con spall.");
            //    VersioneCMBX.Items.Add("angolo 90° |__|__| Affiancato asimmetrico con spall.");
            //    VersioneCMBX.Items.Add("angolo singolo 90° __|__| Affiancato asimmetrico con spall.");
            //    VersioneCMBX.Items.Add("angolo singolo 90° |__|__ Affiancato asimmetrico con spall.");

            //    VersioneCMBX.Text = "angolo _|____|_ (30)";


            //    Supplemento2CMBX.Items.Add(tappiFresati);

            //    H2_label.Visible = true; H2_TXBX.Visible = true; H2_TXBX.Text = "100"; H2_label.Text = "Laterali";
            //    NoteArtVarieLabel.Text = "mix esterno laterali-verificato solo front unico e |__|__| front";

            //    //  UM = "PZ";//Perche' manca caricadati                
            //    flagForCMBXitems = true;
            //    if (ColoreCMBX.Items.Count >= 1) ColoreCMBX.SelectedIndex = 0;
            //}

            //if (VersioneCMBX.Text.Contains("asimmetrico")) { L2_label.Visible = true; L2_TXBX.Visible = true; }
            //else { L2_label.Visible = false; L2_TXBX.Visible = false; L2_TXBX.Text = ""; }//

            //string[] CC;
            //CC = Varie1TCMBX.Text.Split(' ');

            //Elementivari elementivari;
            //elementivari.copriviti = "";
            //elementivari.coverstaffa = "";
            //elementivari.staffaDX = "";
            //elementivari.staffaSX = "";
            //elementivari.staffaCentrale = "staffa centrale";
            //elementivari.squadrette = "";
            //elementivari.attacchiSoffitto = "";
            //elementivari.coestruso = "";
            //elementivari.tappoMantovana = "";
            //elementivari.lateraleCentrale = "";
            //elementivari.tipoStaffa = "";
            //elementivari.spessori = "";
            //elementivari.tipostaffaEspessore = "";
            //elementivari.numeroLaterali = "";

            //int quotaattaccosoffito_spall = 0;

            //#endregion

            //if (H_TXBX.Text != "" && L_TXBX.Text != "" && H2_TXBX.Text != "")//&& Varie1TCMBX.Text != ""
            //{
            //    //00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
            //    luceLperEtich = int.Parse(L_TXBX.Text) + luceL;
            //    luceHperEtich = int.Parse(H_TXBX.Text) + luceH;
            //    //**************************************************** Laterali *****************************************************************
            //    mixTaglioProfilo = int.Parse(H2_TXBX.Text);

            //    #region gestione laterale vecchio

            //    #endregion

            //    if (AttacchiCBX.Text == "Frontale")//spessori
            //    {
            //        elementivari.tipostaffaEspessore = "staffa";
            //        if (H2_TXBX.Text == "") { MessageBox.Show("Ci deve essere una cifra in laterali!", "P_Sun", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); H2_TXBX.Text = "10"; H2_TXBX.Focus(); }
            //        else if (int.Parse(H2_TXBX.Text) > 100 && int.Parse(H2_TXBX.Text) < 130)
            //        {
            //            elementivari.spessori = "Spessori per mantovana da " + (int.Parse(H2_TXBX.Text) - 100).ToString() + "mm";
            //            elementivari.tipostaffaEspessore = "staffa + spessore";
            //            elementivari.staffaCentrale = "staffa centrale";
            //        }
            //        else if (int.Parse(H2_TXBX.Text) == 130)
            //        {
            //            elementivari.spessori = "";
            //            elementivari.tipoStaffa = "Staffe prolungate modificata";
            //            elementivari.tipostaffaEspessore = "staffa prolung.modificata";
            //            elementivari.staffaCentrale = "staffa centrale prolung.";
            //        }
            //        else if (int.Parse(H2_TXBX.Text) > 130 && int.Parse(H2_TXBX.Text) <= 150)
            //        {
            //            elementivari.spessori = "Spessori MOD.vecchio per mantovana da " + (int.Parse(H2_TXBX.Text) - 130) + "mm ";//+ staffe prolungate";
            //            elementivari.tipoStaffa = "staffa prolung.modificata";
            //            elementivari.tipostaffaEspessore = "st. prolung.modif.+spess.";
            //            elementivari.staffaCentrale = "staffa centr. prolung.";
            //        }
            //        else if (int.Parse(H2_TXBX.Text) > 150)
            //        {
            //            elementivari.spessori = "- Tagliare le staffe MOD vecchio";
            //            elementivari.tipoStaffa = "Staffe MOD vecchio";
            //            elementivari.tipostaffaEspessore = "staffa tagliata";

            //        }
            //        else if (int.Parse(H2_TXBX.Text) < 100) { MessageBox.Show("Impossibile fare laterali meno di 100mm!", "P_Sun", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); H2_TXBX.Text = "100"; H2_TXBX.Focus(); }
            //        else elementivari.spessori = "";
            //    }

            //    if (VersioneCMBX.Text.Contains("("))//mixTaglioProfiloSuperiore MANTOVANA
            //    {
            //        if (int.TryParse(VersioneCMBX.Text.Split('(', ')')[1], out int output)) mixTaglioProfiloSuperiore = int.Parse(L_TXBX.Text) + luceL + ((output - (Supplemento2CMBX.Text == tappiFresati ? 13 : 0)) * (VersioneCMBX.Text.Contains("singolo") ? 1 : 2));
            //        else
            //        {
            //            MessageBox.Show("Stringa in versione non corretta!" + "\n" + "Inserisci numero tra le parentesi", "P_Sun", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            mixTaglioProfiloSuperiore = 0;
            //            VersioneCMBX.Select();
            //        }
            //    }
            //    else //90°
            //    {
            //        mixTaglioProfiloSuperiore = (int.Parse(L_TXBX.Text) + luceL + (VersioneCMBX.Text.Contains("singolo") ? -13 : -26));
            //        if (Supplemento2CMBX.Text != tappiFresati)
            //        {
            //            Supplemento2CMBX.Text = tappiFresati;
            //            Supplemento2CMBX.Focus();//questo focus serve per convalidare
            //        }
            //        if (Supplemento2CMBX.Text != "" && Costo_supplemento2TXBX.Text == "0")//se si toglie anualmente "Tappi fresati in alluminio" non viene aggiornato ilvo di questo if prezzo, moti
            //        {
            //            Supplemento2CMBX.Focus();//questo focus serve per convalidare
            //        }

            //    }

            //    #region gestione_mantovana

            //    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //    //**************************************************** INIZIO gestione Mantovana  *********************************************************             
            //    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////         

            //    // spessore mantovana parte centrale: 9.6mm(19+1°), parte grossa 13.4mm(27), cover 1.8mm(3.6)
            //    //°poi c'è un millimetro? che il comando non tocca la staffa in quanto c'è il dentino del profilo della mantovana 
            //    // - 2,9 per rullo medio moto    Quindi se la mix tenda è 100 il tubo 97,1 senza cover 97,5
            //    // affiancate si toglie 2cm ogni tenda  quindi se la tenda è 100 il tubo 48 senza cover 48,2

            //    //mixTaglioTubo = Math.Round((luceLperEtich - 2.9f  + QuotaDoppiorullo) * 10, 1) - Quotamotore("Tubo43");/
            //    // mixTaglioTubo = Math.Round((luceLperEtich_Page1 - 2 + (QuotaDoppiorullo / 2)) * 10, 1) - Quotamotore("Tubo43");
            //    //mixTaglioTubo2 = Math.Round((luceLperEtich_Page1 - 2 + (QuotaDoppiorullo / 2)) * 10, 1) - Quotamotore("Tubo43");


            //    // soffitto: 2.7f - 0.4
            //    // parete : 2.1 -0.4 (2.1= 1.9 + 2mm di comendo che non tocca la staffa)????
            //    //  soffitto con laterale solo ad un lato = 1.3f - 0.2f;(/2=0.6)
            //    //  parete con laterale solo ad un lato = 1.1 - 0.2f;(/2=0.5)



            //    switch (VersioneCMBX.Text)
            //    {
            //        case string a when a.Contains("angolo _|____|_"):

            //            elementivari.copriviti = "-2 Copriviti"; elementivari.squadrette = "-2 Squadrette con grani";
            //            elementivari.numeroLaterali = "2";

            //            if (Supplemento2CMBX.Text == tappiFresati) elementivari.tappoMantovana = "-2 Tappi fresati in alluminio";
            //            else elementivari.tappoMantovana = "-2 Tappi mantovana in PVC";

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 27 - 4;//
            //                elementivari.attacchiSoffitto = "-" + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "attacchi a soffitto", uno: 1000, due: 2100, tre: 2800, quattro: 3500, cinque: 4200, sei: 4900, sette: 5800);
            //                if (ComandiCMBX.Text.Contains("dx") || ComandiCMBX.Text.Contains("DX"))
            //                {
            //                    elementivari.staffaDX = "- montare staffa lato motore a DX a soffitto";
            //                    elementivari.staffaSX = "- montare staffa lato calotta a SX a soffitto";
            //                }
            //                else
            //                {
            //                    elementivari.staffaDX = "- montare staffa lato calotta a DX a soffitto";
            //                    elementivari.staffaSX = "- montare staffa lato motore a SX a soffitto";
            //                }

            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 20 - 4;

            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-2 " + elementivari.spessori /*+ " MOD.nuovo"*/;
            //                }
            //                if (ComandiCMBX.Text.Contains("dx") || ComandiCMBX.Text.Contains("DX"))
            //                {
            //                    elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato motore a DX a parete";
            //                    elementivari.staffaSX = "- montare " + elementivari.tipostaffaEspessore + " lato calotta a SX a parete";
            //                }
            //                else
            //                {
            //                    elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato calotta a DX a parete";
            //                    elementivari.staffaSX = "- montare " + elementivari.tipostaffaEspessore + " lato motore a SX a parete";
            //                }
            //            }

            //            break;
            //        case string a when a.Contains("angolo singolo ____|_"):

            //            elementivari.copriviti = "-2 Copriviti";
            //            elementivari.numeroLaterali = "1";

            //            if (Supplemento2CMBX.Text == tappiFresati) elementivari.tappoMantovana = "-2 Tappi fresati in alluminio";
            //            else elementivari.tappoMantovana = "-2 Tappi mantovana in PVC";

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 13 - 2;
            //                elementivari.attacchiSoffitto = "-" + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "attacchi a soffitto", uno: 0, due: 650, tre: 1350, quattro: 2450, cinque: 3750, sei: 4650, sette: 5550);
            //                elementivari.squadrette = "-1 Squadretta con grani";
            //                if (ComandiCMBX.Text.Contains("dx") || ComandiCMBX.Text.Contains("DX"))
            //                {
            //                    elementivari.staffaDX = "- montare staffa lato motore a DX a soffitto";
            //                    elementivari.staffaSX = "- staffa lato calotta con cover";
            //                }
            //                else
            //                {
            //                    elementivari.staffaDX = "- montare staffa lato calotta a DX a soffitto";
            //                    elementivari.staffaSX = "- staffa lato motore con cover";
            //                }

            //            }
            //            else//parete
            //            {
            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa senza laterale più di 150mm");
            //                elementivari.squadrette = "-1 Squadrette con grani e 1 modificata?";

            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-1 " + elementivari.spessori + " + 1 spessore medio nuovo";
            //                }
            //                if (ComandiCMBX.Text.Contains("dx") || ComandiCMBX.Text.Contains("DX"))
            //                {
            //                    quotaattaccosoffito_spall = 11 - 2;
            //                    elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato motore a DX a parete";
            //                    elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato calotta con cover";
            //                }
            //                else
            //                {
            //                    quotaattaccosoffito_spall = 10 - 2;
            //                    elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato calotta a DX a parete";
            //                    elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato motore con cover";
            //                }
            //            }

            //            break;
            //        case string a when a.Contains("angolo singolo _|____"):

            //            elementivari.copriviti = "-2 Copriviti";
            //            elementivari.numeroLaterali = "1";

            //            if (Supplemento2CMBX.Text == tappiFresati)
            //            {
            //                elementivari.tappoMantovana = "-2 Tappi fresati in alluminio";
            //            }
            //            else elementivari.tappoMantovana = "-2 Tappi mantovana in PVC";

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 13 - 2;
            //                elementivari.attacchiSoffitto = "-" + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "attacchi a soffitto", uno: 0, due: 650, tre: 1350, quattro: 2450, cinque: 3750, sei: 4650, sette: 5550);
            //                elementivari.squadrette = "-1 Squadretta con grani";
            //                if (ComandiCMBX.Text.Contains("dx") || ComandiCMBX.Text.Contains("DX"))
            //                {
            //                    elementivari.staffaDX = "- staffa lato motore con cover";
            //                    elementivari.staffaSX = "- montare staffa lato calotta a SX a soffitto";
            //                }
            //                else
            //                {
            //                    elementivari.staffaDX = "- staffa lato calotta con cover";
            //                    elementivari.staffaSX = "- montare staffa lato motore a SX a soffitto";
            //                }
            //            }
            //            else//parete
            //            {
            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa senza laterale più di 150mm");
            //                elementivari.squadrette = "-1 Squadrette con grani e 1 modificata?";

            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-1 " + elementivari.spessori + " + 1 spessore medio nuovo";
            //                }
            //                if (ComandiCMBX.Text.Contains("dx") || ComandiCMBX.Text.Contains("DX"))
            //                {
            //                    quotaattaccosoffito_spall = 10 - 2;
            //                    elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato calotta a SX a parete";
            //                    elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato motore con cover";
            //                }
            //                else
            //                {
            //                    quotaattaccosoffito_spall = 11 - 2;
            //                    elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato motore a SX a parete";
            //                    elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato calotta con cover";
            //                }
            //            }

            //            break;
            //        case string a when a.Contains("angolo _|__'__|_"):

            //            elementivari.copriviti = "-3 Copriviti";
            //            elementivari.squadrette = "-2 Squadrette con grani";
            //            elementivari.numeroLaterali = "2";

            //            if (Supplemento2CMBX.Text == tappiFresati)
            //            {
            //                elementivari.tappoMantovana = "-2 " + tappiFresati;
            //            }
            //            else elementivari.tappoMantovana = "-2 Tappi mantovana in PVC";

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 12;//( 2.7f - 0.4f)/2;
            //                elementivari.attacchiSoffitto = "-" + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "attacchi a soffitto", uno: 1000, due: 2100, tre: 2800, quattro: 3500, cinque: 4200, sei: 4900, sette: 5800);
            //                elementivari.staffaDX = "- montare le staffe lato comando ai laterali a soffitto";
            //                elementivari.staffaSX = "- 1 staffa cenrale";

            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 9;// (2.1f - 0.4f)/2;

            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa centrale più di 15cm");
            //                if (int.Parse(L_TXBX.Text) > 4000) MessageBox.Show("Larghezza troppo ampia, la mantovana potrebbe imbarcarsi, è consigliata la versione con spalletta centrale.");
            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-2 " + elementivari.spessori + " + 1 spessore medio nuovo??";
            //                }

            //                elementivari.staffaSX = "- " + elementivari.staffaCentrale;
            //                elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato motore ai laterali a parete";
            //            }

            //            break;
            //        case string a when a.Contains("angolo singolo ___'__|_"):

            //            elementivari.copriviti = "-3 Copriviti";
            //            elementivari.numeroLaterali = "1";

            //            if (Supplemento2CMBX.Text == tappiFresati)
            //            {
            //                elementivari.tappoMantovana = "-2 Tappi fresati in alluminio";
            //            }
            //            else elementivari.tappoMantovana = "-2 Tappi mantovana in PVC";

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 6;
            //                elementivari.attacchiSoffitto = "-" + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "attacchi a soffitto", uno: 0, due: 650, tre: 1350, quattro: 2450, cinque: 3750, sei: 4650, sette: 5550);
            //                elementivari.squadrette = "-1 Squadretta con grani";

            //                elementivari.staffaDX = "- staffa lato motore con cover + " + elementivari.staffaCentrale;
            //                elementivari.staffaSX = "- montare staffa lato motore a DX a soffitto";
            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 5;

            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa centrale o esterna senza laterale più di 150mm");
            //                if (int.Parse(L_TXBX.Text) > 400) MessageBox.Show("Larghezza troppo ampia, la mantovana potrebbe imbarcarsi, è consigliata la versione con spalletta centrale.");
            //                elementivari.squadrette = "-1 Squadrette con grani e 1 modificata?";

            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-1 " + elementivari.spessori + " + 2 spessori medio nuovo??";
            //                }

            //                elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato motore a DX a parete";
            //                elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato motore con cover + " + elementivari.staffaCentrale;
            //            }

            //            break;
            //        case string a when a.Contains("angolo singolo _|__'___"):

            //            elementivari.copriviti = "-3 Copriviti";
            //            elementivari.numeroLaterali = "1";

            //            if (Supplemento2CMBX.Text == tappiFresati)
            //            {
            //                elementivari.tappoMantovana = "-2 Tappi fresati in alluminio";
            //            }
            //            else elementivari.tappoMantovana = "-2 Tappi mantovana in PVC";

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 6;
            //                elementivari.attacchiSoffitto = "-" + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "attacchi a soffitto", uno: 0, due: 650, tre: 1350, quattro: 2450, cinque: 3750, sei: 4650, sette: 5550);
            //                elementivari.squadrette = "-1 Squadretta con grani";

            //                elementivari.staffaDX = "- staffa lato motore con cover + " + elementivari.staffaCentrale;
            //                elementivari.staffaSX = "- montare staffa lato motore a SX a soffitto";

            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 5;

            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa centrale o esterna senza laterale più di 150mm");
            //                if (int.Parse(L_TXBX.Text) > 4000) MessageBox.Show("Larghezza troppo ampia, la mantovana potrebbe imbarcarsi, è consigliata la versione con spalletta centrale.");
            //                elementivari.squadrette = "-1 Squadrette con grani e 1 modificata?";

            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-1 " + elementivari.spessori + " + 2 spessori medio nuovo??";
            //                }

            //                elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato motore a SX a parete";
            //                elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato motore con cover + " + elementivari.staffaCentrale;
            //            }

            //            break;
            //        case string a when a.Contains("angolo _|__|__|_"):

            //            elementivari.copriviti = "-3 Copriviti";
            //            elementivari.squadrette = "-3 Squadrette con grani";
            //            elementivari.numeroLaterali = "3";

            //            if (Supplemento2CMBX.Text == tappiFresati)
            //            {
            //                elementivari.tappoMantovana = "-2 " + tappiFresati;
            //            }
            //            else elementivari.tappoMantovana = "-2 Tappi mantovana in PVC";

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 0;

            //                elementivari.staffaDX = "";
            //                elementivari.staffaSX = "";
            //                MessageBox.Show("non e' possibile mettere il laterale centrale quando è a soffitto");
            //                AttacchiCBX.Text = "Frontale";

            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 38;

            //                elementivari.lateraleCentrale = "- staffa centrale vedi campione";
            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-3 " + elementivari.spessori;
            //                    elementivari.lateraleCentrale = "- staffa centrale con spessore vedi campione";
            //                }
            //                elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato motore ai laterali a parete";
            //                elementivari.staffaSX = elementivari.lateraleCentrale;
            //            }

            //            break;
            //        case string a when a.Contains("angolo singolo ___|__|_"):

            //            elementivari.copriviti = "-3 Copriviti";
            //            elementivari.squadrette = "-2 Squadrette con grani + 1 modificata?";
            //            elementivari.numeroLaterali = "2";

            //            if (Supplemento2CMBX.Text == tappiFresati)
            //            {
            //                elementivari.tappoMantovana = "-2 " + tappiFresati;
            //            }
            //            else elementivari.tappoMantovana = "-2 Tappi mantovana in PVC";

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 0;

            //                elementivari.staffaDX = "";
            //                elementivari.staffaSX = "";
            //                MessageBox.Show("non e' possibile mettere il laterale centrale quando è a soffitto");
            //                AttacchiCBX.Text = "Frontale";

            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 0;

            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa senza laterale più di 150mm");
            //                elementivari.squadrette = "-2 Squadrette con grani e 1 modificata?";

            //                elementivari.lateraleCentrale = "- staffa centrale vedi campione";
            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-2 " + elementivari.spessori + " + 1 spessore medio nuovo??";
            //                    elementivari.lateraleCentrale = "- staffa centrale con spessore vedi campione";
            //                }
            //                elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato motore al laterale DX a parete";
            //                elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato mot. e cover + " + elementivari.staffaCentrale + " vedi campione";
            //            }

            //            break;
            //        case string a when a.Contains("angolo singolo _|__|___"):

            //            elementivari.copriviti = "-3 Copriviti";
            //            elementivari.squadrette = "-2 Squadrette con grani + 1 modificata?";
            //            elementivari.numeroLaterali = "2";

            //            if (Supplemento2CMBX.Text == tappiFresati)
            //            {
            //                elementivari.tappoMantovana = "-2 " + tappiFresati;
            //            }
            //            else elementivari.tappoMantovana = "-2 Tappi mantovana in PVC";

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 0;

            //                elementivari.staffaDX = "";
            //                elementivari.staffaSX = "";
            //                MessageBox.Show("non e' possibile mettere il laterale centrale quando è a soffitto");
            //                AttacchiCBX.Text = "Frontale";

            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 0;

            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa senza laterale più di 150mm");
            //                elementivari.squadrette = "-2 Squadrette con grani e 1 modificata?";

            //                elementivari.lateraleCentrale = "- staffa centrale vedi campione";
            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-2 " + elementivari.spessori + " + 1 spessore medio nuovo??";
            //                    elementivari.lateraleCentrale = "- staffa centrale con spessore vedi campione";
            //                }
            //                elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato motore al laterale SX a parete";
            //                elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato mot. e cover + " + elementivari.staffaCentrale + " vedi campione";

            //            }

            //            break;
            //        case string a when a.Contains("angolo 90° |____|"):

            //            elementivari.copriviti = "-2 Copriviti";
            //            elementivari.numeroLaterali = "2";

            //            if (Supplemento2CMBX.Text == tappiFresati) elementivari.tappoMantovana = "-2 tappi angolari fresati";
            //            else
            //            {
            //                MessageBox.Show("Questa versione deve essere con i tappi fresati angolari!");
            //                Supplemento2CMBX.Focus();
            //            }

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 27 - 4;//
            //                elementivari.attacchiSoffitto = "-" + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "attacchi a soffitto", uno: 1000, due: 2100, tre: 2800, quattro: 3500, cinque: 4200, sei: 4900, sette: 5800);
            //                if (ComandiCMBX.Text.Contains("dx") || ComandiCMBX.Text.Contains("DX"))
            //                {
            //                    elementivari.staffaDX = "- montare staffa lato motore a DX a soffitto";
            //                    elementivari.staffaSX = "- montare staffa lato calotta a SX a soffitto";
            //                }
            //                else
            //                {
            //                    elementivari.staffaDX = "- montare staffa lato calotta a DX a soffitto";
            //                    elementivari.staffaSX = "- montare staffa lato motore a SX a soffitto";
            //                }
            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 20 - 4;//

            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-2 " + elementivari.spessori /*+ " MOD.nuovo"*/;
            //                }
            //                if (ComandiCMBX.Text.Contains("dx") || ComandiCMBX.Text.Contains("DX"))
            //                {
            //                    elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato motore a DX a parete";
            //                    elementivari.staffaSX = "- montare " + elementivari.tipostaffaEspessore + " lato calotta a SX a parete";
            //                }
            //                else
            //                {
            //                    elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato calotta a DX a parete";
            //                    elementivari.staffaSX = "- montare " + elementivari.tipostaffaEspessore + " lato motore a SX a parete";
            //                }
            //            }

            //            break;
            //        case string a when a.Contains("angolo singolo 90° ____|"):

            //            elementivari.copriviti = "-2 Copriviti";
            //            elementivari.numeroLaterali = "1";

            //            if (Supplemento2CMBX.Text == tappiFresati) elementivari.tappoMantovana = "-1 tappi angolare fresato e 1 laterale";
            //            else
            //            {
            //                MessageBox.Show("Questa versione deve essere con i tappi fresati angolari!");
            //                Supplemento2CMBX.Focus();
            //            }

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 13 - 2;
            //                elementivari.attacchiSoffitto = "-" + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "attacchi a soffitto", uno: 0, due: 650, tre: 1350, quattro: 2450, cinque: 3750, sei: 4650, sette: 5550);
            //                if (ComandiCMBX.Text.Contains("dx") || ComandiCMBX.Text.Contains("DX"))
            //                {
            //                    elementivari.staffaDX = "- montare staffa lato motore a DX a soffitto";
            //                    elementivari.staffaSX = "- staffa lato calotta con cover";
            //                }
            //                else
            //                {
            //                    elementivari.staffaDX = "- montare staffa lato calotta a DX a soffitto";
            //                    elementivari.staffaSX = "- staffa lato motore con cover";
            //                }

            //            }
            //            else//parete
            //            {

            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa senza laterale più di 150mm");
            //                elementivari.squadrette = "-1 squadetta modificata?";

            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-1 " + elementivari.spessori + " + 1 spessore medio nuovo";
            //                }
            //                if (ComandiCMBX.Text.Contains("dx") || ComandiCMBX.Text.Contains("DX"))
            //                {
            //                    quotaattaccosoffito_spall = 11 - 2;
            //                    elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato motore a DX a parete";
            //                    elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato calotta con cover";
            //                }
            //                else
            //                {
            //                    quotaattaccosoffito_spall = 10 - 2;
            //                    elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato calotta a DX a parete";
            //                    elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato motore con cover";
            //                }
            //            }

            //            break;
            //        case string a when a.Contains("angolo singolo 90° |____"):

            //            elementivari.copriviti = "-2 Copriviti";
            //            elementivari.numeroLaterali = "1";

            //            if (Supplemento2CMBX.Text == tappiFresati) elementivari.tappoMantovana = "-1 tappi angolare fresato e 1 laterale";
            //            else
            //            {
            //                MessageBox.Show("Questa versione deve essere con i tappi fresati angolari!");
            //                Supplemento2CMBX.Focus();
            //            }

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 13 - 2;
            //                elementivari.attacchiSoffitto = "-" + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "attacchi a soffitto", uno: 0, due: 650, tre: 1350, quattro: 2450, cinque: 3750, sei: 4650, sette: 5550);
            //                if (ComandiCMBX.Text.Contains("dx") || ComandiCMBX.Text.Contains("DX"))
            //                {
            //                    elementivari.staffaDX = "- montare staffa lato calotta a SX a soffitto";
            //                    elementivari.staffaSX = "- staffa lato motore con cover";
            //                }
            //                else
            //                {
            //                    elementivari.staffaDX = "- montare staffa lato motore a SX a soffitto";
            //                    elementivari.staffaSX = "- staffa lato calotta con cover";
            //                }

            //            }
            //            else//parete
            //            {

            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa senza laterale più di 150mm");
            //                elementivari.squadrette = "-1 squadetta modificata?";

            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-1 " + elementivari.spessori + " + 1 spessore medio nuovo";
            //                }
            //                if (ComandiCMBX.Text.Contains("dx") || ComandiCMBX.Text.Contains("DX"))
            //                {
            //                    quotaattaccosoffito_spall = 10 - 2;
            //                    elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato calotta a SX a parete";
            //                    elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato motore con cover";
            //                }
            //                else
            //                {
            //                    quotaattaccosoffito_spall = 11 - 2;
            //                    elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato motore a SX a parete";
            //                    elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato calotta con cover";
            //                }
            //            }

            //            break;
            //        case string a when a.Contains("angolo 90° |__'__|"):

            //            elementivari.copriviti = "-3 Copriviti";
            //            elementivari.numeroLaterali = "2";

            //            if (Supplemento2CMBX.Text == tappiFresati) elementivari.tappoMantovana = "-2 tappi angolari fresati";
            //            else
            //            {
            //                MessageBox.Show("Questa versione deve essere con i tappi fresati angolari!");
            //                Supplemento2CMBX.Focus();
            //            }

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 12;//( 2.7f - 0.4f)/2;
            //                elementivari.attacchiSoffitto = "-" + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "attacchi a soffitto", uno: 1000, due: 2100, tre: 2800, quattro: 3500, cinque: 4200, sei: 4900, sette: 5800);
            //                elementivari.staffaDX = "- montare le staffe lato comando ai laterali a soffitto";
            //                elementivari.staffaSX = "- 1 staffa cenrale";

            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 9;// (2.1f - 0.4f)/2;

            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa centrale più di 150mm");
            //                if (int.Parse(L_TXBX.Text) > 4000) MessageBox.Show("Larghezza troppo ampia, la mantovana potrebbe imbarcarsi, è consigliata la versione con spalletta centrale.");
            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-2 " + elementivari.spessori + " + 1 spessore medio nuovo??";
            //                }

            //                elementivari.staffaSX = "- " + elementivari.staffaCentrale;
            //                elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato motore ai laterali a parete";
            //            }

            //            break;
            //        case string a when a.Contains("angolo singolo 90° __'__|"):

            //            elementivari.copriviti = "-3 Copriviti";
            //            elementivari.numeroLaterali = "1";

            //            if (Supplemento2CMBX.Text == tappiFresati) elementivari.tappoMantovana = "-1 tappi angolare fresato e 1 laterale";
            //            else
            //            {
            //                MessageBox.Show("Questa versione deve essere con i tappi fresati angolari!");
            //                Supplemento2CMBX.Focus();
            //            }

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 6;
            //                elementivari.attacchiSoffitto = "-" + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "attacchi a soffitto", uno: 0, due: 650, tre: 1350, quattro: 2450, cinque: 3750, sei: 4650, sette: 5550);
            //                elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato motore a DX a soffitto";
            //                elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato motore con cover + " + elementivari.staffaCentrale;


            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 5;

            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa centrale o esterna senza laterale più di 150mm");
            //                if (int.Parse(L_TXBX.Text) > 4000) MessageBox.Show("Larghezza troppo ampia, la mantovana potrebbe imbarcarsi, è consigliata la versione con spalletta centrale.");
            //                elementivari.squadrette = "-1 squadetta modificata?";

            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-1 " + elementivari.spessori + " + 2 spessori medio nuovo??";
            //                }
            //                elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato motore a DX a parete";
            //                elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato motore con cover + " + elementivari.staffaCentrale;
            //            }

            //            break;
            //        case string a when a.Contains("angolo singolo 90° |__'__"):

            //            elementivari.copriviti = "-3 Copriviti";
            //            elementivari.numeroLaterali = "1";

            //            if (Supplemento2CMBX.Text == tappiFresati) elementivari.tappoMantovana = "-1 tappi angolare fresato e 1 laterale";
            //            else
            //            {
            //                MessageBox.Show("Questa versione deve essere con i tappi fresati angolari!");
            //                Supplemento2CMBX.Focus();
            //            }

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 6;
            //                elementivari.attacchiSoffitto = "-" + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "attacchi a soffitto", uno: 0, due: 650, tre: 1350, quattro: 2450, cinque: 3750, sei: 4650, sette: 5550);
            //                elementivari.staffaSX = "- montare staffa lato motore a SX a soffitto";
            //                elementivari.staffaDX = "- staffa lato motore con cover + " + elementivari.staffaCentrale;


            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 5;

            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa centrale o esterna senza laterale più di 150mm");
            //                if (int.Parse(L_TXBX.Text) > 4000) MessageBox.Show("Larghezza troppo ampia, la mantovana potrebbe imbarcarsi, è consigliata la versione con spalletta centrale.");
            //                elementivari.squadrette = "-1 squadetta modificata?";

            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-1 " + elementivari.spessori + " + 2 spessori medio nuovo??";
            //                }

            //                elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato motore a SX a parete";
            //                elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato motore con cover + " + elementivari.staffaCentrale;
            //            }

            //            break;
            //        case string a when a.Contains("angolo 90° |__|__|"):

            //            elementivari.copriviti = "-3 Copriviti";
            //            elementivari.squadrette = "-1 Squadretta con grani";
            //            elementivari.numeroLaterali = "3";

            //            if (Supplemento2CMBX.Text == tappiFresati)
            //            {
            //                elementivari.tappoMantovana = "-2 tappi angolari fresati";
            //            }
            //            else
            //            {
            //                MessageBox.Show("Questa versione deve essere con i tappi fresati angolari!");
            //                Supplemento2CMBX.Focus();
            //            }

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 0;

            //                elementivari.staffaDX = "";
            //                elementivari.staffaSX = "";
            //                MessageBox.Show("non e' possibile mettere il laterale centrale quando è a soffitto");
            //                AttacchiCBX.Text = "Frontale";

            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 38;

            //                elementivari.lateraleCentrale = "- staffa centrale vedi campione";
            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-3 " + elementivari.spessori;
            //                    elementivari.lateraleCentrale = "- staffa centrale con spessore vedi campione";
            //                }
            //                elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato motore ai laterali a parete";
            //                elementivari.staffaSX = elementivari.lateraleCentrale;

            //            }

            //            break;
            //        case string a when a.Contains("angolo singolo 90° __|__|"):

            //            elementivari.copriviti = "-3 Copriviti";
            //            elementivari.squadrette = "-1 Squadretta con grani + 1 modificata?";
            //            elementivari.numeroLaterali = "2";

            //            if (Supplemento2CMBX.Text == tappiFresati)
            //            {
            //                elementivari.tappoMantovana = "-1 tappi angolare fresato e 1 laterale";
            //            }
            //            else
            //            {
            //                MessageBox.Show("Questa versione deve essere con i tappi fresati angolari!");
            //                Supplemento2CMBX.Focus();
            //            }

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 0;

            //                elementivari.staffaDX = "";
            //                elementivari.staffaSX = "";
            //                MessageBox.Show("non e' possibile mettere il laterale centrale quando è a soffitto");
            //                AttacchiCBX.Text = "Frontale";

            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 38;

            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa senza laterale più di 150mm");
            //                elementivari.squadrette = "-2 Squadrette con grani e 1 modificata?";

            //                elementivari.lateraleCentrale = "- staffa centrale vedi campione";
            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-2 " + elementivari.spessori + " + 1 spessore medio nuovo??";
            //                    elementivari.lateraleCentrale = "- staffa centrale con spessore vedi campione";
            //                }
            //                elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato motore al laterale DX a parete";
            //                elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato mot. e cover + " + elementivari.staffaCentrale + " vedi campione";
            //            }
            //            break;
            //        case string a when a.Contains("angolo singolo 90° |__|__"):

            //            elementivari.copriviti = "-3 Copriviti";
            //            elementivari.squadrette = "-1 Squadretta con grani + 1 modificata?";
            //            elementivari.numeroLaterali = "2";

            //            if (Supplemento2CMBX.Text == tappiFresati)
            //            {
            //                elementivari.tappoMantovana = "-1 tappi angolare fresato e 1 laterale";
            //            }
            //            else
            //            {
            //                MessageBox.Show("Questa versione deve essere con i tappi fresati angolari!");
            //                Supplemento2CMBX.Focus();
            //            }

            //            if (AttacchiCBX.Text.Contains("Soffitto"))
            //            {
            //                quotaattaccosoffito_spall = 0;

            //                elementivari.staffaDX = "";
            //                elementivari.staffaSX = "";
            //                MessageBox.Show("non e' possibile mettere il laterale centrale quando è a soffitto");
            //                AttacchiCBX.Text = "Frontale";

            //            }
            //            else//parete
            //            {
            //                quotaattaccosoffito_spall = 0;

            //                if (int.Parse(H2_TXBX.Text) > 150) MessageBox.Show("Non è possibile realizzare la staffa senza laterale più di 150mm");
            //                elementivari.squadrette = "-2 Squadrette con grani e 1 modificata?";

            //                elementivari.lateraleCentrale = "- staffa centrale vedi campione";
            //                if (elementivari.spessori != "" && !elementivari.spessori.Contains("prolungate"))
            //                {
            //                    elementivari.spessori = "-2 " + elementivari.spessori + " + 1 spessore medio nuovo??";
            //                    elementivari.lateraleCentrale = "- staffa centrale con spessore vedi campione";
            //                }
            //                elementivari.staffaDX = "- montare " + elementivari.tipostaffaEspessore + " lato motore al laterale SX a parete";
            //                elementivari.staffaSX = "- " + elementivari.tipostaffaEspessore + " lato mot. e cover + " + elementivari.staffaCentrale + " vedi campione";
            //            }
            //            break;
            //        default:
            //            MessageBox.Show("Stringa in versione non corretta!", "P_Sun", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            VersioneCMBX.Text = "";
            //            VersioneCMBX.Focus();
            //            break;
            //    }
            //    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //    //**************************************************** FINE gestione Mantovana Frontale *******************************************            
            //    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////         
            //    #endregion

            //    switch (VersioneCMBX.Text)
            //    {
            //        #region 1 telo
            //        case string a when !a.Contains("Affiancato") && a.Contains("angolo"): //1 telo

            //            quantita_supplemento = 1;

            //            mixTaglioTubo = luceLperEtich - 21 - quotaattaccosoffito_spall - Quotamotore("Tubo43");//QuotaMotore è in mm

            //            //---------------------------------------------------------------------------------------------------------------------

            //            if (Varie1TCMBX.Text == "Solo meccanica" | Varie1TCMBX.Text.Contains("meccanica"))//solo meccanica
            //            {
            //                articoloDB = articoliCB.Text.Replace("'", "''")
            //              + Environment.NewLine + " L  " + luceLperEtich + " " + Varie1TCMBX.Text;
            //                mixTeloFinita = mixTaglioTubo - 6 + "x ??";
            //                //  H_TXBX.Enabled = false;
            //                // H_TXBX.Text = "0";
            //                CostoTessutoTXBX.Text = "0";
            //                CostomeccanicaSemplice(int.Parse(L_TXBX.Text));
            //            }
            //            else
            //            {
            //                articoloDB = articoliCB.Text.Replace("'", "''")
            //               + Environment.NewLine + "   " + luceLperEtich + "x" + H_TXBX.Text + " " + Varie1TCMBX.Text;
            //                //  H_TXBX.Enabled = true;

            //                mixTeloFinita = mixTaglioTubo - 6 + "x" + ((int.Parse(H_TXBX.Text) + Tessuti.GetTessutoByName(CC[0]).Arrotolamento_su_tubo) * Tessuti.GetTessutoByName(CC[0]).Tessuto_NeD);

            //                L_TXBX.AutoResetTextHasChanged = false; Varie1TCMBX.AutoResetTextHasChanged = false;
            //                CostomeccanicaSemplice(int.Parse(L_TXBX.Text));
            //                L_TXBX.AutoResetTextHasChanged = true; Varie1TCMBX.AutoResetTextHasChanged = true;
            //                CostoTessuto(int.Parse(L_TXBX.Text));
            //            }

            //            break;
            //        #endregion
            //        #region affiancato
            //        case string a when a.Contains("Affiancato") & !a.Contains("asimmetrico"): // Centrale

            //            quantita_supplemento = 2;
            //            luceLperEtich_Page1 = (int)Math.Round((int.Parse(L_TXBX.Text) + luceL) / 2d, 1);
            //            luceLperEtich_Page2 = (int)Math.Round((int.Parse(L_TXBX.Text) + luceL) / 2d, 1);
            //            //--------------------------------------------------------------------------------------------------------------------                   

            //            mixTaglioTubo = luceLperEtich_Page1 - 20 - (int)(quotaattaccosoffito_spall / 2) - Quotamotore("Tubo43");//verificare
            //            mixTaglioTubo2 = mixTaglioTubo;
            //            //---------------------------------------------------------------------------------------------------------------------                                   
            //            if (Varie1TCMBX.Text == "Solo meccanica" | Varie1TCMBX.Text.Contains("meccanica"))//solo meccanica
            //            {
            //                articoloDB = articoliCB.Text.Replace("'", "''")
            //            + Environment.NewLine + " Affianc. L " + luceLperEtich + "  " + Varie1TCMBX.Text;
            //                //   H_TXBX.Enabled = false;
            //                //H_TXBX.Text = "0";
            //                CostoTessutoTXBX.Text = "0";
            //                mixTeloFinita = mixTaglioTubo - 6 + "x ??";
            //                mixTeloFinita_Page2 = mixTeloFinita;

            //                if (CostomeccanicaSemplice((int)luceLperEtich_Page1)) CostoMeccanicaTXBX.Text = (float.Parse(CostoMeccanicaTXBX.Text) * 2).ToString();
            //            }
            //            else
            //            {
            //                articoloDB = articoliCB.Text.Replace("'", "''")
            //                + Environment.NewLine + " Affianc. " + luceLperEtich + "x" + H_TXBX.Text + "  " + Varie1TCMBX.Text;
            //                //   H_TXBX.Enabled = true;

            //                mixTeloFinita = mixTaglioTubo - 6 + "x" + ((int.Parse(H_TXBX.Text) + Tessuti.GetTessutoByName(CC[0]).Arrotolamento_su_tubo) * Tessuti.GetTessutoByName(CC[0]).Tessuto_NeD);
            //                mixTeloFinita_Page2 = mixTeloFinita;

            //                L_TXBX.AutoResetTextHasChanged = false; VersioneCMBX.AutoResetTextHasChanged = false; Varie1TCMBX.AutoResetTextHasChanged = false; L2_TXBX.AutoResetTextHasChanged = false;
            //                if (CostomeccanicaSemplice((int)luceLperEtich_Page1)) CostoMeccanicaTXBX.Text = (float.Parse(CostoMeccanicaTXBX.Text) * 2).ToString();

            //                L_TXBX.AutoResetTextHasChanged = true; VersioneCMBX.AutoResetTextHasChanged = true; Varie1TCMBX.AutoResetTextHasChanged = true;
            //                if (CostoTessuto((int)luceLperEtich_Page1)) CostoTessutoTXBX.Text = (float.Parse(CostoTessutoTXBX.Text) * 2).ToString();
            //            }
            //            break;
            //        #endregion
            //        #region asimmetrico
            //        case string a when a.Contains("asimmetrico"): // asimmetrico

            //            quantita_supplemento = 2;
            //            if (L2_TXBX.Text != "")
            //            {
            //                luceLperEtich_Page1 = (int)Math.Round(int.Parse(L2_TXBX.Text) + (luceL / 2d), 1);
            //                luceLperEtich_Page2 = (int)Math.Round(int.Parse(L_TXBX.Text) + (luceL / 2d) - int.Parse(L2_TXBX.Text), 1);

            //                if ((int.Parse(L2_TXBX.Text) + (luceL / 2)) >= (int.Parse(L_TXBX.Text) + (luceL / 2)))
            //                {
            //                    MessageBox.Show("La larghezza di 1 Anta non può essere uguale o maggiore della larghezza totale!", "P_Sun", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                    L2_TXBX.Text = ""; L2_TXBX.Focus(); return;
            //                }
            //                //--------------------------------------------------------------------------------------------------------------------                   
            //                mixTaglioTubo = luceLperEtich_Page1 - 20 - (quotaattaccosoffito_spall / 2) - Quotamotore("Tubo43");
            //                mixTaglioTubo2 = luceLperEtich_Page2 - 20 - (quotaattaccosoffito_spall / 2) - Quotamotore("Tubo43");

            //                //---------------------------------------------------------------------------------------------------------------------                                   
            //                if (Varie1TCMBX.Text == "Solo meccanica" | Varie1TCMBX.Text.Contains("meccanica"))//solo meccanica
            //                {
            //                    articoloDB = articoliCB.Text.Replace("'", "''")
            //                + Environment.NewLine + " Affianc. asimm. L " + luceLperEtich + "  " + Varie1TCMBX.Text;
            //                    //   H_TXBX.Enabled = false;
            //                    // H_TXBX.Text = "0";
            //                    CostoTessutoTXBX.Text = "0";
            //                    mixTeloFinita = mixTaglioTubo - 6 + "x ??";
            //                    mixTeloFinita_Page2 = mixTaglioTubo2 - 6 + "x ??";

            //                    float Costo_provvisorio;
            //                    L_TXBX.AutoResetTextHasChanged = false; VersioneCMBX.AutoResetTextHasChanged = false; Varie1TCMBX.AutoResetTextHasChanged = false; L2_TXBX.AutoResetTextHasChanged = false;
            //                    CostomeccanicaSemplice((int)luceLperEtich_Page1);
            //                    L_TXBX.AutoResetTextHasChanged = true; VersioneCMBX.AutoResetTextHasChanged = true; Varie1TCMBX.AutoResetTextHasChanged = true; L2_TXBX.AutoResetTextHasChanged = true;
            //                    Costo_provvisorio = float.Parse(CostoMeccanicaTXBX.Text);
            //                    if (CostomeccanicaSemplice((int)luceLperEtich_Page2) && Costo_provvisorio != 0) CostoMeccanicaTXBX.Text = (float.Parse(CostoMeccanicaTXBX.Text) + Costo_provvisorio).ToString();
            //                    if (Costo_provvisorio == 0) CostoMeccanicaTXBX.Text = "0";
            //                }
            //                else
            //                {
            //                    articoloDB = articoliCB.Text.Replace("'", "''")
            //                    + Environment.NewLine + " Affianc. asimm. " + luceLperEtich + "x" + H_TXBX.Text + "  " + Varie1TCMBX.Text;
            //                    //  H_TXBX.Enabled = true;


            //                    mixTeloFinita = mixTaglioTubo - 6 + "x" + ((int.Parse(H_TXBX.Text) + Tessuti.GetTessutoByName(CC[0]).Arrotolamento_su_tubo) * Tessuti.GetTessutoByName(CC[0]).Tessuto_NeD);
            //                    mixTeloFinita_Page2 = mixTaglioTubo2 - 6 + "x" + ((int.Parse(H_TXBX.Text) + Tessuti.GetTessutoByName(CC[0]).Arrotolamento_su_tubo) * Tessuti.GetTessutoByName(CC[0]).Tessuto_NeD);

            //                    float Costo_provvisorio;
            //                    L_TXBX.AutoResetTextHasChanged = false; H_TXBX.AutoResetTextHasChanged = false; VersioneCMBX.AutoResetTextHasChanged = false; Varie1TCMBX.AutoResetTextHasChanged = false; L2_TXBX.AutoResetTextHasChanged = false;
            //                    CostomeccanicaSemplice((int)luceLperEtich_Page1);
            //                    Costo_provvisorio = float.Parse(CostoMeccanicaTXBX.Text);
            //                    if (CostomeccanicaSemplice((int)luceLperEtich_Page2) && Costo_provvisorio != 0) CostoMeccanicaTXBX.Text = (float.Parse(CostoMeccanicaTXBX.Text) + Costo_provvisorio).ToString();
            //                    if (Costo_provvisorio == 0) CostoMeccanicaTXBX.Text = "0";

            //                    CostoTessuto((int)luceLperEtich_Page1);
            //                    Costo_provvisorio = float.Parse(CostoTessutoTXBX.Text);
            //                    L_TXBX.AutoResetTextHasChanged = true; H_TXBX.AutoResetTextHasChanged = true; VersioneCMBX.AutoResetTextHasChanged = true; Varie1TCMBX.AutoResetTextHasChanged = true; L2_TXBX.AutoResetTextHasChanged = true;
            //                    if (CostoTessuto((int)luceLperEtich_Page2) && Costo_provvisorio != 0) CostoTessutoTXBX.Text = (float.Parse(CostoTessutoTXBX.Text) + Costo_provvisorio).ToString();
            //                    if (Costo_provvisorio == 0) CostoTessutoTXBX.Text = "0";

            //                }
            //            }
            //            else
            //            {
            //                luceLperEtich = 0; luceHperEtich = 0;
            //                mixTeloFinita = "0";
            //                mixTeloFinita_Page2 = "0";
            //                mixTaglioTubo = 0;
            //                mixTaglioTubo2 = 0;
            //                luceLperEtich_Page1 = 0; luceLperEtich_Page2 = 0;
            //                luceLperEtich = 0; luceHperEtich = 0;
            //                ImponibTXBX.Text = ""; IvatoTXBX.Text = "";
            //                //CostoTessutoTXBX.Text = "0"; CostoMeccanicaTXBX.Text = "0";
            //            }
            //            break;
            //        #endregion
            //        default:
            //            MessageBox.Show("Stringa in versione non corretta!", "P_Sun", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            VersioneCMBX.Text = "";
            //            VersioneCMBX.Focus();
            //            break;
            //    }
            //    ////PPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPP
            //}
            //else
            //{
            //    luceLperEtich = 0;
            //    luceHperEtich = 0; mixTeloFinita = "0";
            //    mixTaglioProfiloSuperiore = 0;
            //    mixTaglioProfilo = 0;
            //    mixTaglioTubo = 0;
            //    mixTaglioFondaleManiglia = 0;
            //    mixTaglioFondaleManiglia_Page2 = 0;
            //    ImponibTXBX.Text = ""; IvatoTXBX.Text = "";
            //    //CostoTessutoTXBX.Text = "0"; CostoMeccanicaTXBX.Text = "0";
            //}

            ////8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //Calcolo_QuantitaRullo();
            //if (Supplemento1CMBX.Text != "") Calcolo_supplemento1(int.Parse(PezziTXBX.Text) * quantita_supplemento, (int.Parse(PezziTXBX.Text) * quantita_supplemento).ToString());
            //Calcolo_supplemento2(int.Parse(PezziTXBX.Text) * quantita_supplemento, (int.Parse(PezziTXBX.Text) * quantita_supplemento).ToString());
            //Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);
            ////8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888

        }

        private void RulloMiniMolla()
        {
            Aggiorna();
        }
        private void Sole_33_41_Cat(float Quotaguide, string Nomerullo)
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaSole_33_41_Cat = new EtichettaSole_33_41_Cat(etichetta))
            {
                GraphicsDrawable1 = EtichettaSole_33_41_Cat;
                DrawingSurface1 = EtichettaSole_33_41_Cat.ToBitmap();
            }
            Aggiorna();


            //if (LuceRBN.Checked == true)
            //{
            //    luceL = -2;
            //    luceH = -2;
            //}
            //else
            //{
            //    luceL = 0;
            //    luceH = 0;
            //}

            //if (flagForCMBXitems == false)
            //{
            //    ConfigurazioneRulloCatena();
            //    Supplemento1CMBX.Items.Clear();
            //    AttacchiCBX.Visible = false;
            //    Attacchi_label.Visible = false;
            //    flagForCMBXitems = true;
            //    if (ColoreCMBX.Items.Count >= 3) ColoreCMBX.SelectedIndex = 2;
            //}

            //if (H_TXBX.Text != "" && L_TXBX.Text != "")//&& Varie1TCMBX.Text != ""
            //{
            //    luceLperEtich = int.Parse(L_TXBX.Text) + luceL;
            //    luceHperEtich = int.Parse(H_TXBX.Text) + luceH;
            //    articoloDB = articoliCB.Text.Replace("'", "''")
            // + Environment.NewLine + "   " + luceLperEtich + "x" + luceHperEtich + " " + Varie1TCMBX.Text;
            //    //--------------------------------------------------------------------------------------------------------------------                
            //    mixTaglioCassonetto = luceLperEtich - 18;
            //    mixTaglioFondaleManiglia = luceLperEtich - 51;
            //    mixTaglioGuida = luceHperEtich - Quotaguide;
            //    cordino1String = Math.Round(mixTaglioGuida * 4 / 1000d, 2).ToString("0.00");//mix spazzolino
            //    cordino2String = Math.Round(((luceHperEtich * 2) + luceLperEtich) / 1500d, 2).ToString("0.00");//mix biadesivo //0.00 serve a non far mangiarsi l'ultimo zero

            //    if (int.Parse(L_TXBX.Text) < 1200)
            //    {
            //        annotazioniVerieSuEtichiette = "";
            //        mixTeloFinita = (luceLperEtich - 20) + "x" + (int.Parse(H_TXBX.Text) + 80);

            //    }
            //    else
            //    {
            //        annotazioniVerieSuEtichiette = "Tubo da 24 -1mm";
            //        mixTeloFinita = (luceLperEtich - 21) + "x" + (int.Parse(H_TXBX.Text) + 80);

            //    }
            //    string Pz_biades, RimanenzaPz_biades;
            //    string[] cordino2StringIndice = cordino2String.Split(',');
            //    Pz_biades = cordino2StringIndice[0];

            //    if (!string.IsNullOrEmpty(cordino2StringIndice[1]))
            //    {
            //        RimanenzaPz_biades = cordino2StringIndice[1];

            //        cordino2String = Pz_biades + "Pz +" + Math.Round(float.Parse(RimanenzaPz_biades) * 1.51) * 10 + "mm";
            //        if ((1500 - float.Parse(RimanenzaPz_biades) * 15.1) < 100) cordino2String = Math.Round(float.Parse(Pz_biades) + 1) + "Pz";
            //        if ((float.Parse(RimanenzaPz_biades) * 15.1) < 10) cordino2String = Math.Round(float.Parse(Pz_biades)) + "Pz";


            //    }
            //    else cordino2String = Pz_biades + "Pz";// cordino2String = Pz_biades+"Pz +"+ Math.Round((float.Parse(RimanenzaPz_biades))*1.5,2)+"cm";

            //    //---------------------------------------------------------------------------------------------------------------------                 
            //    if (Varie1TCMBX.Text == "Solo meccanica" | Varie1TCMBX.Text.Contains("meccanica"))//solo meccanica
            //    {
            //        CostoTessutoTXBX.Text = "0";
            //        CostomeccanicaTabella(int.Parse(L_TXBX.Text), int.Parse(H_TXBX.Text));
            //    }
            //    else
            //    {
            //        L_TXBX.AutoResetTextHasChanged = false; H_TXBX.AutoResetTextHasChanged = false; Varie1TCMBX.AutoResetTextHasChanged = false;
            //        CostomeccanicaTabella(int.Parse(L_TXBX.Text), int.Parse(H_TXBX.Text));
            //        L_TXBX.AutoResetTextHasChanged = true; H_TXBX.AutoResetTextHasChanged = true; Varie1TCMBX.AutoResetTextHasChanged = true;
            //        CostoTessuto(int.Parse(L_TXBX.Text));
            //    }
            //}
            //else
            //{
            //    luceLperEtich = 0;
            //    luceHperEtich = 0;
            //    mixTeloFinita = "0";

            //    mixTaglioCassonetto = 0;
            //    mixTaglioFondaleManiglia = 0;
            //    mixTaglioGuida = 0;
            //    cordino1String = "0";//mix spazzolino
            //    cordino2String = "0";//mix biadesivo

            //    CostoTessutoTXBX.Text = "0";
            //    CostoMeccanicaTXBX.Text = "0";
            //    annotazioniVerieSuEtichiette = "";
            //}
            ////8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);
            //Calcolo_supplemento1_CatenaMetallo(PezziTXBX.Text);
            //Calcolo_QuantitaRullo();
            ////8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
        }
        private void Sole_33_41_Cat_Front(float Quotaguide, string Nomerullo)
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }
            Aggiorna();


            //if (LuceRBN.Checked == true)
            //{
            //    luceL = +47;
            //    luceH = Quotaguide;
            //}
            //else
            //{
            //    luceL = 0;
            //    luceH = 0;
            //}

            //if (flagForCMBXitems == false)
            //{
            //    ConfigurazioneRulloCatena();
            //    Supplemento1CMBX.Items.Clear();
            //    AttacchiCBX.Visible = false;
            //    Attacchi_label.Visible = false;
            //    LuceRBN.Text = "L+47 H+" + Quotaguide;

            //    flagForCMBXitems = true;

            //    if (ColoreCMBX.Items.Count >= 3) ColoreCMBX.SelectedIndex = 2;
            //}

            //if (H_TXBX.Text != "" && L_TXBX.Text != "")
            //{
            //    luceLperEtich = int.Parse(L_TXBX.Text) + luceL;
            //    luceHperEtich = int.Parse(H_TXBX.Text) + luceH;
            //    articoloDB = articoliCB.Text.Replace("'", "''")
            // + Environment.NewLine + "   " + luceLperEtich + "x" + luceHperEtich + " " + Varie1TCMBX.Text;

            //    //--------------------------------------------------------------------------------------------------------------------
            //    mixTaglioCassonetto = luceLperEtich - 18;
            //    mixTaglioFondaleManiglia = luceLperEtich - 51;
            //    mixTaglioProfiloInferiore = luceLperEtich - 47;
            //    mixTaglioGuida = luceHperEtich - Quotaguide;
            //    cordino1String = Math.Round(mixTaglioGuida * 4 / 1000d, 2).ToString("0.00");//mix spazzolino
            //    cordino2String = Math.Round(((luceHperEtich * 2) + (luceLperEtich * 2)) / 1500d, 2).ToString("0.00");//mix biadesivo //0.00 serve a non far mangiarsi l'ultimo zero
            //    if (int.Parse(L_TXBX.Text) < 1200)
            //    {
            //        annotazioniVerieSuEtichiette = "";
            //        mixTeloFinita = (luceLperEtich - 20) + "x" + (int.Parse(H_TXBX.Text) + 80);

            //    }
            //    else
            //    {
            //        annotazioniVerieSuEtichiette = "Tubo da 24 -1mm";
            //        mixTeloFinita = (luceLperEtich - 21) + "x" + (int.Parse(H_TXBX.Text) + 80);
            //    }
            //    string Pz_biades, RimanenzaPz_biades;
            //    string[] cordino2StringIndice = cordino2String.Split(',');
            //    Pz_biades = cordino2StringIndice[0];

            //    if (!string.IsNullOrEmpty(cordino2StringIndice[1]))
            //    {
            //        RimanenzaPz_biades = cordino2StringIndice[1];

            //        cordino2String = Pz_biades + "Pz +" + Math.Round(float.Parse(RimanenzaPz_biades) * 1.51) * 10 + "mm";
            //        if ((1500 - float.Parse(RimanenzaPz_biades) * 15.1) < 100) cordino2String = Math.Round(float.Parse(Pz_biades) + 1) + "Pz";
            //        if ((float.Parse(RimanenzaPz_biades) * 15.1) < 10) cordino2String = Math.Round(float.Parse(Pz_biades)) + "Pz";
            //    }
            //    else cordino2String = Pz_biades + "Pz";// cordino2String = Pz_biades+"Pz +"+ Math.Round((float.Parse(RimanenzaPz_biades))*1.5,2)+"cm";
            //                                           //---------------------------------------------------------------------------------------------------------------------                 
            //    if (Varie1TCMBX.Text == "Solo meccanica" | Varie1TCMBX.Text.Contains("meccanica"))//solo meccanica
            //    {
            //        CostoTessutoTXBX.Text = "0";
            //        CostomeccanicaTabella((int)luceLperEtich, (int)luceHperEtich);
            //    }
            //    else
            //    {
            //        L_TXBX.AutoResetTextHasChanged = false; H_TXBX.AutoResetTextHasChanged = false; Varie1TCMBX.AutoResetTextHasChanged = false; LuceRBN.AutoResetCheckHasChanged = false;
            //        CostomeccanicaTabella((int)luceLperEtich, (int)luceHperEtich);
            //        L_TXBX.AutoResetTextHasChanged = true; Varie1TCMBX.AutoResetTextHasChanged = true; H_TXBX.AutoResetTextHasChanged = true; LuceRBN.AutoResetCheckHasChanged = true;
            //        CostoTessuto(int.Parse(L_TXBX.Text));
            //    }
            //}
            //else
            //{
            //    luceLperEtich = 0;
            //    luceHperEtich = 0;
            //    mixTeloFinita = "0";

            //    mixTaglioCassonetto = 0;
            //    mixTaglioFondaleManiglia = 0;
            //    mixTaglioProfiloInferiore = 0;
            //    mixTaglioGuida = 0;
            //    cordino1String = "0";//mix spazzolino
            //    cordino2String = "0";//mix biadesivo
            //    annotazioniVerieSuEtichiette = "";

            //    CostoTessutoTXBX.Text = "0";
            //    CostoMeccanicaTXBX.Text = "0";
            //}
            ////8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);
            //Calcolo_supplemento1_CatenaMetallo(PezziTXBX.Text);
            //Calcolo_QuantitaRullo();
            ////8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
        }
        private void Sole_33_41_Molla(float Quotaguide, string Nomerullo)
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }
            Aggiorna();

            //if (LuceRBN.Checked == true)
            //{
            //    luceL = -2;
            //    luceH = -2;
            //}
            //else
            //{
            //    luceL = 0;
            //    luceH = 0;
            //}

            //if (flagForCMBXitems == false)
            //{
            //    ConfigurazioneRulloMolla();
            //    AttacchiCBX.Visible = false;
            //    Attacchi_label.Visible = false;
            //    flagForCMBXitems = true;
            //    if (ColoreCMBX.Items.Count >= 3) ColoreCMBX.SelectedIndex = 2;
            //}

            //if (H_TXBX.Text != "" && L_TXBX.Text != "")//&& Varie1TCMBX.Text != ""
            //{
            //    luceLperEtich = int.Parse(L_TXBX.Text) + luceL;
            //    luceHperEtich = int.Parse(H_TXBX.Text) + luceH;
            //    articoloDB = articoliCB.Text.Replace("'", "''")
            // + Environment.NewLine + "   " + luceLperEtich + "x" + luceHperEtich + " " + Varie1TCMBX.Text;
            //    //--------------------------------------------------------------------------------------------------------------------
            //    mixTaglioCassonetto = luceLperEtich - 18;
            //    mixTaglioFondaleManiglia = luceLperEtich - 51;
            //    mixTaglioGuida = luceHperEtich - Quotaguide;
            //    cordino1String = Math.Round(mixTaglioGuida * 2 / 1000d, 2).ToString("0.00");//mix spazzolino
            //    cordino2String = Math.Round(((luceHperEtich * 2) + luceLperEtich) / 1500d, 2).ToString("0.00");//mix biadesivo //0.00 serve a non far mangiarsi l'ultimo zero
            //    nAgganciCingoliPannelli = (Math.Floor((mixTaglioGuida - 36) / 253d) * 2).ToString();//N agganci intermedi//verifiare

            //    if (int.Parse(L_TXBX.Text) < 1200)
            //    {
            //        annotazioniVerieSuEtichiette = "";
            //        mixTeloFinita = (luceLperEtich - 20) + "x" + (int.Parse(H_TXBX.Text) + 80);

            //    }
            //    else
            //    {
            //        annotazioniVerieSuEtichiette = "Tubo da 24 -1mm";
            //        mixTeloFinita = (luceLperEtich - 21) + "x" + (int.Parse(H_TXBX.Text) + 80);

            //    }
            //    string Pz_biades, RimanenzaPz_biades;
            //    string[] cordino2StringIndice = cordino2String.Split(',');
            //    Pz_biades = cordino2StringIndice[0];

            //    if (!string.IsNullOrEmpty(cordino2StringIndice[1]))
            //    {
            //        RimanenzaPz_biades = cordino2StringIndice[1];

            //        cordino2String = Pz_biades + "Pz +" + Math.Round(float.Parse(RimanenzaPz_biades) * 1.51) * 10 + "mm";
            //        if ((1500 - float.Parse(RimanenzaPz_biades) * 15.1) < 100) cordino2String = Math.Round(float.Parse(Pz_biades) + 1) + "Pz";
            //        if ((float.Parse(RimanenzaPz_biades) * 15.1) < 10) cordino2String = Math.Round(float.Parse(Pz_biades)) + "Pz";
            //    }
            //    else cordino2String = Pz_biades + "Pz";// cordino2String = Pz_biades+"Pz +"+ Math.Round((float.Parse(RimanenzaPz_biades))*1.5,2)+"cm";

            //    //---------------------------------------------------------------------------------------------------------------------                 
            //    if (Varie1TCMBX.Text == "Solo meccanica" | Varie1TCMBX.Text.Contains("meccanica"))//solo meccanica
            //    {
            //        CostoTessutoTXBX.Text = "0";
            //        CostomeccanicaTabella(int.Parse(L_TXBX.Text), int.Parse(H_TXBX.Text));
            //    }
            //    else
            //    {
            //        L_TXBX.AutoResetTextHasChanged = false; H_TXBX.AutoResetTextHasChanged = false; Varie1TCMBX.AutoResetTextHasChanged = false;
            //        CostomeccanicaTabella(int.Parse(L_TXBX.Text), int.Parse(H_TXBX.Text));
            //        L_TXBX.AutoResetTextHasChanged = true; Varie1TCMBX.AutoResetTextHasChanged = true; H_TXBX.AutoResetTextHasChanged = true;
            //        CostoTessuto(int.Parse(L_TXBX.Text));
            //    }
            //}
            //else
            //{
            //    luceLperEtich = 0;
            //    luceHperEtich = 0;
            //    mixTeloFinita = "0";

            //    mixTaglioCassonetto = 0;
            //    mixTaglioFondaleManiglia = 0;
            //    mixTaglioGuida = 0;
            //    cordino1String = "0";//mix spazzolino
            //    cordino2String = "0";//mix biadesivo
            //    nAgganciCingoliPannelli = "0";

            //    CostoTessutoTXBX.Text = "0";
            //    CostoMeccanicaTXBX.Text = "0";
            //    annotazioniVerieSuEtichiette = "";
            //}
            ////8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);
            //Calcolo_QuantitaRullo();
            ////8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
        }
        private void Sole_33_41_Molla_Front(float Quotaguide, string Nomerullo)
        {
            Aggiorna();
        }
        private void Sole_33_Motor()
        {


            Aggiorna();


            //if (LuceRBN.Checked == true) { luceL = -2; luceH = -2; }
            //else { luceL = 0; luceH = 0; }

            //if (flagForCMBXitems == false)
            //{
            //    ConfigurazioneRulloMotore("Tubo22");
            //    AttacchiCBX.Visible = false;
            //    Attacchi_label.Visible = false;
            //    LoadSconto();

            //    flagForCMBXitems = true;
            //    if (ColoreCMBX.Items.Count >= 3) ColoreCMBX.SelectedIndex = 2;
            //}

            //if (H_TXBX.Text != "" && L_TXBX.Text != "")//&& Varie1TCMBX.Text != ""
            //{
            //    luceLperEtich = int.Parse(L_TXBX.Text) + luceL;
            //    luceHperEtich = int.Parse(H_TXBX.Text) + luceH;
            //    articoloDB = articoliCB.Text.Replace("'", "''")
            //+ Environment.NewLine + "   " + luceLperEtich + "x" + luceHperEtich + " " + Varie1TCMBX.Text;
            //    mixTeloFinita = (luceLperEtich - 29) + "x" + (int.Parse(H_TXBX.Text) + 80);
            //    //--------------------------------------------------------------------------------------------------------------------
            //    mixTaglioCassonetto = luceLperEtich - 20;
            //    mixTaglioTubo = luceLperEtich - 27;
            //    mixTaglioFondaleManiglia = luceLperEtich - 51;
            //    mixTaglioGuida = luceHperEtich - 54;
            //    cordino1String = Math.Round(mixTaglioGuida * 4 / 1000d, 2).ToString("0.00");//mix spazzolino
            //    cordino2String = Math.Round(((luceHperEtich * 2) + luceLperEtich) / 1500d, 2).ToString("0.00");//mix biadesivo //0.00 serve a non far mangiarsi l'ultimo zero

            //    string Pz_biades, RimanenzaPz_biades;
            //    string[] cordino2StringIndice = cordino2String.Split(',');
            //    Pz_biades = cordino2StringIndice[0];

            //    if (!string.IsNullOrEmpty(cordino2StringIndice[1]))
            //    {
            //        RimanenzaPz_biades = cordino2StringIndice[1];

            //        cordino2String = Pz_biades + "Pz +" + Math.Round(float.Parse(RimanenzaPz_biades) * 1.51) * 10 + "mm";
            //        if ((1500 - float.Parse(RimanenzaPz_biades) * 15.1) < 100) cordino2String = Math.Round(float.Parse(Pz_biades) + 1) + "Pz";
            //        if ((float.Parse(RimanenzaPz_biades) * 15.1) < 10) cordino2String = Math.Round(float.Parse(Pz_biades)) + "Pz";
            //    }
            //    else cordino2String = Pz_biades + "Pz";// cordino2String = Pz_biades+"Pz +"+ Math.Round((float.Parse(RimanenzaPz_biades))*1.5,2)+"cm";

            //    //---------------------------------------------------------------------------------------------------------------------                 
            //    if (Varie1TCMBX.Text == "Solo meccanica" | Varie1TCMBX.Text.Contains("meccanica"))//solo meccanica
            //    {
            //        CostoTessutoTXBX.Text = "0";
            //        CostomeccanicaTabella(int.Parse(L_TXBX.Text), int.Parse(H_TXBX.Text));
            //    }
            //    else
            //    {
            //        L_TXBX.AutoResetTextHasChanged = false; H_TXBX.AutoResetTextHasChanged = false; Varie1TCMBX.AutoResetTextHasChanged = false;
            //        CostomeccanicaTabella(int.Parse(L_TXBX.Text), int.Parse(H_TXBX.Text));
            //        L_TXBX.AutoResetTextHasChanged = true; Varie1TCMBX.AutoResetTextHasChanged = true; H_TXBX.AutoResetTextHasChanged = true;
            //        CostoTessuto(int.Parse(L_TXBX.Text));
            //    }
            //}
            //else
            //{
            //    luceLperEtich = 0;
            //    luceHperEtich = 0;
            //    mixTeloFinita = "0";

            //    mixTaglioCassonetto = 0;
            //    mixTaglioTubo = 0;
            //    mixTaglioFondaleManiglia = 0;
            //    mixTaglioGuida = 0;
            //    cordino1String = "0";//mix spazzolino
            //    cordino2String = "0";//mix biadesivo
            //    nAgganciCingoliPannelli = "0";

            //    CostoTessutoTXBX.Text = "0";
            //    CostoMeccanicaTXBX.Text = "0";
            //    annotazioniVerieSuEtichiette = "";
            //}
            ////8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);
            //Calcolo_QuantitaRullo();
            ////8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
        }
        private void Sole_33_Motor_Front()
        {
            Aggiorna();
        }
        private void Sole_41_Motor()
        {
            Aggiorna();
        }
        private void Sole_41_Motor_Front()
        {
            Aggiorna();
        }
        private void Telo_squadrato()//
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }
            Aggiorna();

           // articoloDB = articoliCB.Text.Replace("'", "''")
           //+ Environment.NewLine + "   " + L_TXBX.Text + "x" + H_TXBX.Text + " " + Varie1TCMBX.Text;
           // if (flagForCMBXitems == false)
           // {
           //     Email_A_fornitori_Intestazione("");

           //     LuceRBN.Visible = false;
           //     FinitaRBN.Visible = false;
           //     ColoreCMBX.Visible = false;
           //     Colore_label.Visible = false;
           //     UM = "PZ";
           //     Varie1TCMBX.Visible = true;
           //     Varie1_label.Visible = true;
           //     LoadTessuti();
           //     H_label.Visible = true;
           //     H_TXBX.Visible = true;
           //     L_label.Text = "L(mm)";
           //     L_label.Visible = true;
           //     L_TXBX.Visible = true;
           //     LoadSconto();

           //     Supplemento1CMBX.Items.Add("Tasca per tondino");
           //     flagForEtich_unica = true;
           //     flagForCMBXitems = true;
           //     Varie1TCMBX.Focus();
           //     listino_a_modulo = true;
           // }
           // if (L_TXBX.Text != "" && H_TXBX.Text != "") CostoTessuto(int.Parse(L_TXBX.Text));
           // else
           // {
           //     mixTeloFinita = "0";
           //     CostoTessutoTXBX.Text = "0";
           //     CostoMeccanicaTXBX.Text = "0";
           // }
           // //88888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
           // Calcolo_supplemento1(float.Parse(PezziTXBX.Text), PezziTXBX.Text);
           // Calcolo_QuantitaRullo();
           // Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);
           // //88888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
        }
        private void Telo_pannello_confezionato()//
        {

            //if (flagForCMBXitems == false)
            //{
            //    Email_A_fornitori_Intestazione("  (luce = mix tenda con binario//finita = mix pannello confezionato) ");

            //    ColoreCMBX.Visible = false;
            //    Colore_label.Visible = false;
            //    NoteArtVarieLabel.Visible = true;
            //    NoteArtVarieLabel.Text = "luce = mix tenda con binario//finita = mix pannello confezionato ";
            //    Varie1TCMBX.Visible = true;
            //    Varie1_label.Visible = true;
            //    UM = "PZ";
            //    LoadTessuti();
            //    H_label.Visible = true;
            //    H_TXBX.Visible = true;
            //    L_label.Text = "L(mm)";
            //    L_label.Visible = true;
            //    L_TXBX.Visible = true;
            //    LuceRBN.Checked = true;
            //    FinitaRBN.Checked = false;
            //    LoadSconto();

            //    Supplemento1CMBX.Items.Add("Fondale");
            //    flagForEtich_unica = true;
            //    flagForCMBXitems = true;
            //    Varie1TCMBX.Focus();
            //    listino_a_modulo = true;
            //}
            //if (LuceRBN.Checked == true) luceH = -15;//??????????????????????????+ 2,5
            //else luceH = 0;

            //if (L_TXBX.Text != "" && H_TXBX.Text != "" && Varie1TCMBX.Text != "")
            //{
            //    luceHperEtich = int.Parse(H_TXBX.Text) + luceH;
            //    mixTeloFinita = int.Parse(L_TXBX.Text) + "x" + (luceHperEtich + 55);
            //    CostoTessuto(int.Parse(L_TXBX.Text));

            //    //88888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //    Calcolo_QuantitaRullo();
            //    Calcolo_supplemento2(int.Parse(L_TXBX.Text) / 1000 * int.Parse(PezziTXBX.Text), PezziTXBX.Text);
            //    Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);
            //    //88888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888

                Aggiorna();
        }
        private void Avvolgibili_varie(float Quotastecca, string NomeAvv)////////////////////////////////////////AVVOLGIBILI///////////////////////////AVVOLGIBILI//////////////////////////////////////////
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaAvvolgibili_varie = new EtichettaAvvolgibili_varie(etichetta))
            {
                GraphicsDrawable1 = EtichettaAvvolgibili_varie;
                DrawingSurface1 = EtichettaAvvolgibili_varie.ToBitmap();
            }
            Aggiorna();


            //articoloDB = articoliCB.Text.Replace("'", "''");
            //if (flagForCMBXitems == false)
            //{
            //    Email_A_fornitori_Intestazione("   Mix Finite(in mm)");
            //    Load_modelli_guide_e_supplementi_avvolgibili(false);
            //    Varie1TCMBX.Visible = true;
            //    Varie1_label.Visible = true;
            //    Varie1TCMBX.DropDownStyle = ComboBoxStyle.DropDownList;
            //    Supplemento2CMBX.Items.Clear();
            //    Supplemento2CMBX.Items.Add("Terminale all x Avvolg All");

            //    LuceRBN.Text = "H-Luce(+150)";
            //    FinitaRBN.Text = "H-Finita";

            //    flagForCMBXitems = true;
            //}
            ////-----------------------------------------------------------------------------------------------------------------------------------
            //if (LuceRBN.Checked == true) luceH = +150;
            //else luceH = 0;

            //if (Varie1TCMBX.Text.Contains("(") && Varie1TCMBX.Text.Contains(")"))
            //{
            //    luceL = int.Parse(Regex.Match(Varie1TCMBX.Text.Split('(', ')')[1], @"[-+]?\d+").Value);
            //}
            //else luceL = 0;

            ////------------------------------------------------------------------------------------------------------------------------------------------
            //if (L_TXBX.Text != "" && (H_TXBX.Text != ""))
            //{
            //    luceLperEtich = int.Parse(L_TXBX.Text) + luceL;
            //    luceHperEtich = int.Parse(H_TXBX.Text) + luceH;

            //    mixTaglioProfilo = luceLperEtich - Quotastecca;

            //    double diocane = Math.Round((double)luceHperEtich / 10);

            //    switch (diocane % 10)//prendo l' ultima cifra
            //    {
            //        case 0: break;
            //        case 1: diocane -= 1; break;
            //        case 2: diocane += 3; break;
            //        case 3: diocane += 2; break;
            //        case 4: diocane += 1; break;
            //        case 5: break;
            //        case 6: diocane -= 1; break;
            //        case 7: diocane += 3; break;
            //        case 8: diocane += +2; break;
            //        case 9: diocane += +1; break;
            //    }
            //    luceHperEtich = (int)diocane * 10;

            //    //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //    Calcolo_QuantitaMQ((int)luceLperEtich, (int)luceHperEtich, Fatt_min_Tot);
            //    Calcolo_supplemento_avvolg(luceHperEtich);
            //    Calcolo_supplemento2(int.Parse(L_TXBX.Text) / 1000f * int.Parse(PezziTXBX.Text), PezziTXBX.Text);
            //    Email_A_fornitori2(luceLperEtich.ToString(), luceHperEtich.ToString());
            //    //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            //}
            //else
            //{
            //    luceLperEtich = 0;
            //    mixTaglioProfilo = 0;
            //}
        }
        private void Girasole(string NomeAvv)
        {
            Aggiorna();
        }
        //___________________________________________________________________________________
        private void TuttGliAltri()
        {
            if (MainPageInput.LuceFinita) { MainPageInput.LuceH = -3; MainPageInput.LuceL = -0.3; }
            if (MainPageInput.LNumber > 0 && MainPageInput.HNumber > 0)
            {
                MainPageInput.LuceHperEtich = Math.Round(MainPageInput.HNumber + MainPageInput.LuceH, 1);
                MainPageInput.LuceLperEtich = Math.Round(MainPageInput.LNumber + MainPageInput.LuceL, 1);
                MainPageInput.MixBandaFinita = MainPageInput.HNumber + MainPageInput.LuceH - 4;
                MainPageInput.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                MainPageInput.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(MainPageInput.LNumber, MainPageInput.HNumber);
                CalcoloQuantitaMQ(MainPageInput.LNumber, Math.Max(MainPageInput.HNumber, 200), MainPageInput.Pezzi, 2.5);
            }

            var etichetta = new Etichetta {Alias = MainPageInput.RagioneSociale ?? string.Empty, Colore = MainPageInput.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = MainPageInput.HNumber, Comandi = MainPageInput.Comandi, Hc = MainPageInput.Hc, Attacchi = MainPageInput.Attacchi, PiuGuide = MainPageInput.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = MainPageInput.Supplemento1 ?? string.Empty, Note = MainPageInput.Note, Rif = MainPageInput.Rif };
            using (var EtichettaBandeVerticali = new EtichettaBandeVerticali(etichetta))
            {
                GraphicsDrawable1 = EtichettaBandeVerticali;
                DrawingSurface1 = EtichettaBandeVerticali.ToBitmap();
            }
            Aggiorna();


        //    articoloDB = articoliCB.Text.Replace("'", "''");
        //    GFX.Clear(Color.White);
        //    switch (UM)
        //    {
        //        case "MQ":

        //            if (flagForCMBXitems == false)
        //            {
        //                flagForEtich_unica = true;
        //                Email_A_fornitori_Intestazione("");
        //                UM_label.Text = "MQ";
        //                LuceRBN.Visible = false;
        //                FinitaRBN.Visible = false;

        //                flagForCMBXitems = true;
        //            }
        //            if ((L_TXBX.Text != "") & (H_TXBX.Text != ""))//Calcolo quantita'
        //            {
        //                //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
        //                Calcolo_QuantitaMQ(int.Parse(L_TXBX.Text), int.Parse(H_TXBX.Text), Fatt_min_Tot);
        //                //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
        //            }

        //            GFX.DrawString("PZ " + PezziTXBX.Text, new Font("thaoma", 8), Brushes.Black, 125, 58);
        //            GFX.DrawString("L " + L_TXBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
        //            GFX.DrawString("H " + H_TXBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 75, 40);
        //            break;

        //        case "KG":

        //            if (flagForCMBXitems == false)
        //            {
        //                flagForEtich_unica = true;
        //                Email_A_fornitori_Intestazione("");
        //                UM_label.Text = "KG";

        //                LuceRBN.Visible = false;
        //                FinitaRBN.Visible = false;
        //                L_label.Text = "KG";
        //                H_TXBX.Visible = false;
        //                H_label.Visible = false;

        //                flagForCMBXitems = true;
        //            }
        //            //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
        //            if (L_TXBX.Text != "" && L_TXBX.Text != "0") Calcolo_QuantitaMQ(/*deve essere float*/float.Parse(L_TXBX.Text.Replace(".", ",")) * 1000, 1000, Fatt_min_Tot);//verificare
        //            //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888

        //            GFX.DrawString("KG " + L_TXBX.Text, new Font("thaoma", 8), Brushes.Black, 7, 40);
        //            GFX.DrawString("PZ " + PezziTXBX.Text, new Font("thaoma", 8), Brushes.Black, 125, 40);
        //            break;
        //        case "ML":

        //            if (flagForCMBXitems == false)
        //            {
        //                Email_A_fornitori_Intestazione("");
        //                flagForEtich_unica = true;
        //                UM_label.Text = "ML";
        //                LuceRBN.Visible = false; FinitaRBN.Visible = false;
        //                H_TXBX.Visible = false; H_label.Visible = false;
        //                L_label.Text = "L/H(mm)";


        //                flagForCMBXitems = true;
        //            }
        //            //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
        //            if (L_TXBX.Text != "" && L_TXBX.Text != "0") Calcolo_QuantitaMQ(int.Parse(L_TXBX.Text), 1000, Fatt_min_Tot);
        //            //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888

        //            if (L_TXBX.Text != "")//senza questo if va in errore in quanto all' apertura L_TXBX.Text e' vuoto
        //            {
        //                GFX.DrawString("ml   " + float.Parse(L_TXBX.Text) / 1000, new Font("thaoma", 8), Brushes.Black, 95, 40);
        //            }
        //            GFX.DrawString("mm    " + L_TXBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 40);
        //            GFX.DrawString("PZ " + PezziTXBX.Text, new Font("thaoman", 10, FontStyle.Bold), Brushes.Black, 130, 58);
        //            break;
        //        case "CP":

        //            if (flagForCMBXitems == false)
        //            {
        //                Email_A_fornitori_Intestazione("");
        //                flagForEtich_unica = true;//solo una etich per piu' pezzi
        //                QuantitaTXBX.Enabled = false;
        //                Pezzi_label.Text = "CP";
        //                H_TXBX.Visible = false; H_label.Visible = false;
        //                L_label.Visible = false; L_TXBX.Visible = false;
        //                LuceRBN.Visible = false; FinitaRBN.Visible = false;

        //                flagForCMBXitems = true;
        //            }

        //            QuantitaTXBX.Text = int.Parse(PezziTXBX.Text).ToString();//Calcolo quantita'                    

        //            // GFX.DrawString(";-) ;-)  ;-) ", new Font("Times New Roman", 8), Brushes.Black, 7, 27);                                                                	                                         
        //            GFX.DrawString("CP " + PezziTXBX.Text, new Font("thaoma", 8), Brushes.Black, 125, 50);
        //            break;
        //        case "PZ":

        //            if (flagForCMBXitems == false)
        //            {
        //                Email_A_fornitori_Intestazione("");
        //                flagForEtich_unica = true;

        //                QuantitaTXBX.Enabled = false;
        //                H_TXBX.Visible = false; H_label.Visible = false;
        //                L_label.Visible = false; L_TXBX.Visible = false;
        //                LuceRBN.Visible = false; FinitaRBN.Visible = false;

        //                ColoreCMBX.Focus();

        //                flagForCMBXitems = true;
        //            }
        //            QuantitaTXBX.Text = float.Parse(PezziTXBX.Text).ToString();//Calcolo quantita'                  

        //            GFX.DrawString("PZ " + PezziTXBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 40);
        //            break;

        //        default:

        //            TUTTI_GLI_ALTRI();

        //            break;
        //    }
        //    //8888888888888888888888888888888888888888888888888888888888888888888888888888
        //    Email_A_fornitori(L_TXBX.Text, H_TXBX.Text);
        //    //888888888888888888888888888888888888888888888888888888888888888888888888888
          }

        private double CalcoloQuantitaMQ(double L_, double H_, int pezzi, double fatt_min_Tot)
        {
            var qt = Math.Round(Math.Max(L_ / 100 * (H_ / 100), fatt_min_Tot) * pezzi, Variaglob.CifreDecimali_Quantita);
            MainPageInput.Quantita = qt;
            return qt;
        }
        private void Calcolo_QuantitaRullo()
        {
            MainPageInput.TotaleIvato = Math.Round(MainPageInput.CostoMeccanica + MainPageInput.CostoTessuto, Variaglob.CifreDecimali_Dettaglio);
            MainPageInput.TotaleImponibile = Math.Round(MainPageInput.TotaleIvato / double.Parse(MainPageInput.Articolo.Iva), Variaglob.CifreDecimali_Dettaglio);
            MainPageInput.Quantita = MainPageInput.Pezzi;///Calcolo quantita'
        }
        private void CalcoloSupplemento1(double quantita, double prezzo) //TODO: da elinminare
        {
            if (MainPageInput is not null && !string.IsNullOrEmpty(MainPageInput.Supplemento1))
            {
                MainPageInput.Supplemento1Quantita = Math.Round(quantita, Variaglob.CifreDecimali_Dettaglio);
                MainPageInput.Supplemento1Prezzo = prezzo;
                MainPageInput.Supplemento1PrezzoTotale = Math.Round(prezzo * quantita);
                Supplemento1_NomeSuppDaSalvare = $"{MainPageInput.Supplemento1}{Variaglob.Supplemento}";
            }
        }

        private void LoadDatiSupplemento1(string supplemento1)
        {
            var supplemento = Articoli.FirstOrDefault(x => x.Descrizione == supplemento1);
            if(supplemento != null)
            {
                if (Listino == "7")
                {
                    if (LoadPrezzoScontoPersonale(MainPageInput.Articolo.CodiceArticolo, Documento.Cliente.CodiceCliente, out double Prezzopersonalizzato, out double _))
                    {
                        if (Documento.Cliente.ListinoPersonale.Count == 0)
                        {
                            MainPageInput.Supplemento1Prezzo = Prezzopersonalizzato;
                        }
                        else
                        {
                            var listino = Documento.Cliente.ListinoPersonale.Where(x => x.CodiceArticolo == MainPageInput.Articolo.CodiceArticolo).FirstOrDefault();
                            if (listino != null) { MainPageInput.Supplemento1Prezzo = double.TryParse(listino.Prezzo, out double prezzo) ? prezzo : 0; }
                            else { MainPageInput.Supplemento1Prezzo = Prezzopersonalizzato; }
                        }
                    }
                    else
                    {
                        Shell.Current.DisplayAlert("Errore", "Quando si seleziona un listino personalizzato, deve essere importato un listino da 1 a 6 !!", "Ok");
                    }
                }
                else
                {
                    if (MainPageInput != null && supplemento != null)
                    {
                        switch (Listino)
                        {
                            case "1":
                                MainPageInput.Supplemento1Prezzo = double.TryParse(supplemento.Prezzo1, out double prezzo1) ? prezzo1 : 0;
                                break;
                            case "2":
                                MainPageInput.Supplemento1Prezzo = double.TryParse(supplemento.Prezzo2, out double prezzo2) ? prezzo2 : 0;
                                break;
                            case "3":
                                MainPageInput.Supplemento1Prezzo = double.TryParse(supplemento.Prezzo3, out double prezzo3) ? prezzo3 : 0;
                                break;
                            case "4":
                                MainPageInput.Supplemento1Prezzo = double.TryParse(supplemento.Prezzo4, out double prezzo4) ? prezzo4 : 0;
                                break;
                            case "5":
                                MainPageInput.Supplemento1Prezzo = double.TryParse(supplemento.Prezzo5, out double prezzo5) ? prezzo5 : 0;
                                break;
                            case "6":
                                MainPageInput.Supplemento1Prezzo = double.TryParse(supplemento.Prezzo6, out double prezzo6) ? prezzo6 : 0;
                                break;
                            default:
                                break;
                        }
                    }
                    var umSupp1 = supplemento?.Misura ?? string.Empty; //TODO: da verificare
                    double fattMinTotSuppl1;
                    if (!string.IsNullOrEmpty(supplemento.Generico2) && IsNumeric(Regex.Match(supplemento.Generico2, @"[0-9]*\.?[0-9]+").Value.Replace(".", ",")))
                    {
                        fattMinTotSuppl1 = double.Parse(Regex.Match(supplemento.Generico2, @"[0-9]*\.?[0-9]+").Value.Replace(".", ","));
                    }
                    else
                    {
                        fattMinTotSuppl1 = 0;
                    }
                    var noteSupplemento1Maesrto = supplemento?.NoteDb ?? string.Empty;
                    var CODICE_Suppl1 = supplemento?.Codedue ?? string.Empty;
                    ControlliInterfaccia.ListaSupplemento1Colore.Clear();
                    ControlliInterfaccia.ListaSupplemento1Colore = supplemento.Colori.Select(x => x.CodiceColore).OrderBy(x => x).Distinct().ToList();
                }
            }
            else
            {
                Shell.Current.DisplayAlert("Errore", $"Probabilmente {supplemento.Descrizione} ha un nome differente in maestro!", "Ok");
                ControlliInterfaccia.ListaSupplemento1.Clear();
                MainPageInput.Supplemento1Pezzi = 0;
                MainPageInput.Supplemento1Prezzo = 0;
                MainPageInput.Supplemento1PrezzoTotale = 0;
                MainPageInput.Supplemento1Quantita = 0;
            }
            Aggiorna();
        }
        private bool LoadPrezzoScontoPersonale(string codicearticolo, string codicecliente, out double Prezzopersonalizzato, out double ScontoPersonalizzato)
        {
            var prezzo = PrezziCliente.Where(x => x.CodiceArticolo == codicearticolo && x.CodiceCliente == codicecliente).FirstOrDefault();
            if(prezzo != null)
            {
                Prezzopersonalizzato = double.TryParse(prezzo.Prezzo, out double p) ? p : 0;
                ScontoPersonalizzato = double.TryParse(prezzo.Sconto, out double s) ? s : 0;
                return true;
            }
            else
            {
                Prezzopersonalizzato = 0;
                ScontoPersonalizzato = 0;
                return false;
            }
        }

        public bool IsNumeric(string numero)
        {
            try
            {
                Int32.Parse(numero);
                return true;
            }
            catch (System.FormatException)//aggiunto system novembre2019
            {
                return false;
            }
        }

        private void EmailAFornitori(double l, double h)
        {

        }
        private void Aggiorna()
        {
            OnPropertyChanged(nameof(GraphicsDrawable1));
            OnPropertyChanged(nameof(GraphicsDrawable2));
            OnPropertyChanged(nameof(GraphicsDrawable3));
            OnPropertyChanged(nameof(GraphicsDrawable4));
            OnPropertyChanged(nameof(GraphicsDrawable5));
            OnPropertyChanged(nameof(GraphicsDrawable6));
            OnPropertyChanged(nameof(GraphicsDrawable7));
            OnPropertyChanged(nameof(GraphicsDrawable8));
            OnPropertyChanged(nameof(GraphicsDrawable9));
            OnPropertyChanged(nameof(DrawingSurface1));
            OnPropertyChanged(nameof(DrawingSurface2));
            OnPropertyChanged(nameof(DrawingSurface3));
            OnPropertyChanged(nameof(DrawingSurface4));
            OnPropertyChanged(nameof(DrawingSurface5));
            OnPropertyChanged(nameof(DrawingSurface6));
            OnPropertyChanged(nameof(DrawingSurface7));
            OnPropertyChanged(nameof(DrawingSurface8));
            OnPropertyChanged(nameof(DrawingSurface9));
            OnPropertyChanged(nameof(MainPageInput));
            Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    DrawingSurface1.Dispose();
                    DrawingSurface2.Dispose();
                    DrawingSurface3.Dispose();
                    DrawingSurface4.Dispose();
                    DrawingSurface5.Dispose();
                    DrawingSurface6.Dispose();
                    DrawingSurface7.Dispose();
                    DrawingSurface8.Dispose();
                    DrawingSurface9.Dispose();
                }
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
