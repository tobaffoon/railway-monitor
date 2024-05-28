using railway_monitor.Bases;
using System.Windows;
using System.Windows.Media;

namespace railway_monitor.Components.GraphicItems
{
    public class StraightRailTrackItem : GraphicItem
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

        public Port PortStart { get; set; }
        public Port PortEnd { get; set; }
        public Point Start
        {
            get => PortStart.Pos;
            set
            {
                PortStart.Pos = value;
            }
        }
        public Point End
        {
            get => PortEnd.Pos;
            set
            {
                PortEnd.Pos = value;
            }
        }
                
        public StraightRailTrackItem() : base()
        {
            Status = PlacementStatus.NOT_PLACED;
            PortStart = new Port(this, new Point(0,0));
            PortEnd = new Port(this, new Point(0,0));
            Stroke = RailTrackBrush;
            Fill = RailTrackBrush;
            StrokeThickness = RailTrackStrokeThickness;
        }

        public override void Move_OnPortMoved(object? sender, Point newPos)
        {
        }

        public override void Reassign_OnPortMerged(object? sender, Port oldPort)
        {
            if(sender == null || sender is not Port port || port.GraphicItems.Contains(this)) return;

            if(oldPort == PortStart)
            {
                PortStart = (Port)sender;
            }
            else
            {
                PortEnd = (Port)sender;
            }
        }

        public void PlaceStartPoint(Point point)
        {
            Start = point;
            Status = PlacementStatus.PLACEMENT_STARTED;
        }
        public void PlaceStartPoint(Port port)
        {
            Start = port.Pos;
            port.Merge(PortStart);
            Status = PlacementStatus.PLACEMENT_STARTED;
        }
        public void PlaceEndPoint(Point point)
        {
            End = point;
            Status = PlacementStatus.PLACED;
        }
        public void PlaceEndPoint(Port port)
        {
            End = port.Pos;
            port.Merge(PortEnd);
            Status = PlacementStatus.PLACED;
        }
        protected override Geometry DefiningGeometry
        {
            get
            {
                PathGeometry g;

                // first circle
                Point a1 = new Point(Start.X + _circleRadius, Start.Y);
                Point a2 = new Point(Start.X - _circleRadius, Start.Y);
                PathFigure circle1 = new PathFigure(a1, [
                    new ArcSegment(a2, circleSize, 0, false, SweepDirection.Clockwise, true),
                    new ArcSegment(a1, circleSize, 0, false, SweepDirection.Clockwise, true)
                    ], false);
                g = new PathGeometry([circle1]);

                if (Status != PlacementStatus.NOT_PLACED)
                {
                    // main line
                    PathFigure mainLine = new PathFigure(Start, [
                        new LineSegment(End, true)
                        ], true);

                    // second circle
                    a1 = new Point(End.X + _circleRadius, End.Y);
                    a2 = new Point(End.X - _circleRadius, End.Y);
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
