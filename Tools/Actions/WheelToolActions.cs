using railway_monitor.Components.RailwayCanvas;
using railway_monitor.Components.TopologyItems;

namespace railway_monitor.Tools.Actions {
    public static class WheelToolActions {
        public static void RotateExternalTrack(Tuple<RailwayCanvasViewModel, bool> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            bool IsRotationClockwise = args.Item2;
            ExternalTrackItem? externalTrackItem = canvas.LatestTopologyItem as ExternalTrackItem;
            if (externalTrackItem == null) {
                return;
            }

            if (IsRotationClockwise) {
                switch (externalTrackItem.Orientation) {
                    case ExternalTrackItem.ExternalTrackOrientation.RIGHT:
                        externalTrackItem.Orientation = ExternalTrackItem.ExternalTrackOrientation.DOWN;
                        break;
                    case ExternalTrackItem.ExternalTrackOrientation.DOWN:
                        externalTrackItem.Orientation = ExternalTrackItem.ExternalTrackOrientation.LEFT;
                        break;
                    case ExternalTrackItem.ExternalTrackOrientation.LEFT:
                        externalTrackItem.Orientation = ExternalTrackItem.ExternalTrackOrientation.UP;
                        break;
                    case ExternalTrackItem.ExternalTrackOrientation.UP:
                        externalTrackItem.Orientation = ExternalTrackItem.ExternalTrackOrientation.RIGHT;
                        break;
                }
            }
            else {
                switch (externalTrackItem.Orientation) {
                    case ExternalTrackItem.ExternalTrackOrientation.RIGHT:
                        externalTrackItem.Orientation = ExternalTrackItem.ExternalTrackOrientation.UP;
                        break;
                    case ExternalTrackItem.ExternalTrackOrientation.UP:
                        externalTrackItem.Orientation = ExternalTrackItem.ExternalTrackOrientation.LEFT;
                        break;
                    case ExternalTrackItem.ExternalTrackOrientation.LEFT:
                        externalTrackItem.Orientation = ExternalTrackItem.ExternalTrackOrientation.DOWN;
                        break;
                    case ExternalTrackItem.ExternalTrackOrientation.DOWN:
                        externalTrackItem.Orientation = ExternalTrackItem.ExternalTrackOrientation.RIGHT;
                        break;
                }
            }
        }
        public static void RotateDeadend(Tuple<RailwayCanvasViewModel, bool> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            bool IsRotationClockwise = args.Item2;
            DeadendItem? deadendItem = canvas.LatestTopologyItem as DeadendItem;
            if (deadendItem == null) {
                return;
            }

            if (IsRotationClockwise) {
                switch (deadendItem.Orientation) {
                    case DeadendItem.DeadendOrientation.RIGHT:
                        deadendItem.Orientation = DeadendItem.DeadendOrientation.DOWN;
                        break;
                    case DeadendItem.DeadendOrientation.DOWN:
                        deadendItem.Orientation = DeadendItem.DeadendOrientation.LEFT;
                        break;
                    case DeadendItem.DeadendOrientation.LEFT:
                        deadendItem.Orientation = DeadendItem.DeadendOrientation.UP;
                        break;
                    case DeadendItem.DeadendOrientation.UP:
                        deadendItem.Orientation = DeadendItem.DeadendOrientation.RIGHT;
                        break;
                }
            }
            else {
                switch (deadendItem.Orientation) {
                    case DeadendItem.DeadendOrientation.RIGHT:
                        deadendItem.Orientation = DeadendItem.DeadendOrientation.UP;
                        break;
                    case DeadendItem.DeadendOrientation.UP:
                        deadendItem.Orientation = DeadendItem.DeadendOrientation.LEFT;
                        break;
                    case DeadendItem.DeadendOrientation.LEFT:
                        deadendItem.Orientation = DeadendItem.DeadendOrientation.DOWN;
                        break;
                    case DeadendItem.DeadendOrientation.DOWN:
                        deadendItem.Orientation = DeadendItem.DeadendOrientation.RIGHT;
                        break;
                }
            }
        }
    }
}
