using railway_monitor.Bases;
using railway_monitor.Components.RailwayCanvas;
using railway_monitor.Components.TopologyItems;
using railway_monitor.Utils;
using System.Windows;

namespace railway_monitor.Tools.Actions {
    public static class LeftClickToolActions {
        public static void PlaceStraightRailTrack(Tuple<RailwayCanvasViewModel, Point> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            Point mousePos = args.Item2;
            StraightRailTrackItem? srtItem = canvas.LatestTopologyItem as StraightRailTrackItem;
            if (srtItem == null) {
                srtItem = new StraightRailTrackItem(mousePos);
                canvas.AddTopologyItemBehind(srtItem);
            }

            Port? connectionPort = canvas.TryFindUnderlyingPort(mousePos);
            if (connectionPort != null && ConnectConditions.IsRailConnectable(connectionPort)) {
                // connection port is found and latest srt can be connected to it
                if (srtItem.PlacementStatus == StraightRailTrackItem.RailPlacementStatus.NOT_PLACED) {
                    srtItem.PlaceStartPoint(connectionPort);
                }
                else {
                    srtItem.PlaceEndPoint(connectionPort);
                    canvas.ResetLatestTopologyItem();
                }
            }
            else if (connectionPort == null) {
                // connection port is not found or latest srt cannot be connected to it
                if (srtItem.PlacementStatus == StraightRailTrackItem.RailPlacementStatus.NOT_PLACED) {
                    srtItem.PlaceStartPoint(mousePos);
                }
                else {
                    srtItem.PlaceEndPoint(mousePos);
                    canvas.ResetLatestTopologyItem();
                }
            }
        }
        public static void AddPlatform(Tuple<RailwayCanvasViewModel, Point> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            StraightRailTrackItem? srtItem = canvas.ConnectionPlatformTrack;
            if (srtItem == null || ConnectConditions.RailHasPlatform(srtItem)) return;

            switch (srtItem.PlatformType) {
                case StraightRailTrackItem.RailPlatformType.PASSENGER_HOVER:
                    srtItem.PlatformType = StraightRailTrackItem.RailPlatformType.PASSENGER;
                    break;
                case StraightRailTrackItem.RailPlatformType.CARGO_HOVER:
                    srtItem.PlatformType = StraightRailTrackItem.RailPlatformType.CARGO;
                    break;
                case StraightRailTrackItem.RailPlatformType.NONE_HOVER:
                    srtItem.PlatformType = StraightRailTrackItem.RailPlatformType.NONE_HOVER;
                    break;
            }
        }
        public static void PlaceSwitch(Tuple<RailwayCanvasViewModel, Point> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            Point mousePos = args.Item2;
            SwitchItem? switchItem = canvas.LatestTopologyItem as SwitchItem;
            if (switchItem == null) {
                switchItem = new SwitchItem(mousePos);
                canvas.AddTopologyItem(switchItem);
            }

            switch (switchItem.PlacementStatus) {
                case SwitchItem.SwitchPlacementStatus.NOT_PLACED:
                    Port? connectionPort = canvas.TryFindUnderlyingPort(mousePos);
                    if (connectionPort == null || !ConnectConditions.IsSwitchConnectable(connectionPort)) {
                        // no port found for connection
                        return;
                    }
                    switchItem.Place(connectionPort);
                    canvas.ResetLatestTopologyItem();
                    break;
            }
        }
        public static void PlaceSignal(Tuple<RailwayCanvasViewModel, Point> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            Point mousePos = args.Item2;
            SignalItem? signalItem = canvas.LatestTopologyItem as SignalItem;
            if (signalItem == null) {
                signalItem = new SignalItem(mousePos);
                canvas.AddTopologyItem(signalItem);
            }

            switch (signalItem.PlacementStatus) {
                case SignalItem.SignalPlacementStatus.NOT_PLACED:
                    Port? connectionPort = canvas.TryFindUnderlyingPort(mousePos);
                    if (connectionPort == null || !ConnectConditions.IsSignalConnectable(connectionPort)) {
                        // no port found for connection
                        return;
                    }
                    signalItem.Place(connectionPort);
                    canvas.ResetLatestTopologyItem();
                    break;
            }
        }
        public static void PlaceDeadend(Tuple<RailwayCanvasViewModel, Point> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            Point mousePos = args.Item2;
            DeadendItem? deadendItem = canvas.LatestTopologyItem as DeadendItem;
            if (deadendItem == null) {
                deadendItem = new DeadendItem(mousePos);
                canvas.AddTopologyItem(deadendItem);
            }

            switch (deadendItem.PlacementStatus) {
                case DeadendItem.DeadendPlacementStatus.NOT_PLACED:
                    Port? connectionPort = canvas.TryFindUnderlyingPort(mousePos);
                    if (connectionPort == null || !ConnectConditions.IsDeadendConnectable(connectionPort)) {
                        // no port found for connection
                        return;
                    }
                    deadendItem.Place(connectionPort);
                    canvas.ResetLatestTopologyItem();
                    break;
            }
        }
        public static void PlaceExternalTrack(Tuple<RailwayCanvasViewModel, Point> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            Point mousePos = args.Item2;
            ExternalTrackItem? externalTrackItem = canvas.LatestTopologyItem as ExternalTrackItem;
            if (externalTrackItem == null) {
                externalTrackItem = new ExternalTrackItem(mousePos);
                canvas.AddTopologyItem(externalTrackItem);
            }

            switch (externalTrackItem.PlacementStatus) {
                case ExternalTrackItem.ExternalTrackPlacementStatus.NOT_PLACED:
                    Port? connectionPort = canvas.TryFindUnderlyingPort(mousePos);
                    if (connectionPort == null || !ConnectConditions.IsExternalTrackConnectable(connectionPort)) {
                        // no port found for connection
                        return;
                    }
                    externalTrackItem.Place(connectionPort);
                    canvas.ResetLatestTopologyItem();
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
        public static void TestActions(Tuple<RailwayCanvasViewModel, Point> args) {
        }
    }
}
