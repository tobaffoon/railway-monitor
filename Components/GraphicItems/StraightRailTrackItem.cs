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

        private static readonly Brush _railTrackBrush = new SolidColorBrush(Color.FromRgb(153, 255, 51));
        private static readonly Pen _railTrackPen = new Pen(_railTrackBrush, 6);
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

        protected override void Render(DrawingContext dc)
        {
            dc.DrawEllipse(_railTrackBrush, _railTrackPen, Start, _circleRadius, _circleRadius);

            if (Status != PlacementStatus.NOT_PLACED)
            {
                // main line
                dc.DrawLine(_railTrackPen, Start, End);

                // second circle
                dc.DrawEllipse(_railTrackBrush, _railTrackPen, End, _circleRadius, _circleRadius);
            }
        }
    }
}
