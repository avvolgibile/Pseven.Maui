using System;
using Microsoft.Maui.Graphics.Skia;
using Pseven.Models;
using Pseven.Services;
using SkiaSharp;
using ZXing;
using static System.Net.Mime.MediaTypeNames;
using Font = Microsoft.Maui.Graphics.Font;

namespace Pseven.Etichette
{
    public class EtichettaRulloMantovanaMotore(Etichetta etichetta) : EtichettaDrawBase(etichetta)
    {
        protected override void DrawSpecific(ICanvas canvas, RectF dirtyRect)
        {

            //}
            //public override void Draw(ICanvas canvas, RectF dirtyRect)
            //{
            canvas.Font = new Font("thaoma", 8);
            canvas.DrawString(etichetta.Alias, 5, 9, HorizontalAlignment.Left);

        }
    }
}



//if (VersioneCMBX.Text.Contains("Affiancato"))
//{
//    ComandiCMBX.Items.Clear();
//    ComandiCMBX.Items.Add("DX-SX"); ComandiCMBX.Items.Add("SX-DX"); ComandiCMBX.Items.Add("DX-DX"); ComandiCMBX.Items.Add("SX-SX");
//    if (ComandiCMBX.Text != "DX-SX" && ComandiCMBX.Text != "SX-DX" && ComandiCMBX.Text != "DX-DX" && ComandiCMBX.Text != "SX-SX") ComandiCMBX.Text = "DX-SX";
//    string[] comandiXaffiancate = ComandiCMBX.Text.Split('-');

//    GFX.Clear(Color.White);
//    GFX.DrawString(Alias.Substring(0, Alias.Length > 27 ? 27 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 3);
//    GFX.DrawString("Mantovana Mot", new Font("thaoma", 8), Brushes.Black, 210, 3);
//    GFX.DrawString("Affincato", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 220, 13);
//    GFX.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
//    GFX.DrawString("Col t " + Varie1TCMBX.Text, new Font("thaoma", 8), Brushes.Black, 87, 22);
//    GFX.DrawString("L " + luceLperEtich_Page1 + "   (Ltot" + luceLperEtich + ")", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
//    GFX.DrawString("H " + H_TXBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 115, 40);
//    GFX.DrawString("Cavo " + comandiXaffiancate[0], new Font("thaoma", 8), Brushes.Black, 165, 40);
//    GFX.DrawString("(" + mixTeloFinita + ")tess", new Font("thaoma", 8), Brushes.Black, 95, 58);
//    GFX.DrawString(Supplemento1CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 70);
//    GFX.DrawString(elementivari.tipoStaffa, new Font("thaoma", 7), Brushes.Black, 130, 71);
//    GFX.DrawString("sagomare punte dischetto", new Font("thaoma", 7), Brushes.Black, 200, 58);

//    GFX.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
//    GFX.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);
//    pictureBox1.Image = drawingsurface;

//    GFX2.Clear(Color.White);
//    GFX2.DrawString(Alias.Substring(0, Alias.Length > 27 ? 27 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 3);
//    GFX2.DrawString(VersioneCMBX.Text, new Font("thaoma", 8), Brushes.Black, 35, 15);
//    // GFX2.DrawString("Cavo " + ComandiCMBX.Text, new Font("thaoma", 8), Brushes.Black, 125 , 29);
//    GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioProfiloSuperiore.ToString("0000")), 8), 10, 30);
//    GFX2.DrawString(mixTaglioProfiloSuperiore.ToString(), new Font("thaoma", 7), Brushes.Black, 70, 37);
//    GFX2.DrawString("profilo", new Font("thaoma", 7), Brushes.Black, 14, 37);
//    GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioProfilo.ToString("0000")), 8), 10, 52);
//    GFX2.DrawString(mixTaglioProfilo.ToString() /*+ elementivari.lateraleCentrale*/, new Font("thaoma", 7), Brushes.Black, 70, 59);
//    GFX2.DrawString("laterale pz " + elementivari.numeroLaterali, new Font("thaoma", 7), Brushes.Black, 14, 59);
//    GFX2.DrawString("Spostare pressa troncatrice!", new Font("thaoma", 9, FontStyle.Bold), Brushes.Black, 5, 66);

