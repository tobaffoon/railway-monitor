using railway_monitor.Bases;
using System.Windows;
using System.Windows.Media;

namespace railway_monitor.Components.GraphicItems {
    public class StraightRailTrackItem : GraphicItem {
        public enum RailPlacementStatus {
            NOT_PLACED,
            PLACEMENT_STARTED,
            PLACED
        }

        private static readonly Brush _railTrackBrush = new SolidColorBrush(Color.FromRgb(153, 255, 51));
        private static readonly Pen _railTrackPen = new Pen(_railTrackBrush, 6);
        private static readonly double _circleRadius = 4.21;

        private static Size circleSize = new Size(_circleRadius, _circleRadius);

        public RailPlacementStatus PlacementStatus { get; private set; }

        public Port PortStart { get; set; }
        public Port PortEnd { get; set; }
        public Point Start {
            get => PortStart.Pos;
            set {
                PortStart.Pos.X = value.X;
                PortStart.Pos.Y = value.Y;
                Render();
            }
        }
        public Point End {
            get => PortEnd.Pos;
            set {
                PortEnd.Pos.X = value.X;
                PortEnd.Pos.Y = value.Y;
                Render();
            }
        }

        public StraightRailTrackItem(Point initPos) : base() {
            PlacementStatus = RailPlacementStatus.NOT_PLACED;
            PortStart = new Port(this, initPos);
            PortEnd = new Port(this, initPos);
        }

        public Port GetOtherPort(Port wrongPort) {
            if (wrongPort == PortStart) return PortEnd;
            if (wrongPort == PortEnd) return PortStart;
            else throw new ArgumentException("SRT got wrong port: " + wrongPort + ". SRT only had: " + PortStart + " and " + PortEnd);
        }

        public override void Reassign_OnPortMerged(object? sender, Port oldPort) {
            if (sender == null || sender is not Port port || port.GraphicItems.Contains(this)) return;

            if (oldPort == PortStart) {
                PortStart = (Port)sender;
            }
            else {
                PortEnd = (Port)sender;
            }
        }

        public void PlaceStartPoint(Point point) {
            Start = point;
            PlacementStatus = RailPlacementStatus.PLACEMENT_STARTED;
        }
        public void PlaceStartPoint(Port port) {
            Start = port.Pos;
            port.Merge(PortStart);
            PlacementStatus = RailPlacementStatus.PLACEMENT_STARTED;
        }
        public void PlaceEndPoint(Point point) {
            End = point;
            PlacementStatus = RailPlacementStatus.PLACED;
        }
        public void PlaceEndPoint(Port port) {
            End = port.Pos;
            port.Merge(PortEnd);
            PlacementStatus = RailPlacementStatus.PLACED;
        }

        protected override void Render(DrawingContext dc) {
            dc.DrawEllipse(_railTrackBrush, _railTrackPen, Start, _circleRadius, _circleRadius);

            if (PlacementStatus != RailPlacementStatus.NOT_PLACED) {
                // main line
                dc.DrawLine(_railTrackPen, Start, End);

                // second circle
                dc.DrawEllipse(_railTrackBrush, _railTrackPen, End, _circleRadius, _circleRadius);
            }
        }
    }
}
