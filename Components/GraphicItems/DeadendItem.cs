using railway_monitor.Bases;
using System.Windows.Media;
using System.Windows.Shapes;

namespace railway_monitor.Components.GraphicItems
{
    public class DeadendItem : GraphicItem
    {
        public DeadendItem() : base() { }
        protected override Geometry DefiningGeometry => throw new NotImplementedException();
    }
}
