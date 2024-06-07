using System.Windows.Media;
using System.Windows;

namespace railway_monitor.Utils
{
    public static class GraphicCalc
    {
        public static Point GetPointInDirection(Point src, Point dst, double length)
        {
            double angleToDst = Math.Atan2(dst.Y - src.Y, dst.X - src.X);
            Point orientedPoint = new Point(src.X + length * Math.Cos(angleToDst), src.Y + length * Math.Sin(angleToDst));
            return orientedPoint;
        }
    }
}
