using Pseven.Models;
using Font = Microsoft.Maui.Graphics.Font;

namespace Pseven.Etichette
{
    public class EtichettaVerticale3(Etichetta etichetta) : EtichettaDrawBase(etichetta.Larghezza, etichetta.Altezza)
    {
        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.Font = new Font("thaoma", 8);
            canvas.DrawString(etichetta.Alias, 5, 9, HorizontalAlignment.Left);

        }
    }
}
