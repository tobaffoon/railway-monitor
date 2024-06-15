using railway_monitor.Bases;
using railway_monitor.Components.GraphicItems;
using railway_monitor.Components.RailwayCanvas;
using railway_monitor.Utils;
using System.Windows;

namespace railway_monitor.Tools.Actions {
    public static class LeftClickToolActions {
        public static void PlaceStraightRailTrack(Tuple<RailwayCanvasViewModel, Point> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            Point mousePos = args.Item2;
            StraightRailTrackItem? srtItem = canvas.LatestGraphicItem as StraightRailTrackItem;
            if (srtItem == null) {
                srtItem = new StraightRailTrackItem(mousePos);
                canvas.AddGraphicItemBehind(srtItem);
            }

            Port? connectionPort = canvas.TryFindUnderlyingPort(mousePos);
            if (connectionPort != null && ConnectConditions.IsRailConnectable(connectionPort)) {
                // connection port is found and latest srt can be connected to it
                if (srtItem.PlacementStatus == StraightRailTrackItem.RailPlacementStatus.NOT_PLACED) {
                    srtItem.PlaceStartPoint(connectionPort);
                }
                else {
                    srtItem.PlaceEndPoint(connectionPort);
                    canvas.ResetLatestGraphicItem();
                }
            }
            else {
                // connection port is not found or latest srt cannot be connected to it
                if (srtItem.PlacementStatus == StraightRailTrackItem.RailPlacementStatus.NOT_PLACED) {
                    srtItem.PlaceStartPoint(mousePos);
                }
                else {
                    srtItem.PlaceEndPoint(mousePos);
                    canvas.ResetLatestGraphicItem();
                }
            }
        }
        public static void AddPlatform(Tuple<RailwayCanvasViewModel, Point> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            if (canvas.ConnectionPlatformTrack == null) return;

            StraightRailTrackItem srtItem = canvas.ConnectionPlatformTrack;
            switch (srtItem.PlatformType) {
                case StraightRailTrackItem.RailPlatformType.PASSENGER_HOVER:
                    srtItem.PlatformType = StraightRailTrackItem.RailPlatformType.PASSENGER; 
                    break;
                case StraightRailTrackItem.RailPlatformType.CARGO_HOVER:
                    srtItem.PlatformType = StraightRailTrackItem.RailPlatformType.CARGO; 
                    break;
            }
        }
        public static void PlaceSwitch(Tuple<RailwayCanvasViewModel, Point> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            Point mousePos = args.Item2;
            SwitchItem? switchItem = canvas.LatestGraphicItem as SwitchItem;
            if (switchItem == null) {
                switchItem = new SwitchItem(mousePos);
                canvas.AddGraphicItem(switchItem);
            }

            switch (switchItem.PlacementStatus) {
                case SwitchItem.SwitchPlacementStatus.NOT_PLACED:
                    Port? connectionPort = canvas.TryFindUnderlyingPort(mousePos);
                    if (connectionPort == null || !ConnectConditions.IsSwitchConnectable(connectionPort)) {
                        // no port found for connection
                        return;
                    }
                    switchItem.Place(connectionPort);
                    break;
                case SwitchItem.SwitchPlacementStatus.PLACED:
                    connectionPort = canvas.TryFindUnderlyingPort(mousePos);
                    if (connectionPort == null || !switchItem.IsSourceValid(connectionPort)) {
                        // no valid source port found
                        return;
                    }
                    switchItem.SetSource(connectionPort);
                    canvas.ResetLatestGraphicItem();
                    break;

            }
        }
        public static void PlaceSignal(Tuple<RailwayCanvasViewModel, Point> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            Point mousePos = args.Item2;
            SignalItem? signalItem = canvas.LatestGraphicItem as SignalItem;
            if (signalItem == null) {
                signalItem = new SignalItem(mousePos);
                canvas.AddGraphicItem(signalItem);
            }

            switch (signalItem.PlacementStatus) {
                case SignalItem.SignalPlacementStatus.NOT_PLACED:
                    Port? connectionPort = canvas.TryFindUnderlyingPort(mousePos);
                    if (connectionPort == null || !ConnectConditions.IsSignalConnectable(connectionPort)) {
                        // no port found for connection
                        return;
                    }
                    signalItem.Place(connectionPort);
                    canvas.ResetLatestGraphicItem();
                    break;
            }
        }
        public static void PlaceDeadend(Tuple<RailwayCanvasViewModel, Point> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            Point mousePos = args.Item2;
            DeadendItem? deadendItem = canvas.LatestGraphicItem as DeadendItem;
            if (deadendItem == null) {
                deadendItem = new DeadendItem(mousePos);
                canvas.AddGraphicItem(deadendItem);
            }

            switch (deadendItem.PlacementStatus) {
                case DeadendItem.DeadendPlacementStatus.NOT_PLACED:
                    Port? connectionPort = canvas.TryFindUnderlyingPort(mousePos);
                    if (connectionPort == null || !ConnectConditions.IsDeadendConnectable(connectionPort)) {
                        // no port found for connection
                        return;
                    }
                    deadendItem.Place(connectionPort);
                    canvas.ResetLatestGraphicItem();
                    break;
            }
        }
        public static void PlaceExternalTrack(Tuple<RailwayCanvasViewModel, Point> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            Point mousePos = args.Item2;
            ExternalTrackItem? externalTrackItem = canvas.LatestGraphicItem as ExternalTrackItem;
            if (externalTrackItem == null) {
                externalTrackItem = new ExternalTrackItem(mousePos);
                canvas.AddGraphicItem(externalTrackItem);
            }

            switch (externalTrackItem.PlacementStatus) {
                case ExternalTrackItem.ExternalTrackPlacementStatus.NOT_PLACED:
                    Port? connectionPort = canvas.TryFindUnderlyingPort(mousePos);
                    if (connectionPort == null || !ConnectConditions.IsExternalTrackConnectable(connectionPort)) {
                        // no port found for connection
                        return;
                    }
                    externalTrackItem.Place(connectionPort);
                    canvas.ResetLatestGraphicItem();
                    break;
            }
        }
        public static void CaptureDrag(Tuple<RailwayCanvasViewModel, Point> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            Point mousePos = args.Item2;
            Port? connectionPort = canvas.TryFindUnderlyingPort(mousePos);
            if (connectionPort == null) return;

            canvas.DraggedPort = connectionPort;
        }
    }
}
