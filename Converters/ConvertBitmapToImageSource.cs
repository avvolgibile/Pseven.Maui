using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pseven.Converters
{
    public class ConvertBitmapToImageSource : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            var bitmap = value as Bitmap;

            //MemoryStream stream = new();
            //bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            //stream.Position = 0;
            //return new StreamImageSource { Stream = token => Task.FromResult<Stream>(stream) };

            using MemoryStream ms = new();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            using SKManagedStream skStream = new SKManagedStream(ms);
            using SKBitmap skBitmap = SKBitmap.Decode(skStream);
            using SKImage image = SKImage.FromBitmap(skBitmap);
            SKData data = image.Encode(SKEncodedImageFormat.Png, 100);
            Stream stream = data.AsStream();
            return ImageSource.FromStream(() => stream);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
