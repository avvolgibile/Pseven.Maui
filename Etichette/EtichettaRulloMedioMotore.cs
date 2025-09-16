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
    public class EtichettaRulloMedioMotore(Etichetta etichetta) : EtichettaDrawBase(etichetta)
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


//if (VersioneCMBX.Text == "" || VersioneCMBX.Text == "Doppio")
//{
//    GFX.Clear(Color.White);
//    GFX.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 0);
//    GFX.DrawString("R. medio motore " + Doppiorullo, new Font("thaoma", 8), Brushes.Black, 200, 0);
//    GFX.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
//    GFX.DrawString("Col t " + Varie1TCMBX.Text, new Font("thaoma", 8), Brushes.Black, 87, 22);
//    GFX.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
//    GFX.DrawString("H " + H_TXBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 65, 40);
//    GFX.DrawString("Cavo " + ComandiCMBX.Text, new Font("thaoma", 8), Brushes.Black, 125, 40);
//    GFX.DrawString("(" + mixTeloFinita + ") mix tess", new Font("thaoma", 8), Brushes.Black, 95, 58);
//    GFX.DrawString(Supplemento1CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 70);
//    GFX.DrawString(Supplemento2CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 130, 70);

//    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioTubo.ToString("0000")), 10), PartenzaBarcodeDX, 24);
//    GFX.DrawString(mixTaglioTubo.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 33);
//    GFX.DrawString("Tubo", new Font("thaoma", 6), Brushes.Black, PartenzaBarcodeDX + 14, 34);
//    if (NoteCMBBX.Text.Contains("tasca")) // con fondale in tasca
//    {
//        if (Supplemento2CMBX.Text.Contains("Guida")) { MessageBox.Show("Non si puo' avere fond. in tasca quando è impostato con Guida"); NoteCMBBX.Text = ""; }
//        GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, (mixTaglioTubo - 8).ToString("0000")), 10), PartenzaBarcodeDX, 50);
//        GFX.DrawString((mixTaglioTubo - 8).ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 58);
//        GFX.DrawString("fond.18", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 58);
//    }
//    else GFX.DrawString("Fondale", new Font("thaoma", 6), Brushes.Black, PartenzaBarcodeDX + 14, 41);

//    GFX.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
//    GFX.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);

//    pictureBox1.Image = drawingsurface;

//    GFX2.Clear(Color.White);
//    GFX2.DrawString(Alias.Substring(0, Alias.Length > 27 ? 27 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 2);
//    GFX2.DrawString("Package: ", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 238, 2);
//    GFX2.DrawString("-------------------------------------------------------------------------------", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 10);
//    GFX2.DrawString("- Busta staffe/coprviti e cover", new Font("thaoma", 7), Brushes.Black, 5, 22);
//    GFX2.DrawString("- Istruzioni motore + (brucola finecorsa?)", new Font("thaoma", 7), Brushes.Black, 5, 34);
//    GFX2.DrawString(accessoriCavettate[0], new Font("thaoma", 7), Brushes.Black, 5, 46);
//    GFX2.DrawString(accessoriCavettate[1], new Font("thaoma", 7), Brushes.Black, 5, 58);
//    GFX2.DrawString(accessoriCavettate[2], new Font("thaoma", 7), Brushes.Black, 5, 70);
//    GFX2.DrawString("Busta: " + (string.IsNullOrEmpty(L_TXBX.Text) ? 0 : int.Parse(L_TXBX.Text) + 300) + "mm", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 220, 83);

//    GFX2.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);

//    pictureBox2.Image = drawingsurface2;

//    pictureBox3.Image = null;
//}
//else
//{
//    GFX.Clear(Color.White);
//    GFX.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 0);
//    GFX.DrawString("R. medio motore " + Doppiorullo, new Font("thaoma", 8), Brushes.Black, 200, 0);
//    GFX.DrawString("Affincato", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 220, 12);
//    GFX.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
//    GFX.DrawString("Col t " + Varie1TCMBX.Text, new Font("thaoma", 8), Brushes.Black, 87, 22);
//    GFX.DrawString("L " + luceLperEtich_Page1 + "   (Ltot" + luceLperEtich + ")", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
//    GFX.DrawString("H " + H_TXBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 120, 40);
//    GFX.DrawString("Cavo DX", new Font("thaoma", 8), Brushes.Black, 5, 58);
//    GFX.DrawString("(" + mixTeloFinita + ") mix tess", new Font("thaoma", 8), Brushes.Black, 95, 58);
//    GFX.DrawString(Supplemento1CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 70);
//    GFX.DrawString(Supplemento2CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 130, 70);

