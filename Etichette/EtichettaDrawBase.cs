using Microsoft.Maui.Graphics.Skia;
using Pseven.Models;
using SkiaSharp;
//using System.Drawing;
using Microsoft.Maui.Graphics;

namespace Pseven.Etichette
{
    /// <summary>
    /// Classe base con Template Method:
    /// Draw() NON si override-a. I derivati implementano solo DrawSpecific().
    /// </summary>
    public abstract class EtichettaDrawBase : IDrawable, IDisposable
    {
        private SkiaBitmapExportContext? _bmp;
        private bool disposedValue;


        protected Etichetta Etichetta { get; }   // proprietà protetta, leggibile dai derivati

      
        protected EtichettaDrawBase(Etichetta etichetta)
        {
            Etichetta = etichetta ?? throw new ArgumentNullException(nameof(etichetta));
        }


        //public virtual void Draw(ICanvas canvas, RectF dirtyRect)
        //{
        //}
        //GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG

        // === TEMPLATE METHOD: i derivati NON sovrascrivono Draw ===
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();
            DrawCommonBefore(canvas);      // tutto il codice comune (header, L, font, ecc.)
            DrawSpecific(canvas, dirtyRect);          // SOLO le differenze (override nei derivati)
            DrawCommonAfter(canvas);       // eventuali elementi comuni di coda
            canvas.RestoreState();
        }

        // --- COMUNE (puoi metterci quello che ripeti sempre) ---
        protected virtual void DrawCommonBefore(ICanvas canvas)
        {


            canvas.Font = new Microsoft.Maui.Graphics.Font("thaoma", 8);
            canvas.DrawString(Etichetta.Alias, 5, 9, HorizontalAlignment.Left);

            // Esempi di comune che vedo nelle tue funzioni
            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 4;

            // “L” iniziale (coordinate standard)
            float startX = 50, startY = 50, width = 60, height = 100;
            canvas.DrawLine(startX, startY, startX, startY + height);
            canvas.DrawLine(startX, startY + height, startX + width, startY + height);
        }

        protected virtual void DrawCommonAfter(ICanvas canvas)
        {
            // Se c'è qualcosa di ricorrente “di coda”, mettilo qui.
        }

        // --- HOOK astratto: ogni etichetta disegna solo le sue differenze ---
        protected abstract void DrawSpecific(ICanvas canvas, RectF dirtyRect);

        // === Helper riusabili(li chiami sia da base sia dai derivati) ===
       
        
        protected void DrawText(ICanvas c, string text, float x, float y,
                                float size = 10, bool bold = false,
                                HorizontalAlignment align = HorizontalAlignment.Left)
        {
            c.Font = bold ? Microsoft.Maui.Graphics.Font.DefaultBold : Microsoft.Maui.Graphics.Font.Default;
            c.DrawString(text, x, y, align);
        }

        //protected void FillBadge(Microsoft.Maui.Graphics.ICanvas c, Microsoft.Maui.Graphics.Color color, float x, float y, float w, float h)
        // {
        //     var old = c.FillColor;
        //     c.FillColor = color;
        //     c.FillRectangle(x, y, w, h);
        //     c.FillColor = old;
        // }

        //protected void DrawBarcode(ICanvas c, IImage? image, float x, float y, float w, float h)
        //{
        //    if (image is not null) c.DrawImage(image, x, y, w, h);
        //}

        // === Il resto della tua classe rimane com’è ===
        // ToBitmap(), Dispose(), ecc.
        // ...















        //GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG
        public System.Drawing.Bitmap ToBitmap()
        {
            _bmp = new(330, 135, 1.0f);
            Draw(_bmp.Canvas, new RectF(0, 0, 330, 135));
            using var image = SKImage.FromBitmap(_bmp.Bitmap);
            using var stream = image.Encode(SKEncodedImageFormat.Png, 100).AsStream();
            var bitmap = new System.Drawing.Bitmap(stream);
            return bitmap;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _bmp?.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
