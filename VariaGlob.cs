using System.Reflection;

namespace Pseven
{
    public static class Variaglob
    {
        //88888888888888888888888888888888888888888888888888888888888888888888
        public static string CittaCondiviso;
        public static string TelefonoCondiviso;
        public static string CellCondiviso;
        public static string IndirizzoCondiviso;
        public static string ProvCondiviso;
        public static string CAPCondiviso;
        public static string EmailCondiviso;
        public static string ZonaCondiviso;
        public static string AgenteCondiviso;
        public static float ProvvPerc = 0;
        public static string Trasporto = " °°°°°Trasp.°°°°°";//spazio all' inizio per averlo sempre come prima voce
        public static string Supplemento = " (Suppl.)";

        public static int CifreDecimali_Dettaglio;//si intende:prezzo fornitore/acquisto, prezzo vendita e importo tot riga
        public static int CifreDecimali_Totali;//tot documento
        public static int CifreDecimali_Quantita;//
        //88888888888888888888888888888888888888888888888888888888888888888888
        public static string AvvisoCondiviso;
        public static string Mail_mia_Condiviso;
        public static string SMTP_Condiviso;
        public static string Passwrd_Email_mia_Condiviso;
        public static string Email_Fornitori;
        //public static string Datatable_xmlCondiviso = Properties.Settings.Default.CartellaP_sun;//+ "\\P_sun\\

        public static bool Chiedi_VuoiChiuderePrev = false;
        // public static bool      apriArticoloClienteInesistenteForm = true;//quando da reinserisci o ristampa etichetta non si deve aprire il form articolo inesistente
        public static List<int> ListSpostArticoli = new List<int>();

        //public static Form1 _Form1;
        //public static DocumentoForm _DocumentoForm;
        //public static ElencoDocumentiApertiForm _ElencoDocumentiApertiForm;
        //public static Storico_Documenti _Storico_Documenti;
        //public static MiniForm _MiniForm;
    }

    public class ProprietàTessuto
    {
        /// <summary>
        ///     arrotolamento da fare
        /// </summary>
        public int Arrotolamento_su_tubo { get; set; }
        public byte Tessuto_NeD { get; set; }

        public int Peso_tessuto { get; set; }
        public float Spessore_Tessuto { get; set; }

        public ProprietàTessuto(byte Arrotolamento_su_tubo, byte Tessuto_NeD, int Peso_tessuto, float Spessore_Tessuto)
        {
            this.Arrotolamento_su_tubo = Arrotolamento_su_tubo;
            this.Tessuto_NeD = Tessuto_NeD;
            this.Peso_tessuto = Peso_tessuto;
            this.Spessore_Tessuto = Spessore_Tessuto;
        }
    }

    public class Tessuti
    {                          // Arrotolamento_su_tubo,  Tessuto_NeD, Peso_tessuto,  Spessore_Tessuto
        public static ProprietàTessuto Star1 = new ProprietàTessuto(20, 1, 215, 0.35f);
        public static ProprietàTessuto Star2 = new ProprietàTessuto(20, 1, 180, 0.3f);
        public static ProprietàTessuto Luna = new ProprietàTessuto(20, 1, 210, 0.45f);
        public static ProprietàTessuto Lima = new ProprietàTessuto(20, 1, 200, 0.44f);
        public static ProprietàTessuto Ombra = new ProprietàTessuto(30, 1, 420, 0.55f);
        public static ProprietàTessuto Aspen = new ProprietàTessuto(20, 1, 165, 0.22f);
        public static ProprietàTessuto Cortina = new ProprietàTessuto(20, 1, 420, 0.3f);
        public static ProprietàTessuto Eclissi = new ProprietàTessuto(20, 1, 407, 0.3f);
        public static ProprietàTessuto LunaBLO = new ProprietàTessuto(20, 1, 360, 0.55f);
        public static ProprietàTessuto Star1BLO = new ProprietàTessuto(20, 1, 330, 0.44f);
        public static ProprietàTessuto Star2BLO = new ProprietàTessuto(20, 1, 320, 0.44f);
        public static ProprietàTessuto ApolloBLO = new ProprietàTessuto(20, 1, 430, 0.55f);
        public static ProprietàTessuto Linen = new ProprietàTessuto(20, 1, 360, 0.3f);
        public static ProprietàTessuto Print = new ProprietàTessuto(20, 1, 160, 0.36f);//questi tessuti variano da 70gr a 175,,da 0,22 a 0,36
        public static ProprietàTessuto Porto = new ProprietàTessuto(20, 1, 145, 0.41f);
        public static ProprietàTessuto Opera = new ProprietàTessuto(40, 1, 155, 0.36f);//tessuto troppo trasparenti, si vede il rullo
        public static ProprietàTessuto Vanity = new ProprietàTessuto(40, 1, 82, 0.15f);
        public static ProprietàTessuto TexNet = new ProprietàTessuto(40, 1, 420, 0.6f);
        public static ProprietàTessuto Colorama = new ProprietàTessuto(20, 1, 265, 0.43f);
        public static ProprietàTessuto Office = new ProprietàTessuto(40, 1, 160, 0.35f);
        public static ProprietàTessuto Melange = new ProprietàTessuto(20, 1, 220, 0.50f);
        public static ProprietàTessuto Mineral = new ProprietàTessuto(40, 1, 92, 0.15f);
        public static ProprietàTessuto Seta = new ProprietàTessuto(20, 1, 211, 0.43f);
        public static ProprietàTessuto Greed = new ProprietàTessuto(20, 1, 176, 0.37f);
        public static ProprietàTessuto Shade = new ProprietàTessuto(20, 1, 160, 0.35f);
        public static ProprietàTessuto Decor = new ProprietàTessuto(40, 1, 100, 0.36f); //tessuto troppo trasparenti, si vede il rullo////questi tessuti variano da 80gr a 105,,da 0,28 a 0,40
        public static ProprietàTessuto Soul = new ProprietàTessuto(40, 1, 100, 0.40f);//Dati da verificare------tessuto troppo trasparenti, si vede il rullo
        public static ProprietàTessuto ShadeBLO = new ProprietàTessuto(20, 1, 350, 0.45f);
        public static ProprietàTessuto OperaDecor = new ProprietàTessuto(40, 1, 155, 0.53f);// tessuto troppo trasparenti, si vede il rullo

