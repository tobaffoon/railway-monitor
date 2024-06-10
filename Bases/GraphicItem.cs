using System.Windows;
using System.Windows.Media;

namespace railway_monitor.Bases
{
    public abstract class GraphicItem : FrameworkElement
    {
        private readonly DrawingGroup drawing = new DrawingGroup();

        protected sealed override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            Render();
            drawingContext.DrawDrawing(drawing);
        }

        protected abstract void Render(DrawingContext drawingContext);
        public void Render()
        {
            DrawingContext drawingContext = drawing.Open();
            Render(drawingContext);
            drawingContext.Close();
        }

        public abstract void Move_OnPortMoved(object? sender, Point newPos);
        public abstract void Reassign_OnPortMerged(object? sender, Port oldPort);
    }
}
