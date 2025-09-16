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
    public class EtichettaRullo_Cass_63_83_cat(Etichetta etichetta) : EtichettaDrawBase(etichetta)
    {
        protected override void DrawSpecific(ICanvas canvas, RectF dirtyRect)
        {

          
            canvas.Font = new Font("thaoma", 8);
            canvas.DrawString(etichetta.Alias, 5, 9, HorizontalAlignment.Left);

        }
    }
}


//GFX.Clear(Color.White);
//GFX.DrawString(Alias.Substring(0, Alias.Length > 25 ? 25 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 0);
//GFX.DrawString(Nomerullo, new Font("thaoma", 8), Brushes.Black, 220, 0);
//GFX.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
//GFX.DrawString("Col t " + Varie1TCMBX.Text, new Font("thaoma", 8), Brushes.Black, 87, 22);
//GFX.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
//GFX.DrawString("H " + H_TXBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 60, 40);
//GFX.DrawString("COM " + ComandiCMBX.Text, new Font("thaoma", 8), Brushes.Black, 110, 40);
//GFX.DrawString(Hc, new Font("thaoma", 8), Brushes.Black, 160, 40);
//GFX.DrawString(Att, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 56);
//GFX.DrawString("(" + mixTeloFinita + ") mix tess", new Font("thaoma", 8), Brushes.Black, 95, 56);
//GFX.DrawString(Supplemento1CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 70);
//GFX.DrawString(Supplemento2CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 130, 70);

//GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioCassonetto.ToString("0000")), 10), PartenzaBarcodeDX, 20);
//GFX.DrawString(mixTaglioCassonetto.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 29);
//GFX.DrawString("cass", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 28);

//if (NoteCMBBX.Text.Contains("tasca")) // con fondale in tasca
//{
//    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioTubo.ToString("0000")), 10), PartenzaBarcodeDX, 39);
//    GFX.DrawString(mixTaglioTubo.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 47);
//    GFX.DrawString("Tubo", new Font("thaoma", 6), Brushes.Black, PartenzaBarcodeDX + 14, 48);
//    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, (mixTaglioTubo - 8).ToString("0000")), 10), PartenzaBarcodeDX, 60);
//    GFX.DrawString((mixTaglioTubo - 8).ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 68);
//    GFX.DrawString("fond.18", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 68);
//}
//else
//{
//    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioTubo.ToString("0000")), 10), PartenzaBarcodeDX, 46);
//    GFX.DrawString(mixTaglioTubo.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 56);
//    GFX.DrawString("tubo", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 55);
//    GFX.DrawString("fondale" + (Nomerullo.Contains("75") ? " rettangolare" : ""), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 63);
//}

//GFX.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
//GFX.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);

//pictureBox1.Image = drawingsurface;

//GFX2.Clear(Color.White);
//GFX2.DrawString(Alias.Substring(0, Alias.Length > 27 ? 27 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 2);
//GFX2.DrawString("Package: ", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 238, 2);
//GFX2.DrawString("-------------------------------------------------------------------------------", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 10);
//GFX2.DrawString("- " + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "attacchi " + AttacchiCBX.Text, uno: 0, due: 500, tre: 1200, quattro: 2000, cinque: 2800), new Font("thaoma", 7), Brushes.Black, 5, 22);
//GFX2.DrawString("- fermacatena?? " /*+ annotazioniVerieSuEtichiette*/, new Font("thaoma", 7), Brushes.Black, 5, 34);
//GFX2.DrawString(accessoriCavettate[0], new Font("thaoma", 7), Brushes.Black, 5, 46);
//GFX2.DrawString(accessoriCavettate[1], new Font("thaoma", 7), Brushes.Black, 5, 58);
//GFX2.DrawString(accessoriCavettate[2], new Font("thaoma", 7), Brushes.Black, 5, 70);
//GFX2.DrawString("Busta: " + (string.IsNullOrEmpty(L_TXBX.Text) ? 0 : int.Parse(L_TXBX.Text) + 300) + "mm", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 220, 83);

//GFX2.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);

//pictureBox2.Image = drawingsurface2;