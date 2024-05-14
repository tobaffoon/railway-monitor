using System.Windows;
using railway_monitor.Components.RailwayCanvas;

namespace railway_monitor.Tools.Actions
{
    public sealed class KeyboardActions
    {
        public static void RemoveLatestShape(Tuple<RailwayCanvasViewModel, Point> args)
        {
            RailwayCanvasViewModel canvas = args.Item1;
            canvas.DeleteLatestShape();
        }
    }
}
