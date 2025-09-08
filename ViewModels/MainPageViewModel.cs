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
        private DettaglioDocumento? _dettaglioDocumento = new();

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
                if (DettaglioDocumento.Articolo is not null)
                {
                    DettaglioDocumento.PiuGuide = bool.Parse((string)value);
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
                if (DettaglioDocumento.Articolo is not null)
                {
                    DettaglioDocumento.LuceFinita = bool.Parse((string)value);
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
            if (DettaglioDocumento is not null)
            {
                var supplemento = ElencoArticoli.Single(x => x.Descrizione == DettaglioDocumento.Supplemento1);
                DettaglioDocumento.Supplemento1Prezzo = CalcolaPrezzo(supplemento, Documento.Cliente.Listino);
                DettaglioDocumento.Supplemento1Quantita = 1;
            }
            OnPropertyChanged(nameof(DettaglioDocumento));
            EseguiAggiornaEtichetta();
            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task ArticoloSelezionato()
        {
            if (DettaglioDocumento.Articolo is not null && !string.IsNullOrEmpty(DettaglioDocumento.Articolo.Barcode))
            {
                if (!string.IsNullOrEmpty(DettaglioDocumento.Articolo.Descrizione) && DettaglioDocumento.Articolo.Descrizione.Contains('*'))
                {
                    DescrizioneArticolo = DettaglioDocumento.Articolo.Descrizione;
                }
                if (!string.IsNullOrEmpty(Documento.Cliente.Listino))
                {
                    DettaglioDocumento.Prezzo = CalcolaPrezzo(DettaglioDocumento.Articolo, Documento.Cliente.Listino);

                }
                DettaglioDocumento.PrezzoIvato = Math.Round(DettaglioDocumento.Prezzo + (DettaglioDocumento.Prezzo * int.Parse(DettaglioDocumento.Articolo.Iva) / 100), 0);
                if (!string.IsNullOrEmpty(DettaglioDocumento.Articolo.Generico2))
                {
                    DettaglioDocumento.Quantita = CalcolaFornituraMinima(DettaglioDocumento.Articolo.Generico2);
                }

                OnPropertyChanged(nameof(DettaglioDocumento));
                EseguiAggiornaEtichetta();
            }
            return Task.CompletedTask;
        }
        [RelayCommand]
        private Task ColoreSelezionato()
        {
            if (DettaglioDocumento is not null && DettaglioDocumento.Colore is not null && Colore is not null && !string.IsNullOrEmpty(Colore.Barcode))
            {
                DettaglioDocumento.Colore = Colore.CodiceColore;
                var articolo = ElencoArticoli.Single(x => x.Barcode == Colore.Barcode);
                if (articolo is not null)
                {
                    if (!string.IsNullOrEmpty(articolo.Descrizione) && articolo.Descrizione.Contains('*'))
                    {
                        DescrizioneArticolo = articolo.Descrizione;
                    }
                    if (!string.IsNullOrEmpty(Documento.Cliente.Listino))
                    {
                        DettaglioDocumento.Prezzo = CalcolaPrezzo(articolo, Documento.Cliente.Listino);
                    }
                    if (!string.IsNullOrEmpty(articolo.Iva))
                    {
                        DettaglioDocumento.PrezzoIvato = Math.Round(DettaglioDocumento.Prezzo + (DettaglioDocumento.Prezzo * int.Parse(articolo.Iva) / 100), 0);
                    }
                    if (!string.IsNullOrEmpty(articolo.Generico2))
                    {
                        DettaglioDocumento.Quantita = CalcolaFornituraMinima(articolo.Generico2);
                    }
                    OnPropertyChanged(nameof(DettaglioDocumento));
                }
                EseguiAggiornaEtichetta();
            }
            return Task.CompletedTask;
        }
        [RelayCommand]
        private void EseguiAggiornaEtichetta()
        {

            if (DettaglioDocumento is not null)
            {
                if (Documento is not null && Documento.Cliente is not null)
                {
                    DettaglioDocumento.RagioneSociale = Documento.Cliente.RagioneSociale;
                }
                AggiornaEtichetta();
                //if (DettaglioDocumento.GraphicsDrawable1 is not null)
                //{
                //    GraphicsDrawable1 = DettaglioDocumento.GraphicsDrawable1;
                //}
                //if (DettaglioDocumento.GraphicsDrawable2 is not null)
                //{
                //    GraphicsDrawable2 = DettaglioDocumento.GraphicsDrawable2;
                //}
                //if (DettaglioDocumento.GraphicsDrawable3 is not null)
                //{
                //    GraphicsDrawable3 = DettaglioDocumento.GraphicsDrawable3;
                //}
                if (Documento is not null && Documento.Cliente is not null)
                {
                    if (string.IsNullOrEmpty(Documento.Cliente.Sconto)) { Documento.Cliente.Sconto = "0"; }
                    double Sconto = (100 - double.Parse(Documento.Cliente.Sconto)) / 100;
                    if (DettaglioDocumento.Articolo is not null)
                    {
                        try
                        {
                            if (Ivato)
                            {
                                TotaleInserimento = Math.Round((DettaglioDocumento.Quantita * DettaglioDocumento.PrezzoIvato) + (DettaglioDocumento.Supplemento1PrezzoTotale + DettaglioDocumento.Supplemento2PrezzoTotale) * (double.Parse($"1,{DettaglioDocumento.Articolo.Iva}") * Sconto));
                            }
                            else
                            {
                                TotaleInserimento = Math.Round((DettaglioDocumento.Quantita * DettaglioDocumento.PrezzoIvato) + (DettaglioDocumento.Supplemento1PrezzoTotale + DettaglioDocumento.Supplemento2PrezzoTotale) * Sconto);
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
            DettaglioDocumento = new();
            NuovoProdotto();
        }
        [RelayCommand]
        private void NuovoProdotto()
        {
            DettaglioDocumento.Articolo = new Articolo();
            Colore = new Colore();
            DettaglioDocumento.Articolo.Colori = [];
            DescrizioneArticolo = "";
            Colori = [];
            GraphicsDrawable1 = new EtichettaVuota();
            GraphicsDrawable2 = new EtichettaVuota();
            GraphicsDrawable3 = new EtichettaVuota();
            OnPropertyChanged(nameof(DettaglioDocumento));
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
            if (DettaglioDocumento is not null)
            {
                DettaglioDocumento.Diviso2 = !DettaglioDocumento.Diviso2;
                //TODO: gestire pulsante /2
            }
        }
        [RelayCommand]
        private void Diviso2CU()
        {
            if (DettaglioDocumento is not null)
            {
                DettaglioDocumento.Diviso2CU = !DettaglioDocumento.Diviso2CU;
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
            if (DettaglioDocumento.Articolo != null && DettaglioDocumento.Articolo.Descrizione != null)
            {
                switch (DettaglioDocumento.Articolo.Descrizione.ToLower().TrimStart())
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
                    case "rullo mini cass 36":
                        RulloMiniCass36_43(2, 2.2f, 2.7f, 2.6f, 4.5f, "R mini cass36");
                        break;
                    case "rullo mini cass 43":
                        RulloMiniCass36_43(2, 2.3f, 2.8f, 2.7f, 5.5f, "R mini cass43");
                        break;
                    case "rullo mini molla cass 43":
                        RulloMiniMollaCass43();
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
                        Avvolgibili_varie(0.8f, DettaglioDocumento.Articolo.Descrizione);
                        break;
                    case "avvolgibile pvc" or "avvolgibile pvc mod sole":
                        Avvolgibili_varie(0.6f, "Avvolgibile PVC");
                        break;
                    case "avvolgibile new solar" or "avvolgibile new solar mini" or "avvolgibile new solar elephant":
                        Avvolgibili_varie(2.4f, DettaglioDocumento.Articolo.Descrizione);
                        break;
                    case "girasole" or "orienta":
                        Girasole(DettaglioDocumento.Articolo.Descrizione);
                        break;

                    default: TuttGliAltri(); break;
                };
            }
        }

        //Veneziane
        private void Veneziane15()
        {
            double luce = DettaglioDocumento.LuceFinita ? -0.5 : 0;
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

            if (DettaglioDocumento.LNumber > 0 && DettaglioDocumento.HNumber > 0)
            {
                DettaglioDocumento.LuceLperEtich = Math.Round(DettaglioDocumento.LNumber + luce, 1); //per etichetta
                DettaglioDocumento.LuceHperEtich = Math.Round(DettaglioDocumento.HNumber, 1); //per etichetta
                if (DettaglioDocumento.LNumber > 350 && !DettaglioDocumento.Diviso2 && !DettaglioDocumento.Diviso2CU) { ControlliInterfaccia.MessaggioErrore = "Misura presumibilmente sbagliata"; } //TODO: Manca verifica pulsante /2
                if (DettaglioDocumento.HNumber > 450) { ControlliInterfaccia.MessaggioErrore = "Misura presumibilmente sbagliata"; }
                //888888888888888888888888888888888888888888888888888888888888888888888888  2 fori 88888888888888888888888888888888888888888888888888888888888888888888888888
                if (DettaglioDocumento.LNumber <= 66.9)
                {
                    calcoloFloat = 0;

                    if (DettaglioDocumento.LNumber <= 27)
                    {
                        if (DettaglioDocumento.LNumber <= 15)
                        {
                            CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, 0);
                        }
                        else
                        {
                            CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, DettaglioDocumento.LNumber - 7);
                        }
                    }
                    else
                    {
                        CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, DettaglioDocumento.LNumber - 15);
                    }
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888 3 fori 2/3 cordini 8888888888888888888888888888888888888888888888888888888888888888
                else if (DettaglioDocumento.LNumber <= 112.9)
                {
                    calcoloFloat = (DettaglioDocumento.LNumber + luce - 29) / 2;
                    if (DettaglioDocumento.LNumber <= 100)
                    {
                        CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, DettaglioDocumento.LNumber - 15);
                    }
                    else
                    {
                        CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, DettaglioDocumento.LNumber - 15, 1);
                    }
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888  4 fori 4 cordini 8888888888888888888888888888888888888888888888888888888888888888
                else if (DettaglioDocumento.LNumber <= 152.9)
                {
                    calcoloFloat = (DettaglioDocumento.LNumber + luce - 29) / 3;
                    CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, DettaglioDocumento.LNumber - 15, 2);
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888  5 fori 4 cordini  8888888888888888888888888888888888888888888888888888888888888888
                else if (DettaglioDocumento.LNumber <= 193.9)
                {
                    calcoloFloat = (DettaglioDocumento.LNumber + luce - 29) / 4;
                    CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, DettaglioDocumento.LNumber - 15, 2);
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888  6 fori 4 cordini  8888888888888888888888888888888888888888888888888888888888888888
                else if (DettaglioDocumento.LNumber <= 233.9)
                {
                    calcoloFloat = (DettaglioDocumento.LNumber + luce - 29) / 5;
                    CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, DettaglioDocumento.LNumber - 15, 2);
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888  7 fori 4 cordini  8888888888888888888888888888888888888888888888888888888888888888
                else
                {
                    calcoloFloat = (DettaglioDocumento.LNumber + luce - 29) / 6;
                    CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, DettaglioDocumento.LNumber - 15, 2);
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                double Fatt_min_Tot_temp = Fatt_min_Tot;
                if (User == "io" && Documento.Cliente.Listino == "1") Fatt_min_Tot_temp = 1.5;
                CalcoloQuantitaMQ(DettaglioDocumento.LNumber, DettaglioDocumento.HNumber, DettaglioDocumento.Pezzi, Fatt_min_Tot_temp);
                CalcoloSupplemento1(DettaglioDocumento.Supplemento1Quantita, DettaglioDocumento.Supplemento1Prezzo);
                EmailAFornitori(DettaglioDocumento.LNumber, DettaglioDocumento.HNumber);
            }
            else
            {
                luceLperEtich = 0;
            }
            var etichetta = new Etichetta { Larghezza = 330, Altezza = 135, Alias = DettaglioDocumento.RagioneSociale ?? string.Empty, Colore = DettaglioDocumento.Colore ?? string.Empty, LuceLEtichetta = DettaglioDocumento.LuceLperEtich, LuceHEtichetta = DettaglioDocumento.LuceHperEtich, H = DettaglioDocumento.HNumber, Comandi = DettaglioDocumento.Comandi, Hc = DettaglioDocumento.Hc, Attacchi = DettaglioDocumento.Attacchi, PiuGuide = DettaglioDocumento.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = DettaglioDocumento.Supplemento1 ?? string.Empty, CordiniVeneziane = cordiniVeneziane, Note = DettaglioDocumento.Note, Rif = DettaglioDocumento.Rif };
            using (var etichettaVeneziane15 = new EtichettaVeneziane15mm(etichetta))
            {
                GraphicsDrawable1 = etichettaVeneziane15;
                DrawingSurface1 = etichettaVeneziane15.ToBitmap();
            }
            Aggiorna();
        }
        private void Veneziane25()
        {
            double luce = DettaglioDocumento.LuceFinita ? -0.5 : 0;
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

            if (DettaglioDocumento.LNumber > 0 && DettaglioDocumento.HNumber > 0)
            {
                luceLperEtich = Math.Round(DettaglioDocumento.LNumber + luce, 1); //per etichetta
                luceHperEtich = Math.Round(DettaglioDocumento.HNumber, 1); //per etichetta
                if (DettaglioDocumento.LNumber > 350 && !DettaglioDocumento.Diviso2 && !DettaglioDocumento.Diviso2CU) { ControlliInterfaccia.MessaggioErrore = "Misura presumibilmente sbagliata"; } //TODO: Manca verifica pulsante /2
                if (DettaglioDocumento.HNumber > 450) { ControlliInterfaccia.MessaggioErrore = "Misura presumibilmente sbagliata"; }
                //888888888888888888888888888888888888888888888888888888888888888888888888  2 fori 88888888888888888888888888888888888888888888888888888888888888888888888888
                if (DettaglioDocumento.LNumber <= 77.9)
                {
                    calcoloFloat = 0;

                    if (DettaglioDocumento.LNumber <= 27)
                    {
                        if (DettaglioDocumento.LNumber <= 15)
                        {
                            CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, 0);
                        }
                        else
                        {
                            CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, DettaglioDocumento.LNumber - 7);
                        }
                    }
                    else
                    {
                        CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, DettaglioDocumento.LNumber - 15);
                    }
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888 3 fori 2/3 cordini 8888888888888888888888888888888888888888888888888888888888888888
                else if (DettaglioDocumento.LNumber <= 132.9)
                {
                    calcoloFloat = (DettaglioDocumento.LNumber + luce - 29) / 2;
                    if (DettaglioDocumento.LNumber <= 120)
                    {
                        CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, DettaglioDocumento.LNumber - 15);
                    }
                    else
                    {
                        CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, DettaglioDocumento.LNumber - 15, 1);
                    }
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888  4 fori 4 cordini 8888888888888888888888888888888888888888888888888888888888888888
                else if (DettaglioDocumento.LNumber <= 190.9)
                {
                    calcoloFloat = (DettaglioDocumento.LNumber + luce - 29) / 3;
                    CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, DettaglioDocumento.LNumber - 15, 2);
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888  5 fori 4 cordini  8888888888888888888888888888888888888888888888888888888888888888
                else if (DettaglioDocumento.LNumber <= 209.9)
                {
                    calcoloFloat = (DettaglioDocumento.LNumber + luce - 29) / 4;
                    CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, DettaglioDocumento.LNumber - 15, 2);
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888  6 fori 4 cordini  8888888888888888888888888888888888888888888888888888888888888888
                else if (DettaglioDocumento.LNumber <= 289.9)
                {
                    calcoloFloat = (DettaglioDocumento.LNumber + luce - 29) / 5;
                    CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, DettaglioDocumento.LNumber - 15, 2);
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888  7 fori 4 cordini  8888888888888888888888888888888888888888888888888888888888888888
                else
                {
                    calcoloFloat = (DettaglioDocumento.LNumber + luce - 29) / 6;
                    CalcoliVari.CalcoloTiraggioOrientatore_15_25(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, DettaglioDocumento.LNumber - 15, 2);
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                double Fatt_min_Tot_temp = Fatt_min_Tot;
                if (User == "io" && Documento.Cliente.Listino == "1") Fatt_min_Tot_temp = 1.5;
                CalcoloQuantitaMQ(DettaglioDocumento.LNumber, DettaglioDocumento.HNumber, DettaglioDocumento.Pezzi, Fatt_min_Tot_temp);
                CalcoloSupplemento1(DettaglioDocumento.Supplemento1Quantita, DettaglioDocumento.Supplemento1Prezzo);
                EmailAFornitori(DettaglioDocumento.LNumber, DettaglioDocumento.HNumber);
            }
            else
            {
                luceLperEtich = 0;
            }
            var etichetta = new Etichetta { Larghezza = 330, Altezza = 135, Alias = DettaglioDocumento.RagioneSociale ?? string.Empty, Colore = DettaglioDocumento.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = DettaglioDocumento.HNumber, Comandi = DettaglioDocumento.Comandi, Hc = DettaglioDocumento.Hc, Attacchi = DettaglioDocumento.Attacchi, PiuGuide = DettaglioDocumento.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = DettaglioDocumento.Supplemento1 ?? string.Empty, CordiniVeneziane = cordiniVeneziane, Note = DettaglioDocumento.Note, Rif = DettaglioDocumento.Rif };
            using (var etichettaVeneziane25 = new EtichettaVeneziane25mm(etichetta))
            {
                GraphicsDrawable1 = etichettaVeneziane25;
                DrawingSurface1 = etichettaVeneziane25.ToBitmap();
            }

            Aggiorna();
        }
        private void Veneziane35()
        {
            double luce = DettaglioDocumento.LuceFinita ? -1 : 0;
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

            if (DettaglioDocumento.LNumber > 0 && DettaglioDocumento.HNumber > 0)
            {
                luceLperEtich = Math.Round(DettaglioDocumento.LNumber + luce, 1); //per etichetta
                luceHperEtich = Math.Round(DettaglioDocumento.HNumber, 1); //per etichetta
                if (DettaglioDocumento.LNumber > 350 && !DettaglioDocumento.Diviso2 && !DettaglioDocumento.Diviso2CU) { ControlliInterfaccia.MessaggioErrore = "Misura presumibilmente sbagliata"; } //TODO: Manca verifica pulsante /2
                if (DettaglioDocumento.HNumber > 450) { ControlliInterfaccia.MessaggioErrore = "Misura presumibilmente sbagliata"; }
                //888888888888888888888888888888888888888888888888888888888888888888888888  2 fori 88888888888888888888888888888888888888888888888888888888888888888888888888
                if (DettaglioDocumento.LNumber <= 90)
                {
                    calcoloFloat = 0;

                    if (DettaglioDocumento.LNumber <= 58)
                    {
                        CalcoliVari.CalcoloTiraggioOrientatore_35_50(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, +5, 40);
                    }
                    else
                    {
                        CalcoliVari.CalcoloTiraggioOrientatore_35_50(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, -10, 40);
                    }
                }
                //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                else if (DettaglioDocumento.LNumber <= 154.9)
                {
                    calcoloFloat = (DettaglioDocumento.LNumber + luce - 42) / 2;
                    CalcoliVari.CalcoloTiraggioOrientatore_35_50(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, -10, 40);
                }
                //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                else if (DettaglioDocumento.LNumber <= 205.9)
                {
                    calcoloFloat = (DettaglioDocumento.LNumber + luce - 42) / 3;
                    CalcoliVari.CalcoloTiraggioOrientatore_35_50(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, -10, 40);
                }
                //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                else
                {
                    calcoloFloat = (DettaglioDocumento.LNumber + luce - 42) / 4;
                    CalcoliVari.CalcoloTiraggioOrientatore_35_50(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, -10, 40, 1);
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                CalcoloQuantitaMQ(DettaglioDocumento.LNumber, DettaglioDocumento.HNumber, DettaglioDocumento.Pezzi, Fatt_min_Tot);
                EmailAFornitori(DettaglioDocumento.LNumber, DettaglioDocumento.HNumber);
            }
            else
            {
                luceLperEtich = 0;
                luceHperEtich = 0;
            }
            var etichetta = new Etichetta { Larghezza = 330, Altezza = 135, Alias = DettaglioDocumento.RagioneSociale ?? string.Empty, Colore = DettaglioDocumento.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = DettaglioDocumento.HNumber, Comandi = DettaglioDocumento.Comandi, Hc = DettaglioDocumento.Hc, Attacchi = DettaglioDocumento.Attacchi, PiuGuide = DettaglioDocumento.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = DettaglioDocumento.Supplemento1 ?? string.Empty, CordiniVeneziane = cordiniVeneziane, Note = DettaglioDocumento.Note, Rif = DettaglioDocumento.Rif };
            using (var etichettaVeneziane35 = new EtichettaVeneziane35mm(etichetta))
            {
                GraphicsDrawable1 = etichettaVeneziane35;
                DrawingSurface1 = etichettaVeneziane35.ToBitmap();
            }

            Aggiorna();
        }
        private void Veneziane50()
        {
            double luce = DettaglioDocumento.LuceFinita ? -1 : 0;
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

            if (DettaglioDocumento.LNumber > 0 && DettaglioDocumento.HNumber > 0)
            {
                luceLperEtich = Math.Round(DettaglioDocumento.LNumber + luce, 1); //per etichetta
                luceHperEtich = Math.Round(DettaglioDocumento.HNumber, 1); //per etichetta
                if (DettaglioDocumento.LNumber > 350 && !DettaglioDocumento.Diviso2 && !DettaglioDocumento.Diviso2CU) { ControlliInterfaccia.MessaggioErrore = "Misura presumibilmente sbagliata"; }
                if (DettaglioDocumento.HNumber > 450) { ControlliInterfaccia.MessaggioErrore = "Misura presumibilmente sbagliata"; }
                //888888888888888888888888888888888888888888888888888888888888888888888888  2 fori 88888888888888888888888888888888888888888888888888888888888888888888888888
                if (DettaglioDocumento.LNumber <= 99)
                {
                    calcoloFloat = 0;

                    if (DettaglioDocumento.LNumber <= 58)
                    {
                        CalcoliVari.CalcoloTiraggioOrientatore_35_50(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, +5, 70);
                    }
                    else
                    {
                        CalcoliVari.CalcoloTiraggioOrientatore_35_50(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, -10, 70);
                    }
                }
                //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                else if (DettaglioDocumento.LNumber <= 168.9)
                {
                    calcoloFloat = (DettaglioDocumento.LNumber + luce - 42) / 2;
                    CalcoliVari.CalcoloTiraggioOrientatore_35_50(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, -10, 70);
                }
                //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                else if (DettaglioDocumento.LNumber <= 223.9)
                {
                    calcoloFloat = (DettaglioDocumento.LNumber + luce - 42) / 3;
                    CalcoliVari.CalcoloTiraggioOrientatore_35_50(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, -10, 70);
                }
                //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                else
                {
                    calcoloFloat = (DettaglioDocumento.LNumber + luce - 42) / 4;
                    CalcoliVari.CalcoloTiraggioOrientatore_35_50(ref cordiniVeneziane, DettaglioDocumento.Hc, DettaglioDocumento.HNumber, -10, 70, 1);
                }
                //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                CalcoloQuantitaMQ(DettaglioDocumento.LNumber, DettaglioDocumento.HNumber, DettaglioDocumento.Pezzi, Fatt_min_Tot);
                EmailAFornitori(DettaglioDocumento.LNumber, DettaglioDocumento.HNumber);
            }
            else
            {
                luceLperEtich = 0;
                luceHperEtich = 0;
            }
            var etichetta = new Etichetta { Larghezza = 330, Altezza = 135, Alias = DettaglioDocumento.RagioneSociale ?? string.Empty, Colore = DettaglioDocumento.Colore ?? string.Empty, LuceLEtichetta = luceLperEtich, LuceHEtichetta = luceHperEtich, H = DettaglioDocumento.HNumber, Comandi = DettaglioDocumento.Comandi, Hc = DettaglioDocumento.Hc, Attacchi = DettaglioDocumento.Attacchi, PiuGuide = DettaglioDocumento.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = DettaglioDocumento.Supplemento1 ?? string.Empty, CordiniVeneziane = cordiniVeneziane, Note = DettaglioDocumento.Note, Rif = DettaglioDocumento.Rif };
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
            string varie2 = DettaglioDocumento.AperturaCentrale ? "Apert. Centrale" : "";
            ControlliInterfaccia = new ControlliInterfaccia
            {
                HcVisible = true,
                AperturaCentraleVisible = true,
                ListaNote = ["Cass. col Argento", "Cass. col Avorio", "Comando invertito", "INCLINATA", "!!NOTE!!NOTE!!"],
            };
            if (DettaglioDocumento.LuceFinita) { DettaglioDocumento.LuceH = -3; DettaglioDocumento.LuceL = -0.3; }
            if (DettaglioDocumento.LNumber > 0 && DettaglioDocumento.HNumber > 0)
            {
                DettaglioDocumento.LuceHperEtich = Math.Round(DettaglioDocumento.HNumber + DettaglioDocumento.LuceH, 1);
                DettaglioDocumento.LuceLperEtich = Math.Round(DettaglioDocumento.LNumber + DettaglioDocumento.LuceL, 1);
                DettaglioDocumento.MixBandaFinita = DettaglioDocumento.HNumber + DettaglioDocumento.LuceH - 4;
                DettaglioDocumento.MixTaglioCassonetto = Math.Round((luceLperEtich - 1.5) * 10, 1);
                DettaglioDocumento.MixTaglioTubo = Math.Round((luceLperEtich - 3.1) * 10, 1);
                EmailAFornitori(DettaglioDocumento.LNumber, DettaglioDocumento.HNumber);
                CalcoloQuantitaMQ(DettaglioDocumento.LNumber, Math.Max(DettaglioDocumento.HNumber, 200), DettaglioDocumento.Pezzi, 2.5);
            }
            if (DettaglioDocumento.AperturaCentrale)
            {
                ControlliInterfaccia.ListaSupplemento1 = ["Apertura centrale su Verticale"];
                LoadDatiSupplemento1("Apertura centrale su Verticale");
                CalcoloSupplemento1(DettaglioDocumento.Pezzi, DettaglioDocumento.Pezzi);
            }
            else
            {
                ControlliInterfaccia.ListaSupplemento1.Clear();
                DettaglioDocumento.Supplemento1Pezzi = 0;
                DettaglioDocumento.Supplemento1Prezzo = 0;
                DettaglioDocumento.Supplemento1PrezzoTotale = 0;
                DettaglioDocumento.Supplemento1Quantita = 0;
            }
            var etichetta = new Etichetta { Larghezza = 330, Altezza = 135, Alias = DettaglioDocumento.RagioneSociale ?? string.Empty, Colore = DettaglioDocumento.Colore ?? string.Empty, LuceLEtichetta = DettaglioDocumento.LuceLperEtich, LuceHEtichetta = DettaglioDocumento.LuceHperEtich, H = DettaglioDocumento.HNumber, L = DettaglioDocumento.LNumber, AperturaCentrale = DettaglioDocumento.AperturaCentrale, Comandi = DettaglioDocumento.Comandi, Hc = DettaglioDocumento.Hc, Attacchi = DettaglioDocumento.Attacchi, PiuGuide = DettaglioDocumento.PiuGuide, CalcoloFloat = calcoloFloat, Supplemento1 = DettaglioDocumento.Supplemento1 ?? string.Empty, Note = DettaglioDocumento.Note, Rif = DettaglioDocumento.Rif, MixBandaFinita = DettaglioDocumento.MixBandaFinita, MixTaglioCassonetto = DettaglioDocumento.MixTaglioCassonetto, MixTaglioTubo = DettaglioDocumento.MixTaglioTubo };
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
            Aggiorna();
        }
        private void BandeVerticaliComplete()
        {
            Aggiorna();
        }
        //Binario Pannello
        private void BinarioVerticale()
        {
            Aggiorna();
        }
        private void BinarioPannelloCorda(int n)
        {
            Aggiorna();
        }
        private void BinarioPannelloAsta(int n)
        {
            Aggiorna();
        }
        private void BinarioPannelloManuale(int n)
        {
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
            Aggiorna();
        }
        private void ReteSaldataZebrata()
        {
            Aggiorna();
        }
        private void ZanzarieraFissa()
        {
            Aggiorna();
        }
        private void Unika45()
        {
            Aggiorna();
        }
        private void Incassata45PiUnika45()
        {
            Aggiorna();
        }
        private void ZanzariereAMolla(string nomezanzariera)
        {
            Aggiorna();
        }
        private void ZanzariereAMollaDoppiaG()
        {
            Aggiorna();
        }
        private void ZanzariereACatena(float Quotacassone, float Quotaguide, string Nomezanzariera)
        {
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
            Aggiorna();
        }
        private void BinarioStrappo()
        {
            Aggiorna();
        }
        //Porta a soffietto
        private void PortaASoffietto()
        {
            Aggiorna();
        }
        //Rullo
        private void Cubo80()
        {
            Aggiorna();
        }
        private void Cubo100()
        {
            Aggiorna();
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
            Aggiorna();
        }
        private void RulloMedio()
        {
            Aggiorna();
        }
        private void RulloAngolar()
        {
            Aggiorna();
        }
        private void RulloStandardMedioPortarullo(float QuotaprofiloSuperiore, float Quotatubo, string NomeRullo)
        {
            Aggiorna();
        }
        private void RulloAngolarPortarullo()
        {
            Aggiorna();
        }
        private void RulloMedioMotore()
        {
            Aggiorna();
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
            Aggiorna();
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
            Aggiorna();
        }
        private void Rullo_Cass_63_83_110_mot(float Quotacassone, float Quotatubo, string Tiporullo, string Nomerullo)//aggiungere tipo tubo
        {
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
            Aggiorna();
        }
        private void Lucernaio63()
        {
            Aggiorna();
        }
        private void Lucernaio83_110(float Quotacassone, float Quotatubo, float Quotatubo2, float Quotaguide, string Nomerullo)
        {
            Aggiorna();
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
            Aggiorna();
        }
        private void RulloMantovanaMotore()
        {
            Aggiorna();
        }
        private void RulloMiniCass36_43(float Quotacassone, float Quotatubo, float Quotatessuto, float Quotafondale, float Quotaguide, string Nomerullo)
        {
            Aggiorna();
        }
        private void RulloMiniMollaCass43()
        {
            Aggiorna();
        }
        private void RulloMiniMolla()
        {
            Aggiorna();
        }
        private void Sole_33_41_Cat(float Quotaguide, string Nomerullo)
        {
            Aggiorna();
        }
        private void Sole_33_41_Cat_Front(float Quotaguide, string Nomerullo)
        {
            Aggiorna();
        }
        private void Sole_33_41_Molla(float Quotaguide, string Nomerullo)
        {
            Aggiorna();
        }
        private void Sole_33_41_Molla_Front(float Quotaguide, string Nomerullo)
        {
            Aggiorna();
        }
        private void Sole_33_Motor()
        {
            Aggiorna();
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
            Aggiorna();
        }
        private void Telo_pannello_confezionato()//
        {
            Aggiorna();
        }
        private void Avvolgibili_varie(float Quotastecca, string NomeAvv)////////////////////////////////////////AVVOLGIBILI///////////////////////////AVVOLGIBILI//////////////////////////////////////////
        {
            Aggiorna();
        }
        private void Girasole(string NomeAvv)
        {
            Aggiorna();
        }
        //___________________________________________________________________________________
        private void TuttGliAltri()
        {
            Aggiorna();
        }

        private double CalcoloQuantitaMQ(double L_, double H_, int pezzi, double fatt_min_Tot)
        {
            var qt = Math.Round(Math.Max(L_ / 100 * (H_ / 100), fatt_min_Tot) * pezzi, Variaglob.CifreDecimali_Quantita);
            DettaglioDocumento.Quantita = qt;
            return qt;
        }
        private void Calcolo_QuantitaRullo()
        {
            DettaglioDocumento.TotaleIvato = Math.Round(DettaglioDocumento.CostoMeccanica + DettaglioDocumento.CostoTessuto, Variaglob.CifreDecimali_Dettaglio);
            DettaglioDocumento.TotaleImponibile = Math.Round(DettaglioDocumento.TotaleIvato / double.Parse(DettaglioDocumento.Articolo.Iva), Variaglob.CifreDecimali_Dettaglio);
            DettaglioDocumento.Quantita = DettaglioDocumento.Pezzi;///Calcolo quantita'
        }
        private void CalcoloSupplemento1(double quantita, double prezzo) //TODO: da elinminare
        {
            if (DettaglioDocumento is not null && !string.IsNullOrEmpty(DettaglioDocumento.Supplemento1))
            {
                DettaglioDocumento.Supplemento1Quantita = Math.Round(quantita, Variaglob.CifreDecimali_Dettaglio);
                DettaglioDocumento.Supplemento1Prezzo = prezzo;
                DettaglioDocumento.Supplemento1PrezzoTotale = Math.Round(prezzo * quantita);
                Supplemento1_NomeSuppDaSalvare = $"{DettaglioDocumento.Supplemento1}{Variaglob.Supplemento}";
            }
        }

        private void LoadDatiSupplemento1(string supplemento1)
        {
            var supplemento = Articoli.FirstOrDefault(x => x.Descrizione == supplemento1);
            if(supplemento != null)
            {
                if (Listino == "7")
                {
                    if (LoadPrezzoScontoPersonale(DettaglioDocumento.Articolo.CodiceArticolo, Documento.Cliente.CodiceCliente, out double Prezzopersonalizzato, out double _))
                    {
                        if (Documento.Cliente.ListinoPersonale.Count == 0)
                        {
                            DettaglioDocumento.Supplemento1Prezzo = Prezzopersonalizzato;
                        }
                        else
                        {
                            var listino = Documento.Cliente.ListinoPersonale.Where(x => x.CodiceArticolo == DettaglioDocumento.Articolo.CodiceArticolo).FirstOrDefault();
                            if (listino != null) { DettaglioDocumento.Supplemento1Prezzo = double.TryParse(listino.Prezzo, out double prezzo) ? prezzo : 0; }
                            else { DettaglioDocumento.Supplemento1Prezzo = Prezzopersonalizzato; }
                        }
                    }
                    else
                    {
                        Shell.Current.DisplayAlert("Errore", "Quando si seleziona un listino personalizzato, deve essere importato un listino da 1 a 6 !!", "Ok");
                    }
                }
                else
                {
                    if (DettaglioDocumento != null && supplemento != null)
                    {
                        switch (Listino)
                        {
                            case "1":
                                DettaglioDocumento.Supplemento1Prezzo = double.TryParse(supplemento.Prezzo1, out double prezzo1) ? prezzo1 : 0;
                                break;
                            case "2":
                                DettaglioDocumento.Supplemento1Prezzo = double.TryParse(supplemento.Prezzo2, out double prezzo2) ? prezzo2 : 0;
                                break;
                            case "3":
                                DettaglioDocumento.Supplemento1Prezzo = double.TryParse(supplemento.Prezzo3, out double prezzo3) ? prezzo3 : 0;
                                break;
                            case "4":
                                DettaglioDocumento.Supplemento1Prezzo = double.TryParse(supplemento.Prezzo4, out double prezzo4) ? prezzo4 : 0;
                                break;
                            case "5":
                                DettaglioDocumento.Supplemento1Prezzo = double.TryParse(supplemento.Prezzo5, out double prezzo5) ? prezzo5 : 0;
                                break;
                            case "6":
                                DettaglioDocumento.Supplemento1Prezzo = double.TryParse(supplemento.Prezzo6, out double prezzo6) ? prezzo6 : 0;
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
                DettaglioDocumento.Supplemento1Pezzi = 0;
                DettaglioDocumento.Supplemento1Prezzo = 0;
                DettaglioDocumento.Supplemento1PrezzoTotale = 0;
                DettaglioDocumento.Supplemento1Quantita = 0;
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
            OnPropertyChanged(nameof(DettaglioDocumento));
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
