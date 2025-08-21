using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.Common;
using ZXing;
using Microsoft.Maui.Graphics;
using ZXing.Rendering;
using SkiaSharp;
using Microsoft.Maui.Graphics.Skia;
using Microsoft.Maui.Graphics.Platform;


namespace Pseven.Etichette
{
    public class DrawL : IDrawable
    {
        public DrawL()
        {
        }
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            var writer = new BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 300,
                    Height = 100,
                    Margin = 0
                }
            };
            var pixelData = writer.Write("666666");

            using var skBitmap = new SKBitmap(pixelData.Width, pixelData.Height, SKColorType.Bgra8888, SKAlphaType.Premul);
            System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, skBitmap.GetPixels(), pixelData.Pixels.Length);

            using var image = SKImage.FromBitmap(skBitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            using var stream = data.AsStream();

            var mauiImage = Microsoft.Maui.Graphics.Platform.PlatformImage.FromStream(stream);
            canvas.DrawImage(mauiImage, 0, 0, pixelData.Width, pixelData.Height);

        }
    }

    //-----------------------------------------------------------------------

    public class CustomDrawable : IDrawable
    {
        public List<(string Text, float X, float Y, float Size, Color Color)> Labels { get; set; } = new();
        public List<(string BarcodeValue, float X, float Y, float Width, float Height)> Barcodes { get; set; } = new();

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FontColor = Colors.Black;

            // Disegna le stringhe nelle coordinate specificate
            foreach (var label in Labels)
            {
                canvas.FontSize = label.Size;
                canvas.FontColor = label.Color;
                canvas.DrawString(label.Text, label.X, label.Y, HorizontalAlignment.Left);
            }

            // Disegna i codici a barre nelle coordinate specificate
            foreach (var barcode in Barcodes)
            {
                var barcodeImage = GenerateBarcode(barcode.BarcodeValue, (int)barcode.Width, (int)barcode.Height);

                //barcodeImage  CreateEmptyImage();

                    canvas.DrawImage(barcodeImage, barcode.X, barcode.Y, barcode.Width, barcode.Height);
               // }
            }
        }

        //var barcodeimage = GenerateBarcode("9999", 300, 300);
        //Variaglob imageView = new Image { Source=ImageSource.FromStream(()=>barc)}
        public static Microsoft.Maui.Graphics.IImage? GenerateBarcode(string value, int width, int height)
        {
            try
            {
                var writer = new BarcodeWriterPixelData
                {
                    Format = BarcodeFormat.CODE_128,
                    Options = new EncodingOptions { Width = width, Height = height, Margin=10}
                };
                var pixeldata = writer.Write(value);


                using var bitmap = new SKBitmap(new SKImageInfo(pixeldata.Width, pixeldata.Height, SKColorType.Gray8));
                IntPtr ptr = bitmap.GetPixels();

                System.Runtime.InteropServices.Marshal.Copy(pixeldata.Pixels, 0, ptr, pixeldata.Pixels.Length);

                    
                 using var skImage = SKImage.FromBitmap(bitmap);
                using var imageStream = new MemoryStream();
                using var skData = skImage.Encode(SKEncodedImageFormat.Png, 100);
                skData.SaveTo(imageStream);
                imageStream.Position = 0;

                return PlatformImage.FromStream(imageStream);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Errore nella generazione del barcode: {ex.Message}");
                return null;
            }
        }
    }

    //_________________________________________________________________________


//    public class BarcodeDrawable : IDrawable
//    {
//        public string BarcodeText { get; set; } = "123456789"; // Valore di default

//        public void Draw(ICanvas canvas, RectF dirtyRect)
//        {
//            if (string.IsNullOrWhiteSpace(BarcodeText)) return;

//            // Configura il generatore del codice a barre
//            var writer = new BarcodeWriterPixelData
//            {
//                Format = BarcodeFormat.CODE_128, // Puoi cambiare il tipo di codice a barre
//                Options = new EncodingOptions
//                {
//                    Height = (int)dirtyRect.Height,
//                    Width = (int)dirtyRect.Width,
//                    Margin = 10
//                }
//            };

//            //// Genera il codice a barre
//            //var pixelData = writer.Write(BarcodeText);

//            //// Disegna l'immagine nel GraphicsView
//            //using var bitmap = new Microsoft.Maui.Graphics.Platform.PlatformBitmap(pixelData.Width, pixelData.Height);
//            //bitmap.SetPixels(pixelData.Pixels);

//            //canvas.DrawImage(bitmap, dirtyRect);
//        }
//    }
}