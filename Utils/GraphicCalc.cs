using System.Windows;

namespace railway_monitor.Utils {
    public static class GraphicCalc {
        public static void GetPointInDirection(ref Point outPnt, Point src, Point dst, double length, double shiftAngle = 0) {
            double angleToDst = Math.Atan2(dst.Y - src.Y, dst.X - src.X) + shiftAngle;
            outPnt.X = src.X + length * Math.Cos(angleToDst);
            outPnt.Y = src.Y + length * Math.Sin(angleToDst);
        }
        public static double GetDistance(Point one, Point two) {
            return Math.Sqrt(Math.Pow(one.X - two.X, 2) + Math.Pow(one.Y - two.Y, 2));
        }
    }
}
