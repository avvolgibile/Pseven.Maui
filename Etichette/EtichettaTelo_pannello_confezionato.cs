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
    public class EtichettaTelo_pannello_confezionato(Etichetta etichetta) : EtichettaDrawBase(etichetta)
    {
        protected override void DrawSpecific(ICanvas canvas, RectF dirtyRect)
        {

           
            canvas.Font = new Font("thaoma", 8);
            canvas.DrawString(etichetta.Alias, 5, 9, HorizontalAlignment.Left);

        }
    }
}



//GFX.Clear(Color.White);
//GFX.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 3);
//GFX.DrawString("Telo pannello", new Font("thaoma", 8), Brushes.Black, 210, 3);
//GFX.DrawString("Col t " + Varie1TCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
//GFX.DrawString("L " + L_TXBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
//GFX.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 75, 40);
//GFX.DrawString("PZ " + PezziTXBX.Text, new Font("thaoma", 8), Brushes.Black, 150, 40);
//GFX.DrawString("Segno con penna 55mm", new Font("thaoma", 7), Brushes.Black, 200, 70);
//GFX.DrawString("(" + mixTeloFinita + ") mix tess", new Font("thaoma", 8), Brushes.Black, 130, 58);
//GFX.DrawString(Supplemento1CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 65);

//GFX.DrawString("NOTE " + NoteCMBBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 83);
//GFX.DrawString("Rif " + RifTXBX.Text, new Font("thaoma", 8), Brushes.Black, 220, 83);

//pictureBox1.Image = drawingsurface;