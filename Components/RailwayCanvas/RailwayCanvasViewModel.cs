using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using railway_monitor.MVVM.ViewModels;

namespace railway_monitor.Components.RailwayCanvas
{
    public class RailwayCanvasViewModel : ViewModelBase
    {
        private Shape? _latestShape = null;

        internal Shape? LatestShape
        {
            get => _latestShape;
            set => _latestShape = value;
        }
    }
}
