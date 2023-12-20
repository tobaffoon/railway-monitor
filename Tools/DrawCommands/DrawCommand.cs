using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace railway_monitor.Tools.DrawCommands
{
    public abstract class DrawCommand : CommandBase
    {
        public abstract void Execute(List<Shape> graphicItemsList);

        public override void Execute(object? parameter)
        {
            throw new NotImplementedException("Command takes exactly one List<Shape> argument");
        }
    }
}
