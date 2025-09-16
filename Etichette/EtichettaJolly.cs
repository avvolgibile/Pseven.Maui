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
    public class EtichettaJolly(Etichetta etichetta) : EtichettaDrawBase(etichetta)
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



//if (VersioneCMBX.Text != "")
//{
//    GFX.Clear(Color.White);
//    GFX.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 3);
//    GFX.DrawString(DD1[0] + " 2A", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 215, 3);
//    GFX.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
//    GFX.DrawString("L " + luceLperEtich_Page1 + "   (Ltot" + luceLperEtich + ")", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
//    GFX.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 125, 40);
//    GFX.DrawString(DateTime.Now.ToString("dd-MM-yyyy"), new Font("thaoma", 7), Brushes.Black, 5, 60);

//    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, luceHperEtich.ToString("0000")), 10), PartenzaBarcodeDX, 21);
//    GFX.DrawString(luceHperEtich.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 32);//26
//    GFX.DrawString("H", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 31);    //  25 
//    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioProfiloInferiore.ToString("0000")), 10), PartenzaBarcodeDX, 55);
//    GFX.DrawString(mixTaglioProfiloInferiore.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 67);
//    GFX.DrawString("G. a terra", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 66);

//    GFX.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
//    GFX.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);

//    pictureBox1.Image = drawingsurface;
//    GFX2.Clear(Color.White);
//    GFX2.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 0);
//    GFX2.DrawString(DD1[0] + " 2A", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 215, 0);
//    GFX2.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
//    GFX2.DrawString("L " + luceLperEtich_Page1 + "   (Ltot" + luceLperEtich + ")", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
//    GFX2.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 125, 40);

//    GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTagliocompensatore.ToString("0000")), 10), PartenzaBarcodeDX, 21);
//    GFX2.DrawString(mixTagliocompensatore.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 31);
//    GFX2.DrawString("compens.", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 30);
//    //GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioFondaleManiglia.ToString("0000")), 10), PartenzaBarcodeDX, 55);
//    //GFX2.DrawString(mixTaglioFondaleManiglia.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX+70, 67);
//    //GFX2.DrawString("M.manig", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX+14, 66);

//    GFX2.DrawRectangle(pen1, 7, 54, 305, 25);
//    GFX2.DrawString("(" + cordino1String + ")Filo 8", new Font("thaoma", 8), Brushes.Black, 9, 54);
//    GFX2.DrawString("(" + cordino2String + ")Lamina", new Font("thaoma", 8), Brushes.Black, 9, 66);
//    GFX2.DrawString("(" + nAgganciCingoliPannelli + ")cingoli", new Font("thaoma", 8), Brushes.Black, 123, 54);
//    GFX2.DrawString("(2)antivento sostit. ogni" + " " + Math.Round(double.Parse(nAgganciCingoliPannelli) / 3), new Font("thaoma", 8), Brushes.Black, 180, 54);
//    GFX2.DrawString("(" + calcoloString + ")giri Molla", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 123, 66);//

//    GFX2.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
//    GFX2.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);

//    pictureBox2.Image = drawingsurface2;
//    GFX4.Clear(Color.White);
//    GFX4.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 3);
//    GFX4.DrawString(DD1[0] + " 2A", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 215, 3);
//    GFX4.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
//    GFX4.DrawString("L " + luceLperEtich_Page2 + "   (Ltot" + luceLperEtich + ")", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
//    GFX4.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 125, 40);
//    GFX4.DrawString(DateTime.Now.ToString("dd-MM-yyyy"), new Font("thaoma", 7), Brushes.Black, 5, 60);

//    GFX4.DrawImage(barcode.Draw(string.Format(barcodeStruct, luceHperEtich.ToString("0000")), 10), PartenzaBarcodeDX, 21);
//    GFX4.DrawString(luceHperEtich.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 32);//26
//    GFX4.DrawString("H", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 31);    //  25 
//                                                                                               //GFX4.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioProfiloInferiore.ToString("0000")), 10), PartenzaBarcodeDX, 55);
//                                                                                               //GFX4.DrawString(mixTaglioProfiloInferiore.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 67);
//                                                                                               //GFX4.DrawString("G. a terra", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 66);

//    GFX4.DrawString(Note, new Font("thaoma", 8), Brushes.Black, 5, 83);
//    GFX4.DrawString(Rif, new Font("thaoma", 8), Brushes.Black, 220, 83);

