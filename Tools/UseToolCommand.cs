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
        public override void Execute(object? parameter)
        {
            if (parameter == null) return;

            if (parameter is not Tuple<RailwayCanvasViewModel, Point>)
            {
                throw new NotImplementedException("Command takes RailwayCanvasViewModel, Point arguments. " + parameter.GetType() + " got instead");
            }

            ExecuteDelegate.Invoke((Tuple<RailwayCanvasViewModel, Point>)parameter);
        }

        public UseToolCommand(Action<Tuple<RailwayCanvasViewModel, Point>> executeDelegate) : base(executeDelegate) { }
    }
}
