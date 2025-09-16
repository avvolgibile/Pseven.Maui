using Pseven.Models;
using Pseven.Varie;
using System.Drawing;
using ZXing.QrCode.Internal;
using Font = Microsoft.Maui.Graphics.Font;

namespace Pseven.Etichette
{
    public class EtichettaZanzarieraFissa(Etichetta etichetta) : EtichettaDrawBase(etichetta)
    {
        protected override void DrawSpecific(ICanvas canvas, RectF dirtyRect)
        {

          
            canvas.Font = new Font("thaoma", 8);
            canvas.DrawString(etichetta.Alias, 5, 9, HorizontalAlignment.Left);

        }
    }
}


//GFX.Clear(Color.White);
//GFX.DrawString(Alias.Substring(0, Alias.Length > 30 ? 30 : Alias.Length), new Font("thaoma", 8), Brushes.Black, 5, 0);
//GFX.DrawString("Z.Fissa", new Font("thaoma", 8), Brushes.Black, 210, 0);

//GFX.DrawString("COL " + ColoreCMBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 22);
//GFX.DrawString("L " + luceLperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 5, 40);
//GFX.DrawString("H " + luceHperEtich, new Font("thaoma", 8, FontStyle.Bold), Brushes.Black, 75, 40);

//if (Supplemento1CMBX.Text != "")
//{
//    GFX.FillRectangle(Brushes.Gray, 5, 67, 125, 16);
//    GFX.DrawString(Supplemento1CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.White, 5, 68);
//}

//if (Supplemento2CMBX.Text != "")
//{
//    GFX.FillRectangle(Brushes.Gray, 155, 67, 170, 16);
//    GFX.DrawString(Supplemento2CMBX.Text, new Font("thaoma", 8, FontStyle.Bold), Brushes.White, 155, 68);
//}

//GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioProfiloSuperiore.ToString("0000")), 10), PartenzaBarcodeDX, 13);
//GFX.DrawString(mixTaglioProfiloSuperiore.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 21);
//GFX.DrawString("L", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 21);
//GFX.DrawImage(barcode.Draw(string.Format(barcodeStruct, mixTaglioProfilo.ToString("0000")), 10), PartenzaBarcodeDX, 33);
//GFX.DrawString(mixTaglioProfilo.ToString(), new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 70, 41);
//GFX.DrawString("H", new Font("thaoma", 7), Brushes.Black, PartenzaBarcodeDX + 14, 41);
//GFX.DrawString("-Vista frontale-", new Font("thaoma", 8), Brushes.Black, 218, 53);

//GFX.DrawString("NOTE: " + NoteCMBBX.Text, new Font("thaoma", 8), Brushes.Black, 5, 83);
//GFX.DrawString("Rif " + RifTXBX.Text, new Font("thaoma", 8), Brushes.Black, 220, 83);

//pictureBox1.Image = drawingsurface;