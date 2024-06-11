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
            GraphicItem? item = canvas.LatestGraphicItem;
            if (item == null)
            {
                item = new StraightRailTrackItem();
                canvas.AddGraphicItemBehind(item);
            }
            else if (item is not StraightRailTrackItem)
            {
                canvas.DeleteLatestGraphicItem();
                item = new StraightRailTrackItem();
                canvas.AddGraphicItemBehind(item);
            }

            Point mousePos = args.Item2;
            StraightRailTrackItem srt = (StraightRailTrackItem)item;
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
                    canvas.ResetLatestGraphicItem();
                }
                return;
            }

            if (srt.Status == StraightRailTrackItem.PlacementStatus.NOT_PLACED) {
                srt.PlaceStartPoint(mousePos);
            }
            else
            {
                srt.PlaceEndPoint(mousePos);
                canvas.ResetLatestGraphicItem();
            }
        }
        public static void PlaceSwitch(Tuple<RailwayCanvasViewModel, Point> args)
        {
            RailwayCanvasViewModel canvas = args.Item1;
            GraphicItem? item = canvas.LatestGraphicItem;
            if (item == null)
            {
                item = new SwitchItem();
                canvas.AddGraphicItem(item);
            }
            else if (item is not SwitchItem)
            {
                canvas.DeleteLatestGraphicItem();
                item = new SwitchItem();
                canvas.AddGraphicItem(item);
            }

            Point mousePos = args.Item2;
            SwitchItem switchItem = (SwitchItem)item;
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
                    canvas.ResetLatestGraphicItem();
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
    }
}