        public static ProprietàTessuto Soltis88 = new ProprietàTessuto(20, 1, 360, 0.45f);
        public static ProprietàTessuto Soltis92 = new ProprietàTessuto(20, 1, 420, 0.45f);
        public static ProprietàTessuto Soltis96 = new ProprietàTessuto(20, 1, 400, 0.45f);
        public static ProprietàTessuto SergeGS = new ProprietàTessuto(Arrotolamento_su_tubo: 20, 1, 615, 0.87f);
        public static ProprietàTessuto Opatex = new ProprietàTessuto(20, 1, 850, 0.68f);
        public static ProprietàTessuto Prec622BLO = new ProprietàTessuto(20, 1, 750, 0.65f);
        public static ProprietàTessuto Prec302 = new ProprietàTessuto(20, 1, 490, 0.36f);
        public static ProprietàTessuto Ultrascreen = new ProprietàTessuto(20, 1, 200, 0.53f);

        public static ProprietàTessuto Dixie = new ProprietàTessuto(20, 2, 100, 0.35f);
        public static ProprietàTessuto Rave = new ProprietàTessuto(20, 2, 110, 0.41f);
        public static ProprietàTessuto PortoDuo = new ProprietàTessuto(20, 2, 100, 0.40f);
        public static ProprietàTessuto RigaDuo = new ProprietàTessuto(20, 2, 100, 0.44f);
        public static ProprietàTessuto Boston = new ProprietàTessuto(20, 2, 80, 0.33f);
        public static ProprietàTessuto MagicL = new ProprietàTessuto(20, 2, 110, 0.33f);
        public static ProprietàTessuto MagicW = new ProprietàTessuto(20, 2, 115, 0.55f);

        public static ProprietàTessuto Luce = new ProprietàTessuto(20, 1, 262, 0.66f);
        public static ProprietàTessuto Solo_meccanica = new ProprietàTessuto(0, 1, 40, 40);
        public static ProprietàTessuto Nullo = new ProprietàTessuto(0, 0, 0, 0);

        public static string[] GetListaTessuti()
        {
            List<string> listaTessuti = [];
            Type myType = typeof(Tessuti);
            FieldInfo[] myField = myType.GetFields();
            foreach (FieldInfo m in myField)
            {
                listaTessuti.Add(m.Name);
            }
            return [.. listaTessuti];
        }
        public static ProprietàTessuto GetTessutoByName(string name)
        {
            if (name == "")
            {
                name = "Nullo";
            }
            Type myType = typeof(Tessuti);
            ProprietàTessuto o;
            try
            {
                o = (ProprietàTessuto)myType.GetField(name).GetValue(null);
            }
            catch //(NullReferenceException)
            {
                o = Nullo;
            }
            return o;
        }
    }
}
