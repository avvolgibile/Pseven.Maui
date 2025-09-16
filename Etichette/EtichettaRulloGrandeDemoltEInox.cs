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
    public class EtichettaRulloGrandeDemoltEInox(Etichetta etichetta) : EtichettaDrawBase(etichetta)
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
//GFX.DrawString(Nomerullo, new Font("thaoma", 8), Brushes.Black, 210, 3);
//GFX.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
//GFX.DrawString("Col t " + Varie1TCMBX.Text, new Font("thaoma", 8), Brushes.Black, 83, 22);
//GFX.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
//GFX.DrawString("H " + H_TXBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 70, 40);
//GFX.DrawString("COM " + ComandiCMBX.Text, new Font("thaoma", 8), Brushes.Black, 109, 40);
//GFX.DrawString(Hc, new Font("thaoma", 8), Brushes.Black, 158, 40);
//GFX.DrawString(Att, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 56);
//GFX.DrawString("(" + mixTeloFinita + ")tess + Coestr.", new Font("thaoma", 8), Brushes.Black, 90, 56);
//GFX.DrawString(Supplemento1CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 70);

//GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioTubo.ToString("0000")), 10), PartenzaBarcodeDX, 20);
//GFX.DrawString(mixTaglioTubo.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 29);
//GFX.DrawString("Tubo", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 29);
//if (NoteCMBBX.Text.Contains("tasca")) // con fondale in tasca
//{
//    if (Nomerullo.ToLower().Contains("inox")) { MessageBox.Show("Cavi inox ha già il fondale in tasca"); NoteCMBBX.Text = ""; }

//    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, (mixTaglioTubo - 8).ToString("0000")), 10), PartenzaBarcodeDX, 43);
//    GFX.DrawString((mixTaglioTubo - 8).ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 51);
//    GFX.DrawString("fond.18", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 51);
//}
//else
//{

//    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioFondaleManiglia.ToString("0000")), 10), PartenzaBarcodeDX, 43);
//    GFX.DrawString(mixTaglioFondaleManiglia.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 51);
//    GFX.DrawString("Fondale", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 51);
//}

//GFX.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
//GFX.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);

////-------------------------------------------------------------------------------------------
//if (Nomerullo.ToLower().Contains("inox"))
//{
//    GFX2.Clear(Color.White);
//    GFX2.DrawString(Alias.Substring(0, Alias.Length > 27 ? 27 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 3);
//    /* GFX*/
//    GFX.DrawString("- Zavorra: " + Math.Round(string.IsNullOrEmpty(L_TXBX.Text) ? 0 : int.Parse(L_TXBX.Text) - 100d, 1) + " mm   con scorrev guidato RC.275", new Font("thaoma", 7), Brushes.Black, 110, 70);//attenzione GFX
//    GFX2.DrawString("Package: ", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 238, 2);
//    GFX2.DrawString("-------------------------------------------------------------------------------", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 10);
//    GFX2.DrawString(accessoriCavettate[0], new Font("thaoma", 7), Brushes.Black, 5, 22);
//    GFX2.DrawString("- 2 cavi inox Ø 3 da: " + (string.IsNullOrEmpty(H_TXBX.Text) ? 0 : int.Parse(H_TXBX.Text) + 100) + "mm", new Font("thaoma", 7), Brushes.Black, 5, 46);
//    GFX2.DrawString(accessoriCavettate[1], new Font("thaoma", 7), Brushes.Black, 5, 34);
//    GFX2.DrawString(accessoriCavettate[2], new Font("thaoma", 7), Brushes.Black, 5, 58);
//    GFX2.DrawString("Busta: " + (string.IsNullOrEmpty(L_TXBX.Text) ? 0 : int.Parse(L_TXBX.Text) + 300) + "mm", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 220, 83);
//    GFX2.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
//    pictureBox2.Image = drawingsurface2;
//}
//else
//{

//}

//pictureBox1.Image = drawingsurface;//deve stare qui!!