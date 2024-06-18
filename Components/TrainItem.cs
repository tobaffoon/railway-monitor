using railway_monitor.Bases;
using railway_monitor.Components.GraphicItems;
using railway_monitor.Utils;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace railway_monitor.Components {
    public class TrainItem : GraphicItem {
        public enum TrainType {
            PASSENGER,
            CARGO,
            NONE
        }

        private static readonly Brush _trainBrush = new SolidColorBrush(Colors.Black);
        private static readonly Brush _trainOutlinesBrush = new SolidColorBrush(Colors.LightGoldenrodYellow);
        private static readonly Pen _trainPen = new Pen(_trainOutlinesBrush, 0.1);

        #region Drawing params
        private static readonly double _triangleSide = 6;
        private static readonly double _triangleAngle = 0.542;  // radians = 60 deg
        #endregion

        #region Drawing Points
        private Point _triangleBase = new Point(0, 0);
        private Point TriangleBase {
            get {
                GraphicCalc.GetPointInDirection(ref _triangleBase, CurrentTrack.MovementStart, CurrentTrack.MovementEnd, CurrentTrack.Length * TrackProgress);
                return _triangleBase; 
            } 
        }
        private Point _triangleSideOne = new Point(0, 0);
        private Point TriangleSideOne {
            get {
                GraphicCalc.GetPointInDirection(ref _triangleSideOne, _triangleBase, CurrentTrack.MovementStart, _triangleSide, _triangleAngle);
                return _triangleSideOne;
            }
        }
        private Point _triangleSideTwo = new Point(0, 0);
        private Point TriangleSideTwo {
            get {
                GraphicCalc.GetPointInDirection(ref _triangleSideTwo, _triangleBase, CurrentTrack.MovementStart, _triangleSide, -_triangleAngle);
                return _triangleSideTwo;
            }
        }
        #endregion

        private StraightRailTrackItem _currentTrack;
        public StraightRailTrackItem CurrentTrack {
            get { 
                return _currentTrack;
            }
            set {
                _currentTrack = value;
                Render();
            }
        }

        private double _trackProgress;
        public double TrackProgress {
            get {
                return _trackProgress;
            }
            set {
                if (0.1 <= value && value <= 1) {
                    _trackProgress = value;
                }
                else {
                    throw new InvalidDataException("Track progress of train " + Id + " attempted to be set outside of [0.1; 1]");
                }
                Render();
            }
        }

        public int Id { get; }

        public TrainItem(int id, StraightRailTrackItem startTrack) {
            Id = id;
            _currentTrack = startTrack;
            _trackProgress = 0.1;
        }
        protected override void Render(DrawingContext dc) {
            PathFigure triangle = new PathFigure(TriangleBase, [
                new LineSegment(TriangleSideOne, true),
                new LineSegment(TriangleSideTwo, true),
                ], true);
            PathGeometry triangleGeometry = new PathGeometry([triangle]);
            dc.DrawGeometry(_trainBrush, _trainPen, triangleGeometry);
        }
    }
}
