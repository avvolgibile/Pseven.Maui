using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.ObjectModel;

namespace Pseven.Models
{
    #region Enumerators
    public enum TipoDocumento
    {
        Ordine,
        Preventivo
    }
    public enum DimensioniEtichetta
    {
        width = 330,
        height = 135,
        None = 0
    }
    #endregion

    #region Models
    public class Categoria
    {
        public int CategoriaId { get; set; }
        public string? CategoriaName { get; set; }
        public Sottocategoria? Sottocategoria { get; set; }
    }
    public class Sottocategoria
    {
        public int SottocategoriaId { get; set; }
        public string? SottocategoriaName { get; set; }
    }
    public partial class Articolo
    {
        [PrimaryKey, AutoIncrement]
        public int ArticoloId { get; set; }
        [JsonProperty("DESCRIZION")]
        public string? Descrizione { get; set; }
        [JsonProperty("PREZZOACQ")]
        public string? PrezzoAcquisto { get; set; }
        [JsonProperty("IVA")]
        public string? Iva { get; set; }
        [JsonProperty("PREZZO1")]
        public string? Prezzo1 { get; set; }
        [JsonProperty("CODICE")]
        public string? CodiceArticolo { get; set; }
        [JsonProperty("FORNITORE")]
        public string? Fornitore { get; set; }
        [JsonProperty("CARICO")]
        public string? Carico { get; set; }
        [JsonProperty("SCORTA")]
        public string? Scorta { get; set; }
        [JsonProperty("VALORE")]
        public string? Valore { get; set; }
        [JsonProperty("SCONTO")]
        public string? Sconto { get; set; }
        [JsonProperty("PREZZO2")]
        public string? Prezzo2 { get; set; }
        [JsonProperty("PREZZO3")]
        public string? Prezzo3 { get; set; }
        [JsonProperty("CODEDUE")]
        public string? Codedue { get; set; }
        [JsonProperty("BARCODE")]
        public string? Barcode { get; set; }
        [JsonProperty("MISURA")]
        public string? Misura { get; set; }
        [JsonProperty("RICARICO")]
        public string? Ricarico { get; set; }
        [JsonProperty("PROVVIGION")]
        public string? Provvigion { get; set; }
        [JsonProperty("IMBALLO")]
        public string? Imballo { get; set; }
        [JsonProperty("SCOMAX")]
        public string? Scomax { get; set; }
        [JsonProperty("PESO")]
        public string? Peso { get; set; }
        [JsonProperty("INGROSSO")]
        public string? Ingrosso { get; set; }
        [JsonProperty("CARICHI")]
        public string? Carichi{ get; set; }
        [JsonProperty("SCARICHI")]
        public string? Scarichi { get; set; }
        [JsonProperty("RICARING")]
        public string? Ricaring { get; set; }
        [JsonProperty("ETICHETTE")]
        public string? Etichette { get; set; }
        [JsonProperty("NUMFOR")]
        public string? NumFor { get; set; }
        [JsonProperty("NUMREG")]
        public string? NumReg { get; set; }
        [JsonProperty("UBICAZIONE")]
        public string? Ubicazione { get; set; }
        [JsonProperty("PRZBASE1")]
        public string? PrzBase1 { get; set; }
        [JsonProperty("PRZBASE2")]
        public string? PrzBase2 { get; set; }
        [JsonProperty("CHECKIVA1")]
        public string? CheckIva1 { get; set; }
        [JsonProperty("CHECKIVA2")]
        public string? CheckIva2 { get; set; }
        [JsonProperty("NUMCAT")]
        public string? NumCat { get; set; }
        [JsonProperty("CATEGORIA")]
        public string? Categoria { get; set; }
        [JsonProperty("SCOACQ")]
        public string? ScoAcq { get; set; }
        [JsonProperty("PRZBASE3")]
        public string? PrzBase3 { get; set; }
        [JsonProperty("CHECKIVA3")]
        public string? CheckIva3 { get; set; }
        [JsonProperty("RICARTRE")]
        public string? RicarTre { get; set; }
        [JsonProperty("FOTO")]
        public string? Foto { get; set; }
        [JsonProperty("REPARTO")]
        public string? Reparto { get; set; }
        [JsonProperty("NOTE")]
        public string? NoteDb{ get; set; }
        [JsonProperty("SCOAC2")]
        public string? ScoAc2 { get; set; }
        [JsonProperty("MAGAZIN")]
        public string? Magazin { get; set; }
        [JsonProperty("COMPOSTO")]
        public string? Composto { get; set; }
        [JsonProperty("GLOTTO")]
        public string? GLotto { get; set; }
        [JsonProperty("DATACR")]
        public string? DataCr { get; set; }
        [JsonProperty("DATACA")]
        public string? DataCa { get; set; }
        [JsonProperty("DATASC")]
        public string? DataSc { get; set; }
        [JsonProperty("LASTPRZAC")]
        public string? LasTPrzAc { get; set; }
        [JsonProperty("PREZZOMED")]
        public string? PrezzoMed { get; set; }
        [JsonProperty("LASTPRZVE")]
        public string? LastPrzVe { get; set; }
        [JsonProperty("CodTre")]
        public string? CODETRE { get; set; }
        [JsonProperty("NUMMAG")]
        public string? NumMag { get; set; }
        [JsonProperty("NUMSCT")]
        public string? NumSct { get; set; }
        [JsonProperty("PREZZO4")]
        public string? Prezzo4 { get; set; }
        [JsonProperty("PREZZO5")]
        public string? Prezzo5 { get; set; }
        [JsonProperty("PREZZO6")]
        public string? Prezzo6 { get; set; }
        [JsonProperty("PRZBASE4")]
        public string? PrzBase4 { get; set; }
        [JsonProperty("PRZBASE5")]
        public string? PrzBase5 { get; set; }
        [JsonProperty("PRZBASE6")]
        public string? PrzBase6 { get; set; }
        [JsonProperty("CHECKIVA4")]
        public string? CheckIva4 { get; set; }
        [JsonProperty("CHECKIVA5")]
        public string? CheckIva5 { get; set; }
        [JsonProperty("CHECKIVA6")]
        public string? CheckIva6 { get; set; }
        [JsonProperty("RICARQUA")]
        public string? RicarQua { get; set; }
        [JsonProperty("RICARCIN")]
        public string? RicarCin { get; set; }
        [JsonProperty("RICARSEI")]
        public string? RicarSei { get; set; }
        [JsonProperty("FISSO1")]
        public string? Fisso1 { get; set; }
        [JsonProperty("FISSO2")]
        public string? Fisso2 { get; set; }
        [JsonProperty("FISSO3")]
        public string? Fisso3 { get; set; }
        [JsonProperty("FISSO4")]
        public string? Fisso4 { get; set; }
        [JsonProperty("FISSO5")]
        public string? Fisso5 { get; set; }
        [JsonProperty("FISSO6")]
        public string? Fisso6 { get; set; }
        [JsonProperty("ISSUPFX")]
        public string? IssuPfx { get; set; }
        [JsonProperty("ISROUND")]
        public string? IsRound { get; set; }
        [JsonProperty("CALCPRZ")]
        public string? CalcPrz { get; set; }
        [JsonProperty("VAROUND")]
        public string? VaRound { get; set; }
        [JsonProperty("IMPFIX1")]
        public string? ImpFix1 { get; set; }
        [JsonProperty("IMPFIX2")]
        public string? ImpFix2 { get; set; }
        [JsonProperty("IMPFIX3")]
        public string? ImpFix3 { get; set; }
        [JsonProperty("IMPFIX4")]
        public string? ImpFix4 { get; set; }
        [JsonProperty("IMPFIX5")]
        public string? ImpFix5 { get; set; }
        [JsonProperty("IMPFIX6")]
        public string? ImpFix6 { get; set; }
        [JsonProperty("MAGAZZINO")]
        public string? Magazzino { get; set; }
        [JsonProperty("SUBCATEG")]
        public string? SubCateg { get; set; }
        [JsonProperty("FLAG")]
        public string? Flag { get; set; }
        [JsonProperty("SCOAV2")]
        public string? ScoAv2 { get; set; }
        [JsonProperty("SCOAV3")]
        public string? ScoAv3 { get; set; }
        [JsonProperty("IVAACQ")]
        public string? IvaAcq { get; set; }
        [JsonProperty("OFFERTA")]
        public string? Offerta { get; set; }
        [JsonProperty("PRZSCO0")]
        public string? PrzSco0 { get; set; }
        [JsonProperty("PRZSCO1")]
        public string? PrzSco1 { get; set; }
        [JsonProperty("PRZSCO2")]
        public string? PrzSco2 { get; set; }
        [JsonProperty("PRZSCO3")]
        public string? PrzSco3 { get; set; }
        [JsonProperty("PRZSCO4")]
        public string? PrzSco4 { get; set; }
        [JsonProperty("PRZSCO5")]
        public string? PrzSco5 { get; set; }
        [JsonProperty("PRZSCO6")]
        public string? PrzSco6 { get; set; }
        [JsonProperty("MRG1")]
        public string? Mrg1 { get; set; }
        [JsonProperty("MRG2")]
        public string? Mrg2 { get; set; }
        [JsonProperty("MRG3")]
        public string? Mrg3 { get; set; }
        [JsonProperty("MRG4")]
        public string? Mrg4 { get; set; }
        [JsonProperty("MRG5")]
        public string? Mrg5 { get; set; }
        [JsonProperty("MRG6")]
        public string? Mrg6 { get; set; }
        [JsonProperty("ISRIC")]
        public string? Isric{ get; set; }
        [JsonProperty("CODIVAACQ")]
        public string? CodIvaAcq { get; set; }
        [JsonProperty("CODIVAVND")]
        public string? CodIvaVnd { get; set; }
        [JsonProperty("ISTOUCH")]
        public string? IsTouch { get; set; }
        [JsonProperty("TIPOLOGIA")]
        public string? Tipologia { get; set; }
        [JsonProperty("ECOMMERCE")]
        public string? ECommerce { get; set; }
        [JsonProperty("GRUPPO")]
        public string? Gruppo { get; set; }
        [JsonProperty("NUMGRU")]
        public string? NumGru { get; set; }
        [JsonProperty("PRODUTTORE")]
        public string? Produttore { get; set; }
        [JsonProperty("NUMPRO")]
        public string? NumPro { get; set; }
        [JsonProperty("GENERICO1")]
        public string? Generico1 { get; set; }
        [JsonProperty("GENERICO2")]
        public string? Generico2 { get; set; }
        [JsonProperty("GENERICO3")]
        public string? Generico3 { get; set; }
        [JsonProperty("GENERICO4")]
        public string? Generico4 { get; set; }
        [JsonProperty("TARA")]
        public string? Tara { get; set; }
        [JsonProperty("PRODUZIONE")]
        public string? Produzione { get; set; }
        [JsonProperty("COLORE")]
        public string? Colore { get; set; }
        [JsonProperty("TIPOTAGLIA")]
        public string? TipoTaglia { get; set; }
        [JsonProperty("STAGIONE")]
        public string? Stagione { get; set; }
        [JsonProperty("ANNO")]
        public string? Anno { get; set; }
        [JsonProperty("NUMFAT")]
        public string? NumFat { get; set; }
        [JsonProperty("MYDATA")]
        public string? MyData { get; set; }
        [JsonProperty("IDSHOP")]
        public string? IdShop { get; set; }
        [JsonProperty("ISCHANGED")]
        public string? IsChanged { get; set; }
        [JsonProperty("GIACENZA")]
        public string? Giacenza { get; set; }
        [JsonProperty("IDSHOP_E")]
        public string? IdShopE { get; set; }
        [JsonProperty("MONOPOLIO")]
        public string? Monopolio { get; set; }
        [JsonProperty("INUSO")]
        public string? InUso { get; set; }
        [OneToMany]
        public List<Colore>? Colori { get; set; } = [];
        [OneToMany]
        public List<ArticoloNote>? ArticoloNote { get; set; } = [];
    }
    public class ArticoloNote
    {
        [PrimaryKey, AutoIncrement]
        public int ElencoNoteId { get; set; }
        public int ArticoloId { get; set; }
        public string? ArticoloNota { get; set; }
    }
    public class ElencoArticoli
    {
        [JsonProperty("DATI")]
        public List<Articolo>? Articoli { get; set; }
    }
    public class Colore
    {
        [PrimaryKey, AutoIncrement]
        public int ColoreId { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string CodiceColore { get; set; } = string.Empty;
    }
    public class Cliente
    {
        [PrimaryKey, AutoIncrement]
        public int ClienteId { get; set; }
        [JsonProperty("CODICE")]
        public string? CodiceCliente { get; set; }
        [JsonProperty("NOME")]
        public string? Nome { get; set; }
        [JsonProperty("ADDRESS")]
        public string? Address { get; set; }
        [JsonProperty("CAP")]
        public string? Cap { get; set; }
        [JsonProperty("CITTA")]
        public string? Citta { get; set; }
        [JsonProperty("TELEFONO")]
        public string? Telefono { get; set; }
        [JsonProperty("FAX")]
        public string? Fax { get; set; }
        [JsonProperty("MOBILE")]
        public string? Mobile { get; set; }
        [JsonProperty("EMAIL")]
        public string? Email { get; set; }
        [JsonProperty("PIVACF")]
        public string? PIvaCf { get; set; }
        [JsonProperty("SPESEI")]
        public string? Spesei { get; set; }
        [JsonProperty("SPESET")]
        public string? Speset { get; set; }
        [JsonProperty("BOLLI")]
        public string? Bolli { get; set; }
        [JsonProperty("ZONA")]
        public string? Zona { get; set; }
        [JsonProperty("AGENTE")]
        public string? Agente { get; set; }
        [JsonProperty("CASUALE")]
        public string? Casuale { get; set; }
        [JsonProperty("PAGAMENTO")]
        public string? Pagamento { get; set; }
        [JsonProperty("BANCA")]
        public string? Banca { get; set; }
        [JsonProperty("DESTINAZIO")]
        public string? Destinazio { get; set; }
        [JsonProperty("ABI")]
        public string? Abi { get; set; }
        [JsonProperty("CAB")]
        public string? Cab { get; set; }
        [JsonProperty("DETING")]
        public string? Deting{ get; set; }
        [JsonProperty("DESTINAZI1")]
        public string? Destinazi1 { get; set; }
        [JsonProperty("SCONTO")]
        public string? Sconto { get; set; } = "0";
        [JsonProperty("CLIENT1")]
        public string? Client1 { get; set; }
        [JsonProperty("LABELS")]
        public string? Labels { get; set; }
        [JsonProperty("MOTESIVA")]
        public string? MotesIva { get; set; }
        [JsonProperty("ISESIVA")]
        public string? IsesIva { get; set; }
        [JsonProperty("NOTE")]
        public string? Note { get; set; }
        [JsonProperty("NUMAGE")]
        public string? NumAge { get; set; }
        [JsonProperty("LISTINO")]
        public string? Listino { get; set; }
        [JsonProperty("PROV")]
        public string? Prov { get; set; }
        [JsonProperty("DARE")]
        public string? Dare { get; set; }
        [JsonProperty("CODFISC")]
        public string? CodFisc { get; set; }
        [JsonProperty("ALIQUOTA")]
        public string? Aliquota { get; set; }
        [JsonProperty("BARCODE")]
        public string? Barcode { get; set; }
        [JsonProperty("DECPUNTI")]
        public string? DecPunti { get; set; }
        [JsonProperty("FLAG")]
        public string? Flag { get; set; }
        [JsonProperty("LISTPERS")]
        public string? ListPers { get; set; }
        [JsonProperty("IBAN")]
        public string? Iban { get; set; }
        [JsonProperty("WEB")]
        public string? Web { get; set; }
        [JsonProperty("FIDO")]
        public string? Fido { get; set; }
        [JsonProperty("SALDO")]
        public string? Saldo { get; set; }
        [JsonProperty("CODIVA")]
        public string? CodIva { get; set; }
        [JsonProperty("SETTORE")]
        public string? Settore { get; set; }
        [JsonProperty("AVVISO")]
        public string? Avviso { get; set; }
        [JsonProperty("GRUPPO")]
        public string? Gruppo { get; set; }
        [JsonProperty("SOLVENZA")]
        public string? Solvenza { get; set; }
        [JsonProperty("LOGIN")]
        public string? Login { get; set; }
        [JsonProperty("PASSWD")]
        public string? Passwd { get; set; }
        [JsonProperty("IDPAESE")]
        public string? IsPaese { get; set; }
        [JsonProperty("CODICEPA")]
        public string? CodicePa { get; set; }
        [JsonProperty("GENERICO")]
        public string? Generico { get; set; }
        [JsonProperty("CODICEFA")]
        public string? CodiceFa { get; set; }
        [JsonProperty("PEC")]
        public string? Pec { get; set; }
        [JsonProperty("ISPUBBLICO")]
        public string? IsPubblico { get; set; }
        [JsonProperty("NAZIONE")]
        public string? Nazione { get; set; }
        [JsonProperty("PRZDDT")]
        public string? PrzDdt { get; set; }
        [JsonProperty("MYDATA")]
        public string? MyData { get; set; }
        [JsonProperty("PECL")]
        public string? PecL { get; set; }
        [JsonProperty("RITPGIU")]
        public string? RitPGiu { get; set; }
        [JsonProperty("FECASPAG")]
        public string? FecasPag { get; set; }
        [JsonProperty("TPCASSA")]
        public string? TpCassa { get; set; }
        [JsonProperty("FETIPODATO")]
        public string? FeTipoDato { get; set; }
        [JsonProperty("FETIPOTEST")]
        public string? FeTipoTest { get; set; }
        [JsonProperty("CAUSALE")]
        public string? Causale { get; set; }
        [JsonProperty("FERIFENUME")]
        public string? FeRifeNume { get; set; }
        [JsonProperty("FERIFEDATA")]
        public string? FeRifeData { get; set; }
        public string? RagioneSociale
        {
            get
            {
                if (!string.IsNullOrEmpty(Client1)&&!string.IsNullOrEmpty(Nome))
                {
                    return $"{Nome} - {Client1}";
                }
                else if(!string.IsNullOrEmpty(Nome))
                {
                    return Nome;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        [OneToMany]
        public List<PrezziCliente> ListinoPersonale { get; set; } = [];
    }
    public class ElencoClienti
    {
        [JsonProperty("DATI")]
        public required List<Cliente> Clienti { get; set; }
    }
    public class Agente
    {
        [PrimaryKey, AutoIncrement]
        public int AgenteId { get; set; }
        public string? COMMENTO { get; set; }
        [JsonProperty("AGENTE")]
        public string? NomeAgente { get; set; }
        public string? ZONA1 { get; set; }
        public string? ZONA2 { get; set; }
        public string? ZONA3 { get; set; }
        public string? ZONA4 { get; set; }
        public string? ZONA5 { get; set; }
        public string? NUMREG { get; set; }
        [JsonProperty("PERCPROV")]
        public string? PercentualeProvvigione { get; set; }
        public string? NOTE { get; set; }
        public string? PASSWD { get; set; }
        public string? MYDATA { get; set; }
    }
    public class ElencoAgenti
    {
        [JsonProperty("DATI")]
        public List<Agente>? Agenti { get; set; }
    }
    public class Documento
    {
        [PrimaryKey, AutoIncrement]
        public int DocumentoId { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        [OneToOne]
        public Cliente Cliente { get; set; } = new();
        public string? NomeAgente { get; set; }
        [OneToMany]
        public List<MainPageInput> DettagliDocumento { get; set; } = [];
        public DateTime DataDocumento { get; set; }

        public bool IsSospeso { get; set; }
        public string? NoteDocumento { get; set; }
    }





    public class MainPageInput// e'il contenitore dei dati che l'utente inserisce nella main page .Ogni Entry che hai nella page corrisponde (va a riempire) ad una proprietà di questa classe
    {                               //quando l' utente digita o aggiorna qualcosa il binding aggiorna subito i valori delle proprietà

        [PrimaryKey, AutoIncrement]
        public int MainPageInputId { get; set; }
        [ForeignKey(typeof(Documento))]
        public int DocumentoId { get; set; }
        public string? RagioneSociale { get; set; }
        [OneToOne]
        public Articolo Articolo { get; set; } = new();
        [OneToOne]
        public string Colore { get; set; } = string.Empty;
        public double Prezzo { get; set; }
        public double PrezzoIvato { get; set; }
        public double TotaleImponibile { get; set; }
        public double TotaleIvato { get; set; }
        public double Sconto { get; set; }
        public double CostoMeccanica { get; set; }
        public double CostoTessuto { get; set; }
        public double Larghezza { get; set; }
        public double Altezza { get; set; }
        public double LarghezzaEffettiva { get; set; }
        public double AltezzaEffettiva { get; set; }
        public double Quantita { get; set; }
        public int Pezzi { get; set; } = 1;
        public bool LuceFinita { get; set; } 
        public bool PiuGuide { get; set; } = true;
        public bool AperturaCentrale { get; set; } = false;
        public string SpDx { get; set; } = string.Empty;
        public string NoteArtVarie { get; set; } = string.Empty;
        public string HcText { get; set; } = string.Empty;
        public double L2Number { get; set; }
        public double HNumber { get; set; }
        public double LNumber { get; set; }
        public double LuceHperEtich { get; set; }
        public double LuceLperEtich { get; set; }
        public double MixBandaFinita { get; set; }
        public double MixTaglioCassonetto { get; set; }
        public double MixTaglioTubo { get; set; }
        public string Hc { get; set; } = string.Empty;
        public string Rif { get; set; } = string.Empty;
        public string Varie1 { get; set; } = string.Empty;
        public string Versione { get; set; } = string.Empty;
        public string Comandi { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public string Supplemento1 { get; set; } = string.Empty;
        public int Supplemento1Pezzi { get; set; } = 1;
        public double Supplemento1Quantita { get; set; }
        public double Supplemento1Prezzo { get; set; }
        public double Supplemento1PrezzoTotale { get; set; }
        public string Supplemento1Colore { get; set; } = string.Empty;
        public string Supplemento2 { get; set; } = string.Empty;
        public int Supplemento2Pezzi { get; set; } = 1;
        public double Supplemento2Quantita { get; set; }
        public double Supplemento2Prezzo { get; set; }
        public double Supplemento2PrezzoTotale { get; set; }
        public string Supplemento2Colore { get; set; } = string.Empty;
        public string Attacchi { get; set; } = string.Empty;
        public bool Diviso2 { get; set; } = false;
        public bool Diviso2CU { get; set; } = false;
        public double LuceH { get; set; }
        public double LuceL { get; set; }
        [OneToOne]
        public IDrawable? GraphicsDrawable1 { get; set; }
        [OneToOne]
        public IDrawable? GraphicsDrawable2 { get; set; }
        [OneToOne]
        public IDrawable? GraphicsDrawable3 { get; set; }
    }
  
    
    
    
    
    
    
    public class ControlliInterfaccia
    {
        //Visibilità Controlli
        public bool Varie1Visible { get; set; } = false;
        public bool VersioneVisible { get; set; } = false;
        public bool LuceFinitaVisible { get; set; } = true;
        public bool NoteArtVarieVisible { get; set; } = false;
        public bool LVisible { get; set; } = true;
        public string LLabel { get; set; } = "L(cm)";
        public bool HVisible { get; set; } = true;
        public string HLabel { get; set; } = "H(cm)";
        public string HcLabel { get; set; } = "Hc";
        public bool L2Visible { get; set; } = false;
        public bool AperturaCentraleVisible { get; set; } = false;
        public bool SpDxVisible { get; set; } = false;
        public bool ComandiVisible { get; set; } = false;
        public bool GuideVisible { get; set; } = false;
        public bool HcVisible { get; set; } = false;
        public bool AttacchiVisible { get; set; } = false;

        //Valori Controlli
        public List<string> ListaVarie1 { get; set; } = [];
        public List<string> ListaVersione { get; set; } = [];
        public List<string> ListaComandi { get; set; } = [];
        public List<string> ListaNote { get; set; } = [];
        public List<string> ListaSupplemento1 { get; set; } = [];
        public List<string> ListaSupplemento1Colore { get; set; } = [];
        public List<string> ListaSupplemento2 { get; set; } = [];
        public List<string> ListaAttacchi { get; set; } = [];
        public Etichetta Etichetta { get; set; } = new();
        public string MessaggioErrore { get; set; } = string.Empty;
    }
  
    
    
    
    
    public class Etichetta
    {
     
        public string Alias { get; set; } = string.Empty;
        public string Colore { get; set; } = string.Empty;
        public double LuceLEtichetta { get; set; }
        public double LuceHEtichetta { get; set; }
        public double H { get; set; }
        public double L { get; set; }
        public string Comandi { get; set; } = string.Empty;
        public string Hc { get; set; } = string.Empty;
        public string Attacchi { get; set; } = string.Empty;
        public bool PiuGuide { get; set; }
        public bool AperturaCentrale { get; set; }
        public double CalcoloFloat { get; set; }
        public string Supplemento1 { get; set; } = string.Empty;
        public double[] CordiniVeneziane { get; set; } = [];
        public string Note { get; set; } = string.Empty;
        public string Rif { get; set; } = string.Empty;
        public double MixBandaFinita { get; set; }
        public double MixTaglioCassonetto { get; set; }
        public double MixTaglioTubo { get; set; }
    }
    public class ElencoPrezziCliente
    {
        [JsonProperty("DATI")]
        public List<PrezziCliente>? PrezziCliente { get; set; }
    }
    public class PrezziCliente
    {
        [JsonProperty("NUMCLI")]
        public string? CodiceCliente{ get; set; }

        [JsonProperty("CODICE")]
        public string? CodiceArticolo { get; set; }

        [JsonProperty("PREZZO")]
        public string? Prezzo { get; set; }


        [JsonProperty("SCONTO")]
        public string? Sconto { get; set; }
    }
    #endregion
}
