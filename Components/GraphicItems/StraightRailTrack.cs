using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace railway_monitor.Components.GraphicItems
{
    public class StraightRailTrack : Shape
    {
        public enum PlacementStatus
        {
            NOT_PLACED,
            PLACEMENT_STARTED,
            PLACED
        }

        private static readonly Brush RailTrackBrush = new SolidColorBrush(Color.FromRgb(153, 255, 51));
        private static readonly int RailTrackStrokeThickness = 6;
        private static readonly double _circleRadius = 4.21;

        // circle is two arcs (semicircle)
        private static Size circleSize = new Size(_circleRadius, _circleRadius);

        public PlacementStatus Status { get; set; }

        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }

        public StraightRailTrack()
        {
            Status = PlacementStatus.NOT_PLACED;
            X1 = 0;
            Y1 = 0;
            X2 = 0;
            Y2 = 0;
            Stroke = RailTrackBrush;
            Fill = RailTrackBrush;
            StrokeThickness = RailTrackStrokeThickness;
            StrokeMiterLimit = 2.4;
        }

        public void PlaceFirstEnd(Point point)
        {
            X1 = point.X; Y1 = point.Y;
            Status = PlacementStatus.PLACEMENT_STARTED;
        }
        public void PlaceSecondEnd(Point point)
        {
            X2 = point.X; Y2 = point.Y;
            Status = PlacementStatus.PLACED;
        }
        protected override Geometry DefiningGeometry
        {
            get
            {
                PathGeometry g;
                Point p1 = new Point(X1, Y1);
                Point p2 = new Point(X2, Y2);

                // first circle
                Point a1 = new Point(X1 + _circleRadius, Y1);
                Point a2 = new Point(X1 - _circleRadius, Y1);
                PathFigure circle1 = new PathFigure(a1, [
                    new ArcSegment(a2, circleSize, 0, false, SweepDirection.Clockwise, true),
                    new ArcSegment(a1, circleSize, 0, false, SweepDirection.Clockwise, true)
                    ], false);
                g = new PathGeometry([circle1], FillRule.Nonzero, Transform.Identity);

                if (Status != PlacementStatus.NOT_PLACED)
                {
                    // main line
                    PathFigure mainLine = new PathFigure(p1, [
                        new LineSegment(p2, true)
                        ], true);

                    // second circle
                    a1 = new Point(X2 + _circleRadius, Y2);
                    a2 = new Point(X2 - _circleRadius, Y2);
                    PathFigure circle2 = new PathFigure(a1, [
                        new ArcSegment(a2, circleSize, 0, false, SweepDirection.Clockwise, true),
                        new ArcSegment(a1, circleSize, 0, false, SweepDirection.Clockwise, true)
                        ], false);

                    g.Figures.Add(circle2);
                    g.Figures.Add(mainLine);
                }
                return g;
            }
        }
    }
}
