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
    public class EtichettaBinarioPannelloCorda(Etichetta etichetta) : EtichettaDrawBase(etichetta )
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
 //           GFX.DrawString(Alias.Substring(0, Alias.Length > 35 ? 35 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 0);
 //           GFX.DrawString(NumeroVie.ToString() + " vie corda", new Font("thaoma", 8), Brushes.Black, 220, 0);
 //           GFX.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 22);
 //           GFX.DrawString("COM " + ComandiCMBX.Text, new Font("thaoma", 8), Brushes.Black, 63, 22);
 //           GFX.DrawString(Hc.ToString(), new Font("thaoma", 8), Brushes.Black, 130, 22);

 //           GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioBinario.ToString("0000")), 8), PartenzaBarcodeDX, 20);
 //           GFX.DrawString(mixTaglioBinario.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX+70, 26);
 //           GFX.DrawString("Binario", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX+14, 26);

 //           GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioProfiloInferiore.ToString("0000")), 8), PartenzaBarcodeDX, 39);
 //           GFX.DrawString(mixTaglioProfiloInferiore.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 46);
 //           GFX.DrawString(NumeroVie+ " Portateli", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 46);


 //           GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, (mixTaglioProfiloInferiore-12).ToString("0000")), 8), PartenzaBarcodeDX, 61);
 //           GFX.DrawString((mixTaglioProfiloInferiore-12).ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX+70, 67);
 //           GFX.DrawString(NumeroVie + " Pesi", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 67);

 //           GFX.DrawRectangle(pen1, 7, 40, 190, 37);
 //           GFX.DrawString("(" + CalcoliVari.CalcoloCordaVerticale_Pannello(HcTXBX.Text, L_TXBX.Text) + ")mix corda", new Font("thaoma", 8), Brushes.Black, 10, 43);
 //           GFX.DrawString("(" + Math.Round(calcoloFLoat, 2) + ")mix strappo", new Font("thaoma", 8), Brushes.Black, 10, 60);
 //           GFX.DrawString("(" + (NumeroVie >5? (int.Parse(CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "", uno: 0, due: 1100, tre: 1900, quattro: 2600, cinque: 3400, sei: 5000, sette: 6000))* 2).ToString() : CalcoliVari.N_clipSupportiChiavette2(luceLperEtich, "", uno: 0, due: 1100, tre: 1900, quattro: 2600, cinque: 3400, sei: 5000, sette: 6000)) + ")N°clip", new Font("thaoma", 8), Brushes.Black, 140, 43);
 //           GFX.DrawString("Sag. punte Portateli", new Font("thaoma", 7, FontStyle.Bold), Brushes.Black, 100, 60);

 //           GFX.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 80);
 //           GFX.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 250, 80);

 //           pictureBox1.Image = drawingsurface;