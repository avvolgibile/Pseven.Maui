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
    public class EtichettaSole_33_41_Cat(Etichetta etichetta) : EtichettaDrawBase(etichetta)
    {
        protected override void DrawSpecific(ICanvas canvas, RectF dirtyRect)
        {

            canvas.Font = new Font("thaoma", 8);
            canvas.DrawString(etichetta.Alias, 5, 9, HorizontalAlignment.Left);

        }
    }
}


//GFX.Clear(Color.White);
//GFX.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 0);
//GFX.DrawString(Nomerullo, new Font("thaoma", 8), Brushes.Black, 210, 0);
//GFX.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
//GFX.DrawString("Col t " + Varie1TCMBX.Text, new Font("thaoma", 8), Brushes.Black, 95, 22);
//GFX.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
//GFX.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 60, 40);
//GFX.DrawString("COM " + ComandiCMBX.Text, new Font("thaoma", 8), Brushes.Black, 109, 40);
//GFX.DrawString(Hc, new Font("thaoma", 8), Brushes.Black, 158, 40);
//GFX.DrawString("(" + mixTeloFinita + ") mix tess", new Font("thaoma", 8), Brushes.Black, 95, 56);
//GFX.DrawString(Supplemento1CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 70);

//GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioCassonetto.ToString("0000")), 10), PartenzaBarcodeDX, 20);
//GFX.DrawString(mixTaglioCassonetto.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 30);
//GFX.DrawString("Cass/tubo", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 30);
//GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioFondaleManiglia.ToString("0000")), 10), PartenzaBarcodeDX, 46);
//GFX.DrawString(mixTaglioFondaleManiglia.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 57);
//GFX.DrawString("Maniglia", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 57);
//GFX.DrawString(annotazioniVerieSuEtichiette, new Font("thaoma", 7, FontStyle.Bold), Brushes.Black, 214, 66);

//pictureBox1.Image = drawingsurface;

//GFX.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
//GFX.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);

//GFX2.Clear(Color.White);
//GFX2.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 0);
//GFX2.DrawString(Nomerullo, new Font("thaoma", 8), Brushes.Black, 210, 0);
//GFX2.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
//GFX2.DrawString("Col t " + Varie1TCMBX.Text, new Font("thaoma", 8), Brushes.Black, 95, 22);
//GFX2.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
//GFX2.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 60, 40);
//GFX2.DrawRectangle(pen1, 7, 54, 190, 25);
//GFX2.DrawString("(" + cordino1String + "m)spazzolino", new Font("thaoma", 8), Brushes.Black, 9, 54);
//GFX2.DrawString("(" + cordino2String + ")biadesivo", new Font("thaoma", 8), Brushes.Black, 9, 66);
//GFX2.DrawString("Pacco B", new Font("thaoma", 12, FontStyle.Bold), Brushes.Black, 123, 58);

//GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioGuida.ToString("0000")), 10), PartenzaBarcodeDX, 20);
//GFX2.DrawString(mixTaglioGuida.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 30);
//GFX2.DrawString("Guide", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 30);

//GFX2.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
//GFX2.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);

//pictureBox2.Image = drawingsurface2;