//    GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioTubo.ToString("0000")), 10), PartenzaBarcodeDX, 30);
//    GFX2.DrawString(mixTaglioTubo.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 38);
//    GFX2.DrawString("tubo", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 37);
//    if (Tessuti.GetTessutoByName(CC[0]).Tessuto_NeD == 2) // se e' tessuto N&D
//    {
//        GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, (mixTaglioTubo - 5).ToString("0000")), 10), PartenzaBarcodeDX, 52);
//        GFX2.DrawString((mixTaglioTubo - 5).ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 60);
//        GFX2.DrawString("fond. N&D", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 60);
//    }
//    else
//    {
//        if (NoteCMBBX.Text.Contains("tasca")) // con fondale in tasca
//        {
//            GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, (mixTaglioTubo - 8).ToString("0000")), 10), PartenzaBarcodeDX, 58);
//            GFX2.DrawString((mixTaglioTubo - 8).ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 66);
//            GFX2.DrawString("fond.18", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 66);
//        }
//        else GFX2.DrawString("fondale rettangolare", new Font("thaoma", 7, FontStyle.Bold), Brushes.Black, PartenzaBarcodeDX + 14, 47);
//    }

//    GFX2.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
//    GFX2.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);
//    pictureBox2.Image = drawingsurface2;

//    ////////////////////////////////////////////////////////////////
//    GFX3.Clear(Color.White);
//    GFX3.DrawString("(" + AttacchiCBX.Text + ")" + "COM " + ComandiCMBX.Text + "-------------------------------------------------------------------------", new Font("thaoma", 8), Brushes.Black, 5, 3);

//    GFX3.DrawString(elementivari.staffaDX, new Font("thaoma", 7), Brushes.Black, 5, 13);
//    GFX3.DrawString(elementivari.staffaSX, new Font("thaoma", 7), Brushes.Black, 5, 22);
//    GFX3.DrawString(elementivari.spessori, new Font("thaoma", 7), Brushes.Black, 5, 32);
//    GFX3.DrawString(elementivari.copriviti, new Font("thaoma", 8), Brushes.Black, 5, 43);
//    GFX3.DrawString(elementivari.tappoMantovana, new Font("thaoma", 8), Brushes.Black, 100, 43);


//    GFX3.DrawString(elementivari.squadrette, new Font("thaoma", 8), Brushes.Black, 5, 54);
//    GFX3.DrawString(elementivari.attacchiSoffitto, new Font("thaoma", 8), Brushes.Black, 5, 65);
//    GFX3.DrawString("- Istruzioni motore + (brucola finecorsa?)", new Font("thaoma", 7), Brushes.Black, 5, 74);
//    GFX3.DrawString("Busta: " + (string.IsNullOrEmpty(L_TXBX.Text) ? 0 : int.Parse(L_TXBX.Text) + 300) + "mm", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 220, 83);

//    GFX3.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
//    pictureBox3.Image = drawingsurface3;

//    GFX4.Clear(Color.White);
//    GFX4.DrawString(Alias.Substring(0, Alias.Length > 27 ? 27 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 3);
//    GFX4.DrawString("Mantovana Mot", new Font("thaoma", 8), Brushes.Black, 210, 3);
//    GFX4.DrawString("Affincato", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 220, 13);
//    GFX4.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
//    GFX4.DrawString("Col t " + Varie1TCMBX.Text, new Font("thaoma", 8), Brushes.Black, 87, 22);
//    GFX4.DrawString("L " + luceLperEtich_Page2 + "   (Ltot" + luceLperEtich + ")", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
//    GFX4.DrawString("H " + H_TXBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 115, 40);
//    GFX4.DrawString("Cavo " + comandiXaffiancate[1], new Font("thaoma", 8), Brushes.Black, 165, 40);
//    GFX4.DrawString("(" + mixTeloFinita_Page2 + ")tess", new Font("thaoma", 8), Brushes.Black, 95, 58);
//    GFX4.DrawString("sagomare punte dischetto", new Font("thaoma", 7), Brushes.Black, 200, 58);

