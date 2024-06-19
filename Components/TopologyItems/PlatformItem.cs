using railway_monitor.Bases;
using railway_monitor.Utils;
using System.Windows;
using System.Windows.Media;

namespace railway_monitor.Components.GraphicItems {
    public class PlatformItem : TopologyItem {
        public enum PlatformItemType {
            PASSENGER,
            CARGO,
            NONE
        }

        public static readonly double ConnectRadius = 7;
        #region Draw params
        private static readonly double _platformOffset = 10;
        private static readonly double _platformTiltAngle = 1.048;  // radians = 60 deg
        private static readonly double _platformSideLength = 7;
        private static readonly double _platformLength = 30;

        private static readonly double _crossHandLength = 10;
        #endregion

        private static readonly Brush _platformOutlineBrush = new SolidColorBrush(Colors.Black);
        private static readonly Brush _passengerPlatformBrush = new SolidColorBrush(Color.FromRgb(185, 111, 92));
        private static readonly Pen _passengerPlatformPen = new Pen(_platformOutlineBrush, 0.5);
        private static readonly Brush _cargoPlatformBrush = new SolidColorBrush(Color.FromRgb(112, 146, 189));
        private static readonly Pen _cargoPlatformPen = new Pen(_platformOutlineBrush, 0.5);
        private static readonly Brush _nonePlatformBrush = new SolidColorBrush(Color.FromRgb(235, 220, 185));
        private static readonly Pen _nonePlatformPen = new Pen(_platformOutlineBrush, 0.5);
        private static readonly Brush _errorCrossBrush = new SolidColorBrush(Colors.DarkRed);
        private static readonly Pen _errorCrossPen = new Pen(_errorCrossBrush, 4);

        private PlatformItemType _platformType;
        public PlatformItemType PlatformType {
            get {
                return _platformType;
            }
            set {
                _platformType = value;
                Render();
            }
        }

        private Point _pos = new Point(0, 0);
        public Point Pos {
            get {
                return _pos;
            }
            set {
                _pos.X = value.X;
                _pos.Y = value.Y;
                Render();
            }
        }

        private bool _connectionErrorOccured = false;
        public bool ConnectionErrorOccured {
            get {
                return _connectionErrorOccured;
            }
            set {
                _connectionErrorOccured = value;
                Render();
            }
        }

        #region Drawing points 
        private Point _platfromCornerOne = new Point(0, 0);
        private Point PlatfromCornerOne {
            get {
                _platfromCornerOne.X = _pos.X + _platformLength / 2;
                _platfromCornerOne.Y = _pos.Y;
                return _platfromCornerOne;
            }
        }

        private Point _platfromCornerTwo = new Point(0, 0);
        private Point PlatfromCornerTwo {
            get {
                _platfromCornerTwo.X = _pos.X - _platformLength / 2;
                _platfromCornerTwo.Y = _pos.Y;
                return _platfromCornerTwo;
            }
        }

        private Point _platfromCornerThree = new Point(0, 0);
        private Point PlatfromCornerThree {
            get {
                GraphicCalc.GetPointInDirection(ref _platfromCornerThree, _platfromCornerTwo, _platfromCornerOne, _platformSideLength, -_platformTiltAngle);
                return _platfromCornerThree;
            }
        }

        private Point _platfromCornerFour = new Point(0, 0);
        private Point PlatfromCornerFour {
            get {
                GraphicCalc.GetPointInDirection(ref _platfromCornerFour, _platfromCornerOne, _platfromCornerTwo, _platformSideLength, _platformTiltAngle);
                return _platfromCornerFour;
            }
        }

        private Point _crossHandOneStart = new Point(0, 0);
        private Point CrossHandOneStart {
            get {
                _crossHandOneStart.X = _pos.X - _crossHandLength / 2;
                _crossHandOneStart.Y = _pos.Y - _crossHandLength / 2;
                return _crossHandOneStart;
            }
        }

        private Point _crossHandOneEnd = new Point(0, 0);
        private Point CrossHandOneEnd {
            get {
                _crossHandOneEnd.X = _pos.X + _crossHandLength / 2;
                _crossHandOneEnd.Y = _pos.Y + _crossHandLength / 2;
                return _crossHandOneEnd;
            }
        }

        private Point _crossHandTwoStart = new Point(0, 0);
        private Point CrossHandTwoStart {
            get {
                _crossHandTwoStart.X = _pos.X + _crossHandLength / 2;
                _crossHandTwoStart.Y = _pos.Y - _crossHandLength / 2;
                return _crossHandTwoStart;
            }
        }

        private Point _crossHandTwoEnd = new Point(0, 0);
        private Point CrossHandTwoEnd {
            get {
                _crossHandTwoEnd.X = _pos.X - _crossHandLength / 2;
                _crossHandTwoEnd.Y = _pos.Y + _crossHandLength / 2;
                return _crossHandTwoEnd;
            }
        }
        #endregion

        public PlatformItem(Point initPos) : base() {
            Pos = initPos;
            Visibility = Visibility.Collapsed;
            PlatformType = PlatformItemType.PASSENGER;
        }

        public override void Reassign_OnPortMerged(object? sender, Port oldPort) {
            throw new NotImplementedException();
        }

        protected override void Render(DrawingContext dc) {
            PathFigure platform = new PathFigure(PlatfromCornerOne, [
                        new LineSegment(PlatfromCornerTwo, true),
                        new LineSegment(PlatfromCornerThree, true),
                        new LineSegment(PlatfromCornerFour, true),
                    ], true);
            PathGeometry platformGeometry = new PathGeometry([platform]);

            switch (PlatformType) {
                case PlatformItemType.PASSENGER:
                    dc.DrawGeometry(_passengerPlatformBrush, _passengerPlatformPen, platformGeometry);
                    break;
                case PlatformItemType.CARGO:
                    dc.DrawGeometry(_cargoPlatformBrush, _cargoPlatformPen, platformGeometry);
                    break;
                case PlatformItemType.NONE:
                    dc.DrawGeometry(_nonePlatformBrush, _nonePlatformPen, platformGeometry);
                    break;
            }
            
            if (ConnectionErrorOccured) {
                // cross
                dc.DrawLine(_errorCrossPen, CrossHandOneStart, CrossHandOneEnd);
                dc.DrawLine(_errorCrossPen, CrossHandTwoStart, CrossHandTwoEnd);
            }
        }
    }
}
