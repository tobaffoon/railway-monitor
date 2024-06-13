using railway_monitor.Bases;
using railway_monitor.Components.RailwayCanvas;

namespace railway_monitor.Tools {
    public class CanvasCommand : CommandBase<RailwayCanvasViewModel> {
        public CanvasCommand(Action<RailwayCanvasViewModel> executeDelegate) : base(executeDelegate) { }
    }
}
