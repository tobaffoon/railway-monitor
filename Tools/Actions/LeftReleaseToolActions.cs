using railway_monitor.Components.RailwayCanvas;
using System.Windows;

namespace railway_monitor.Tools.Actions {
    public static class LeftReleaseToolActions {
        public static void ReleaseDrag(Tuple<RailwayCanvasViewModel, Point> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            if (canvas.DraggedPort != null) canvas.DraggedPort = null;
        }
    }
}
