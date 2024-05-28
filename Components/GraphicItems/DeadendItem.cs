using railway_monitor.Bases;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace railway_monitor.Components.GraphicItems
{
    public class DeadendItem : GraphicItem
    {
        public DeadendItem() : base() { }
        protected override Geometry DefiningGeometry => throw new NotImplementedException();

        public override void Move_OnPortMoved(object? sender, Point newPos)
        {
            throw new NotImplementedException();
        }

        public override void Reassign_OnPortMerged(object? sender, Port oldPort)
        {
            throw new NotImplementedException();
        }
    }
}
