using railway_monitor.Bases;
using railway_monitor.Utils;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
namespace railway_monitor.Components.GraphicItems
{
    public class SwitchItem : GraphicItem
    {
        public enum PlacementStatus
        {
            ERROR,
            NOT_PLACED,
            PLACED,
            CONNECTED
        }


        private static readonly Brush SwitchBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        private static readonly int SwitchStrokeThickness = 4;
        private static readonly double _circleRadius = 3.0;
        private static readonly double _lineLength = 14.0;

        #region Arrow params
        private static readonly double _arrowDistance = 40.0;
        private static readonly double _arrowLength = 10.0;
        private static readonly double _arrowTipsLength = 10.0;
        private static readonly double _arrowTipsAngle = 0.524;  // radians = 30 deg
        #endregion

        // circle is two arcs (semicircle)
        private static Size circleSize = new Size(_circleRadius, _circleRadius);

        private Point _arrowPos;
        private Point _arrowHeadPos;
        private Point _arrowHelpPos;
        private Point _arrowHeadHelpPos;
        private Port _portSrc { get; set; }
        public Point SrcPos {
            get
            {
                return _portSrc.Pos;
            }
            set
            {
                _portSrc.Pos = value;
            }
        }

        private Port _portDstOne {  get; set; }
        private Port _portDstTwo {  get; set; }

        private bool _switchedToTwo = true;
        public bool SwitchedToOne { 
            get 
            { 
                return _switchedToTwo;
            }
            set
            {
                _switchedToTwo = value;
                this.InvalidateMeasure();
            }
        }

        public PlacementStatus Status { get; set; } = PlacementStatus.NOT_PLACED;

        private Point _lineHeadPos;
        public Port Port { get; private set; }
        public Point Pos { 
            get
            {
                return Port.Pos;
            }
            set
            {
                Port.Pos = value;
            }
        }

        public SwitchItem() : base()
        {
            Port = new Port(this, new Point(0, 0)); 
            _lineHeadPos = new Point(0, 0);
            _portSrc = new Port(this, new Point(0, 0));
            _arrowPos = new Point(0, 0);
            _arrowHeadPos = new Point(0, 0);
            _arrowHelpPos = new Point(0, 0);
            _arrowHeadHelpPos = new Point(0, 0);
            _portDstOne = new Port(this, new Point(0, 0));
            _portDstTwo = new Port(this, new Point(0, 0));
            
            Stroke = SwitchBrush;
            Fill = SwitchBrush;
            StrokeThickness = SwitchStrokeThickness;
        }

        public void Place(Port mainPort)
        {
            mainPort.Merge(Port);
            Port = mainPort;
            Status = PlacementStatus.PLACED;
        }

        public void SetSource(Port source)
        {            
            if(source == Port)
            {
                // user has chosen port where switch is placed
                return; 
            }
            var connectedRails = Port.GraphicItems.OfType<StraightRailTrackItem>();
            var srcRail = connectedRails.Where((rail) => rail.PortStart == source || rail.PortEnd == source).FirstOrDefault();
            if (srcRail == null)
            {
                // user has chosen port not out of three connected ports
                return; 
            }
            connectedRails = connectedRails.Except([srcRail]);

            // set Source Port
            _portSrc = source;

            // set first Destination Port
            StraightRailTrackItem dstOne = connectedRails.ElementAt(0);
            if (dstOne.PortStart != Port)
            {
                _portDstOne = dstOne.PortStart;
            }
            else
            {
                _portDstOne = dstOne.PortEnd;
            }

            // set second Destination Port
            StraightRailTrackItem dstTwo = connectedRails.ElementAt(1);
            if (dstTwo.PortStart != Port)
            {
                _portDstTwo = dstTwo.PortStart;
            }
            else
            {
                _portDstTwo = dstTwo.PortEnd;
            }

            Status = PlacementStatus.CONNECTED;
        }

        public override void Move_OnPortMoved(object? sender, Point newPos)
        {
        }

        public override void Reassign_OnPortMerged(object? sender, Port oldPort)
        {
            if (sender == null || sender is not Bases.Port) return;

            Port = (Port)sender;
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                PathGeometry g;

                // circle
                Point a1 = new Point(Pos.X + _circleRadius, Pos.Y);
                Point a2 = new Point(Pos.X - _circleRadius, Pos.Y);
                PathFigure circle = new PathFigure(a1, [
                    new ArcSegment(a2, circleSize, 0, false, SweepDirection.Clockwise, true),
                    new ArcSegment(a1, circleSize, 0, false, SweepDirection.Clockwise, true)
                    ], false);
                g = new PathGeometry([circle]);
                PathFigure switchLine;

                #region Draw arrow
                if (Status >= PlacementStatus.PLACED)
                {
                    //Point arrowTipOne = GraphicCalc.GetPointInDirection(arrowHead, arrowHead, _arrowTipsLength, _arrowTipsAngle);
                    //Point arrowTipTwo = GraphicCalc.GetPointInDirection(arrowHead, arrowHead, _arrowTipsLength, -_arrowTipsAngle);
                    
                    GraphicCalc.GetPointInDirection(ref _arrowPos, Pos, SrcPos, _arrowDistance);
                    GraphicCalc.GetPointInDirection(ref _arrowHeadPos, _arrowPos, Pos, _arrowLength);
                    _arrowHelpPos.X = 2 * Pos.X - _arrowPos.X;
                    _arrowHelpPos.Y = 2 * Pos.Y - _arrowPos.Y;
                    _arrowHeadHelpPos.X = 2 * Pos.X - _arrowHeadPos.X;
                    _arrowHeadHelpPos.Y = 2 * Pos.Y - _arrowHeadPos.Y;

                    PathFigure arrow = new PathFigure(_arrowPos, [
                        new LineSegment(_arrowHeadPos, true),
                        new LineSegment(_arrowHeadHelpPos, false),
                        //new LineSegment(_arrowHeadPos, false),
                        //new LineSegment(arrowTipOne, true),
                        //new LineSegment(arrowHead, false),
                        //new LineSegment(arrowTipTwo, true),
                        ], false);
                    g.Figures.Add(arrow);
                }
                #endregion

                #region Draw direction line

                if (Status == PlacementStatus.CONNECTED)
                {
                    if (SwitchedToOne) GraphicCalc.GetPointInDirection(ref _lineHeadPos, Pos, _portDstOne.Pos, _lineLength);
                    else GraphicCalc.GetPointInDirection(ref _lineHeadPos, Pos, _portDstTwo.Pos, _lineLength);
                }
                else
                {
                    _lineHeadPos.X = Pos.X + _lineLength;
                    _lineHeadPos.Y = Pos.Y;
                }
                //Trace.WriteLine("_lineHeadPos = " + _lineHeadPos);

                Point help = new Point(2*Pos.X-_lineHeadPos.X, 2*Pos.Y-_lineHeadPos.Y);
                Point help2 = new Point(2*Pos.X-_lineHeadPos.X-10, 2*Pos.Y-_lineHeadPos.Y-10);
                switchLine = new PathFigure(_arrowHeadHelpPos, [
                            new ArcSegment(help, circleSize, 0, false, SweepDirection.Clockwise, true),
                            new LineSegment(Pos, true),
                            new LineSegment(_lineHeadPos, true),
                        ], false);
                #endregion

                g.Figures.Add(switchLine);
                return g;
            }
        }
    }
}
