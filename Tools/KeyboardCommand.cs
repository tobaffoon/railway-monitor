using railway_monitor.Bases;
using railway_monitor.Components.RailwayCanvas;
using System.Windows.Input;

namespace railway_monitor.Tools {
    public class KeyboardCommand : CommandBase<Tuple<RailwayCanvasViewModel, Key>> {
        public KeyboardCommand(Action<Tuple<RailwayCanvasViewModel, Key>> executeDelegate) : base(executeDelegate) { }
    }
}
