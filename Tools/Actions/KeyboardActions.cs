using railway_monitor.Components.RailwayCanvas;
using System.Windows;

namespace railway_monitor.Tools.Actions {
    public sealed class KeyboardActions {
        public static void RemoveLatestShape(Tuple<RailwayCanvasViewModel, Point> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            canvas.DeleteLatestGraphicItem();
        }
    }
}
