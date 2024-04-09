﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace railway_monitor.Components.GraphicItems
{
    public class StraightRailTrack : Shape
    {
        private static double _circleRadius = 3;

        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }

        public StraightRailTrack()
        {
            X1 = 0;
            Y1 = 0;
            X2 = 0;
            Y2 = 0;
        }
        protected override Geometry DefiningGeometry
        {
            get
            {
                Point p1 = new Point(X1, Y1);
                Point p2 = new Point(X2, Y2);
                Size circleSize = new Size(_circleRadius, _circleRadius);

                PathFigure pf = new PathFigure(p1, new List<PathSegment>(), false);

                // circle is two arcs (semicircle)


                // first circle
                Point a1 = new Point(X1 + _circleRadius, Y1);
                Point a2 = new Point(X1 - _circleRadius, Y1);
                pf.Segments.Add(new LineSegment(a1, false));
                pf.Segments.Add(new ArcSegment(a2, circleSize, 0, true, SweepDirection.Clockwise, true));
                pf.Segments.Add(new ArcSegment(a1, circleSize, 0, true, SweepDirection.Clockwise, true));
                pf.Segments.Add(new LineSegment(p1, false));

                // Main line
                pf.Segments.Add(new LineSegment(p2, true));

                // second circle
                a1 = new Point(X2 + _circleRadius, Y2);
                a2 = new Point(X2 - _circleRadius, Y2);
                pf.Segments.Add(new LineSegment(a1, false));
                pf.Segments.Add(new ArcSegment(a2, circleSize, 0, false, SweepDirection.Clockwise, true));
                pf.Segments.Add(new ArcSegment(a1, circleSize, 0, false, SweepDirection.Clockwise, true));
                pf.Segments.Add(new LineSegment(p2, false));

                pf.IsClosed = true;

                Geometry g = new PathGeometry([pf], FillRule.EvenOdd, Transform.Identity);
                return g;
            }
        }
    }
}