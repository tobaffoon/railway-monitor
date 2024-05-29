using railway_monitor.Bases;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

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

        // circle is two arcs (semicircle)
        private static Size circleSize = new Size(_circleRadius, _circleRadius);

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
            _portDstOne = new Port(this, new Point(0, 0));
            _portDstTwo = new Port(this, new Point(0, 0));
            Stroke = SwitchBrush;
            Fill = SwitchBrush;
            StrokeThickness = SwitchStrokeThickness;
        }

        public void Place(Port mainPort)
        {
            mainPort.Merge(Port);
            Status = PlacementStatus.PLACED;
        }

        public void SetSource(Port source)
        {            
            if(source == Port)
            {
                return; // this happens if user has chosen port where switch is placed
            }
            var connectedRails = Port.GraphicItems.OfType<StraightRailTrackItem>();
            var srcRail = connectedRails.Where((rail) => rail.PortStart == source || rail.PortEnd == source).FirstOrDefault();
            if (srcRail == null)
            {
                return; // this happens if user has chosen not one of three connected ports
            }
            connectedRails = connectedRails.Except([srcRail]);
            
            StraightRailTrackItem dstOne = connectedRails.ElementAt(0);
            if (dstOne.PortStart != Port)
            {
                _portDstOne = dstOne.PortStart;
            }
            else
            {
                _portDstOne = dstOne.PortEnd;
            }

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
                PathFigure circle1 = new PathFigure(a1, [
                    new ArcSegment(a2, circleSize, 0, false, SweepDirection.Clockwise, true),
                    new ArcSegment(a1, circleSize, 0, false, SweepDirection.Clockwise, true)
                    ], true);
                g = new PathGeometry([circle1]);
                PathFigure switchLine;
                PathFigure addCircle;

                if (Status == PlacementStatus.CONNECTED)
                {
                    if (SwitchedToOne)
                    {
                        double angleToOne = Math.Atan2(_portDstOne.Pos.Y - Pos.Y, _portDstOne.Pos.X - Pos.X);
                        Point orientedPoint = new Point(Pos.X + _lineLength * Math.Cos(angleToOne), Pos.Y + _lineLength * Math.Sin(angleToOne));
                        switchLine = new PathFigure(Pos, [
                            new LineSegment(orientedPoint, true)
                            ], false);

                    }
                    else
                    {
                        double angleToTwo = Math.Atan2(_portDstTwo.Pos.Y - Pos.Y, _portDstTwo.Pos.X - Pos.X);
                        Point orientedPoint = new Point(Pos.X + _lineLength * Math.Cos(angleToTwo), Pos.Y + _lineLength * Math.Sin(angleToTwo));
                        switchLine = new PathFigure(Pos, [
                            new LineSegment(orientedPoint, true)
                            ], false);

                    }

                }
                else
                {
                    Point lineEnd = new Point(Pos.X + _lineLength, Pos.Y);
                    switchLine = new PathFigure(Pos, [
                        new LineSegment(lineEnd, true)
                        ], true);
                }
                g.Figures.Add(switchLine);
                return g;
            }
        }
    }
}
