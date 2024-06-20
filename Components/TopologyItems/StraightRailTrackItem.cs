using railway_monitor.Bases;
using railway_monitor.Components.GraphicItems;
using railway_monitor.Utils;
using System.Windows;
using System.Windows.Media;

namespace railway_monitor.Components.TopologyItems {
    public class StraightRailTrackItem : TopologyItem {
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
            NONE,
            NONE_HOVER
        }

        private static readonly double defaultLength = 1;
        #region Main line params
        private static readonly double _railWidth = 6;
        private static readonly double _circleRadius = 4.21;
        private static readonly double _platformOffset = 10; // must be bigger than circle radius
        private static readonly double _platformTiltAngle = 1.048;  // radians = 60 deg
        private static readonly double _platformSideLength = 11;
        #endregion

        private static readonly Brush _railTrackBrush = new SolidColorBrush(Color.FromRgb(153, 255, 51));
        private static readonly Pen _railTrackPen = new Pen(_railTrackBrush, _railWidth);
        private static readonly Brush _railArrowBrush = new SolidColorBrush(Colors.Black);
        private static readonly Pen _railArrowPen = new Pen(_railArrowBrush, 1);
        private static readonly Brush _passengerTrackBrush = new SolidColorBrush(Color.FromRgb(185, 111, 92));
        private static readonly Pen _passengerTrackPen = new Pen(_passengerTrackBrush, 0);
        private static readonly Brush _passengerHoverBrush = new SolidColorBrush(Color.FromArgb(100, 185, 111, 92));
        private static readonly Pen _passengerHoverPen = new Pen(_passengerHoverBrush, 0);
        private static readonly Brush _cargoTrackBrush = new SolidColorBrush(Color.FromRgb(112, 146, 189));
        private static readonly Pen _cargoTrackPen = new Pen(_cargoTrackBrush, 0);
        private static readonly Brush _cargoHoverBrush = new SolidColorBrush(Color.FromArgb(100, 112, 146, 189));
        private static readonly Pen _cargoHoverPen = new Pen(_cargoHoverBrush, 0);
        private static readonly Brush _noneHoverBrush = new SolidColorBrush(Color.FromArgb(100, 235, 220, 185));
        private static readonly Pen _noneHoverPen = new Pen(_noneHoverBrush, 0);
        private static readonly Pen _brokenRailTrackPen = new Pen(brokenBrush, _railWidth);

        static StraightRailTrackItem() {
            _railArrowPen.StartLineCap = PenLineCap.Round;
            _railArrowPen.EndLineCap = PenLineCap.Round;
        }

        #region Arrow params
        private static readonly double _arrowLength = 10.0;
        private static readonly double _arrowTipsLength = 4.0;
        private static readonly double _arrowTipsAngle = 0.524;  // radians = 30 deg
        #endregion

        private static readonly double _minDrawableLength = Math.Min(
            2 * _platformOffset + 2 * _platformSideLength * Math.Cos(_platformTiltAngle),
            _arrowLength);

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
        public Port MovementPortStart {
            get {
                if (StartsFromStart) {
                    return PortStart;
                }
                else {
                    return PortEnd;
                }
            }
        }
        public Port MovementPortEnd {
            get {
                if (StartsFromStart) {
                    return PortEnd;
                }
                else {
                    return PortStart;
                }
            }
        }
        public Point MovementStart {
            get {
                if (StartsFromStart) {
                    return Start;
                }
                else {
                    return End;
                }
            }
        }
        public Point MovementEnd {
            get {
                if (StartsFromStart) {
                    return End;
                }
                else {
                    return Start;
                }
            }
        }
        public double GraphicLength {
            get {
                return GraphicCalc.GetDistance(Start, End);
            }
        }

        private bool _startsFromStart = true;
        public bool StartsFromStart {
            get {
                return _startsFromStart;
            }
            set {
                _startsFromStart = value;
                Render();
            }
        }

