using ICSharpCode.AvalonEdit.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace DictationAssistant
{
    public class HighlightedLineBackgroundRenderer : DependencyObject, IBackgroundRenderer
    {
        public HighlightedLineBackgroundRenderer(TextView textView)
        {
            TextView = textView ?? throw new ArgumentNullException(nameof(textView));
            TextView.BackgroundRenderers.Add(this);
        }

        public TextView TextView { get; }

        public KnownLayer Layer => KnownLayer.Background;


        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(HighlightedLineBackgroundRenderer), new PropertyMetadata(PropertyChanged));


        public int LineNumber
        {
            get { return (int)GetValue(LineNumberProperty); }
            set { SetValue(LineNumberProperty, value); }
        }
        public static readonly DependencyProperty LineNumberProperty =
            DependencyProperty.Register("LineNumber", typeof(int), typeof(HighlightedLineBackgroundRenderer), new PropertyMetadata(PropertyChanged));


        public static void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((HighlightedLineBackgroundRenderer)d).TextView.InvalidateLayer(KnownLayer.Background);
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            foreach (var v in textView.VisualLines)
            {
                var rc = BackgroundGeometryBuilder.GetRectsFromVisualSegment(textView, v, 0, 1000).First();
                if (v.FirstDocumentLine.LineNumber == LineNumber)
                {
                    drawingContext.DrawRectangle(Background, null, new Rect(0, rc.Top, textView.ActualWidth, rc.Height));
                }
            }
        }
    }
}
