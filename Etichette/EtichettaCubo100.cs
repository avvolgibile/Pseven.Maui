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
    public class EtichettaCubo100(Etichetta etichetta) : EtichettaDrawBase(etichetta)
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
//GFX.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 2);
//GFX.DrawString(DD1[0], new Font("thaoma", 8), Brushes.Black, 215, 2);
//GFX.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
//GFX.DrawString("Col t " + Varie1TCMBX.Text, new Font("thaoma", 8), Brushes.Black, 87, 22);
//GFX.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
//GFX.DrawString("H " + H_TXBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 60, 40);
//GFX.DrawString("COM " + ComandiCMBX.Text, new Font("thaoma", 8), Brushes.Black, 115, 40);
//GFX.DrawString("(" + mixTeloFinita + ") mix tess", new Font("thaoma", 8), Brushes.Black, 88, 58);
//GFX.DrawString(Supplemento1CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 70);

//GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioCassonetto.ToString("0000")), 9), PartenzaBarcodeDX, 18);
//GFX.DrawString(mixTaglioCassonetto.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 25);
//GFX.DrawString("cass" + (Supplemento2CMBX.Text.ToLower().Contains("flat") ? "+ flat" : ""), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 25);
//GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioTubo.ToString("0000")), 9), PartenzaBarcodeDX, 39);
//GFX.DrawString(mixTaglioTubo.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 46);
//GFX.DrawString("tubo " + Tipo_tubo, new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 46);
//GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioFondaleManiglia.ToString("0000")), 9), PartenzaBarcodeDX, 59);
//GFX.DrawString(mixTaglioFondaleManiglia.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 66);
//GFX.DrawString("fondale", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 66);

//GFX.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
//GFX.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);

//pictureBox1.Image = drawingsurface;

//GFX2.Clear(Color.White);
//GFX2.DrawString(Alias.Substring(0, Alias.Length > 31 ? 31 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 2);
//GFX2.DrawString(DD1[0], new Font("thaoma", 8), Brushes.Black, 215, 2);
//GFX2.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 20);
//GFX2.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 35);
//GFX2.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 75, 35);

//GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioGuida.ToString("0000")), 8), PartenzaBarcodeDX, 18);
//GFX2.DrawString(mixTaglioGuida.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 25);
//GFX2.DrawString("guide", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 25);
//GFX2.DrawString("Mix Zavorra: " + MixZavorra, new Font("thaoma", 9), Brushes.Black, PartenzaBarcodeDX - 20, 49);
//GFX2.DrawString(Supplemento2CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 60);


//GFX2.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
//GFX2.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);

//pictureBox2.Image = drawingsurface2;

//GFX3.Clear(Color.White);
//GFX3.DrawString(Alias.Substring(0, Alias.Length > 27 ? 27 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 2);
//GFX3.DrawString("-----Package:------------------------------------------------------------------------", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 18);
//GFX3.DrawString(annotazioniVerieSuEtichiette, new Font("thaoma", 8), Brushes.Black, 5, 45);
//GFX3.DrawString("-Istruzioni motore", new Font("thaoma", 8), Brushes.Black, 5, 57);
//GFX3.DrawString("-Mix dima : " + mixDima, new Font("thaoma", 8), Brushes.Black, 5, 69);

//GFX3.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
//GFX3.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);

//pictureBox3.Image = drawingsurface3;