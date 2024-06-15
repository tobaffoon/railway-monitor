﻿using railway_monitor.Components.GraphicItems;
using railway_monitor.Components.RailwayCanvas;
using System.Windows.Input;

namespace railway_monitor.Tools.Actions {
    public sealed class KeyboardActions {
        public static void CanvasKeyDown(Tuple<RailwayCanvasViewModel, Key> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            Key pressedKey = args.Item2;
            switch (pressedKey) {
                case Key.Escape:
                    canvas.DeleteLatestGraphicItem();
                    break;
            }
        }
        public static void RotateExternalTrack(Tuple<RailwayCanvasViewModel, Key> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            Key pressedKey = args.Item2;
            ExternalTrackItem? externalTrackItem = canvas.LatestGraphicItem as ExternalTrackItem;
            if (externalTrackItem == null) {
                return;
            }

            switch (pressedKey) {
                case Key.Left:
                    externalTrackItem.Orientation = ExternalTrackItem.ExternalTrackOrientation.LEFT;
                    break;
                case Key.Up:
                    externalTrackItem.Orientation = ExternalTrackItem.ExternalTrackOrientation.UP;
                    break;
                case Key.Right:
                    externalTrackItem.Orientation = ExternalTrackItem.ExternalTrackOrientation.RIGHT;
                    break;
                case Key.Down:
                    externalTrackItem.Orientation = ExternalTrackItem.ExternalTrackOrientation.DOWN;
                    break;
            }
        }
        public static void RotateDeadend(Tuple<RailwayCanvasViewModel, Key> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            Key pressedKey = args.Item2;
            DeadendItem? deadendItem = canvas.LatestGraphicItem as DeadendItem;
            if (deadendItem == null) {
                return;
            }

            switch (pressedKey) {
                case Key.Left:
                    deadendItem.Orientation = DeadendItem.DeadendOrientation.LEFT;
                    break;
                case Key.Up:
                    deadendItem.Orientation = DeadendItem.DeadendOrientation.UP;
                    break;
                case Key.Right:
                    deadendItem.Orientation = DeadendItem.DeadendOrientation.RIGHT;
                    break;
                case Key.Down:
                    deadendItem.Orientation = DeadendItem.DeadendOrientation.DOWN;
                    break;
            }
        }
    }
}
