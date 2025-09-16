using Microsoft.Maui.Graphics.Skia;
using Pseven.Models;
using Pseven.Services;
using SkiaSharp;
using System;
using System.Drawing;
using ZXing;
using static System.Net.Mime.MediaTypeNames;
using Font = Microsoft.Maui.Graphics.Font;

namespace Pseven.Etichette
{
    public class EtichettaTuttGliAltri(Etichetta etichetta) : EtichettaDrawBase(etichetta)
    {
        protected override void DrawSpecific(ICanvas canvas, RectF dirtyRect)
        {

           
            canvas.Font = new Font("thaoma", 8);
            canvas.DrawString(etichetta.Alias, 5, 9, HorizontalAlignment.Left);

        }
    }
}


//GFX.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 3);//manca perche da' errore quando chiudo prodotto provvisorio
//GFX.DrawString(articoliCB.Text, new Font("Arial", 8), Brushes.Black, 5, 22);
//GFX.DrawString("Col  " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 58);

//GFX.DrawString("NOTE: " + NoteCMBBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 83);
//GFX.DrawString("Rif " + RifTXBX.Text, new Font("thaoma", 8), Brushes.Black, 175, 83);

//pictureBox1.Image = drawingsurface;