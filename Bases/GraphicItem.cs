using System.Windows.Shapes;

namespace railway_monitor.Bases
{
    public abstract class GraphicItem : Shape
    {
        public GraphicItem()
        {
            StrokeMiterLimit = 2.4;
        }
    }
}