//    pictureBox4.Image = drawingsurface4;
//    GFX5.Clear(Color.White);
//    GFX5.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 0);
//    GFX5.DrawString(DD1[0] + " 2A", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 215, 0);
//    GFX5.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
//    GFX5.DrawString("L " + luceLperEtich_Page2 + "   (Ltot" + luceLperEtich + ")", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
//    GFX5.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 125, 40);

//    GFX5.DrawRectangle(pen1, 7, 54, 305, 25);
//    GFX5.DrawString("(" + cordino1String_Page2 + ")Filo 8", new Font("thaoma", 8), Brushes.Black, 9, 54);
//    GFX5.DrawString("(" + cordino2String_Page2 + ")Lamina", new Font("thaoma", 8), Brushes.Black, 9, 66);
//    GFX5.DrawString("(" + nAgganciCingoliPannelli_Page2 + ")cingoli", new Font("thaoma", 8), Brushes.Black, 123, 54);
//    GFX5.DrawString("(2)antivento sostit. ogni" + " " + Math.Round(double.Parse(nAgganciCingoliPannelli_Page2) / 3), new Font("thaoma", 8), Brushes.Black, 180, 54);
//    GFX5.DrawString("(" + calcoloString_Page2 + ")giri Molla", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 123, 66);

//    GFX5.DrawString("NOTE " + NoteCMBBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 83);
//    GFX5.DrawString("Rif " + RifTXBX.Text, new Font("thaoma", 8), Brushes.Black, 220, 83);

//    pictureBox5.Image = drawingsurface5;
//}
//else
//{
//    GFX.Clear(Color.White);
//    GFX.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 3);
//    GFX.DrawString(DD1[0], new Font("thaoma", 8), Brushes.Black, 215, 3);
//    GFX.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
//    GFX.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
//    GFX.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 75, 40);
//    GFX.DrawString(DateTime.Now.ToString("dd-MM-yyyy"), new Font("thaoma", 7), Brushes.Black, 5, 60);

//    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, luceHperEtich.ToString("0000")), 10), PartenzaBarcodeDX, 21);
//    GFX.DrawString(luceHperEtich.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 32);//26
//    GFX.DrawString("H", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 31);//  25 
//    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioProfiloInferiore.ToString("0000")), 10), PartenzaBarcodeDX, 55);
//    GFX.DrawString(mixTaglioProfiloInferiore.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 67);
//    GFX.DrawString("G. a terra", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 66);

//    GFX.DrawString("NOTE " + NoteCMBBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 83);
//    GFX.DrawString("Rif " + RifTXBX.Text, new Font("thaoma", 8), Brushes.Black, 220, 83);

//    pictureBox1.Image = drawingsurface;
//    GFX2.Clear(Color.White);
//    GFX2.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 0);
//    GFX2.DrawString(DD1[0], new Font("thaoma", 8), Brushes.Black, 215, 0);
//    GFX2.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
//    GFX2.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
//    GFX2.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 75, 40);

//    GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTagliocompensatore.ToString("0000")), 10), PartenzaBarcodeDX, 21);
//    GFX2.DrawString(mixTagliocompensatore.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 32);
//    GFX2.DrawString("compens.", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 31);
//    // GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioFondaleManiglia.ToString("0000")), 10), PartenzaBarcodeDX, 55);
//    //GFX2.DrawString(mixTaglioFondaleManiglia.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX+70, 67);
//    //GFX2.DrawString("B.manig", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX+14, 66);
//    GFX2.DrawRectangle(pen1, 7, 54, 305, 25);

//    GFX2.DrawString("(" + cordino1String + ")Filo 8", new Font("thaoma", 8), Brushes.Black, 9, 54);
//    GFX2.DrawString("(" + cordino2String + ")Lamine", new Font("thaoma", 8), Brushes.Black, 9, 66);
//    GFX2.DrawString("(" + nAgganciCingoliPannelli + ")cingoli", new Font("thaoma", 8), Brushes.Black, 123, 54);
//    GFX2.DrawString("(2)antivento sostit. ogni" + " " + Math.Round(double.Parse(nAgganciCingoliPannelli) / 3), new Font("thaoma", 8), Brushes.Black, 180, 54);
//    GFX2.DrawString("(" + calcoloString + ")giri Molla", new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 123, 66);
//    GFX2.DrawString("NOTE " + NoteCMBBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 83);
//    GFX2.DrawString("Rif " + RifTXBX.Text, new Font("thaoma", 8), Brushes.Black, 220, 83);

//    pictureBox2.Image = drawingsurface2;

//    pictureBox4.Image = null;
//    pictureBox5.Image = null;
//}