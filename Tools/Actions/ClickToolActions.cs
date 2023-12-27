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
using railway_monitor.Components.GraphicItems;

namespace railway_monitor.Tools.Actions
{
    public sealed class ClickToolActions
    {
        public static void StartStraightRailTrack(Tuple<Canvas, Shape> args)
        {
            Canvas canvas = args.Item1;
            Shape currentShape = args.Item2;
            Point mousePos = Mouse.GetPosition(canvas);

            StraightRailTrack currentRailTrack = (StraightRailTrack)currentShape;
            currentRailTrack.X1 = mousePos.X;
            currentRailTrack.Y1 = mousePos.Y;
        }
        public static void FinishStraightRailTrack(Tuple<Canvas, Shape> args)
        {
            Canvas canvas = args.Item1;
            Shape currentShape = args.Item2;
            Point mousePos = Mouse.GetPosition(canvas);

            StraightRailTrack currentRailTrack = (StraightRailTrack)currentShape;
            currentRailTrack.X2 = mousePos.X;
            currentRailTrack.Y2 = mousePos.Y;
        }

        public static void PlaceSwitch(Tuple<Canvas, Shape> args)
        {
            throw new NotImplementedException("Switch");
        }
        public static void PlaceSignal(Tuple<Canvas, Shape> args)
        {
            throw new NotImplementedException("Signal");
        }
        public static void PlaceDeadend(Tuple<Canvas, Shape> args)
        {
            throw new NotImplementedException("Deadend");
        }
        public static void PlaceExternalTrack(Tuple<Canvas, Shape> args)
        {
            throw new NotImplementedException("External track");
        }

    }
}
