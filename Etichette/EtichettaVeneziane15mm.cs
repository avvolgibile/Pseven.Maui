using Microsoft.Maui.Graphics;
using Pseven.Models;
using ZXing.QrCode.Internal;
using Font = Microsoft.Maui.Graphics.Font;
using ZXing.Common;
using ZXing;
using ZXing.Net.Maui;
using System.Drawing.Text;
using SkiaSharp;
using ZXing.Rendering;
using System.Runtime.InteropServices;
using Microsoft.Maui.Graphics.Skia;
using static Pseven.Etichette.BarcodeGenerator;
using Microsoft.Maui.Controls;
//using Microsoft.UI.Xaml.Controls;
using Microsoft.Maui.Graphics.Platform;
//using static BarcodeGenerator;


namespace Pseven.Etichette
{
    public class EtichettaVeneziane15mm(Etichetta etichetta) : EtichettaDrawBase(etichetta.Larghezza, etichetta.Altezza) ,IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {

            canvas.DrawString($"L {etichetta.LuceLEtichetta}", 5, 22, HorizontalAlignment.Left);


            canvas.DrawImage(BarcodeGenerator.GenerateBarcode("123888"),3,3,100,100);
        }
    }


    public static class BarcodeGenerator
    {
        public static Microsoft.Maui.Graphics.IImage GenerateBarcode(string text, int width = 10, int height = 30)
        {
           
            var encodingOptions = new EncodingOptions
            {
                Width = width,
                Height = height,
                Margin = 1
            };

            // Log or print the EncodingOptions properties
            Console.WriteLine($"EncodingOptions - Width: {encodingOptions.Width}, Height: {encodingOptions.Height}, Margin: {encodingOptions.Margin}");

            var barcodeWriter = new BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.CODE_128,
                Options = encodingOptions
            };


            var pixelData = barcodeWriter.Write(text);
            Console.WriteLine($"PixelData - Width: {pixelData.Width}, Height: {pixelData.Height}, Length: {pixelData.Pixels.Length}");

            var imageInfo = new SKImageInfo(pixelData.Width, pixelData.Height, SKColorType.Gray8);
            using var bitmap = new SKBitmap(imageInfo);

            // Verify the memory allocation
            if (bitmap == null || bitmap.GetPixels() == IntPtr.Zero)
            {
                throw new InvalidOperationException("Failed to allocate memory for SKBitmap.");
            }

            IntPtr ptr = bitmap.GetPixels();
            if (ptr == IntPtr.Zero)
            {
                throw new InvalidOperationException("Failed to get a valid pointer for SKBitmap pixels.");
            }

            if (pixelData.Pixels.Length != imageInfo.Width * imageInfo.Height)
            {
                throw new InvalidOperationException("The size of the pixel data does not match the allocated memory.");
            }
            Marshal.Copy(pixelData.Pixels, 0, ptr, pixelData.Pixels.Length);

            using var skImage = SKImage.FromBitmap(bitmap);
            using var imageStream = new MemoryStream();
            using var skData = skImage.Encode(SKEncodedImageFormat.Png, 50);
            skData.SaveTo(imageStream);
            imageStream.Position = 0;

            return PlatformImage.FromStream(imageStream);
        }
    }
    //Microsoft.Maui.Graphics.






    //     // Crea direttamente un'immagine (IImage) nel Draw
    //            var width = 10;  // Dimensione dell'immagine
    //        var height = 10;

    //            // Usa SkiaBitmapExportContext per creare l'immagine in memoria
    //            using var bitmapExport = new SkiaBitmapExportContext(width, height, 1.0f, 1);
    //    var imageCanvas = bitmapExport.Canvas;


    //            for (int x = 0; x<width; x++)
    //            {
    //                for (int y = 0; y<height; y++)
    //                {
    //                    imageCanvas.FillColor = Colors.Black ;
    //                    imageCanvas.FillRectangle(x, y, 1, 1); // Disegna un pixel
    //                }
    //            }

    //            // L'immagine è ora pronta e puoi disegnarla
    //            var generatedImage = bitmapExport.Image;

    //// Usa canvas.DrawImage per disegnare l'immagine nel contesto del GraphicsView
    //canvas.DrawImage(generatedImage, 0, 0, dirtyRect.Width, dirtyRect.Height);



    public static class ImageGenerator
        {
            public static Task<Microsoft.Maui.Graphics.IImage> CreateImageAsync(int width, int height)
            {
                // Usa SkiaSharp per creare un contesto bitmap esportabile
                using var bitmapExport = new SkiaBitmapExportContext(width, height, 1.0f);
                var canvas = bitmapExport.Canvas;

                // Genera pixel casuali
                var random = new Random();
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        var color = Color.FromRgb(random.Next(256), random.Next(256), random.Next(256));
                        canvas.FillColor = color;
                        canvas.FillRectangle(x, y, 1, 1); // Disegna un singolo pixel
                    }
                }

                // Restituisce l'immagine generata
                return Task.FromResult(bitmapExport.Image);
            }
        }







    



    //public class BarcodeDrawablee : IDrawable
    //{
    //    private readonly string _data;

    //    public BarcodeDrawablee(string data)
    //    {
    //        _data = data;
    //    }

    //    public void Draw(ICanvas canvas, RectF dirtyRect)
    //    {
    //        // Configurazione del generatore di codici a barre
    //        var writer = new BarcodeWriterPixelData
    //        {
    //            Format = ZXing.BarcodeFormat.CODE_128, // Tipo di codice a barre (puoi cambiare formato)
    //            Options = new EncodingOptions
    //            {
    //                Width = (int)dirtyRect.Width,
    //                Height = (int)dirtyRect.Height,
    //                Margin = 10
    //            }
    //        };

    //        // Genera il codice a barre
    //        var pixelData = writer.Write(_data);

    //        // Disegna il codice a barre
    //        for (int y = 0; y < pixelData.Height; y++)
    //        {
    //            for (int x = 0; x < pixelData.Width; x++)
    //            {
    //                var index = y * pixelData.Width + x;
    //                var pixelColor = pixelData.Pixels[index] == 0 ? Colors.Black : Colors.White;
    //                canvas.FillColor = pixelColor;
    //                canvas.FillRectangle(x, y, 1, 1); // Disegna ogni pixel
    //            }
    //        }
    //    }
    //}

    //public class BarcodeDrawableeee : IDrawable
    //{
    //    public void Draw(ICanvas canvas, RectF dirtyRect)
    //    {
    //        // Esempio di dati binari per il codice a barre (1 = nero, 0 = bianco)
    //        string barcodeData = "1010011100101110001";

    //        float barWidth = dirtyRect.Width / barcodeData.Length;
    //        float barHeight = dirtyRect.Height;

    //        for (int i = 0; i < barcodeData.Length; i++)
    //        {
    //            if (barcodeData[i] == '1')
    //            {
    //                // Disegna una barra nera
    //                canvas.FillColor = Colors.Black;
    //                canvas.FillRectangle(i * barWidth, 0, barWidth, barHeight);
    //            }
    //            else
    //            {
    //                // Lascia spazio bianco (opzionale per maggiore chiarezza)
    //                canvas.FillColor = Colors.White;
    //                canvas.FillRectangle(i * barWidth, 0, barWidth, barHeight);
    //            }
    //        }
    //    }
    //}





    //public class BarcodeBrawable : IDrawable
    //{
    //    private string _text;
    //        private BarcodeFormat _format;

    //    public BarcodeDrawable(string text, BarcodeFormat format = BarcodeFormat.CODE_128)
    //    {
    //        _text = text;
    //        _format = format;
    //    }

    //    public void Draw(ICanvas canvas, RectF dirtyRect)
    //    {

    //        var writer = new BarcodeWriterPixelData
    //        {
    //            Format = _format,
    //            Options = new EncodingOptions
    //            {
    //                Width = (int)dirtyRect.Width,
    //                Height = (int)dirtyRect.Height,
    //                Margin = 10
    //            }
    //        };

    //        var pixelData = writer.Write(_text);
    //        var image=new Microsoft.Maui.Graphics.image
    //    }
    //}

}
