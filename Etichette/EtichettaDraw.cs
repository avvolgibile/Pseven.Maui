using Pseven.Models;
using Font = Microsoft.Maui.Graphics.Font;

namespace Pseven.Etichette
{
    public class EtichettaDraw(Etichetta etichetta) : EtichettaDrawBase(etichetta.Larghezza, etichetta.Altezza)
    {
        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {

        }
    }
}
