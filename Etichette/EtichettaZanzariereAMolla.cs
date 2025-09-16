using Pseven.Models;
using Pseven.Varie;
using Font = Microsoft.Maui.Graphics.Font;
using ZXing.QrCode.Internal;

namespace Pseven.Etichette
{
    public class EtichettaZanzariereAMolla(Etichetta etichetta) : EtichettaDrawBase(etichetta)
    {
        protected override void DrawSpecific(ICanvas canvas, RectF dirtyRect)
        {

          
            canvas.Font = new Font("thaoma", 8);
            canvas.DrawString(etichetta.Alias, 5, 9, HorizontalAlignment.Left);

        }
    }
}


//GFX.Clear(Color.White);
//GFX.DrawString(Alias.Substring(0, Alias.Length > 31 ? 31 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 2);
//GFX.DrawString(nomezanzariera, new Font("thaoma", 8), Brushes.Black, 215, 2);
//GFX.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
//GFX.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 37);
//GFX.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 75, 37);
//GFX.DrawString(DateTime.Now.ToString("dd-MM-yyyy"), new Font("thaoma", 7), Brushes.Black, 5, 51);

//if (Supplemento1CMBX.Text != "")
//{
//    GFX.FillRectangle(Brushes.Gray, 5, 65, 90, 16);
//    GFX.DrawString(Supplemento1CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.White, 5, 65);
//}
//else GFX.FillRectangle(Brushes.White, 0, 0, 0, 0);

//if (User == "io")
//{
//    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioCassonetto.ToString("0000")), 9), PartenzaBarcodeDX, 21);
//    GFX.DrawString(mixTaglioCassonetto.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 29);
//    GFX.DrawString("cass", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 28);
//    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioTubo.ToString("0000")), 9), PartenzaBarcodeDX, 41);
//    GFX.DrawString(mixTaglioTubo.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 48);
//    GFX.DrawString("tubo", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 47);
//    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioFondaleManiglia.ToString("0000")), 9), PartenzaBarcodeDX, 61);
//    GFX.DrawString(mixTaglioFondaleManiglia.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 68);
//    GFX.DrawString("maniglia", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 67);

//}
//else//Alessio
//{
//    GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, (luceLperEtich - 22).ToString("0000")), 10), PartenzaBarcodeDX, 21);
//    GFX.DrawString((luceLperEtich - 22).ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 31);
//    GFX.DrawString("L", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 30);
//}

//GFX.DrawString("NOTE: " + NoteCMBBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 83);
//GFX.DrawString("Rif " + RifTXBX.Text, new Font("thaoma", 8), Brushes.Black, 220, 83);

//pictureBox1.Image = drawingsurface;

//GFX2.Clear(Color.White);
//GFX2.DrawString(Alias.Substring(0, Alias.Length > 31 ? 31 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 2);
//GFX2.DrawString(nomezanzariera, new Font("thaoma", 8), Brushes.Black, 215, 2);
//GFX2.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 20);
//GFX2.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 35);
//GFX2.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 75, 35);

//GFX2.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioGuida.ToString("0000")), 10), PartenzaBarcodeDX, 21);
//GFX2.DrawString(mixTaglioGuida.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 29);
//GFX2.DrawString("guide", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 29);

//GFX2.DrawString("NOTE: " + NoteCMBBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 49);
//GFX2.DrawString("Rif " + RifTXBX.Text, new Font("thaoma", 8), Brushes.Black, 220, 49);

//pictureBox2.Image = drawingsurface2;

//GFX3.Clear(Color.White);
//GFX3.DrawString(Alias.Substring(0, Alias.Length > 31 ? 31 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 2);
//GFX3.DrawString(nomezanzariera, new Font("thaoma", 8), Brushes.Black, 215, 2);
//GFX3.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 20);
//GFX3.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 35);
//GFX3.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 75, 35);

//GFX3.DrawImage(barcode.Draw(string.Format(barcodeStruct, luceLperEtich.ToString("0000")), 10), PartenzaBarcodeDX, 21);
//GFX3.DrawString("(" + mixTeloFinita + ")rete", new Font("thaoma", 8), Brushes.Black, 213, 35);
//if (Supplemento2CMBX.Text != "")
//{
//    GFX3.FillRectangle(Brushes.Gray, 210, 50, 85, 16);
//    GFX3.DrawString(Supplemento2CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.White, 213, 50);
//}
//else GFX3.FillRectangle(Brushes.White, 0, 0, 0, 0);

//GFX3.DrawString("NOTE: " + NoteCMBBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 83);
//GFX3.DrawString("Rif " + RifTXBX.Text, new Font("thaoma", 8), Brushes.Black, 220, 83);

//pictureBox3.Image = drawingsurface3;