//    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioTubo.ToString("0000")), 10), PartenzaBarcodeDX, 24);
//    GFX.DrawString(mixTaglioTubo.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 33);
//    GFX.DrawString("Tubo", new Font("thaoma", 6), Brushes.Black, PartenzaBarcodeDX + 14, 34);
//    if (NoteCMBBX.Text.Contains("tasca")) // con fondale in tasca
//    {
//        GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, (mixTaglioTubo - 8).ToString("0000")), 10), PartenzaBarcodeDX, 45);
//        GFX.DrawString((mixTaglioTubo - 8).ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 53);
//        GFX.DrawString("fond.18", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 53);
//    }
//    else GFX.DrawString("Fondale", new Font("thaoma", 6), Brushes.Black, PartenzaBarcodeDX + 14, 41);

//    GFX.DrawString(Note.ToString(), new Font("thaoma", 8), Brushes.Black, 5, 83);
//    GFX.DrawString(Rif.ToString(), new Font("thaoma", 8), Brushes.Black, 220, 83);
//    pictureBox1.Image = drawingsurface;

//    GFX2.Clear(Color.White);
//    GFX2.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 0);
//    GFX2.DrawString("R. medio motore " + Doppiorullo, new Font("thaoma", 8), Brushes.Black, 200, 0);
//    GFX2.DrawString("Affiancato", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 220, 12);
//    GFX2.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
//    GFX2.DrawString("Col t " + Varie1TCMBX.Text, new Font("thaoma", 8), Brushes.Black, 87, 22);
//    GFX2.DrawString("L " + luceLperEtich_Page2 + "   (Ltot" + luceLperEtich + ")", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
//    GFX2.DrawString("H " + H_TXBX.Text.Replace(".", ","), new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 120, 40);
//    GFX2.DrawString("Cavo SX", new Font("thaoma", 8), Brushes.Black, 5, 58);
//    GFX2.DrawString("(" + mixTeloFinita_Page2 + ") mix tess", new Font("thaoma", 8), Brushes.Black, 95, 58);
//    GFX2.DrawString(Supplemento1CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 70);
//    GFX2.DrawString(Supplemento2CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 130, 70);

//    GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioTubo2.ToString("0000")), 10), PartenzaBarcodeDX, 24);
//    GFX2.DrawString(mixTaglioTubo2.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 33);
//    GFX2.DrawString("Tubo", new Font("thaoma", 6), Brushes.Black, PartenzaBarcodeDX + 14, 34);
//    if (NoteCMBBX.Text.Contains("tasca")) // con fondale in tasca
//    {
//        GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioTubo2.ToString("0000")), 10), PartenzaBarcodeDX, 24);
//        GFX2.DrawString(mixTaglioTubo2.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 33);
//        GFX2.DrawString("Tubo", new Font("thaoma", 6), Brushes.Black, PartenzaBarcodeDX + 14, 34);
//        GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, (mixTaglioTubo2 - 8).ToString("0000")), 10), PartenzaBarcodeDX, 45);
//        GFX2.DrawString((mixTaglioTubo2 - 8).ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 53);
//        GFX2.DrawString("fond.18", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 53);
//    }
//    else GFX2.DrawString("Fondale", new Font("thaoma", 6), Brushes.Black, PartenzaBarcodeDX + 14, 41);

//    GFX2.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
//    GFX2.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);

//    pictureBox2.Image = drawingsurface2;

//    GFX3.Clear(Color.White);
//    GFX3.DrawString(Alias.Substring(0, Alias.Length > 27 ? 27 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 2);
//    GFX3.DrawString("-----Package:------------------------------------------------------------------------", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 18);
//    GFX3.DrawString("- Busta staffe, coprviti, cover, staffa centrale con gommini copriviti", new Font("thaoma", 8), Brushes.Black, 5, 31);
//    GFX3.DrawString("- Istruzioni motore + (brucola finecorsa?)", new Font("thaoma", 8), Brushes.Black, 5, 44);
//    GFX3.DrawString("2 Buste: " + (string.IsNullOrEmpty(L_TXBX.Text) ? 0 : int.Parse(L_TXBX.Text) + 300) + "mm", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 220, 83);

//    GFX3.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);

//    pictureBox3.Image = drawingsurface3;
//}
//xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx       