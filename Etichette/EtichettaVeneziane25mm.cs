using Microsoft.Maui.Graphics.Skia;
using Pseven.Models;
using Pseven.Services;
using SkiaSharp;
using ZXing;
using static System.Net.Mime.MediaTypeNames;
using Font = Microsoft.Maui.Graphics.Font;

namespace Pseven.Etichette
{
    public class EtichettaVeneziane25mm : EtichettaDrawBase
    {

        public EtichettaVeneziane25mm(Etichetta Etichetta):base(Etichetta)
        {
            
        }
        protected override void DrawSpecific(ICanvas canvas, RectF dirtyRect)
        {

           
            //canvas.Font = new Font("thaoma", 8);
            //canvas.DrawString(etichetta.Alias, 5, 9, HorizontalAlignment.Left);
            canvas.DrawString("-25-", 204, 9, HorizontalAlignment.Left);
            canvas.FontColor = Colors.Black;

            // canvas.DrawImage(GraphicsService.GeneraBarcode("123456"), 50, 50, 40, 10);//questo mostra a video il codice a barre ma nom lo stampa
            //////////////////////////////////////////////////////////////////
            canvas.DrawString("1234", 150, 57, HorizontalAlignment.Left);
            var barcodeRect = new RectF(50, 80, 200, 60);
            GraphicsService.DrawBarcodePixels(canvas, "012340000000", barcodeRect);//questo mostra a video il codice a barre e viene anche stampato

            // Altri elementi...
            ///////////////////////////////////////////////////////

            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 4;

            // Coordinata per la L
            float startX = 50;
            float startY = 50;
            float height = 100;
            float width = 60;

            // Linea verticale
            canvas.DrawLine(startX, startY, startX, startY + height);

            // Linea orizzontale alla base
            canvas.DrawLine(startX, startY + height, startX + width, startY + height);

            //////////////////////////////////////////////////////
            //var image = GraphicsService.CreaBarcodeComeIImage("77788999");//disegna il codice a barre ma non lo stampa
            //canvas.DrawImage(image, 5, 4, 200, 60);

            ////////////////////////////////////////////////////////////

            canvas.DrawString($"COL {Etichetta.Colore}", 230, 9, HorizontalAlignment.Left);
            canvas.Font = Font.DefaultBold;
            canvas.DrawString($"L {Etichetta.LuceLEtichetta}", 5, 22, HorizontalAlignment.Left);
            canvas.DrawString($"H {Etichetta.H}", 75, 22, HorizontalAlignment.Left);
            canvas.Font = new Font("thaoma", 8);
            if (Etichetta.Comandi != null && Etichetta.Comandi.Contains("TS"))
            {
                canvas.FillColor = Colors.LightPink;
                canvas.FillRectangle(134, 12, 45, 13);
            }
            canvas.DrawString($"COM {Etichetta.Comandi}", 135, 22, HorizontalAlignment.Left);
            canvas.DrawString($"Hc {Etichetta.Hc}", 80, 40, HorizontalAlignment.Left);
            canvas.DrawString($"Att {Etichetta.Attacchi}", 143, 40, HorizontalAlignment.Left);
            canvas.DrawString(Etichetta.PiuGuide ? "+ Guide" : "SENZA Guide", 5, 40, HorizontalAlignment.Left);
            canvas.DrawString($"*{Math.Round(Etichetta.CalcoloFloat, 1)}*", 7, 57, HorizontalAlignment.Left);
            canvas.Font = Font.DefaultBold;
            canvas.DrawString(Etichetta.Supplemento1, 5, 70, HorizontalAlignment.Left);
            canvas.DrawRectangle(190, 28, 80, 30);
            canvas.Font = new Font("thaoma", 8);
            //if (!etichetta.Supplemento1.Contains("catena") && !etichetta.Supplemento1.Contains("Motor"))
            //{
            //    //canvas.DrawString(etichetta.CordiniVeneziane[0].ToString("0.00"), 195, 41, HorizontalAlignment.Left);
            //    canvas.DrawString(etichetta.CordiniVeneziane[1].ToString("0.00"), 245, 41, HorizontalAlignment.Left);
            //    canvas.Font = Font.DefaultBold;
            //    canvas.DrawString($"Asta  {etichetta.CordiniVeneziane[2]}", 195, 53, HorizontalAlignment.Left);
            //}
            //else
            //{
            //    canvas.DrawString($"{(etichetta.LuceHEtichetta + 30) / 100:0.00} ogni foro", 195, 41, HorizontalAlignment.Left);
            //}
            canvas.Font = new Font("thaoma", 8);
            canvas.DrawString($"NOTE {Etichetta.Note}", 5, 83, HorizontalAlignment.Left);
            canvas.DrawString($"Rif {Etichetta.Rif}", 220, 83, HorizontalAlignment.Left);



            


        }
    }
}
