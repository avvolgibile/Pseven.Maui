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
    public class EtichettaReteSaldata(Etichetta etichetta) : EtichettaDrawBase(etichetta)
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
//GFX.DrawString("Rete Saldata", new Font("thaoma", 8), Brushes.Black, 210, 3);
//GFX.DrawString("L " + L_TXBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 22);
//GFX.DrawString("H " + H_TXBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 75, 22);

//GFX.DrawString("NOTE: " + NoteCMBBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 83);
//GFX.DrawString("Rif " + RifTXBX.Text, new Font("thaoma", 8), Brushes.Black, 220, 83);

//pictureBox1.Image = drawingsurface;