using railway_monitor.Components.GraphicItems;
using railway_monitor.Components.RailwayCanvas;

namespace railway_monitor.Tools.Actions {
    public static class WheelToolActions {
        public static void RotateExternalTrack(Tuple<RailwayCanvasViewModel, bool> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            bool IsRotationClockwise = args.Item2;
            ExternalTrackItem? externalTrackItem = canvas.LatestGraphicItem as ExternalTrackItem;
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
    }
}
