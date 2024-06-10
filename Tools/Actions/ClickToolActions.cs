using System.Diagnostics;
using System.Windows;
using railway_monitor.Bases;
using railway_monitor.Components.GraphicItems;
using railway_monitor.Components.RailwayCanvas;

namespace railway_monitor.Tools.Actions
{
    public static class ClickToolActions
    {
        public static void PlaceStraightRailTrack(Tuple<RailwayCanvasViewModel, Point> args)
        {
            RailwayCanvasViewModel canvas = args.Item1;
            FrameworkElement? element = canvas.LatestElement;
            if (element == null)
            {
                element = new StraightRailTrackItem();
                canvas.AddElement(element);
            }
            else if (element is not StraightRailTrackItem)
            {
                canvas.DeleteLatestElement();
                element = new StraightRailTrackItem();
                canvas.AddElement(element);
            }

            Point mousePos = args.Item2;
            StraightRailTrackItem srt = (StraightRailTrackItem)element;
            Port? connectionPort = canvas.TryFindUnderlyingPort(mousePos);
            if(connectionPort != null)
            {
                if (srt.Status == StraightRailTrackItem.PlacementStatus.NOT_PLACED)
                {
                    srt.PlaceStartPoint(connectionPort);
                }
                else
                {
                    srt.PlaceEndPoint(connectionPort);
                    canvas.ResetLatestElement();
                }
                return;
            }

            if (srt.Status == StraightRailTrackItem.PlacementStatus.NOT_PLACED) {
                srt.PlaceStartPoint(mousePos);
            }
            else
            {
                srt.PlaceEndPoint(mousePos);
                canvas.ResetLatestElement();
            }
        }
        public static void PlaceSwitch(Tuple<RailwayCanvasViewModel, Point> args)
        {
            RailwayCanvasViewModel canvas = args.Item1;
            FrameworkElement? element = canvas.LatestElement;
            if (element == null)
            {
                element = new SwitchItem();
                canvas.AddElement(element);
            }
            else if (element is not SwitchItem)
            {
                canvas.DeleteLatestElement();
                element = new SwitchItem();
                canvas.AddElement(element);
            }

            Point mousePos = args.Item2;
            SwitchItem switchItem = (SwitchItem)element;
            switch (switchItem.Status)
            {
                case SwitchItem.PlacementStatus.NOT_PLACED:
                    Port? connectionPort = canvas.TryFindUnderlyingPort(mousePos);
                    if (connectionPort == null || connectionPort.GraphicItems.OfType<StraightRailTrackItem>().Count() != 3 || connectionPort.GraphicItems.OfType<SwitchItem>().Count() != 0)
                    {
                        return;
                    }
                    switchItem.Place(connectionPort);
                    break;
                case SwitchItem.PlacementStatus.PLACED:
                    connectionPort = canvas.TryFindUnderlyingPort(mousePos);
                    if (connectionPort == null)
                    {
                        return;
                    }
                    switchItem.SetSource(connectionPort);
                    if(switchItem.Status == SwitchItem.PlacementStatus.PLACED)
                    {
                        Trace.WriteLine("Can't set Exterior ports as Switch's source port");
                        return;
                    }
                    canvas.ResetLatestElement();
                    switchItem.Render();
                    break;

            }
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
        public static void CaptureDrag(Tuple<RailwayCanvasViewModel, Point> args)
        {
            RailwayCanvasViewModel canvas = args.Item1;
            Point mousePos = args.Item2;
            Port? connectionPort = canvas.TryFindUnderlyingPort(mousePos);
            if (connectionPort == null) return;

            canvas.DraggedPort = connectionPort;
        }
        public static void ReleaseDrag(Tuple<RailwayCanvasViewModel, Point> args)
        {
            RailwayCanvasViewModel canvas = args.Item1;
            if (canvas.DraggedPort != null) canvas.DraggedPort = null;
        }
    }
}
