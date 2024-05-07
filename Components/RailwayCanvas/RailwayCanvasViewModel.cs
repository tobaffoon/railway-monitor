using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using railway_monitor.Utils;
using railway_monitor.MVVM.ViewModels;

namespace railway_monitor.Components.RailwayCanvas
{
    public class RailwayCanvasViewModel : ViewModelBase
    {
        public ObservableHashSet<Shape> GraphicItems { get; } = new ObservableHashSet<Shape>();
        public Shape? LatestShape { get; set; }
        public int Len { get { return GraphicItems.Count; } }

        public void AddShape(Shape shape)
        {
            GraphicItems.Add(shape);
            LatestShape = shape;
        }

        public void DeleteShape(Shape shape)
        {
            GraphicItems.Remove(shape);
            if(shape == LatestShape)
            {
                LatestShape = null;
            }
        }

        public void ResetLatestShape()
        {
            LatestShape = null;
        }
    }
}
