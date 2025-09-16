using Microsoft.Maui.Graphics.Skia;
using Pseven.Models;
using Pseven.Services;
using SkiaSharp;
using System;
using System.Drawing;
using ZXing;
using ZXing.QrCode.Internal;
using static System.Net.Mime.MediaTypeNames;
using Font = Microsoft.Maui.Graphics.Font;

namespace Pseven.Etichette
{
    public class EtichettaPortaASoffietto(Etichetta etichetta) : EtichettaDrawBase(etichetta)
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
//GFX.DrawString(Alias.Substring(0, Alias.Length > 20 ? 20 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 0);
//GFX.DrawString("P.Soff", new Font("thaoma", 8), Brushes.Black, 180, 0);
//GFX.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 225, 0);
//GFX.DrawString("L " + L_TXBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 22);
//GFX.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 75, 22);
//GFX.DrawString("(" + (luceHperEtich - 32) + ") mix pannello", new Font("thaoma", 8), Brushes.Black, 17, 44);
//GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioProfilo.ToString("0000")), 12), PartenzaBarcodeDX, 22);
//GFX.DrawString(mixTaglioProfilo.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 36);
//if (Supplemento1CMBX.Text != "") GFX.DrawString("Con Serratura", new Font("thaoma", 10, FontStyle.Bold), Brushes.Black, 117, 40);
//GFX.DrawString(calcoloString.ToString(), new Font("thaoma", 8), Brushes.Black, 5, 60);

//GFX.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
//GFX.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);
//pictureBox1.Image = drawingsurface;