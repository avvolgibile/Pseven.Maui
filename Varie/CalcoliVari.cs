using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pseven.Varie
{
    public class CalcoliVari
    {
        static public string Nylon35_50(string H_TXBX, bool PiuGuideRDBT)
        {
            if (H_TXBX != "" && H_TXBX != "0" && PiuGuideRDBT)
            {
                return double.Parse(H_TXBX) switch
                {
                    > 340 => "?",
                    > 320 => "Q",
                    > 300 => "P",
                    > 280 => "O",
                    > 260 => "N",
                    > 240 => "M",
                    > 220 => "L",
                    > 200 => "I",
                    > 180 => "H",
                    > 160 => "G",
                    > 140 => "F",
                    > 120 => "E",
                    > 100 => "D",
                    > 80 => "C",
                    > 60 => "B",
                    > 40 => "A",
                    < 40 => "?",
                    _ => ""
                };
            }
            else return "";
        }
        public static double Get_MixAstina(double _H_TXBX)
        {
            return _H_TXBX switch
            {
                double n when n <= 24 => n,
                double n when n <= 45 => 25,
                double n when n <= 75 => 50,
                double n when n <= 105 => 75,
                double n when n <= 130 => 100,
                double n when n <= 155 => 125,
                double n when n <= 180 => 150,
                double n when n <= 205 => 175,
                double n when n <= 230 => 200,
                double n when n <= 255 => 225,
                double n when n <= 280 => 250,
                double n when n <= 305 => 275,
                double n when n <= 330 => 300,
                _ => _H_TXBX
            };
        }

        static public void CalcoloTiraggioOrientatore_15_25(ref double[] CordiniVeneziane, string HcTXBX, double H_TXBX, double L_TXBX, byte nCordeCentrali = 0)
        {

            switch (HcTXBX)
            {
                ////888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                case string a when a.Contains('+'):

                    HcTXBX = double.Parse(Regex.Match(HcTXBX, @"[-+]?\d+").Value).ToString();

                    CordiniVeneziane[0] = Math.Round(((H_TXBX * 4) + L_TXBX + (double.Parse(HcTXBX.Replace(".", ",")) * 2)) / 100, 1);
                    CordiniVeneziane[2] = H_TXBX + double.Parse(HcTXBX);
                    break;
                ////888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                case string a when a.ToUpper().Contains("AST"):

                    HcTXBX = double.Parse(Regex.Match(HcTXBX, @"[-+]?\d+").Value).ToString();

                    CordiniVeneziane[0] = Math.Round(((H_TXBX * 4) + L_TXBX + 60) / 100, 1);
                    CordiniVeneziane[2] = double.Parse(HcTXBX);
                    break;
                ////888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                case string a when a.Equals("f", StringComparison.OrdinalIgnoreCase):

                    CordiniVeneziane[0] = Math.Round(((H_TXBX * 4) + L_TXBX + 60) / 100, 1);
                    CordiniVeneziane[2] = Get_MixAstina(H_TXBX);
                    break;
                ////888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                case string a when a.Equals("p", StringComparison.OrdinalIgnoreCase):

                    CordiniVeneziane[0] = Math.Round(((H_TXBX * 4) + L_TXBX + 60) / 100, 1);
                    CordiniVeneziane[2] = Get_MixAstina(H_TXBX > 100 ? H_TXBX - 80 : H_TXBX);
                    break;
                ////888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                case string a when a.Any(c => char.IsDigit(c)) && !a.Contains('+')://solo numeri

                    CordiniVeneziane[0] = Math.Round(((H_TXBX * 2) + L_TXBX + (double.Parse(HcTXBX.Replace(".", ",")) * 2)) / 100, 1);
                    CordiniVeneziane[2] = double.Parse(HcTXBX);
                    break;
                case "":
                    ////888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                    CordiniVeneziane[0] = Math.Round(((H_TXBX * 4) + L_TXBX + 60) / 100, 1);
                    CordiniVeneziane[2] = Get_MixAstina(H_TXBX > 240 ? H_TXBX - 80 : H_TXBX);
                    break;
                ////888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                default:
                    //MessageBox.Show("Stringa in Hc non riconosciuta!", "P_Sun", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //Variaglob._Form1.HcTXBX.Text = "";
                    //Variaglob._Form1.HcTXBX.Focus();
                    break;
            }
            if (L_TXBX == 0) CordiniVeneziane[0] = Math.Round(((H_TXBX * 2) + 35) / 100, 1); //// se e' piccola senza tiraggio


            if (nCordeCentrali == 0) CordiniVeneziane[1] = 0;
            else if (nCordeCentrali == 1) CordiniVeneziane[1] = CordiniVeneziane[0] / 2;
            else CordiniVeneziane[1] = CordiniVeneziane[1] = CordiniVeneziane[0];
        }
        static public void CalcoloTiraggioOrientatore_35_50(ref double[] CordiniVeneziane, string HcTXBX, double H_TXBX, double L_TXBX, int AvvolgPiuNodi_Orient, byte nCordeCentrali = 0)
        {
            if (H_TXBX < 50) AvvolgPiuNodi_Orient = AvvolgPiuNodi_Orient + 20;
            switch (HcTXBX)
            {
                ////888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                case string a when a.Contains("+"):

                    HcTXBX = double.Parse(Regex.Match(HcTXBX, @"[-+]?\d+").Value).ToString();

                    CordiniVeneziane[0] = Math.Round(((H_TXBX * 4) + L_TXBX + (double.Parse(HcTXBX.Replace(".", ",")) * 2)) / 100, 1);
                    CordiniVeneziane[2] = Math.Round(((H_TXBX * 2) + (double.Parse(HcTXBX.Replace(".", ",")) * 2) + AvvolgPiuNodi_Orient) / 100, 1);
                    break;
                ////888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                case string a when a.Equals("pf", StringComparison.OrdinalIgnoreCase) || a.Equals("fp", StringComparison.OrdinalIgnoreCase):

                    CordiniVeneziane[0] = Math.Round(((H_TXBX * 4) + L_TXBX + 45) / 100, 1);
                    CordiniVeneziane[2] = Math.Round(((H_TXBX * 2) - 200 + AvvolgPiuNodi_Orient) / 100, 1);
                    break;
                //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                case string a when a.Equals("f", StringComparison.OrdinalIgnoreCase):

                    CordiniVeneziane[0] = Math.Round(((H_TXBX * 4) + L_TXBX + 45) / 100, 1);
                    CordiniVeneziane[2] = Math.Round(((H_TXBX * 2) + AvvolgPiuNodi_Orient) / 100, 1);
                    break;
                //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                case string a when a.Equals("p", StringComparison.OrdinalIgnoreCase):

                    CordiniVeneziane[0] =   Math.Round(((H_TXBX * 4) + L_TXBX - 200) / 100, 1);
                    CordiniVeneziane[2] = Math.Round(((H_TXBX * 2) - 200 + AvvolgPiuNodi_Orient) / 100, 1);
                    break;
                //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                case string a when a.Any(c => char.IsDigit(c)) && !a.Contains("+")://solo numero

                    CordiniVeneziane[0] = Math.Round(((H_TXBX * 2) + L_TXBX + (float.Parse(HcTXBX.Replace(".", ",")) * 2)) / 100, 1);
                    CordiniVeneziane[2] = Math.Round(((float.Parse(HcTXBX.Replace(".", ",")) * 2) + AvvolgPiuNodi_Orient) / 100, 1);
                    break;
                //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                case "":
                    if (H_TXBX <= 220)//finestra
                    {
                        CordiniVeneziane[0] = Math.Round(((H_TXBX * 4) + L_TXBX + 45) / 100, 1);
                        CordiniVeneziane[2] = Math.Round(((H_TXBX * 2) + AvvolgPiuNodi_Orient) / 100, 1);
                    }
                    else
                    {
                        CordiniVeneziane[0] = Math.Round(((H_TXBX * 4) + L_TXBX - 200) / 100, 1);
                        CordiniVeneziane[2] = Math.Round(((H_TXBX * 2) - 200 + AvvolgPiuNodi_Orient) / 100, 1);
                    }
                    break;
                //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                default:
                    //MessageBox.Show("Stringa in Hc non riconosciuta!", "P_Sun", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //Variaglob._Form1.HcTXBX.Text = "";
                    //Variaglob._Form1.HcTXBX.Focus();
                    break;
                    //888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            }
            if (nCordeCentrali == 0) CordiniVeneziane[1] = 0;
            else CordiniVeneziane[1] = CordiniVeneziane[0] / 2;
        }
        static public string N_clipVerticali_pannelli(double L_TXBX)
        {
            if (L_TXBX > 0)
            {

                //if (float.Parse(L_TXBX.Replace(".", ",")) / 55 <= 2) return "2";
                //else if(float.Parse(L_TXBX.Replace(".", ",")) <= 120 && float.Parse(L_TXBX.Replace(".", ",")) <= 180) return "3";
                //return Math.Round(float.Parse(L_TXBX.Replace(".", ",")) / 70).ToString();
                if (L_TXBX <= 110) return "2";
                else if (L_TXBX <= 180) return "3";
                else return Math.Round(L_TXBX / 70) <= 3 ? "4" : Math.Round(L_TXBX / 70).ToString();

            }
            else return "0";
        }
        static public string N_bande(double L_TXBX, bool AperturaCentraleCKBX) //TODO: Modificato
        {
            if (L_TXBX > 0 && L_TXBX <= 600)
            {
                if (AperturaCentraleCKBX)
                {
                    double[] tabella = [26.5f, 48, 71.5f, 94, 118.5f, 140, 162, 185, 209, 231, 254, 276.5f, 300, 324, 346, 368, 391, 414, 436.5f, 460, 482, 505.5f, 527.5f, 550.5f, 573.5f, 594.5f, 600];
                    int i = 0;
                    while (L_TXBX > tabella[i]) i++;
                    return "*" + (i + 1).ToString() + "**" + (i + 1).ToString() + " Apert. Centrale";
                }
                else
                {
                    double[] tabella = [ 26.5f, 36.5f, 48, 59.5f, 71.5f, 83, 94 , 107,117.5f,128.5f,140,151,162,175,185,197,209,220.5f,231,243,254,265,276.5f,
                    288,300,311,324,334,348,359,370,381.5f,391,403, 414,425,436.5f,448,460,475,482,494,505.5f,516,527.5f,539, 550.5f,562,573.5f,584.5f,594.5f,600];
                    int i = 0;
                    while (L_TXBX > tabella[i]) i++;
                    return "*" + (i + 2).ToString() + "*";
                }
            }
            else return "* *  ";
        }
        static public string CalcoloCordaVerticale_Pannello(string HcTXBX, double L_TXBX)
        {
            if (HcTXBX != "" && HcTXBX != "0" && L_TXBX > 0 && float.TryParse(HcTXBX, out _))
            {
                return Math.Round(((float.Parse(HcTXBX.Replace(".", ",")) * 2) + ((L_TXBX * 2) - 8)) / 100, 2).ToString("0.00");
            }
            else return "???";
        }
        static public string CalcoloCordiniZanzarierePlisse0(double L_, double H_)
        {
            int N_Cordini;
            if (H_ <= 50.1f) N_Cordini = 2;
            else if (H_ <= 106.6f) N_Cordini = 3;
            else if (H_ <= 145.1f) N_Cordini = 4;
            else if (H_ <= 190.1f) N_Cordini = 5;
            else if (H_ <= 235.1f) N_Cordini = 6;
            else if (H_ <= 279) N_Cordini = 7;
            else N_Cordini = 8;
            double Distanza_tra_fori = Math.Round((((H_ - 3.2f) * 10) - 80) / (N_Cordini - 1));
            //00000000000000000000000000000000000000000000000000000000000000000000000000000000
            string Cordini2_3, Cordini3_4, Cordini5_6, Cordini7_8;

            Cordini2_3 = Math.Round((L_ * 10) + Math.Round((H_ - 3.2f) * 10) + 100, 1).ToString();
            Cordini2_3 = Math.Ceiling(double.Parse(Cordini2_3) / 10).ToString();
            Cordini2_3 = Math.Ceiling(double.Parse(Cordini2_3) / 10).ToString();
            Cordini2_3 = double.Parse(Cordini2_3) * 100 + "  2 Pz";

            if (N_Cordini == 3)
            {
                Cordini3_4 = Math.Round((L_ * 10) + Distanza_tra_fori + 350, 1).ToString();
                Cordini3_4 = Math.Ceiling(double.Parse(Cordini3_4) / 10).ToString();
                Cordini3_4 = Math.Ceiling(double.Parse(Cordini3_4) / 10).ToString();
                Cordini3_4 = double.Parse(Cordini3_4) * 100 + "  1 Pz";
            }
            else if (N_Cordini > 3)
            {
                Cordini3_4 = Math.Round((L_ * 10) + Distanza_tra_fori + 350, 1).ToString();
                Cordini3_4 = Math.Ceiling(double.Parse(Cordini3_4) / 10).ToString();
                Cordini3_4 = Math.Ceiling(double.Parse(Cordini3_4) / 10).ToString();
                Cordini3_4 = double.Parse(Cordini3_4) * 100 + "  2 Pz";
            }
            else Cordini3_4 = "";

            if (N_Cordini == 5)
            {
                Cordini5_6 = Math.Round((L_ * 10) + (Distanza_tra_fori * 2) + 350, 1).ToString();
                Cordini5_6 = Math.Ceiling(double.Parse(Cordini5_6) / 10).ToString();
                Cordini5_6 = Math.Ceiling(double.Parse(Cordini5_6) / 10).ToString();
                Cordini5_6 = double.Parse(Cordini5_6) * 100 + "  1 Pz";
            }
            else if (N_Cordini > 5)
            {
                Cordini5_6 = Math.Round((L_ * 10) + (Distanza_tra_fori * 2) + 350, 1).ToString();
                Cordini5_6 = Math.Ceiling(double.Parse(Cordini5_6) / 10).ToString();
                Cordini5_6 = Math.Ceiling(double.Parse(Cordini5_6) / 10).ToString();
                Cordini5_6 = double.Parse(Cordini5_6) * 100 + "  2 Pz";
            }
            else Cordini5_6 = "";

            if (N_Cordini == 7)
            {
                Cordini7_8 = Math.Round((L_ * 10) + (Distanza_tra_fori * 3) + 350, 1).ToString();
                Cordini7_8 = Math.Ceiling(double.Parse(Cordini7_8) / 10).ToString();
                Cordini7_8 = Math.Ceiling(double.Parse(Cordini7_8) / 10).ToString();
                Cordini7_8 = double.Parse(Cordini7_8) * 100 + "  1 Pz";
            }
            else if (N_Cordini > 7)
            {
                Cordini7_8 = Math.Round((L_ * 10) + (Distanza_tra_fori * 3) + 350, 1).ToString();
                Cordini7_8 = Math.Ceiling(double.Parse(Cordini7_8) / 10).ToString();
                Cordini7_8 = Math.Ceiling(double.Parse(Cordini7_8) / 10).ToString();
                Cordini7_8 = double.Parse(Cordini7_8) * 100 + "  2 Pz";
            }
            else Cordini7_8 = "";

            return Cordini2_3 + "\n" + Cordini3_4 + "\n" + Cordini5_6 + "\n" + Cordini7_8;

        }
    }
}
