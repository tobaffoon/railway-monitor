using railway_monitor.Bases;
using railway_monitor.Utils;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace railway_monitor.Components.GraphicItems {
    public class StraightRailTrackItem : GraphicItem {
        public enum RailPlacementStatus {
            NOT_PLACED,
            PLACEMENT_STARTED,
            PLACED
        }
        public enum RailPlatformType {
            PASSENGER,
            PASSENGER_HOVER,
            CARGO,
            CARGO_HOVER,
            NONE
        }

        private static readonly Brush _railTrackBrush = new SolidColorBrush(Color.FromRgb(153, 255, 51));
        private static readonly Pen _railTrackPen = new Pen(_railTrackBrush, 6);
        private static readonly Brush _passengerTrackBrush = new SolidColorBrush(Color.FromRgb(185, 111, 92));
        private static readonly Pen _passengerTrackPen = new Pen(_passengerTrackBrush, 0);
        private static readonly Brush _passengerHoverBrush = new SolidColorBrush(Color.FromArgb(100, 185, 111, 92));
        private static readonly Pen _passengerHoverPen = new Pen(_passengerHoverBrush, 0);
        private static readonly Brush _cargoTrackBrush = new SolidColorBrush(Color.FromRgb(112, 146, 189));
        private static readonly Pen _cargoTrackPen = new Pen(_cargoTrackBrush, 0);
        private static readonly Brush _cargoHoverBrush = new SolidColorBrush(Color.FromArgb(100, 112, 146, 189));
        private static readonly Pen _cargoHoverPen = new Pen(_cargoHoverBrush, 0);

        #region Draw params
        private static readonly double _circleRadius = 4.21;
        private static readonly double _platformOffset = 10; // must be bigger than circle radius
        private static readonly double _platformTiltAngle = 1.048;  // radians = 60 deg
        private static readonly double _platformSideLength = 11;
        #endregion
        private static readonly double _minDrawablePlatformLength = 2 * _platformOffset + 2 * _platformSideLength * Math.Cos(_platformTiltAngle);

        private static Size circleSize = new Size(_circleRadius, _circleRadius);

        public RailPlacementStatus PlacementStatus { get; private set; }
        
        private RailPlatformType _platformType;
        public RailPlatformType PlatformType {
            get {
                return _platformType;
            }
            set { 
                _platformType = value;
                Render();
            }
        }

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
        public double Length {
            get {
                return GraphicCalc.GetDistance(Start, End);
            }
        }

        private Point _center = new Point(0, 0);
        public Point Center {
            get {
                GraphicCalc.GetPointInDirection(ref _center, Start, End, Length / 2);
                return _center;
            }
        }

        #region Drawing points 
        private Point _platfromCornerOne = new Point(0, 0);
        private Point PlatfromCornerOne {
            get {
                GraphicCalc.GetPointInDirection(ref _platfromCornerOne, End, Start, _platformOffset);
                return _platfromCornerOne;
            }
        }

        private Point _platfromCornerTwo = new Point(0, 0);
        private Point PlatfromCornerTwo {
            get {
                GraphicCalc.GetPointInDirection(ref _platfromCornerTwo, Start, End, _platformOffset);
                return _platfromCornerTwo;
            }
        } 

        private Point _platfromCornerThree = new Point(0, 0);
        private Point PlatfromCornerThree {
            get {
                if (Start.X < End.X) {
                    GraphicCalc.GetPointInDirection(ref _platfromCornerThree, _platfromCornerTwo, End, _platformSideLength, -_platformTiltAngle);
                }
                else {
                    GraphicCalc.GetPointInDirection(ref _platfromCornerThree, _platfromCornerTwo, End, _platformSideLength, _platformTiltAngle);
                }
                return _platfromCornerThree;
            }
        } 

        private Point _platfromCornerFour = new Point(0, 0);
        private Point PlatfromCornerFour {
            get {
                if (Start.X < End.X) {
                    GraphicCalc.GetPointInDirection(ref _platfromCornerFour, _platfromCornerOne, Start, _platformSideLength, _platformTiltAngle);
                }
                else {
                    GraphicCalc.GetPointInDirection(ref _platfromCornerFour, _platfromCornerOne, Start, _platformSideLength, -_platformTiltAngle);
                }
                return _platfromCornerFour;
            }
        }
        #endregion

        public StraightRailTrackItem(Point initPos) : base() {
            PortStart = new Port(this, initPos);
            PortEnd = new Port(this, initPos);
            PlacementStatus = RailPlacementStatus.NOT_PLACED;
            PlatformType = RailPlatformType.NONE;
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
            // first circle
            dc.DrawEllipse(_railTrackBrush, _railTrackPen, Start, _circleRadius, _circleRadius);

            if (PlacementStatus != RailPlacementStatus.NOT_PLACED) {
                if (Length >= _minDrawablePlatformLength) {
                    // platform. NOTE: it's drawn before other parts to avoid boresome calculations of main line edge (it is replace with simple wide pen)
                    PathFigure platform = new PathFigure(PlatfromCornerOne, [
                        new LineSegment(PlatfromCornerTwo, true),
                        new LineSegment(PlatfromCornerThree, true),
                        new LineSegment(PlatfromCornerFour, true),
                    ], true);
                    PathGeometry platformGeometry = new PathGeometry([platform]);
                    switch (PlatformType) {
                        case RailPlatformType.PASSENGER:
                            dc.DrawGeometry(_passengerTrackBrush, _passengerTrackPen, platformGeometry);
                            break;
                        case RailPlatformType.PASSENGER_HOVER:
                            dc.DrawGeometry(_passengerHoverBrush, _passengerHoverPen, platformGeometry);
                            break;
                        case RailPlatformType.CARGO:
                            dc.DrawGeometry(_cargoTrackBrush, _cargoTrackPen, platformGeometry);
                            break;
                        case RailPlatformType.CARGO_HOVER:
                            dc.DrawGeometry(_cargoHoverBrush, _cargoHoverPen, platformGeometry);
                            break;
                    }
                }

                // main line
                dc.DrawLine(_railTrackPen, Start, End);

                // second circle
                dc.DrawEllipse(_railTrackBrush, _railTrackPen, End, _circleRadius, _circleRadius);
            }
        }
    }
}
