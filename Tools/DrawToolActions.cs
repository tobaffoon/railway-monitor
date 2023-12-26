using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace railway_monitor.Tools
{
    public sealed class DrawToolActions
    {
        private static Brush railTrackBrush = new SolidColorBrush(Color.FromRgb(153, 255, 51));
        public static void DrawStraightRailTrack(Canvas canvas)
        {
            Point mousePos = Mouse.GetPosition(canvas);

            Line newLine = new Line
            {
                X1 = mousePos.X,
                Y1 = mousePos.Y,
                X2 = mousePos.X + 20,
                Y2 = mousePos.Y + 20,
                Stroke = railTrackBrush,
                StrokeThickness = 5
            };

            canvas.Children.Add(newLine);
        }
        public static void DrawSwitch(Canvas canvas)
        {
            throw new NotImplementedException("Switch");
        }
        public static void DrawSignal(Canvas canvas)
        {
            throw new NotImplementedException("Signal");
        }
        public static void DrawDeadend(Canvas canvas)
        {
            throw new NotImplementedException("Deadend");
        }
        public static void DrawExternalTrack(Canvas canvas)
        {
            throw new NotImplementedException("External track");
        }
    }
}
