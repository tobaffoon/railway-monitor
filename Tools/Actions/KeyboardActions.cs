using railway_monitor.Components.RailwayCanvas;

namespace railway_monitor.Tools.Actions {
    public sealed class KeyboardActions {
        public static void RemoveLatestShape(RailwayCanvasViewModel canvas) {
            canvas.DeleteLatestGraphicItem();
        }
    }
}
