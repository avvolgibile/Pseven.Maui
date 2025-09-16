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
    public class EtichettaBinarioVerticale(Etichetta etichetta) : EtichettaDrawBase(etichetta)
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
//GFX.DrawString("Binario Verticale", new Font("thaoma", 8), Brushes.Black, 220, 3);
//GFX.DrawString("Col " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 20);
//GFX.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 35);
//GFX.DrawString(Hc.ToString(), new Font("thaoma", 8), Brushes.Black, 135, 35);
//GFX.DrawRectangle(pen1, 7, 50, 177, 31);
//GFX.DrawString("(" + CalcoliVari.CalcoloCordaVerticale_Pannello(HcTXBX.Text, L_TXBX.Text) + ")corda", new Font("thaoma", 8), Brushes.Black, 121, 53);
//GFX.DrawString(CalcoliVari.N_bande(L_TXBX, AperturaCentraleCKBX.Checked), new Font("thaoma", 8), Brushes.Black, 5, 53);
//GFX.DrawString("(" + CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "", uno: 0, due: 1100, tre: 1900, quattro: 3000, cinque: 4000, sei: 6001) + ")N°clip", new Font("thaoma", 8), Brushes.Black, 7, 67);

//GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioCassonetto.ToString("0000")), 11), 190, 23);
//GFX.DrawString(mixTaglioCassonetto.ToString(), new Font("thaoma", 7), Brushes.Black, 250, 32);
//GFX.DrawString("Binario", new Font("thaoma", 6), Brushes.Black, 200, 34);
//GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioTubo.ToString("0000")), 11), 190, 45);
//GFX.DrawString(mixTaglioTubo.ToString(), new Font("thaoma", 7), Brushes.Black, 250, 56);
//GFX.DrawString("Alberino", new Font("thaoma", 6), Brushes.Black, 200, 57);

//GFX.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
//GFX.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);

//pictureBox1.Image = drawingsurface;