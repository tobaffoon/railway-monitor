using System.Windows;

namespace railway_monitor.Bases
{
    public abstract class GraphicItem : FrameworkElement
    {
        public abstract void Move_OnPortMoved(object? sender, Point newPos);
        public abstract void Reassign_OnPortMerged(object? sender, Port oldPort);
    }
}
