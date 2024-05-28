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
using railway_monitor.Bases;
using railway_monitor.Components.GraphicItems;
using railway_monitor.Components.RailwayCanvas;

namespace railway_monitor.Tools.Actions
{
    public sealed class ClickToolActions
    {
        public static void PlaceStraightRailTrack(Tuple<RailwayCanvasViewModel, Point> args)
        {
            RailwayCanvasViewModel canvas = args.Item1;
            Shape? shape = canvas.LatestShape;
            if (shape == null)
            {
                shape = new StraightRailTrackItem();
                canvas.AddShape(shape);
            }
            else if (shape is not StraightRailTrackItem)
            {
                canvas.DeleteLatestShape();
                shape = new StraightRailTrackItem();
                canvas.AddShape(shape);
            }

            Point mousePos = args.Item2;
            StraightRailTrackItem srt = (StraightRailTrackItem)shape;
            Port? connectionPort = canvas.TryFindRailConnection(mousePos);
            if(connectionPort != null)
            {
                if (srt.Status == StraightRailTrackItem.PlacementStatus.NOT_PLACED)
                {
                    srt.PlaceStartPoint(connectionPort);
                }
                else
                {
                    srt.PlaceEndPoint(connectionPort);
                    canvas.ResetLatestShape();
                }
                return;
            }

            if (srt.Status == StraightRailTrackItem.PlacementStatus.NOT_PLACED) {
                srt.PlaceStartPoint(mousePos);
            }
            else
            {
                srt.PlaceEndPoint(mousePos);
                canvas.ResetLatestShape();
            }
        }

        public static void PlaceSwitch(Tuple<RailwayCanvasViewModel, Point> args)
        {
            RailwayCanvasViewModel canvas = args.Item1;
            Shape? shape = canvas.LatestShape;
            if (shape == null)
            {
                shape = new SwitchItem();
                canvas.AddShape(shape);
            }
            else if (shape is not SwitchItem)
            {
                canvas.DeleteLatestShape();
                shape = new SwitchItem();
                canvas.AddShape(shape);
            }

            Point mousePos = args.Item2;
            SwitchItem switchItem = (SwitchItem)shape;
            Port? connectionPort = canvas.TryFindRailConnection(mousePos);
            if (connectionPort == null || connectionPort.GraphicItems.OfType<StraightRailTrackItem>().Count() != 3) 
            {
                return;
            }
            // Connect

            canvas.ResetLatestShape();
        }
        public static void PlaceSignal(Tuple<RailwayCanvasViewModel, Point> args)
        {
            throw new NotImplementedException("Signal");
        }
        public static void PlaceDeadend(Tuple<RailwayCanvasViewModel, Point> args)
        {
            throw new NotImplementedException("Deadend");
        }
        public static void PlaceExternalTrack(Tuple<RailwayCanvasViewModel, Point> args)
        {
            throw new NotImplementedException("External track");
        }

    }
}
