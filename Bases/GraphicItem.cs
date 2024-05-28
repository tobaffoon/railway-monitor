using System.Windows;
using System.Windows.Shapes;

namespace railway_monitor.Bases
{
    public abstract class GraphicItem : Shape
    {
        public GraphicItem()
        {
            StrokeMiterLimit = 2.4;
        }
        public abstract void Move_OnPortMoved(object? sender, Point newPos);
        public abstract void Reassign_OnPortMerged(object? sender, Port oldPort);
    }
}
