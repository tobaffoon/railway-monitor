﻿using railway_monitor.Bases;
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
        }
        public static void MovePlatform(Tuple<RailwayCanvasViewModel, Point> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            Point mousePos = args.Item2;
            PlatformItem? platformItem = canvas.LatestGraphicItem as PlatformItem;
            if (platformItem == null) {
                platformItem = new PlatformItem(mousePos);
                canvas.AddGraphicItemBehind(platformItem);
            }

            platformItem.Visibility = Visibility.Visible;
            platformItem.ConnectionErrorOccured = false;
            StraightRailTrackItem? previousSrt = canvas.ConnectionPlatformTrack;
            StraightRailTrackItem? connectionSrt = canvas.TryFindRailForPlatform(mousePos);
            Point connectionPos;
            if (connectionSrt == null) {
                connectionPos = mousePos;
            }
            else if (ConnectConditions.RailHasPlatform(connectionSrt)) {
                // srt already has platform 
                connectionPos = mousePos;
                platformItem.ConnectionErrorOccured = true;
            }
            else {
                platformItem.Visibility = Visibility.Collapsed;

                connectionPos = connectionSrt.Center;
                switch (platformItem.PlatformType) {
                    case PlatformItem.MiniPlatformType.PASSENGER:
                        connectionSrt.PlatformType = StraightRailTrackItem.RailPlatformType.PASSENGER_HOVER;
                        break;
                    case PlatformItem.MiniPlatformType.CARGO:
                        connectionSrt.PlatformType = StraightRailTrackItem.RailPlatformType.CARGO_HOVER;
                        break;
                }
            }

            // previously connected srt was left
            if (previousSrt != null && previousSrt != connectionSrt
                && (previousSrt.PlatformType == StraightRailTrackItem.RailPlatformType.PASSENGER_HOVER
                    || previousSrt.PlatformType == StraightRailTrackItem.RailPlatformType.CARGO_HOVER)) {
                    previousSrt.PlatformType = StraightRailTrackItem.RailPlatformType.NONE;
            }

            platformItem.Pos = connectionPos;
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
                    break;
            }
        }
        public static void MoveDeadend(Tuple<RailwayCanvasViewModel, Point> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            Point mousePos = args.Item2;
            DeadendItem? deadendItem = canvas.LatestGraphicItem as DeadendItem;
            if (deadendItem == null) {
                deadendItem = new DeadendItem(mousePos);
                canvas.AddGraphicItem(deadendItem);
            }

            Port? connectionPort = canvas.TryFindUnderlyingPort(mousePos);
            switch (deadendItem.PlacementStatus) {
                case DeadendItem.DeadendPlacementStatus.NOT_PLACED:
                    Point connectionPos;

                    if (connectionPort == null) {
                        connectionPos = mousePos;
                    }
                    else {
                        if (!ConnectConditions.IsDeadendConnectable(connectionPort)) {
                            connectionPos = mousePos;
                            canvas.ConnectionErrorOccured = true;
                        }
                        else {
                            connectionPos = connectionPort.Pos;
                        }
                    }

                    deadendItem.Pos = connectionPos;
                    break;
            }
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
