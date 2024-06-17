using railway_monitor.Bases;
using System.Windows;
using System.Windows.Media;

namespace railway_monitor.Components.GraphicItems {
    public class DeadendItem : GraphicItem {
        public enum DeadendPlacementStatus {
            ERROR,
            NOT_PLACED,
            PLACED
        }
        public DeadendPlacementStatus PlacementStatus { get; private set; } = DeadendPlacementStatus.NOT_PLACED;

        public enum DeadendOrientation {
            RIGHT,
            DOWN,
            LEFT,
            UP
        }
        private DeadendOrientation _orientation = DeadendOrientation.RIGHT;
        public DeadendOrientation Orientation {
            get {
                return _orientation;
            }
            set {
                _orientation = value;
                Render();
            }
        }
        #region Draw params
        private static readonly double _deadendLength = 30;
        private static readonly double _deadendWidth = 5;
        private static readonly double _portCircleRadius = 6;
        #endregion

        private static readonly Brush _deadendBrush = new SolidColorBrush(Colors.Red);
        private static readonly Pen _deadendPen = new Pen(_deadendBrush, _deadendWidth);
        private static readonly Brush _deadendPortBrush = new SolidColorBrush(Color.FromRgb(153, 255, 51));
        private static readonly Pen _deadendPortPen = new Pen(_deadendPortBrush, 0);

        #region Drawing points 
        private Point _cornerOne = new Point(0, 0);
        private Point CornerOne {
            get {
                switch (Orientation) {
                    case DeadendOrientation.RIGHT:
                        _cornerOne.X = Pos.X + _portCircleRadius + _deadendWidth / 2;
                        _cornerOne.Y = Pos.Y - _deadendLength / 2;
                        break;
                    case DeadendOrientation.DOWN:
                        _cornerOne.X = Pos.X + _deadendLength / 2;
                        _cornerOne.Y = Pos.Y + _portCircleRadius + _deadendWidth / 2;
                        break;
                    case DeadendOrientation.LEFT:
                        _cornerOne.X = Pos.X - _portCircleRadius - _deadendWidth / 2;
                        _cornerOne.Y = Pos.Y + _deadendLength / 2;
                        break;
                    case DeadendOrientation.UP:
                        _cornerOne.X = Pos.X - _deadendLength / 2;
                        _cornerOne.Y = Pos.Y - _portCircleRadius - _deadendWidth / 2;
                        break;
                }
                return _cornerOne;
            }
        }

        private Point _cornerTwo = new Point(0, 0);
        private Point CornerTwo {
            get {
                switch (Orientation) {
                    case DeadendOrientation.RIGHT:
                        _cornerTwo.X = Pos.X + _portCircleRadius + _deadendWidth / 2;
                        _cornerTwo.Y = Pos.Y + _deadendLength / 2;
                        break;
                    case DeadendOrientation.DOWN:
                        _cornerTwo.X = Pos.X - _deadendLength / 2;
                        _cornerTwo.Y = Pos.Y + _portCircleRadius + _deadendWidth / 2;
                        break;
                    case DeadendOrientation.LEFT:
                        _cornerTwo.X = Pos.X - _portCircleRadius - _deadendWidth / 2;
                        _cornerTwo.Y = Pos.Y - _deadendLength / 2;
                        break;
                    case DeadendOrientation.UP:
                        _cornerTwo.X = Pos.X + _deadendLength / 2;
                        _cornerTwo.Y = Pos.Y - _portCircleRadius - _deadendWidth / 2;
                        break;
                }
                return _cornerTwo;
            }
        }
        #endregion

        public Port Port { get; private set; }
        public Point Pos {
            get {
                return Port.Pos;
            }
            set {
                Port.Pos.X = value.X;
                Port.Pos.Y = value.Y;
                Render();
            }
        }

        public DeadendItem(Point initPoint) : base() {
            Port = new Port(this, initPoint);
        }

        public void Place(Port mainPort) {
            mainPort.Merge(Port);
            PlacementStatus = DeadendPlacementStatus.PLACED;
            Render();
        }

        public override void Reassign_OnPortMerged(object? sender, Port oldPort) {
            if (sender is not Port newPort) return;
            Port = newPort;
        }

        protected override void Render(DrawingContext dc) {
            if (PlacementStatus == DeadendPlacementStatus.NOT_PLACED) {
                dc.DrawEllipse(_deadendPortBrush, _deadendPortPen, Pos, _portCircleRadius, _portCircleRadius);
            }

            dc.DrawLine(_deadendPen, CornerOne, CornerTwo);
        }
    }
}
