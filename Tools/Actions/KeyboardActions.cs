using railway_monitor.Components.RailwayCanvas;

namespace railway_monitor.Tools.Actions {
    public sealed class KeyboardActions {
        public static void RemoveLatestItem(RailwayCanvasViewModel canvas) {
            canvas.DeleteLatestGraphicItem();
        }
    }
}
