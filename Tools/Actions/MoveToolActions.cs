using railway_monitor.Bases;
using railway_monitor.Components.GraphicItems;
using railway_monitor.Components.RailwayCanvas;
using System.Windows;

namespace railway_monitor.Tools.Actions
{
    public static class MoveToolActions
    {
        private static bool IsRailConnectable(Port connectionPort)
        {
            int switches = connectionPort.GraphicItems.OfType<SwitchItem>().Count();
            int srts = connectionPort.GraphicItems.OfType<StraightRailTrackItem>().Count();

            if (switches != 0 && srts >= 3) return false;
            return true;
        }

        public static void MoveStraightRailTrack(Tuple<RailwayCanvasViewModel, Point> args)
        {
            RailwayCanvasViewModel canvas = args.Item1;
            GraphicItem? item = canvas.LatestGraphicItem;
            if (item == null)
            {
                item = new StraightRailTrackItem();
                canvas.AddElement(item);
            }
            else if (item is not StraightRailTrackItem)
            {
                item = new StraightRailTrackItem();
                canvas.AddElement(item);
            }

            Point mousePos = args.Item2;
            StraightRailTrackItem srt = (StraightRailTrackItem)item;
            Port? connectionPort = canvas.TryFindUnderlyingPort(mousePos);
            Point connectionPos;
            if (connectionPort == null)
            {
                connectionPos = mousePos;
            }
            else
            {
                if (!IsRailConnectable(connectionPort))
                {
                    connectionPos = mousePos;
                    canvas.ConnectionErrorOccured = true;
                }
                else
                {
                    connectionPos = connectionPort.Pos;
                }
            }

            if (srt.Status == StraightRailTrackItem.PlacementStatus.NOT_PLACED)
            {
                srt.Start = connectionPos;
            }
            else
            {
                srt.End = connectionPos;
            }

            srt.Render();
        }

        private static bool IsSwitchConnectable(Port connectionPort)
        {
            return connectionPort.GraphicItems.OfType<StraightRailTrackItem>().Count() == 3 && connectionPort.GraphicItems.OfType<SwitchItem>().Count() == 0;
        }

        public static void MoveSwitch(Tuple<RailwayCanvasViewModel, Point> args)
        {
            RailwayCanvasViewModel canvas = args.Item1;
            GraphicItem? item = canvas.LatestGraphicItem;
            if (item == null)
            {
                item = new SwitchItem();
                canvas.AddElement(item);
            }
            else if (item is not SwitchItem)
            {
                item = new SwitchItem();
                canvas.AddElement(item);
            }

            Point mousePos = args.Item2;
            SwitchItem switchItem = (SwitchItem)item;
            Port? connectionPort = canvas.TryFindUnderlyingPort(mousePos);
            switch (switchItem.Status)
            {
                case SwitchItem.PlacementStatus.NOT_PLACED:
                    Point connectionPos;

                    if (connectionPort == null)
                    {
                        connectionPos = mousePos;
                    }
                    else 
                    {
                        if (!IsSwitchConnectable(connectionPort)) 
                        {
                            connectionPos = mousePos;
                            canvas.ConnectionErrorOccured = true;
                        }
                        else
                        {
                            connectionPos = connectionPort.Pos;
                        } 
                    }

                    switchItem.Pos = connectionPos;
                    switchItem.Render();
                    break;
                case SwitchItem.PlacementStatus.PLACED:
                    connectionPos = connectionPort == null ? mousePos : connectionPort.Pos;
                    switchItem.SrcPos = connectionPos;
                    switchItem.Render();
                    break;
                default:
                    return;
            }
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
        public static void MoveDrag(Tuple<RailwayCanvasViewModel, Point> args)
        {
            RailwayCanvasViewModel canvas = args.Item1;
            Point mousePos = args.Item2;
            if (canvas.DraggedPort != null)
            {
                canvas.DraggedPort.Pos = mousePos;
                canvas.RenderDraggedPort();
            }
            else
            {
                canvas.TryFindUnderlyingPort(mousePos);
            }
        }
    }
}
