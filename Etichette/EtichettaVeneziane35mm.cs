using Pseven.Models;
using Pseven.Varie;
using Font = Microsoft.Maui.Graphics.Font;

namespace Pseven.Etichette
{
    public class EtichettaVeneziane35mm(Etichetta etichetta) : EtichettaDrawBase(etichetta.Larghezza, etichetta.Altezza)
    {
        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.Font = new Font("thaoma", 8);
            canvas.DrawString(etichetta.Alias, 5, 9, HorizontalAlignment.Left);
            canvas.FillColor = Colors.Gray;
            canvas.FillRectangle(201, -2, 25, 14);
            canvas.FontColor = Colors.White;
            canvas.DrawString("-35-", 204, 9, HorizontalAlignment.Left);
            canvas.FontColor = Colors.Black;
            canvas.DrawString($"COL {etichetta.Colore}", 230, 9, HorizontalAlignment.Left);
            canvas.Font = Font.DefaultBold;
            canvas.DrawString($"L {etichetta.LuceLEtichetta}", 5, 22, HorizontalAlignment.Left);
            canvas.DrawString($"H {etichetta.H}", 75, 22, HorizontalAlignment.Left);
            canvas.Font = new Font("thaoma", 8);
            if (etichetta.Comandi != null && etichetta.Comandi.Contains("TD"))
            {
                canvas.FillColor = Colors.LightSalmon;
                canvas.FillRectangle(134, 12, 45, 13);
            }
            canvas.DrawString($"COM {etichetta.Comandi}", 135, 22, HorizontalAlignment.Left);
            canvas.DrawString($"Hc {etichetta.Hc}", 80, 40, HorizontalAlignment.Left);
            canvas.DrawString($"Att {etichetta.Attacchi}", 143, 40, HorizontalAlignment.Left);
            if(etichetta.PiuGuide)
            {
                canvas.DrawString("+ Guide", 5, 40, HorizontalAlignment.Left);
            }
            else
            {
                canvas.FillColor = Colors.LightGray;
                canvas.FillRectangle(5, 30, 70, 13);
                canvas.DrawString("SENZA Guide", 5, 40, HorizontalAlignment.Left);
            }
            canvas.DrawString($"*{Math.Round(etichetta.CalcoloFloat, 1)}*", 7, 57, HorizontalAlignment.Left);
            canvas.Font = Font.DefaultBold;
            canvas.DrawString(CalcoliVari.Nylon35_50(etichetta.H.ToString(), etichetta.PiuGuide), 155, 58, HorizontalAlignment.Left);
            canvas.DrawRectangle(210, 28, 90, 30);
            canvas.Font = new Font("thaoma", 8);
            canvas.DrawString(etichetta.CordiniVeneziane[0].ToString("0.00"), 220, 41, HorizontalAlignment.Left);
            canvas.DrawString(etichetta.CordiniVeneziane[1].ToString("0.00"), 265, 41, HorizontalAlignment.Left);
            canvas.Font = Font.DefaultBold;
            canvas.DrawString($"Orient.  {etichetta.CordiniVeneziane[2]:0.00}", 220, 53, HorizontalAlignment.Left);
            canvas.Font = new Font("thaoma", 8);
            canvas.DrawString($"NOTE {etichetta.Note}", 5, 83, HorizontalAlignment.Left);
            canvas.DrawString($"Rif {etichetta.Rif}", 220, 83, HorizontalAlignment.Left);
        }
    }
}
