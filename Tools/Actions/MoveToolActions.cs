using railway_monitor.Components.GraphicItems;
using railway_monitor.Components.RailwayCanvas;
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
    public sealed class MoveToolActions
    {
        public static void MoveStraightRailTrack(Tuple<RailwayCanvasViewModel, Point> args)
        {
            RailwayCanvasViewModel canvas = args.Item1;
            Shape? shape = canvas.LatestShape;
            if (shape == null)
            {
                shape = new StraightRailTrack();
                canvas.AddShape(shape);
            }

            Point mousePos = args.Item2;
            StraightRailTrack srt = (StraightRailTrack)shape;
            if (srt.Status == StraightRailTrack.PlacementStatus.NOT_PLACED)
            {
                srt.X1 = mousePos.X;
                srt.Y1 = mousePos.Y;
                srt.X2 = mousePos.X;
                srt.Y2 = mousePos.Y;
            }
            else
            {
                srt.X2 = mousePos.X;
                srt.Y2 = mousePos.Y;
            }

            srt.InvalidateMeasure();
        }

        public static void MoveSwitch(Tuple<RailwayCanvasViewModel, Point> args)
        {
            throw new NotImplementedException("Switch");
        }
        public static void MoveSignal(Tuple<RailwayCanvasViewModel, Point> args)
        {
            throw new NotImplementedException("Signal");
        }
        public static void MoveDeadend(Tuple<RailwayCanvasViewModel, Point> args)
        {
            throw new NotImplementedException("Deadend");
        }
        public static void MoveExternalTrack(Tuple<RailwayCanvasViewModel, Point> args)
        {
            throw new NotImplementedException("External track");
        }
    }
}
