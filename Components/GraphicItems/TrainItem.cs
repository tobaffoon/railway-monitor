using railway_monitor.Bases;
using railway_monitor.Components.TopologyItems;
using railway_monitor.Utils;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace railway_monitor.Components.GraphicItems {
    public class TrainItem : GraphicItem
    {
        public enum TrainType
        {
            PASSENGER,
            CARGO,
            NONE
        }

        public static double minDrawableProgress = 1e-10;

        private static readonly Brush _trainBrush = new SolidColorBrush(Colors.Black);
        private static readonly Brush _trainBrokenBrush = new SolidColorBrush(Colors.DarkRed);
        private static readonly Brush _trainOutlinesBrush = new SolidColorBrush(Colors.LightGoldenrodYellow);
        private static readonly Pen _trainPen = new Pen(_trainOutlinesBrush, 0.1);

        #region Drawing params
        private static readonly double _triangleSide = 6;
        private static readonly double _triangleAngle = 0.542;  // radians = 30 deg
        #endregion

        #region Drawing Points
        private Point _triangleBase = new Point(0, 0);
        private Point TriangleBase
        {
            get
            {
                GraphicCalc.GetPointInDirection(ref _triangleBase, FlowCurrentTrack.MovementStart, FlowCurrentTrack.MovementEnd, FlowCurrentTrack.Length * FlowTrackProgress);
                return _triangleBase;
            }
        }
        private Point _triangleSideOne = new Point(0, 0);
        private Point TriangleSideOne
        {
            get
            {
                GraphicCalc.GetPointInDirection(ref _triangleSideOne, _triangleBase, FlowCurrentTrack.MovementStart, _triangleSide, _triangleAngle);
                return _triangleSideOne;
            }
        }
        private Point _triangleSideTwo = new Point(0, 0);
        private Point TriangleSideTwo
        {
            get
            {
                GraphicCalc.GetPointInDirection(ref _triangleSideTwo, _triangleBase, FlowCurrentTrack.MovementStart, _triangleSide, -_triangleAngle);
                return _triangleSideTwo;
            }
        }
        #endregion

        #region LastRealPos info
        private StraightRailTrackItem _currentTrack;
        public StraightRailTrackItem CurrentTrack
        {
            set
            {
                _currentTrack = value;
                FlowCurrentTrack = value;
            }
        }

        private double _trackProgress;
        public double TrackProgress
        {
            set
            {
                if (0.1 <= value && value <= 1)
                {
                    _trackProgress = value;
                    FlowTrackProgress = value;
                }
                else
                {
                    throw new InvalidDataException("Track progress of train " + Id + " attempted to be set outside of [0.1; 1]");
                }
            }
        }
        #endregion
        #region FlowPos info
        private StraightRailTrackItem _flowCurrentTrack;
        public StraightRailTrackItem FlowCurrentTrack
        {
            get
            {
                return _flowCurrentTrack;
            }
            set
            {
                _flowCurrentTrack = value;
                Render();
            }
        }

        private double _flowTrackProgress;
        public double FlowTrackProgress
        {
            get
            {
                return _flowTrackProgress;
            }
            set
            {
                if (0 <= value && value <= 1)
                {
                    if (value <= minDrawableProgress)
                    {
                        _flowTrackProgress = minDrawableProgress;
                    }
                    else
                    {
                        _flowTrackProgress = value;
                    }
                }
                else
                {
                    throw new InvalidDataException("Track progress of train " + Id + " attempted to be set outside of [0.1; 1]");
                }
                Render();
            }
        }
        #endregion

        public int Id { get; }

        private bool _isBroken = false;
        public bool IsBroken
        {
            get
            {
                return _isBroken;
            }
            set
            {
                _isBroken = value;
                Render();
            }
        }

        public TrainItem(int id, StraightRailTrackItem startTrack)
        {
            Id = id;
            _currentTrack = startTrack;
            _flowCurrentTrack = startTrack;
            _trackProgress = minDrawableProgress;
            _flowTrackProgress = minDrawableProgress;
        }
        protected override void Render(DrawingContext dc)
        {
            PathFigure triangle = new PathFigure(TriangleBase, [
                new LineSegment(TriangleSideOne, true),
                new LineSegment(TriangleSideTwo, true),
                ], true);
            PathGeometry triangleGeometry = new PathGeometry([triangle]);
            if (IsBroken) {
                dc.DrawGeometry(_trainBrokenBrush, _trainPen, triangleGeometry);
            }
            else {
                dc.DrawGeometry(_trainBrush, _trainPen, triangleGeometry);
            }
        }
    }
}