        private Point _center = new Point(0, 0);
        public Point Center {
            get {
                GraphicCalc.GetPointInDirection(ref _center, Start, End, GraphicLength / 2);
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

        private Point _arrowTailPos = new Point(0, 0);
        private Point ArrowTailPos {
            get {
                if (StartsFromStart) {
                    GraphicCalc.GetPointInDirection(ref _arrowTailPos, Center, Start, _arrowLength / 2);
                }
                else {
                    GraphicCalc.GetPointInDirection(ref _arrowTailPos, Center, End, _arrowLength / 2);
                }
                return _arrowTailPos;
            }
        }

        private Point _arrowHeadPos = new Point(0, 0);
        private Point ArrowHeadPos {
            get {
                if (StartsFromStart) {
                    GraphicCalc.GetPointInDirection(ref _arrowHeadPos, _arrowTailPos, End, _arrowLength);
                }
                else {
                    GraphicCalc.GetPointInDirection(ref _arrowHeadPos, _arrowTailPos, Start, _arrowLength);
                }
                return _arrowHeadPos;
            }
        }

        private Point _arrowTipOne = new Point(0, 0);
        private Point ArrowTipOne {
            get {
                GraphicCalc.GetPointInDirection(ref _arrowTipOne, _arrowHeadPos, _arrowTailPos, _arrowTipsLength, _arrowTipsAngle);
                return _arrowTipOne;
            }
        }
        private Point _arrowTipTwo = new Point(0, 0);
        private Point ArrowTipTwo {
            get {
                GraphicCalc.GetPointInDirection(ref _arrowTipTwo, _arrowHeadPos, _arrowTailPos, _arrowTipsLength, -_arrowTipsAngle);
                return _arrowTipTwo;
            }
        }
        #endregion

        private bool _isBroken = false;
        public bool IsBroken {
            get {
                return _isBroken;
            }
            set {
                _isBroken = value;
                Render();
            }
        }

        public double Length;

        public StraightRailTrackItem(Point initPos, double length) : base() {
            PortStart = new Port(this, initPos);
            PortEnd = new Port(this, initPos);
            PlacementStatus = RailPlacementStatus.NOT_PLACED;
            PlatformType = RailPlatformType.NONE;
            Length = length;
        }
        public StraightRailTrackItem(Point initPos) : this(initPos, defaultLength) { }

        public Port GetOtherPort(Port wrongPort) {
            if (wrongPort == PortStart) return PortEnd;
            if (wrongPort == PortEnd) return PortStart;
            else throw new ArgumentException("SRT got wrong port: " + wrongPort + ". SRT only had: " + PortStart + " and " + PortEnd);
        }

        public override void Reassign_OnPortMerged(object? sender, Port oldPort) {
            if (sender == null || sender is not Port port || port.TopologyItems.Contains(this)) return;

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
            if (IsBroken) {
                dc.DrawEllipse(brokenBrush, brokenPen, Start, _circleRadius, _circleRadius);
            }
            else {
                dc.DrawEllipse(_railTrackBrush, _railTrackPen, Start, _circleRadius, _circleRadius);
            }

            if (PlacementStatus != RailPlacementStatus.NOT_PLACED) {
                if (GraphicLength >= _minDrawableLength) {
                    // platform. NOTE: it's drawn behind other parts to avoid boresome calculations of main line edge (it is replace with simple wide pen)
                    PathFigure platform = new PathFigure(PlatfromCornerOne, [
                        new LineSegment(PlatfromCornerTwo, true),
                        new LineSegment(PlatfromCornerThree, true),
                        new LineSegment(PlatfromCornerFour, true),
                    ], true);
                    PathGeometry platformGeometry = new PathGeometry([platform]);
                    if (IsBroken && PlatformType != RailPlatformType.NONE) {
                        dc.DrawGeometry(brokenBrush, brokenPen, platformGeometry);
                    }
                    else {
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
                            case RailPlatformType.NONE_HOVER:
                                dc.DrawGeometry(_noneHoverBrush, _noneHoverPen, platformGeometry);
                                break;
                        }
                    }
                }

                // main line
                if (IsBroken) {
                    dc.DrawLine(_brokenRailTrackPen, Start, End);
                }
                else {
                    dc.DrawLine(_railTrackPen, Start, End);
                }

                if (GraphicLength >= _minDrawableLength) {
                    // direction arrow 
                    dc.DrawLine(_railArrowPen, ArrowTailPos, ArrowHeadPos);
                    dc.DrawLine(_railArrowPen, _arrowHeadPos, ArrowTipOne);
                    dc.DrawLine(_railArrowPen, _arrowHeadPos, ArrowTipTwo);
                }

                // second circle
                if (IsBroken) {
                    dc.DrawEllipse(brokenBrush, brokenPen, End, _circleRadius, _circleRadius);
                }
                else {
                    dc.DrawEllipse(_railTrackBrush, _railTrackPen, End, _circleRadius, _circleRadius);
                }
            }
        }
    }
}
