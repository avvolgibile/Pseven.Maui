using Pseven.Models;
using Font = Microsoft.Maui.Graphics.Font;

namespace Pseven.Etichette
{
    public class EtichettaDraw(Etichetta etichetta) : EtichettaDrawBase(etichetta)
    {
       

        protected override void DrawSpecific(ICanvas canvas, RectF dirtyRect)
        {

        }
    }
}
