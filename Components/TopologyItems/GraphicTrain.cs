using railway_monitor.Bases;
using System.Windows;
using System.Windows.Media;

namespace railway_monitor.Components.GraphicItems
{
    public class GraphicTrain : TopologyItem {
        public override void Reassign_OnPortMerged(object? sender, Port oldPort) {
            throw new NotImplementedException();
        }

        protected override void Render(DrawingContext drawingContext) {
            throw new NotImplementedException();
        }
    }
}
