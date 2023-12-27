using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace railway_monitor.Tools
{
    public class MoveGraphicItemCommand : CommandBase<Tuple<Canvas, Shape>>
    {
        public Func<Shape>? ShapeGetter { get; set; }

        public override void Execute(object? parameter)
        {
            if(parameter == null || parameter is not Tuple<Canvas, Shape>)
                throw new NotImplementedException("Command takes exactly Canvas and Shape arguments");

            Tuple<Canvas, Shape> tuple = new Tuple<Canvas, Shape>(
                ((Tuple<Canvas, Shape>)parameter).Item1,
                ShapeGetter.Invoke());
            
            ExecuteDelegate.Invoke(tuple);
        }

        public MoveGraphicItemCommand(Action<Tuple<Canvas, Shape>> executeDelegate, Func<Shape> shapeGetter) : base(executeDelegate)
        {
            ShapeGetter = shapeGetter;
        }
        public MoveGraphicItemCommand() { }
    }
}
