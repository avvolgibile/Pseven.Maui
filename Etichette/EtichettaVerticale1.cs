using Pseven.Models;
using Pseven.Varie;
using Font = Microsoft.Maui.Graphics.Font;

namespace Pseven.Etichette
{
    public class EtichettaVerticale1(Etichetta etichetta) : EtichettaDrawBase(etichetta)
    {

        protected override void DrawSpecific(ICanvas canvas, RectF dirtyRect)
        {

        //}
        //public override void Draw(ICanvas canvas, RectF dirtyRect)
        //{
            canvas.Font = new Font("thaoma", 8);
            canvas.DrawString(etichetta.Alias, 5, 9, HorizontalAlignment.Left);
            canvas.DrawString($"COL {etichetta.Colore}", 230, 9, HorizontalAlignment.Left);
            canvas.Font = Font.DefaultBold;
            canvas.DrawString($"L {etichetta.LuceLEtichetta}", 5, 22, HorizontalAlignment.Left);
            canvas.DrawString($"H {etichetta.LuceHEtichetta}", 75, 22, HorizontalAlignment.Left);
            canvas.Font = Font.Default;
            canvas.DrawString($"{CalcoliVari.N_bande(etichetta.L, etichetta.AperturaCentrale)}", 5, 58, HorizontalAlignment.Left);
            canvas.DrawString($"({etichetta.MixBandaFinita}) mix banda", 120, 40, HorizontalAlignment.Left);
            canvas.DrawString($"NOTE {etichetta.Note}", 5, 80, HorizontalAlignment.Left);
            canvas.DrawString($"Rif {etichetta.Rif}", 160, 80, HorizontalAlignment.Left);
        }
    }
}
