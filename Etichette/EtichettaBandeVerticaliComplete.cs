using System;
using Microsoft.Maui.Graphics.Skia;
using Pseven.Models;
using Pseven.Services;
using SkiaSharp;
using ZXing;
using static System.Net.Mime.MediaTypeNames;
using Font = Microsoft.Maui.Graphics.Font;

namespace Pseven.Etichette
{
    public class EtichettaBandeVerticaliComplete(Etichetta etichetta) : EtichettaDrawBase(etichetta)
    {

        protected override void DrawSpecific(ICanvas canvas, RectF dirtyRect)
        {

        }
    }
}
