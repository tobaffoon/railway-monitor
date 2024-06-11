using railway_monitor.Bases;
using System.Windows.Media;
using System.Windows;

namespace railway_monitor.Components.GraphicItems
{
    public class HighlightConnection : GraphicItem
    {
        public static readonly double ConnectRadius = 15;
        private static readonly Brush _highlightNormalBrush = new SolidColorBrush(Color.FromArgb(100, 51, 153, 255));
        private static readonly Pen _highlightNormalPen = new Pen(_highlightNormalBrush, 0);
        private static readonly Brush _highlightErrorBrush = new SolidColorBrush(Color.FromArgb(100, 230, 20, 20));
        private static readonly Pen _highlightErrorPen = new Pen(_highlightErrorBrush, 0);

        public Point Pos = new Point(0, 0);

        public HighlightConnection() : base()
        {
            Visibility = Visibility.Collapsed;
        }

        public bool ConnectionErrorOccured = false;
        public override void Reassign_OnPortMerged(object? sender, Port oldPort)
        {
            throw new NotImplementedException();
        }

        protected override void Render(DrawingContext dc)
        {
            if (Visibility != Visibility.Visible) return;

            if(ConnectionErrorOccured)
            {
                dc.DrawEllipse(_highlightErrorBrush, _highlightErrorPen, Pos, ConnectRadius, ConnectRadius);
            }
            else
            {
                dc.DrawEllipse(_highlightNormalBrush, _highlightNormalPen, Pos, ConnectRadius, ConnectRadius);
            }
        }
    }
}
