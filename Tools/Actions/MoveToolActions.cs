using railway_monitor.Bases;
using railway_monitor.Components.GraphicItems;
using railway_monitor.Components.RailwayCanvas;
using railway_monitor.Utils;
using System.Windows;

namespace railway_monitor.Tools.Actions {
    public static class MoveToolActions {
        public static void MoveStraightRailTrack(Tuple<RailwayCanvasViewModel, Point> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            Point mousePos = args.Item2;
            StraightRailTrackItem? srtItem = canvas.LatestGraphicItem as StraightRailTrackItem;
            if (srtItem == null) {
                srtItem = new StraightRailTrackItem(mousePos);
                canvas.AddGraphicItemBehind(srtItem);
            }

            // calculate proper point for port placement
            Port? connectionPort = canvas.TryFindUnderlyingPort(mousePos);
            Point connectionPos;
            if (connectionPort == null) {
                connectionPos = mousePos;
            }
            else {
                if (!ConnectConditions.IsRailConnectable(connectionPort)) {
                    connectionPos = mousePos;
                    canvas.ConnectionErrorOccured = true;
                }
                else {
                    connectionPos = connectionPort.Pos;
                }
            }

            // place port
            if (srtItem.PlacementStatus == StraightRailTrackItem.RailPlacementStatus.NOT_PLACED) {
                srtItem.Start = connectionPos;
            }
            else {
                srtItem.End = connectionPos;
            }

            srtItem.Render();
        }
        public static void MoveSwitch(Tuple<RailwayCanvasViewModel, Point> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            Point mousePos = args.Item2;
            SwitchItem? switchItem = canvas.LatestGraphicItem as SwitchItem;
            if (switchItem == null) {
                switchItem = new SwitchItem(mousePos);
                canvas.AddGraphicItem(switchItem);
            }

            Port? connectionPort = canvas.TryFindUnderlyingPort(mousePos);
            switch (switchItem.PlacementStatus) {
                case SwitchItem.SwitchPlacementStatus.NOT_PLACED:
                    Point connectionPos;

                    if (connectionPort == null) {
                        connectionPos = mousePos;
                    }
                    else {
                        if (!ConnectConditions.IsSwitchConnectable(connectionPort)) {
                            connectionPos = mousePos;
                            canvas.ConnectionErrorOccured = true;
                        }
                        else {
                            connectionPos = connectionPort.Pos;
                        }
                    }

                    switchItem.Pos = connectionPos;
                    switchItem.Render();
                    break;
                case SwitchItem.SwitchPlacementStatus.PLACED:
                    if (connectionPort == null) {
                        connectionPos = mousePos;
                    }
                    else {
                        if (!switchItem.IsSourceValid(connectionPort)) {
                            connectionPos = mousePos;
                            canvas.ConnectionErrorOccured = true;
                        }
                        else {
                            connectionPos = connectionPort.Pos;
                        }
                    }
                    switchItem.SrcPos = connectionPos;
                    switchItem.Render();
                    break;
            }
        }
        public static void MoveSignal(Tuple<RailwayCanvasViewModel, Point> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            Point mousePos = args.Item2;
            SignalItem? signalItem = canvas.LatestGraphicItem as SignalItem;
            if (signalItem is not SignalItem) {
                signalItem = new SignalItem(mousePos);
                canvas.AddGraphicItem(signalItem);
            }

            Port? connectionPort = canvas.TryFindUnderlyingPort(mousePos);
            switch (signalItem.PlacementStatus) {
                case SignalItem.SignalPlacementStatus.NOT_PLACED:
                    Point connectionPos;

                    if (connectionPort == null) {
                        connectionPos = mousePos;
                    }
                    else {
                        if (!ConnectConditions.IsSignalConnectable(connectionPort)) {
                            connectionPos = mousePos;
                            canvas.ConnectionErrorOccured = true;
                        }
                        else {
                            connectionPos = connectionPort.Pos;
                        }
                    }

                    signalItem.Pos = connectionPos;
                    signalItem.Render();
                    break;
            }
        }
        public static void MoveDeadend(Tuple<RailwayCanvasViewModel, Point> args) {
            throw new NotImplementedException("Deadend");
        }
        public static void MoveExternalTrack(Tuple<RailwayCanvasViewModel, Point> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            Point mousePos = args.Item2;
            ExternalTrackItem? externalTrackItem = canvas.LatestGraphicItem as ExternalTrackItem;
            if (externalTrackItem == null) {
                externalTrackItem = new ExternalTrackItem(mousePos);
                canvas.AddGraphicItem(externalTrackItem);
            }

            Port? connectionPort = canvas.TryFindUnderlyingPort(mousePos);
            switch (externalTrackItem.PlacementStatus) {
                case ExternalTrackItem.ExternalTrackPlacementStatus.NOT_PLACED:
                    Point connectionPos;

                    if (connectionPort == null) {
                        connectionPos = mousePos;
                    }
                    else {
                        if (!ConnectConditions.IsExternalTrackConnectable(connectionPort)) {
                            connectionPos = mousePos;
                            canvas.ConnectionErrorOccured = true;
                        }
                        else {
                            connectionPos = connectionPort.Pos;
                        }
                    }

                    externalTrackItem.Pos = connectionPos;
                    externalTrackItem.Render();
                    break;
            }
        }
        public static void MoveDrag(Tuple<RailwayCanvasViewModel, Point> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            Point mousePos = args.Item2;
            if (canvas.DraggedPort != null) {
                canvas.DraggedPort.Pos = mousePos;
                canvas.RenderDraggedPort();
            }
            else {
                canvas.TryFindUnderlyingPort(mousePos);
            }
        }
    }
}
