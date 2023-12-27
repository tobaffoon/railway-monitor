using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using railway_monitor.Tools.Actions;

namespace railway_monitor.Tools
{
    public static class RailwayDesignerCommands
    {
        public static readonly AddGraphicsItemCommand AddItem = new AddGraphicsItemCommand();
        public static readonly MoveGraphicItemCommand MoveItem = new MoveGraphicItemCommand();
    }
}
