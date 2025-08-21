using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using SkiaSharp;
using System.Drawing;
using System.Drawing.Printing;
using SDImage = System.Drawing.Image;

namespace Pseven.Services;

public static class StampaHelper
{
    public static SDImage RenderDrawableToImage(IDrawable drawable, int width, int height)
    {
        using var surface = SKSurface.Create(new SKImageInfo(width, height));
        var canvas = surface.Canvas;
        canvas.Clear(SKColors.White);

        // Adapter Maui canvas to Skia canvas
        var skCanvas = new SkiaCanvas { Canvas = canvas };
        drawable.Draw(skCanvas, new RectF(0, 0, width, height));

        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var stream = new MemoryStream(data.ToArray());
        return SDImage.FromStream(stream);
    }
    public static void StampaImmagine(SDImage image)
    {
        PrintDocument pd = new PrintDocument();
        pd.PrintPage += (sender, e) =>
        {
            e.Graphics.DrawImage(image, new System.Drawing. Point(0, 0));
        };
        pd.Print();
    }
}
