﻿using railway_monitor.Bases;
using railway_monitor.Utils;
using System.Windows;
using System.Windows.Media;
namespace railway_monitor.Components.GraphicItems {
    public class SwitchItem : TopologyItem {
        public enum SwitchPlacementStatus {
            ERROR,
            NOT_PLACED,
            PLACED,
            SOURCE_SET
        }


        private static readonly Brush _switchBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        private static readonly Brush _switchArrowBrush = new SolidColorBrush(Colors.ForestGreen);
        private static readonly Pen _switchPen = new Pen(_switchBrush, 3);
        private static readonly Pen _switchArrowPen = new Pen(_switchArrowBrush, 1);
        private static readonly double _circleRadius = 3.0;
        private static readonly double _lineLength = 14.0;

        #region Arrow params
        private static readonly double _arrowDistance = 15.0;
        private static readonly double _arrowLength = 10.0;
        private static readonly double _arrowTipsLength = 4.0;
        private static readonly double _arrowTipsAngle = 0.524;  // radians = 30 deg
        #endregion

        static SwitchItem() {
            _switchArrowPen.StartLineCap = PenLineCap.Round;
            _switchArrowPen.EndLineCap = PenLineCap.Round;
        }

        #region Drawing points        
        private Point _arrowTailPos = new Point(0, 0);
        private Point ArrowTailPos {
            get {
                GraphicCalc.GetPointInDirection(ref _arrowTailPos, Pos, PortSrc.Pos, _arrowDistance);
                return _arrowTailPos;
            }
        }

        private Point _arrowHeadPos = new Point(0, 0);
        private Point ArrowHeadPos {
            get {
                GraphicCalc.GetPointInDirection(ref _arrowHeadPos, _arrowTailPos, Pos, _arrowLength);
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

        private Point _lineHeadPos = new Point(0, 0);
        private Point LineHeadPos {
            get {
                if (PlacementStatus == SwitchPlacementStatus.SOURCE_SET) {
                    GraphicCalc.GetPointInDirection(ref _lineHeadPos, Pos, DstPos, _lineLength);
                }
                else {
                    _lineHeadPos.X = Pos.X + _lineLength;
                    _lineHeadPos.Y = Pos.Y;
                }

                return _lineHeadPos;
            }
        }
        #endregion

        public Port PortSrc { get; private set; }
        public Point SrcPos {
            get {
                return PortSrc.Pos;
            }
            set {
                PortSrc.Pos.X = value.X;
                PortSrc.Pos.Y = value.Y;
                Render();
            }
        }

        public Port PortDstOne { get; private set; }
        public Port PortDstTwo { get; private set; }
        public Point DstPos {
            get {
                if (SwitchedToOne) {
                    return PortDstOne.Pos;
                }
                else {
                    return PortDstTwo.Pos;
                }
            }
        }

        private bool _switchedToTwo = true;
        public bool SwitchedToOne {
            get {
                return _switchedToTwo;
            }
            set {
                _switchedToTwo = value;
                Render();
            }
        }

        public SwitchPlacementStatus PlacementStatus { get; private set; } = SwitchPlacementStatus.NOT_PLACED;

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

        public SwitchItem(Point initPos) : base() {
            Port = new Port(this, initPos);

            PortSrc = new Port(this, new Point(0, 0));
            PortDstOne = new Port(this, new Point(0, 0));
            PortDstTwo = new Port(this, new Point(0, 0));
        }

        public void Place(Port mainPort) {
            mainPort.Merge(Port);
            PlacementStatus = SwitchPlacementStatus.PLACED;
            Render();
        }

        public bool IsSourceValid(Port source) {
            if (source == Port) {
                // user has chosen port where switch is placed
                return false;
            }
            var connectedRails = Port.TopologyItems.OfType<StraightRailTrackItem>();
            var srcRail = connectedRails.Where((rail) => rail.PortStart == source || rail.PortEnd == source).FirstOrDefault();
            if (srcRail == null) {
                // user has chosen port not out of three connected ports
                return false;
            }

            return true;
        }

        /// <summary>
        /// Tries to set port as switch's source of train flow. If it's not valid - doesn't set
        /// </summary>
        public void SetSource(Port source) {
            if (source == Port) {
                // user has chosen port where switch is placed
                return;
            }
            var connectedRails = Port.TopologyItems.OfType<StraightRailTrackItem>();
            var srcRail = connectedRails.Where((rail) => rail.PortStart == source || rail.PortEnd == source).FirstOrDefault();
            if (srcRail == null) {
                // user has chosen port not out of three connected ports
                return;
            }
            connectedRails = connectedRails.Except([srcRail]);

            // set Source Port
            PortSrc = source;

            // set first Destination Port
            StraightRailTrackItem dstOne = connectedRails.ElementAt(0);
            PortDstOne = dstOne.GetOtherPort(Port);

            // set second Destination Port
            StraightRailTrackItem dstTwo = connectedRails.ElementAt(1);
            PortDstTwo = dstTwo.GetOtherPort(Port);

            SwitchedToOne = true;

            PlacementStatus = SwitchPlacementStatus.SOURCE_SET;
            Render();
        }

        public override void Reassign_OnPortMerged(object? sender, Port oldPort) {
            if (sender is not Port newPort) return;
            Port = newPort;
        }

        protected override void Render(DrawingContext dc) {
            // body
            dc.DrawEllipse(_switchBrush, _switchPen, Pos, _circleRadius, _circleRadius);

            // main line
            dc.DrawLine(_switchPen, Pos, LineHeadPos);

            // source arrow
            if (PlacementStatus == SwitchPlacementStatus.PLACED) {
                dc.DrawLine(_switchArrowPen, ArrowTailPos, ArrowHeadPos);
                dc.DrawLine(_switchArrowPen, _arrowHeadPos, ArrowTipOne);
                dc.DrawLine(_switchArrowPen, _arrowHeadPos, ArrowTipTwo);
            }
        }
    }
}