//    GFX4.DrawString(Supplemento1CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 70);
//    GFX4.DrawString(elementivari.tipoStaffa, new Font("thaoma", 7), Brushes.Black, 130, 71);

//    GFX4.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
//    GFX4.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);
//    pictureBox4.Image = drawingsurface4;

//    GFX5.Clear(Color.White);
//    GFX5.DrawString(Alias.Substring(0, Alias.Length > 27 ? 27 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 3);
//    GFX5.DrawString(VersioneCMBX.Text, new Font("thaoma", 8), Brushes.Black, 35, 15);
//    GFX5.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioTubo2.ToString("0000")), 10), PartenzaBarcodeDX, 30);
//    GFX5.DrawString(mixTaglioTubo2.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 38);
//    GFX5.DrawString("tubo", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 37);
//    if (Tessuti.GetTessutoByName(CC[0]).Tessuto_NeD == 2) // se e' tessuto N&D
//    {
//        GFX5.DrawImage(barcode.Draw(string.Format(barcodeStruct, (mixTaglioTubo2 - 5).ToString("0000")), 8), PartenzaBarcodeDX, 52);
//        GFX5.DrawString((mixTaglioTubo2 - 5).ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 60);
//        GFX5.DrawString("fond. N&D", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 60);
//    }
//    else
//    {
//        if (NoteCMBBX.Text.Contains("tasca")) // con fondale in tasca
//        {
//            GFX5.DrawImage(barcode.Draw(string.Format(barcodeStruct, (mixTaglioTubo2 - 8).ToString("0000")), 10), PartenzaBarcodeDX, 58);
//            GFX5.DrawString((mixTaglioTubo2 - 8).ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 66);
//            GFX5.DrawString("fond.18", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 66);
//        }
//        else GFX5.DrawString("fondale rettangolare", new Font("thaoma", 7, FontStyle.Bold), Brushes.Black, PartenzaBarcodeDX + 14, 47);
//    }

//    GFX5.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
//    GFX5.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);
//    pictureBox5.Image = drawingsurface5;
//}
//else
//{
//    ComandiCMBX.Items.Clear();
//    ComandiCMBX.Items.Add("DX"); ComandiCMBX.Items.Add("SX");
//    if (ComandiCMBX.Text != "DX" && ComandiCMBX.Text != "SX") ComandiCMBX.Text = "DX";

//    GFX.Clear(Color.White);
//    GFX.DrawString(Alias.Substring(0, Alias.Length > 27 ? 27 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 3);
//    GFX.DrawString("Mantovana Mot", new Font("thaoma", 8), Brushes.Black, 210, 3);
//    GFX.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
//    GFX.DrawString("Col t " + Varie1TCMBX.Text, new Font("thaoma", 8), Brushes.Black, 87, 22);
//    GFX.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
//    GFX.DrawString("H " + H_TXBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 115, 40);
//    GFX.DrawString("Cavo " + ComandiCMBX.Text, new Font("thaoma", 8), Brushes.Black, 165, 40);
//    GFX.DrawString("(" + mixTeloFinita + ")tess", new Font("thaoma", 8), Brushes.Black, 95, 58);
//    GFX.DrawString("sagomare punte dischetto", new Font("thaoma", 7), Brushes.Black, 200, 58);

//    GFX.DrawString(Supplemento1CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 70);
//    GFX.DrawString(elementivari.tipoStaffa, new Font("thaoma", 7), Brushes.Black, 130, 71);

//    GFX.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
//    GFX.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);
//    pictureBox1.Image = drawingsurface;

