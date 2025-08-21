using System.Drawing;
using System.Numerics;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Text;

namespace Pseven.Converters
{
    public class GraphicsCanvas : ICanvas
    {
        private readonly Graphics _graphics;

        public float DisplayScale { get; set; }
        public float StrokeSize { get;  set; }
        public float MiterLimit { get; set; }
        public Microsoft.Maui.Graphics.Color StrokeColor { get; set; }
        public LineCap StrokeLineCap { get; set; }
        public LineJoin StrokeLineJoin { get; set; }
        public float[] StrokeDashPattern { get; set; }
        public float StrokeDashOffset { get; set; }
        public Microsoft.Maui.Graphics.Color FillColor { get; set; }
        public Microsoft.Maui.Graphics.Color FontColor { get; set; }
        public IFont Font { get; set; }
        public float FontSize { get; set; }
        public float Alpha { get; set; }
        public bool Antialias { get; set; }
        public BlendMode BlendMode { get; set; }

        public GraphicsCanvas(Graphics graphics)
        {
            _graphics = graphics;
        }

        public void DrawLine(float x1, float y1, float x2, float y2)
        {
            _graphics.DrawLine(Pens.Black, x1, y1, x2, y2);
        }

        public void DrawRectangle(float x, float y, float width, float height)
        {
            _graphics.DrawRectangle(Pens.Black, x, y, width, height);
        }

        public void FillRectangle(float x, float y, float width, float height)
        {
            _graphics.FillRectangle(Brushes.Gray, x, y, width, height);
        }

        public void DrawEllipse(float x, float y, float width, float height)
        {
            _graphics.DrawEllipse(Pens.Black, x, y, width, height);
        }

        public void FillEllipse(float x, float y, float width, float height)
        {
            _graphics.FillEllipse(Brushes.Gray, x, y, width, height);
        }

        public void DrawString(string value, float x, float y, HorizontalAlignment horizontalAlignment)
        {
            var format = new StringFormat();

            switch (horizontalAlignment)
            {
                case HorizontalAlignment.Center:
                    format.Alignment = StringAlignment.Center;
                    break;
                case HorizontalAlignment.Right:
                    format.Alignment = StringAlignment.Far;
                    break;
                default:
                    format.Alignment = StringAlignment.Near;
                    break;
            }

            //_graphics.DrawString(value, System.Drawing.Font, Brushes.Black, x, y, format);
        }
        public void DrawString(string value, System.Drawing.Font font, float x, float y, HorizontalAlignment horizontalAlignment)
        {
            var format = new StringFormat();

            switch (horizontalAlignment)
            {
                case HorizontalAlignment.Center:
                    format.Alignment = StringAlignment.Center;
                    break;
                case HorizontalAlignment.Right:
                    format.Alignment = StringAlignment.Far;
                    break;
                default:
                    format.Alignment = StringAlignment.Near;
                    break;
            }

            _graphics.DrawString(value, font, Brushes.Black, x, y, format);
        }

        public void DrawPath(PathF path)
        {
            throw new NotImplementedException();
        }

        public void FillPath(PathF path, WindingMode windingMode)
        {
            throw new NotImplementedException();
        }

        public void SubtractFromClip(float x, float y, float width, float height)
        {
            throw new NotImplementedException();
        }

        public void ClipPath(PathF path, WindingMode windingMode = WindingMode.NonZero)
        {
            throw new NotImplementedException();
        }

        public void ClipRectangle(float x, float y, float width, float height)
        {
            throw new NotImplementedException();
        }

        public void DrawArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise, bool closed)
        {
            throw new NotImplementedException();
        }

        public void FillArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise)
        {
            throw new NotImplementedException();
        }

        public void DrawRoundedRectangle(float x, float y, float width, float height, float cornerRadius)
        {
            throw new NotImplementedException();
        }

        public void FillRoundedRectangle(float x, float y, float width, float height, float cornerRadius)
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

        public void Rotate(float degrees, float x, float y)
        {
            throw new NotImplementedException();
        }

        public void Rotate(float degrees)
        {
            throw new NotImplementedException();
        }

        public void Scale(float sx, float sy)
        {
            throw new NotImplementedException();
        }

        public void Translate(float tx, float ty)
        {
            throw new NotImplementedException();
        }

        public void ConcatenateTransform(Matrix3x2 transform)
        {
            throw new NotImplementedException();
        }

        public void SaveState()
        {
            throw new NotImplementedException();
        }

        public bool RestoreState()
        {
            throw new NotImplementedException();
        }

        public void ResetState()
        {
            throw new NotImplementedException();
        }

        public void SetShadow(Microsoft.Maui.Graphics.SizeF offset, float blur, Microsoft.Maui.Graphics.Color color)
        {
            throw new NotImplementedException();
        }

        public void SetFillPaint(Paint paint, RectF rectangle)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Microsoft.Maui.Graphics.IImage image, float x, float y, float width, float height)
        {
            throw new NotImplementedException();
        }

        public Microsoft.Maui.Graphics.SizeF GetStringSize(string value, IFont font, float fontSize)
        {
            throw new NotImplementedException();
        }

        public Microsoft.Maui.Graphics.SizeF GetStringSize(string value, IFont font, float fontSize, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
        {
            throw new NotImplementedException();
        }

        // Implementa gli altri metodi di ICanvas...
    }


}
