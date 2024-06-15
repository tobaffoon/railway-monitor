using railway_monitor.Bases;
using System.Windows;
using System.Windows.Media;

namespace railway_monitor.Components.GraphicItems {
    public class ExternalTrackItem : GraphicItem {
        public enum ExternalTrackPlacementStatus {
            ERROR,
            NOT_PLACED,
            PLACED
        }
        public ExternalTrackPlacementStatus PlacementStatus { get; private set; } = ExternalTrackPlacementStatus.NOT_PLACED;

        public enum ExternalTrackType {
            IN,
            OUT
        }
        private ExternalTrackType _type = ExternalTrackType.IN;
        public ExternalTrackType Type {
            get {
                return _type;
            }
            set {
                _type = value;
                Render();
            }
        }

        public enum ExternalTrackOrientation {
            RIGHT,
            DOWN,
            LEFT,
            UP
        }
        private ExternalTrackOrientation _orientation = ExternalTrackOrientation.RIGHT;
        public ExternalTrackOrientation Orientation {
            get {
                return _orientation;
            }
            set {
                _orientation = value;
                Render();
            }
        }

        private static readonly Brush _externalTrackBrush = new SolidColorBrush(Colors.Black);
        private static readonly Pen _externalTrackPen = new Pen(_externalTrackBrush, 0);
        private static readonly Brush _externalPortBrush = new SolidColorBrush(Color.FromRgb(153, 255, 51));
        private static readonly Pen _externalPortPen = new Pen(_externalPortBrush, 0);

        #region Draw params
        private static readonly double _triangleSide = 30;
        private static readonly double _portCircleRadius = 6;
        #endregion
        
        #region Drawing points 
        private Point _triangleBasePos = new Point(0, 0);
        private Point TriangleBasePos {
            get {
                if (Type == ExternalTrackType.OUT) {
                    return Port.Pos;
                }

                switch (Orientation) {
                    case ExternalTrackOrientation.RIGHT:
                        _triangleBasePos.X = Port.Pos.X - _triangleSide * Math.Sqrt(3) / 2;
                        _triangleBasePos.Y = Port.Pos.Y;
                        break;
                    case ExternalTrackOrientation.DOWN:
                        _triangleBasePos.X = Port.Pos.X;
                        _triangleBasePos.Y = Port.Pos.Y - _triangleSide * Math.Sqrt(3) / 2;
                        break;
                    case ExternalTrackOrientation.LEFT:
                        _triangleBasePos.X = Port.Pos.X + _triangleSide * Math.Sqrt(3) / 2;
                        _triangleBasePos.Y = Port.Pos.Y;
                        break;
                    case ExternalTrackOrientation.UP:
                        _triangleBasePos.X = Port.Pos.X;
                        _triangleBasePos.Y = Port.Pos.Y + _triangleSide * Math.Sqrt(3) / 2;
                        break;
                }
                return _triangleBasePos;
            }
        }

        private Point _triangleTipPos = new Point(0, 0);
        private Point TriangleTipPos {
            get {
                if (Type == ExternalTrackType.IN) {
                    return Port.Pos;
                }

                switch (Orientation) {
                    case ExternalTrackOrientation.RIGHT:
                        _triangleTipPos.X = Port.Pos.X + _triangleSide * Math.Sqrt(3) / 2;
                        _triangleTipPos.Y = Port.Pos.Y;
                        break;
                    case ExternalTrackOrientation.DOWN:
                        _triangleTipPos.X = Port.Pos.X;
                        _triangleTipPos.Y = Port.Pos.Y + _triangleSide * Math.Sqrt(3) / 2;
                        break;
                    case ExternalTrackOrientation.LEFT:
                        _triangleTipPos.X = Port.Pos.X - _triangleSide * Math.Sqrt(3) / 2;
                        _triangleTipPos.Y = Port.Pos.Y;
                        break;
                    case ExternalTrackOrientation.UP:
                        _triangleTipPos.X = Port.Pos.X;
                        _triangleTipPos.Y = Port.Pos.Y - _triangleSide * Math.Sqrt(3) / 2;
                        break;
                }
                return _triangleTipPos;
            }
        }

        private Point _sideVertexOne = new Point(0, 0);
        private Point SideVertexOne {
            get {
                switch (Orientation) {
                    case ExternalTrackOrientation.RIGHT:
                        _sideVertexOne.X = TriangleBasePos.X;
                        _sideVertexOne.Y = TriangleBasePos.Y - _triangleSide / 2;
                        break;
                    case ExternalTrackOrientation.DOWN:
                        _sideVertexOne.X = TriangleBasePos.X + _triangleSide / 2;
                        _sideVertexOne.Y = TriangleBasePos.Y;
                        break;
                    case ExternalTrackOrientation.LEFT:
                        _sideVertexOne.X = TriangleBasePos.X;
                        _sideVertexOne.Y = TriangleBasePos.Y + _triangleSide / 2;
                        break;
                    case ExternalTrackOrientation.UP:
                        _sideVertexOne.X = TriangleBasePos.X - _triangleSide / 2;
                        _sideVertexOne.Y = TriangleBasePos.Y;
                        break;
                }
                return _sideVertexOne;
            }
        }

        private Point _sideVertexTwo = new Point(0, 0);
        private Point SideVertexTwo {
            get {
                switch (Orientation) {
                    case ExternalTrackOrientation.RIGHT:
                        _sideVertexTwo.X = TriangleBasePos.X;
                        _sideVertexTwo.Y = TriangleBasePos.Y + _triangleSide / 2;
                        break;
                    case ExternalTrackOrientation.DOWN:
                        _sideVertexTwo.X = TriangleBasePos.X - _triangleSide / 2;
                        _sideVertexTwo.Y = TriangleBasePos.Y;
                        break;
                    case ExternalTrackOrientation.LEFT:
                        _sideVertexTwo.X = TriangleBasePos.X;
                        _sideVertexTwo.Y = TriangleBasePos.Y - _triangleSide / 2;
                        break;
                    case ExternalTrackOrientation.UP:
                        _sideVertexTwo.X = TriangleBasePos.X + _triangleSide / 2;
                        _sideVertexTwo.Y = TriangleBasePos.Y;
                        break;
                }
                return _sideVertexTwo;
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

        public ExternalTrackItem(Point initPos) : base() {
            Port = new Port(this, initPos);
        }

        public void Place(Port mainPort) {
            mainPort.Merge(Port);
            PlacementStatus = ExternalTrackPlacementStatus.PLACED;
            Render();
        }

        public override void Reassign_OnPortMerged(object? sender, Port oldPort) {
            if (sender is not Port newPort) return;
            Port = newPort;
        }

        protected override void Render(DrawingContext dc) {
            if (PlacementStatus == ExternalTrackPlacementStatus.NOT_PLACED) {
                dc.DrawEllipse(_externalPortBrush, _externalPortPen, Pos, _portCircleRadius, _portCircleRadius);
            }

            PathFigure triangle = new PathFigure(TriangleBasePos, [
                new LineSegment(SideVertexOne, true),
                new LineSegment(TriangleTipPos, true),
                new LineSegment(SideVertexTwo, true),
                ], true);
            PathGeometry triangleGeometry = new PathGeometry([triangle]);
            dc.DrawGeometry(_externalTrackBrush, _externalTrackPen, triangleGeometry);
        }
    }
}