//    GFX2.Clear(Color.White);
//    GFX2.DrawString(Alias.Substring(0, Alias.Length > 27 ? 27 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 3);
//    GFX2.DrawString(VersioneCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 15);
//    // GFX2.DrawString("Cavo " + ComandiCMBX.Text, new Font("thaoma", 8), Brushes.Black, 200, 4);
//    GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioProfiloSuperiore.ToString("0000")), 8), 10, 30);
//    GFX2.DrawString(mixTaglioProfiloSuperiore.ToString(), new Font("thaoma", 7), Brushes.Black, 70, 37);
//    GFX2.DrawString("profilo", new Font("thaoma", 7), Brushes.Black, 14, 37);
//    GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioProfilo.ToString("0000")), 8), 10, 52);
//    GFX2.DrawString(mixTaglioProfilo.ToString(), new Font("thaoma", 7), Brushes.Black, 70, 59);
//    GFX2.DrawString("laterale pz " + elementivari.numeroLaterali, new Font("thaoma", 7), Brushes.Black, 14, 59);
//    GFX2.DrawString("Spostare pressa troncatrice!", new Font("thaoma", 9, FontStyle.Bold), Brushes.Black, 5, 66);
//    GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioTubo.ToString("0000")), 10), PartenzaBarcodeDX, 30);
//    GFX2.DrawString(mixTaglioTubo.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 38);
//    GFX2.DrawString("tubo", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 37);

//    if (Tessuti.GetTessutoByName(CC[0]).Tessuto_NeD == 2) // se e' tessuto N&D
//    {
//        GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, (mixTaglioTubo - 5).ToString("0000")), 10), PartenzaBarcodeDX, 52);
//        GFX2.DrawString((mixTaglioTubo - 5).ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 60);
//        GFX2.DrawString("fond. N&D", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 60);
//    }
//    else
//    {
//        if (NoteCMBBX.Text.Contains("tasca")) // con fondale in tasca
//        {
//            GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, (mixTaglioTubo - 8).ToString("0000")), 10), PartenzaBarcodeDX, 58);
//            GFX2.DrawString((mixTaglioTubo - 8).ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 66);
//            GFX2.DrawString("fond.18", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 66);
//        }
//        else GFX2.DrawString("fondale rettangolare", new Font("thaoma", 7, FontStyle.Bold), Brushes.Black, PartenzaBarcodeDX + 14, 47);
//    }

//    GFX2.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
//    GFX2.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);
//    pictureBox2.Image = drawingsurface2;

//    GFX3.Clear(Color.White);

//    GFX3.DrawString("(" + AttacchiCBX.Text + ")" + "COM " + ComandiCMBX.Text + "-------------------------------------------------------------------------", new Font("thaoma", 8), Brushes.Black, 5, 3);
//    GFX3.DrawString(elementivari.staffaDX, new Font("thaoma", 7), Brushes.Black, 5, 13);
//    GFX3.DrawString(elementivari.staffaSX, new Font("thaoma", 7), Brushes.Black, 5, 22);

//    GFX3.DrawString(elementivari.copriviti, new Font("thaoma", 7), Brushes.Black, 5, 43);
//    GFX3.DrawString(elementivari.tappoMantovana, new Font("thaoma", 8), Brushes.Black, 100, 43);
//    GFX3.DrawString(elementivari.spessori, new Font("thaoma", 8), Brushes.Black, 5, 32);

//    GFX3.DrawString(elementivari.squadrette, new Font("thaoma", 8), Brushes.Black, 5, 54);
//    GFX3.DrawString(elementivari.attacchiSoffitto, new Font("thaoma", 8), Brushes.Black, 5, 65);
//    GFX3.DrawString("- Istruzioni motore + (brucola finecorsa?)", new Font("thaoma", 7), Brushes.Black, 5, 74);
//    GFX3.DrawString("Busta: " + (string.IsNullOrEmpty(L_TXBX.Text) ? 0 : int.Parse(L_TXBX.Text) + 300) + "mm", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 220, 83);

//    GFX3.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);

//    pictureBox3.Image = drawingsurface3;

//    pictureBox4.Image = null;
//    pictureBox5.Image = null;
//}


