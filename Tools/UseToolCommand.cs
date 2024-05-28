using railway_monitor.Bases;
using railway_monitor.Components.RailwayCanvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace railway_monitor.Tools
{
    public class UseToolCommand : CommandBase<Tuple<RailwayCanvasViewModel, Point>>
    {
        public UseToolCommand(Action<Tuple<RailwayCanvasViewModel, Point>> executeDelegate) : base(executeDelegate) { }
    }
}
