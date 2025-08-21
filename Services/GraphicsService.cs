using Pseven.Converters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using Microsoft.Maui.Graphics;
//using System.Drawing;
using System.Drawing.Imaging;
using Size = System.Drawing.Size;
using Microsoft.Maui.Graphics.Skia;
using SkiaSharp;
using System.IO;
using ZXing.Rendering;
using ZXing;


namespace Pseven.Services;

public class GraphicsService
{   
    public static Microsoft.Maui.Graphics.IImage GeneraBarcode(string testo)//questo mostra a video il codice a barre ma nom lo stampa
    {
        var writer = new BarcodeWriterPixelData
        {
            Format = ZXing.BarcodeFormat.CODE_128,
            Options = new ZXing.Common.EncodingOptions
            {
                Width = 100,
                Height = 100,
                Margin = 0
            }
        };
        var pixelData = writer.Write(testo);

        using var skiBitmap = new SKBitmap(pixelData.Width, pixelData.Height, SKColorType.Bgra8888, SKAlphaType.Premul);
        System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, skiBitmap.GetPixels(), pixelData.Pixels.Length);

        using var image = SKImage.FromBitmap(skiBitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var stream = data.AsStream();

        return Microsoft.Maui.Graphics.Platform.PlatformImage.FromStream(stream);
    }

    public static Stream GeneraBarcodeStream(string testo)
    {
        var writer = new ZXing.BarcodeWriterPixelData
        {
            Format = ZXing.BarcodeFormat.CODE_128,
            Options = new ZXing.Common.EncodingOptions
            {
                Width = 200,
                Height = 60,
                Margin = 0
            }
        };

        var pixelData = writer.Write(testo);

        using var bitmap = new SKBitmap(pixelData.Width, pixelData.Height, SKColorType.Bgra8888, SKAlphaType.Premul);
        System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmap.GetPixels(), pixelData.Pixels.Length);
        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);

        var stream = new MemoryStream();
        data.SaveTo(stream);
        stream.Position = 0;

        return stream;
    }

    //Disegna il codice a barre ma non lo stampa
    public static void DrawBarcodePixels(ICanvas canvas, string testo, RectF targetRect)
    {
        var writer = new ZXing.BarcodeWriterPixelData
        {
            Format = ZXing.BarcodeFormat.CODE_128,
            Options = new ZXing.Common.EncodingOptions
            {
                Width = 400,//si allarga in codice a barre
                Height = 20,
                Margin = 0
            }
        };

        var pixelData = writer.Write(testo);

        float scaleX = targetRect.Width / pixelData.Width;
        float scaleY = targetRect.Height / pixelData.Height;

        float offsetX = targetRect.X;
        float offsetY = targetRect.Y;

        for (int py = 0; py < pixelData.Height; py++)
        {
            for (int px = 0; px < pixelData.Width; px++)
            {
                int index = (py * pixelData.Width + px) * 4;
                byte r = pixelData.Pixels[index + 0];
                byte g = pixelData.Pixels[index + 1];
                byte b = pixelData.Pixels[index + 2];
                byte a = pixelData.Pixels[index + 3];

                if (a > 0)
                {
                    canvas.FillColor =Microsoft.Maui.Graphics. Color.FromRgba(r, g, b, a);
                    canvas.FillRectangle(
                        offsetX + px * scaleX,
                        offsetY + py * scaleY,
                        scaleX,
                        scaleY);
                }
            }
        }
    } //Disegna il codice a barre e lo stampa


    public static Microsoft.Maui.Graphics.IImage CreaBarcodeComeIImage(string testo)
    {
        var writer = new ZXing.BarcodeWriterPixelData
        {
            Format = ZXing.BarcodeFormat.CODE_128,
            Options = new ZXing.Common.EncodingOptions
            {
                Width = 200,
                Height = 60,
                Margin = 0
            }
        };

        var pixelData = writer.Write(testo);

        var bitmap = new SKBitmap(pixelData.Width, pixelData.Height, SKColorType.Bgra8888, SKAlphaType.Premul);
        System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmap.GetPixels(), pixelData.Pixels.Length);

        return FromSkBitmap(bitmap);
    }
    public static Microsoft.Maui.Graphics.IImage FromSkBitmap(SKBitmap bitmap)
    {
        // Crea immagine da SKBitmap
        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var stream = new MemoryStream(data.ToArray());

        // Ritorna PlatformImage, che è IImage compatibile con MAUI
        return Microsoft.Maui.Graphics.Platform.PlatformImage.FromStream(stream);
    }
}






