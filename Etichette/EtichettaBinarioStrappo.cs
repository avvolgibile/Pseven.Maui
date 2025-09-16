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
    public class EtichettaBinarioStrappo(Etichetta etichetta) : EtichettaDrawBase(etichetta)
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
//GFX.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 3);
//GFX.DrawString("Binario strappo", new Font("thaoma", 8), Brushes.Black, 200, 3);
//GFX.DrawString("L " + L_TXBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 16, 40);
//GFX.DrawString(H_TXBX.Text + " " + VersioneCMBX.Text + " " + H2_TXBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);//   versione

//GFX.DrawRectangle(pen1, 8, 55, 120, 20);
//GFX.DrawString(calcoloString, new Font("thaoma", 8), Brushes.Black, 10, 57);

//GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioBinario.ToString("0000")), 13), 200, 27);
//GFX.DrawString(mixTaglioXetich, new Font("thaoma", 8), Brushes.Black, 196, 56);
////  GFX.DrawString("Binario", new Font("thaoma", 7), Brushes.Black, 214, 41);

//GFX.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
//GFX.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);
//pictureBox1.Image = drawingsurface;