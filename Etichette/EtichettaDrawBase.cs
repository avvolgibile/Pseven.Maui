using Microsoft.Maui.Graphics.Skia;
using Pseven.Models;
using SkiaSharp;
using System.Drawing;

namespace Pseven.Etichette
{
    public class EtichettaDrawBase(int width, int height) : IDrawable, IDisposable
    {
        private SkiaBitmapExportContext? _bmp;
        private bool disposedValue;

        public virtual void Draw(ICanvas canvas, RectF dirtyRect)
        {
        }

        public Bitmap ToBitmap()
        {
            _bmp = new(width, height, 1.0f);
            Draw(_bmp.Canvas, new RectF(0, 0, width, height));
            using var image = SKImage.FromBitmap(_bmp.Bitmap);
            using var stream = image.Encode(SKEncodedImageFormat.Png, 100).AsStream();
            var bitmap = new Bitmap(stream);
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
