using railway_monitor.Components.RailwayCanvas;

namespace railway_monitor.Tools.Actions {
    public static class LeftReleaseToolActions {
        public static void ReleaseDrag(RailwayCanvasViewModel canvas) {
            canvas.DraggedPort = null;
        }
    }
}
