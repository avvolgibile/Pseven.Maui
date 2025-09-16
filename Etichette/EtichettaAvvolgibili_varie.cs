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
    public class EtichettaAvvolgibili_varie(Etichetta etichetta) : EtichettaDrawBase(etichetta)
    {


        protected override void DrawSpecific(ICanvas canvas, RectF dirtyRect)
        {

        }


    }


   


    }


//GFX.Clear(Color.White);
//GFX.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 3);
//GFX.DrawString(NomeAvv, new Font("thaoma", 8), Brushes.Black, 210, 3);
//GFX.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
//GFX.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
//GFX.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 75, 40);

//GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioProfilo.ToString("0000")), 12), PartenzaBarcodeDX, 30);
//GFX.DrawString(mixTaglioProfilo.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 41);
//GFX.DrawString("stecca", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 40);
//GFX.DrawString(Supplemento2CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 65);

//GFX.DrawString("NOTE: " + NoteCMBBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 83);
//GFX.DrawString("Rif " + RifTXBX.Text, new Font("thaoma", 8), Brushes.Black, 175, 83);
//pictureBox1.Image = drawingsurface;

//if (Supplemento1CMBX.Text != "")
//{
//    GFX2.Clear(Color.White);
//    GFX2.DrawString(Alias.Substring(0, Alias.Length > 31 ? 31 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 0);

//    GFX2.DrawString(DD_suppl1[0], new Font("thaoma", 8), Brushes.Black, 5, 17);

//    GFX2.DrawString("COL " + Colore_Supplemento1CMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 33);
//    GFX2.DrawString("L " + L_finita_supplemento1, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 50);
//    GFX2.DrawString(H_finita_supplemento1 != 0 ? "H " + H_finita_supplemento1 : "", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 60, 50);
//    GFX2.DrawString(pzSupplXetichetta, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 65);
//    GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioProfiloSupplemento1.ToString("0000")), 8), PartenzaBarcodeDX, 46);
//    GFX2.DrawString(mixTaglioProfiloSupplemento1.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 56);// Qnt_suppTXBX.Text = 

//    GFX2.DrawString("NOTE: " + NoteCMBBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 83);
//    GFX2.DrawString("Rif " + RifTXBX.Text, new Font("thaoma", 8), Brushes.Black, 175, 83);

//    pictureBox2.Image = drawingsurface2;
//}
//else pictureBox2.Image = null;