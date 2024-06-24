using railway_monitor.Bases;
using railway_monitor.Components.RailwayCanvas;

namespace railway_monitor.Tools {
    public class WheelCommand : CommandBase<Tuple<RailwayCanvasViewModel, bool>> {
        public WheelCommand(Action<Tuple<RailwayCanvasViewModel, bool>> executeDelegate) : base(executeDelegate) { }
    }
}
