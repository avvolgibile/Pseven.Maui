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
    public class EtichettaLucernaio63(Etichetta etichetta) : EtichettaDrawBase(etichetta)
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


//GFX.Clear(Color.White);
//GFX.DrawString(Alias.Substring(0, Alias.Length > 35 ? 35 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 0);
//GFX.DrawString("Lucernaio 63", new Font("thaoma", 8), Brushes.Black, 230, 0);
//GFX.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
//GFX.DrawString("Col t " + Varie1TCMBX.Text, new Font("thaoma", 8), Brushes.Black, 87, 22);
//GFX.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
//GFX.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 75, 40);
//GFX.DrawString("Cavo " + ComandiCMBX.Text, new Font("thaoma", 8), Brushes.Black, 135, 40);
//GFX.DrawString(Att.ToString(), new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 58);
//GFX.DrawString("(" + mixTeloFinita + ") mix tess", new Font("thaoma", 8), Brushes.Black, 95, 58);
//GFX.DrawString(Supplemento1CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 70);

//GFX.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
//GFX.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);
//pictureBox1.Image = drawingsurface;

//GFX2.Clear(Color.White);
//GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioCassonetto.ToString("0000")), 10), 10, 3);
//GFX2.DrawString(mixTaglioCassonetto.ToString(), new Font("thaoma", 7), Brushes.Black, 70, 12);
//GFX2.DrawString("cass", new Font("thaoma", 7), Brushes.Black, 14, 11);
//GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioTubo.ToString("0000")), 10), 10, 26);
//GFX2.DrawString(mixTaglioTubo.ToString(), new Font("thaoma", 7), Brushes.Black, 70, 34);
//GFX2.DrawString("tuboMotore", new Font("thaoma", 7), Brushes.Black, 14, 33);
//GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioTubo2.ToString("0000")), 10), 10, 49);
//GFX2.DrawString(mixTaglioTubo2.ToString(), new Font("thaoma", 7), Brushes.Black, 70, 57);
//GFX2.DrawString("tuboMolla", new Font("thaoma", 7), Brushes.Black, 14, 56);

//GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioGuida.ToString("0000")), 10), PartenzaBarcodeDX, 26);
//GFX2.DrawString(mixTaglioGuida.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 34);
//GFX2.DrawString("guide", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 33);

//GFX2.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
//pictureBox2.Image = drawingsurface2;