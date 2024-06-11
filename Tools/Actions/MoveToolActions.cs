using railway_monitor.Bases;
using railway_monitor.Components.GraphicItems;
using railway_monitor.Components.RailwayCanvas;
using railway_monitor.Utils;
using System.Diagnostics;
using System.Windows;

namespace railway_monitor.Tools.Actions
{
    public static class MoveToolActions
    {
        public static void MoveStraightRailTrack(Tuple<RailwayCanvasViewModel, Point> args)
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
                item = new StraightRailTrackItem();
                canvas.AddGraphicItemBehind(item);
            }

            Point mousePos = args.Item2;
            StraightRailTrackItem srt = (StraightRailTrackItem)item;
            Port? connectionPort = canvas.TryFindUnderlyingPort(mousePos);
            
            // calculate proper point for port placement
            Point connectionPos;
            if (connectionPort == null)
            {
                connectionPos = mousePos;
            }
            else
            {
                if (!ConnectConditions.IsRailConnectable(connectionPort))
                {
                    connectionPos = mousePos;
                    canvas.ConnectionErrorOccured = true;
                }
                else
                {
                    connectionPos = connectionPort.Pos;
                }
            }

            // place port
            if (srt.PlacementStatus == StraightRailTrackItem.RailPlacementStatus.NOT_PLACED)
            {
                srt.Start = connectionPos;
            }
            else
            {
                srt.End = connectionPos;
            }

            srt.Render();
        }

        public static void MoveSwitch(Tuple<RailwayCanvasViewModel, Point> args)
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
                item = new SwitchItem();
                canvas.AddGraphicItem(item);
            }

            Point mousePos = args.Item2;
            SwitchItem switchItem = (SwitchItem)item;
            Port? connectionPort = canvas.TryFindUnderlyingPort(mousePos);
            switch (switchItem.PlacementStatus)
            {
                case SwitchItem.SwitchPlacementStatus.NOT_PLACED:
                    Point connectionPos;

                    if (connectionPort == null)
                    {
                        connectionPos = mousePos;
                    }
                    else 
                    {
                        if (!ConnectConditions.IsSwitchConnectable(connectionPort)) 
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
                case SwitchItem.SwitchPlacementStatus.PLACED:
                    if (connectionPort == null)
                    {
                        connectionPos = mousePos;
                    }
                    else
                    {
                        if (!switchItem.IsSourceValid(connectionPort))
                        {
                            connectionPos = mousePos;
                            canvas.ConnectionErrorOccured = true;
                        }
                        else
                        {
                            connectionPos = connectionPort.Pos;
                        }
                    }
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
