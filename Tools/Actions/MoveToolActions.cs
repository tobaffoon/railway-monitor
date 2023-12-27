using railway_monitor.Components.GraphicItems;
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

namespace railway_monitor.Tools.Actions
{
    public class MoveToolActions
    {
        public static void MoveStraightRailTrackStart(Tuple<Canvas, Shape> args)
        {
            Canvas canvas = args.Item1;
            StraightRailTrack railTrack = (StraightRailTrack)args.Item2;

            Point mousePos = Mouse.GetPosition(canvas);
            railTrack.X1 = mousePos.X;
            railTrack.Y1 = mousePos.Y;
            railTrack.X2 = mousePos.X;
            railTrack.Y2 = mousePos.Y;

            if (!canvas.Children.Contains(railTrack))
            {
                canvas.Children.Add(railTrack);
            }
            else
            {
                railTrack.InvalidateMeasure();
            }
        }
        public static void MoveStraightRailTrackFinish(Tuple<Canvas, Shape> args)
        {
            Canvas canvas = args.Item1;
            StraightRailTrack railTrack = (StraightRailTrack)args.Item2;

            Point mousePos = Mouse.GetPosition(canvas);
            railTrack.X2 = mousePos.X;
            railTrack.Y2 = mousePos.Y;

            railTrack.InvalidateMeasure();
        }

        public static void MoveSwitch(Tuple<Canvas, Shape> args)
        {
            throw new NotImplementedException("Switch");
        }
        public static void MoveSignal(Tuple<Canvas, Shape> args)
        {
            throw new NotImplementedException("Signal");
        }
        public static void MoveDeadend(Tuple<Canvas, Shape> args)
        {
            throw new NotImplementedException("Deadend");
        }
        public static void MoveExternalTrack(Tuple<Canvas, Shape> args)
        {
            throw new NotImplementedException("External track");
        }
    }
}
