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
    public class EtichettaRullo_Cass_63_83_110_mot(Etichetta etichetta) : EtichettaDrawBase(etichetta)
    {
        protected override void DrawSpecific(ICanvas canvas, RectF dirtyRect)
        {

            canvas.Font = new Font("thaoma", 8);
            canvas.DrawString(etichetta.Alias, 5, 9, HorizontalAlignment.Left);

        }
    }
}

//GFX.Clear(Color.White);
//GFX.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 1);
//GFX.DrawString(Nomerullo, new Font("thaoma", 8), Brushes.Black, 220, 1);
//GFX.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
//GFX.DrawString("Col t " + Varie1TCMBX.Text, new Font("thaoma", 8), Brushes.Black, 87, 22);
//GFX.DrawString("L " + luceLperEtich.ToString(), new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
//GFX.DrawString("H " + H_TXBX.Text.Replace(".", ","), new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 65, 40);
//GFX.DrawString("Cavo " + ComandiCMBX.Text, new Font("thaoma", 8), Brushes.Black, 120, 40);
//GFX.DrawString(Att, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 58);
//GFX.DrawString("(" + mixTeloFinita + ") mix tess", new Font("thaoma", 8), Brushes.Black, 95, 58);
//GFX.DrawString(Supplemento1CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 70);
//GFX.DrawString(Supplemento2CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 130, 70);

//GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioCassonetto.ToString("0000")), 10), PartenzaBarcodeDX, 19);
//GFX.DrawString(mixTaglioCassonetto.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 28);
//GFX.DrawString("cass", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 27);
//if (NoteCMBBX.Text.Contains("tasca")) // con fondale in tasca
//{
//    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioTubo.ToString("0000")), 10), PartenzaBarcodeDX, 39);
//    GFX.DrawString(mixTaglioTubo.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 47);
//    GFX.DrawString("Tubo", new Font("thaoma", 6), Brushes.Black, PartenzaBarcodeDX + 14, 48);
//    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, (mixTaglioTubo - 8).ToString("0000")), 10), PartenzaBarcodeDX, 60);
//    GFX.DrawString((mixTaglioTubo - 8f).ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 68);
//    GFX.DrawString("fond.18", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 68);
//}
////else if (Nomerullo.Contains("CEILING")) // CEILING
////{
////    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioTubo.ToString("0000")), 10), PartenzaBarcodeDX, 39);
////    GFX.DrawString(mixTaglioTubo.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 47);
////    GFX.DrawString("Tubo", new Font("thaoma", 6), Brushes.Black, PartenzaBarcodeDX + 14, 48);
////    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, (luceLperEtich - 8).ToString("0000")), 10), PartenzaBarcodeDX, 60);
////    GFX.DrawString((luceLperEtich - 8f).ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 68);
////    GFX.DrawString("fond.TRIANG.", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX , 68);
////}
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