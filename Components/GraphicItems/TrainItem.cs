﻿using railway_monitor.Bases;
using railway_monitor.Components.TopologyItems;
using railway_monitor.Utils;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace railway_monitor.Components.GraphicItems {
    public class TrainItem : GraphicItem {
        public enum TrainType {
            PASSENGER,
            CARGO,
            NONE
        }
        public static readonly double minDrawableProgress = 1e-10;
        public static readonly int defaultSpeed = 10;
        public static readonly int defaultLength = 400;

        private static readonly double _trainBorderWidth = 0.3;

        private static readonly Brush _trainBrush = new SolidColorBrush(Colors.Black);
        private static readonly Brush _trainBrokenBrush = new SolidColorBrush(Colors.DarkRed);
        private static readonly Brush _trainOutlinesBrush = new SolidColorBrush(Colors.DeepPink);
        private static readonly Pen _trainPen = new Pen(_trainOutlinesBrush, _trainBorderWidth);

        #region Drawing params
        private static readonly double _triangleSide = 16;
        private static readonly double _triangleAngle = 0.2;  // radians = 30 deg
        #endregion

        #region Drawing Points
        private Point _triangleBase = new Point(0, 0);
        private Point TriangleBase {
            get {
                GraphicCalc.GetPointInDirection(ref _triangleBase, FlowStartingPort.Pos, FlowEndingPort.Pos, FlowCurrentTrack.GraphicLength * FlowTrackProgress);
                return _triangleBase;
            }
        }
        private Point _triangleSideOne = new Point(0, 0);
        private Point TriangleSideOne {
            get {
                GraphicCalc.GetPointInDirection(ref _triangleSideOne, _triangleBase, FlowStartingPort.Pos, _triangleSide, _triangleAngle);
                return _triangleSideOne;
            }
        }
        private Point _triangleSideTwo = new Point(0, 0);
        private Point TriangleSideTwo {
            get {
                GraphicCalc.GetPointInDirection(ref _triangleSideTwo, _triangleBase, FlowStartingPort.Pos, _triangleSide, -_triangleAngle);
                return _triangleSideTwo;
            }
        }
        #endregion

        #region LastRealPos info
        private StraightRailTrackItem _currentTrack;

        private double _trackProgress;
        public double TrackProgress {
            set { 
                _trackProgress = value;
                FlowTrackProgress = value;
            }
        }
        #endregion
        #region FlowPos info
        private StraightRailTrackItem _flowCurrentTrack;
        public StraightRailTrackItem FlowCurrentTrack {
            get {
                return _flowCurrentTrack;
            }
            private set {
                _flowCurrentTrack = value;
                Render();
            }
        }
        public void SetTrack(StraightRailTrackItem track, int timeStamp) {
            FlowCurrentTrack = track;
            vertexPassedTimeStamp = timeStamp;
        }
        private Port _flowEndingPort;
        public Port FlowEndingPort {
            get {
                return _flowEndingPort;
            }
            set {
                _flowEndingPort = value;
                Render();
            }
        }

        public Port FlowStartingPort {
            get {
                return _flowCurrentTrack.GetOtherPort(FlowEndingPort);
            }
        }

        private double _flowTrackProgress;
        public double FlowTrackProgress {
            get {
                return _flowTrackProgress;
            }
            set {
                if (0 <= value && value <= 1) {
                    if (value <= minDrawableProgress) {
                        _flowTrackProgress = minDrawableProgress;
                    }
                    else {
                        _flowTrackProgress = value;
                    }
                }
                else {
                    throw new InvalidDataException("Track progress of train " + Id + " attempted to be set outside of [0.1; 1]");
                }
                Render();
            }
        }
        #endregion

        public int Id { get; }
        public double Speed;

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

        public int vertexPassedTimeStamp = 0;

        public TrainItem(int id, StraightRailTrackItem startTrack, Port endingPort, double initSpeed) {
            Id = id;
            _currentTrack = startTrack;
            _flowCurrentTrack = startTrack;
            _flowEndingPort = endingPort;
            _trackProgress = minDrawableProgress;
            _flowTrackProgress = minDrawableProgress;
            Speed = initSpeed;
        }
        public TrainItem(int id, StraightRailTrackItem startTrack, Port endingPort) : this(id, startTrack, endingPort, defaultSpeed) { }
        public TrainItem(int id, ExternalTrackItem externalTrack) : this(
            id,
            externalTrack.Port.TopologyItems.OfType<StraightRailTrackItem>().First(),
            externalTrack.Port.TopologyItems.OfType<StraightRailTrackItem>().First().GetOtherPort(externalTrack.Port)
            ) { }
        public TrainItem(int id, ExternalTrackItem externalTrack, double initSpeed) : this(
            id,
            externalTrack.Port.TopologyItems.OfType<StraightRailTrackItem>().First(),
            externalTrack.Port.TopologyItems.OfType<StraightRailTrackItem>().First().GetOtherPort(externalTrack.Port),
            initSpeed
            ) { }

        protected override void Render(DrawingContext dc) {
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
