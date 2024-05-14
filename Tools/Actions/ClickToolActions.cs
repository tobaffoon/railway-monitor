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

            Point mousePos = args.Item2;
            Point connectionPos = canvas.TryFindRailConnection(mousePos);
            StraightRailTrackItem srt = (StraightRailTrackItem)shape;
            if (srt.Status == StraightRailTrackItem.PlacementStatus.NOT_PLACED) {
                srt.PlaceFirstEnd(connectionPos);
            }
            else
            {
                srt.PlaceSecondEnd(connectionPos);
                canvas.ResetLatestShape();
            }
        }

        public static void PlaceSwitch(Tuple<RailwayCanvasViewModel, Point> args)
        {
            throw new NotImplementedException("Switch");
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
