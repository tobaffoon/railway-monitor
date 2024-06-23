using System.Windows;
using System.Windows.Media;

namespace railway_monitor.Bases {
    public abstract class GraphicItem : FrameworkElement {
        protected static readonly Brush brokenBrush = new SolidColorBrush(Color.FromArgb(255, 234, 67, 53));
        protected static readonly Pen brokenPen = new Pen(brokenBrush, 0.5);

        private readonly DrawingGroup drawing = new DrawingGroup();

        protected sealed override void OnRender(DrawingContext drawingContext) {
            base.OnRender(drawingContext);

            Render();
            drawingContext.DrawDrawing(drawing);
        }

        protected abstract void Render(DrawingContext drawingContext);
        public void Render() {
            DrawingContext drawingContext = drawing.Open();
            Render(drawingContext);
            drawingContext.Close();
        }

        public int Id { get; set; } = -1;
    }
}
