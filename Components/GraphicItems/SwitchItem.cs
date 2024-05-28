using railway_monitor.Bases;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace railway_monitor.Components.GraphicItems
{
    public class SwitchItem : GraphicItem
    {
        public enum PlacementStatus
        {
            NOT_PLACED,
            PLACED
        }

        private static readonly Brush SwitchBrush = new SolidColorBrush(Color.FromRgb(191, 191, 191));
        private static readonly int SwitchStrokeThickness = 4;
        private static readonly double _circleRadius = 3.0;
        private static readonly double _lineLenght = 14.0;

        // circle is two arcs (semicircle)
        private static Size circleSize = new Size(_circleRadius, _circleRadius);

        private Point ConnectionOne {  get; set; }
        private Point ConnectionTwo {  get; set; }
        public bool SwitchedToOne { get; set; } = true;

        public PlacementStatus Status { get; set; } = PlacementStatus.NOT_PLACED;
        public Point Pos { get; set; }

        public SwitchItem() : base()
        {
            Pos = new Point(0, 0);
            Stroke = SwitchBrush;
            Fill = SwitchBrush;
            StrokeThickness = SwitchStrokeThickness;
        }

        public void Connect(StraightRailTrackItem src, StraightRailTrackItem dstOne, StraightRailTrackItem dstTwo)
        {
            Pos = src.End;

            double angleToOne = Math.Atan2(dstOne.End.Y-Pos.Y, dstOne.End.X-Pos.X) * 180.0 / Math.PI;
            ConnectionOne = new Point(Pos.X + Math.Cos(angleToOne) * _lineLenght, Pos.Y + Math.Sin(angleToOne) * _lineLenght);

            double angleToTwo = Math.Atan2(dstTwo.End.Y-Pos.Y, dstTwo.End.X-Pos.X) * 180.0 / Math.PI;
            ConnectionTwo = new Point(Pos.X + Math.Cos(angleToOne) * _lineLenght, Pos.Y + Math.Sin(angleToOne) * _lineLenght);

            Status = PlacementStatus.PLACED;
        }
        protected override Geometry DefiningGeometry
        {
            get
            {
                PathGeometry g;
                Point center = Pos;

                // circle
                Point a1 = new Point(Pos.X + _circleRadius, Pos.Y);
                Point a2 = new Point(Pos.X - _circleRadius, Pos.Y);
                PathFigure circle1 = new PathFigure(a1, [
                    new ArcSegment(a2, circleSize, 0, false, SweepDirection.Clockwise, true),
                    new ArcSegment(a1, circleSize, 0, false, SweepDirection.Clockwise, true)
                    ], true);
                g = new PathGeometry([circle1]);

                PathFigure switchLine;
                //if (Status == PlacementStatus.NOT_PLACED)
                //{
                //    // switch faces right
                //    Point lineEnd = new Point(Pos.X + _lineLenght, Pos.Y);
                //    switchLine = new PathFigure(center, [
                //        new LineSegment(lineEnd, true)
                //        ], true);
                //}
                //else
                //{
                //    // switch faces one of the chosen directions
                //    if (SwitchedToOne)
                //    {
                //        switchLine = new PathFigure(center, [
                //            new LineSegment(ConnectionOne, true)
                //            ], true);
                //    }
                //    else
                //    {
                //        switchLine = new PathFigure(center, [
                //            new LineSegment(ConnectionTwo, true)
                //            ], true);
                //    }
                //}
                Point lineEnd = new Point(Pos.X + _lineLenght, Pos.Y);
                    switchLine = new PathFigure(center, [
                        new LineSegment(lineEnd, true)
                        ], true);
                g.Figures.Add(switchLine);
                return g;
            }
        }
    }
}
