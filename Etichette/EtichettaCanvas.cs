using Microsoft.Maui.Graphics.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Pseven.Etichette
{
    public class EtichettaCanvas : ICanvas
    {
        public float DisplayScale { get; set; }
        public float StrokeSize { set => throw new NotImplementedException(); }
        public float MiterLimit { set => throw new NotImplementedException(); }
        public Color StrokeColor { set => throw new NotImplementedException(); }
        public LineCap StrokeLineCap { set => throw new NotImplementedException(); }
        public LineJoin StrokeLineJoin { set => throw new NotImplementedException(); }
        public float[] StrokeDashPattern { set => throw new NotImplementedException(); }
        public float StrokeDashOffset { set => throw new NotImplementedException(); }
        public Color FillColor { set => throw new NotImplementedException(); }
        public Color FontColor { set => throw new NotImplementedException(); }
        public IFont Font { set => throw new NotImplementedException(); }
        public float FontSize { set => throw new NotImplementedException(); }
        public float Alpha { set => throw new NotImplementedException(); }
        public bool Antialias { set => throw new NotImplementedException(); }
        public BlendMode BlendMode { set => throw new NotImplementedException(); }

        public void ClipPath(PathF path, WindingMode windingMode = WindingMode.NonZero)
        {
            throw new NotImplementedException();
        }

        public void ClipRectangle(float x, float y, float width, float height)
        {
            throw new NotImplementedException();
        }

        public void ConcatenateTransform(Matrix3x2 transform)
        {
            throw new NotImplementedException();
        }

        public void DrawArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise, bool closed)
        {
            throw new NotImplementedException();
        }

        public void DrawEllipse(float x, float y, float width, float height)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Microsoft.Maui.Graphics.IImage image, float x, float y, float width, float height)
        {
            throw new NotImplementedException();
        }

        public void DrawLine(float x1, float y1, float x2, float y2)
        {
            throw new NotImplementedException();
        }

        public void DrawPath(PathF path)
        {
            throw new NotImplementedException();
        }

        public void DrawRectangle(float x, float y, float width, float height)
        {
            throw new NotImplementedException();
        }

        public void DrawRoundedRectangle(float x, float y, float width, float height, float cornerRadius)
        {
            throw new NotImplementedException();
        }

        public void DrawString(string value, float x, float y, HorizontalAlignment horizontalAlignment)
        {
            throw new NotImplementedException();
        }

        public void DrawString(string value, float x, float y, float width, float height, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, TextFlow textFlow = TextFlow.ClipBounds, float lineSpacingAdjustment = 0)
        {
            throw new NotImplementedException();
        }

        public void DrawText(IAttributedText value, float x, float y, float width, float height)
        {
            throw new NotImplementedException();
        }

        public void FillArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise)
        {
            throw new NotImplementedException();
        }

        public void FillEllipse(float x, float y, float width, float height)
        {
            throw new NotImplementedException();
        }

        public void FillPath(PathF path, WindingMode windingMode)
        {
            throw new NotImplementedException();
        }

        public void FillRectangle(float x, float y, float width, float height)
        {
            throw new NotImplementedException();
        }

        public void FillRoundedRectangle(float x, float y, float width, float height, float cornerRadius)
        {
            throw new NotImplementedException();
        }

        public SizeF GetStringSize(string value, IFont font, float fontSize)
        {
            throw new NotImplementedException();
        }

        public SizeF GetStringSize(string value, IFont font, float fontSize, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
        {
            throw new NotImplementedException();
        }

        public void ResetState()
        {
            throw new NotImplementedException();
        }

        public bool RestoreState()
        {
            throw new NotImplementedException();
        }

        public void Rotate(float degrees, float x, float y)
        {
            throw new NotImplementedException();
        }

        public void Rotate(float degrees)
        {
            throw new NotImplementedException();
        }

        public void SaveState()
        {
            throw new NotImplementedException();
        }

        public void Scale(float sx, float sy)
        {
            throw new NotImplementedException();
        }

        public void SetFillPaint(Paint paint, RectF rectangle)
        {
            throw new NotImplementedException();
        }

        public void SetShadow(SizeF offset, float blur, Color color)
        {
            throw new NotImplementedException();
        }

        public void SubtractFromClip(float x, float y, float width, float height)
        {
            throw new NotImplementedException();
        }

        public void Translate(float tx, float ty)
        {
            throw new NotImplementedException();
        }
    }
}
