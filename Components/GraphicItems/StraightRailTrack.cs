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
        private static readonly int RailTrackStrokeThickness = 5;
        private static double _circleRadius = 3;

        // circle is two arcs (semicircle)
        private static Size circleSize = new Size(_circleRadius, _circleRadius);

        public PlacementStatus status { get; set; }

        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }

        public StraightRailTrack()
        {
            status = PlacementStatus.NOT_PLACED;
            X1 = 0;
            Y1 = 0;
            X2 = 0;
            Y2 = 0;
            Stroke = RailTrackBrush;
            StrokeThickness = RailTrackStrokeThickness;
        }

        public void PlaceFirstEnd(Point point)
        {
            X1 = point.X; Y1 = point.Y;
            status = PlacementStatus.PLACEMENT_STARTED;
        }
        public void PlaceSecondEnd(Point point)
        {
            X2 = point.X; Y2 = point.Y;
            status = PlacementStatus.PLACED;
        }
        protected override Geometry DefiningGeometry
        {
            get
            {
                Point p1 = new Point(X1, Y1);
                Point p2 = new Point(X2, Y2);

                PathFigure pf = new PathFigure(p1, new List<PathSegment>(), false);

                // first circle
                Point a1 = new Point(X1 + _circleRadius, Y1);
                Point a2 = new Point(X1 - _circleRadius, Y1);
                pf.Segments.Add(new LineSegment(a1, false));
                pf.Segments.Add(new ArcSegment(a2, circleSize, 0, true, SweepDirection.Clockwise, true));
                pf.Segments.Add(new ArcSegment(a1, circleSize, 0, true, SweepDirection.Clockwise, true));
                pf.Segments.Add(new LineSegment(p1, false));

                // second circle
                a1 = new Point(X2 + _circleRadius, Y2);
                a2 = new Point(X2 - _circleRadius, Y2);
                pf.Segments.Add(new LineSegment(a1, false));
                pf.Segments.Add(new ArcSegment(a2, circleSize, 0, true, SweepDirection.Clockwise, true));
                pf.Segments.Add(new ArcSegment(a1, circleSize, 0, true, SweepDirection.Clockwise, true));
                pf.Segments.Add(new LineSegment(p2, false));

                pf.IsClosed = true;

                Geometry g = new PathGeometry([pf], FillRule.Nonzero, Transform.Identity);
                return g;
            }
        }
    }
}
