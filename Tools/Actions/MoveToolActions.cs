﻿using railway_monitor.Components.GraphicItems;
using railway_monitor.Components.RailwayCanvas;
using System.Windows;
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
                shape = new StraightRailTrackItem();
                canvas.AddShape(shape);
            }

            Point mousePos = args.Item2;
            Point connectionPos = canvas.TryFindRailConnection(mousePos);
            StraightRailTrackItem srt = (StraightRailTrackItem)shape;
            if (srt.Status == StraightRailTrackItem.PlacementStatus.NOT_PLACED)
            {
                srt.X1 = connectionPos.X;
                srt.Y1 = connectionPos.Y;
            }
            else
            {
                srt.X2 = connectionPos.X;
                srt.Y2 = connectionPos.Y;
            }

            srt.InvalidateMeasure();
        }

        public static void MoveSwitch(Tuple<RailwayCanvasViewModel, Point> args)
        {
            RailwayCanvasViewModel canvas = args.Item1;
            Shape? shape = canvas.LatestShape;
            if (shape == null)
            {
                shape = new SwitchItem();
                canvas.AddShape(shape);
            }

            Point mousePos = args.Item2;
            Point connectionPos = canvas.TryFindRailConnection(mousePos);
            SwitchItem switchItem = (SwitchItem)shape;
            switchItem.Pos = connectionPos;

            switchItem.InvalidateMeasure();
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
