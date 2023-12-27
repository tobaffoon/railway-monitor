using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using railway_monitor.Components.RailwayCanvas;

namespace railway_monitor.Tools
{
    public class AddGraphicsItemCommand : CommandBase<Tuple<Canvas, Shape>>
    {
        public Func<Shape>? ShapeGetter { get; set; }
        public Action? ExecutedHandler { get; set; }

        public override void Execute(object? parameter)
        {
            if(parameter == null) return;

            if (parameter is not Canvas)
            {
                throw new NotImplementedException("Command takes exactly one UIElement argument " + parameter.GetType() + " got instead");
            }

            Shape currentItem = ShapeGetter.Invoke();
            ExecuteDelegate.Invoke(Tuple.Create((Canvas)parameter, currentItem));
            ExecutedHandler.Invoke();
        }

        public AddGraphicsItemCommand(Action<Tuple<Canvas, Shape>> executeDelegate, Func<Shape> shapeGetter) : base(executeDelegate)
        {
            ShapeGetter = shapeGetter;
        }
        public AddGraphicsItemCommand() { }
    }
}
