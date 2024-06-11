using railway_monitor.Bases;
using railway_monitor.Components.RailwayCanvas;
using System.Windows;

namespace railway_monitor.Tools
{
    public class UseToolCommand : CommandBase<Tuple<RailwayCanvasViewModel, Point>>
    {
        public UseToolCommand(Action<Tuple<RailwayCanvasViewModel, Point>> executeDelegate) : base(executeDelegate) { }
    }
}
