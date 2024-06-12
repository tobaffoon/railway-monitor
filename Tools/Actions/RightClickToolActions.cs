using railway_monitor.Bases;
using railway_monitor.Components.GraphicItems;
using railway_monitor.Components.RailwayCanvas;
using System.Windows;

namespace railway_monitor.Tools.Actions {
    public class RightClickToolActions {
        public static void ToggleExternalTrackType(Tuple<RailwayCanvasViewModel, Point> args) {
            RailwayCanvasViewModel canvas = args.Item1;
            ExternalTrackItem? externalTrackItem = canvas.LatestGraphicItem as ExternalTrackItem;
            if (externalTrackItem == null) {
                return;
            }

            switch (externalTrackItem.Type) {
                case ExternalTrackItem.ExternalTrackType.IN:
                    externalTrackItem.Type = ExternalTrackItem.ExternalTrackType.OUT;
                    break;
                case ExternalTrackItem.ExternalTrackType.OUT:
                    externalTrackItem.Type = ExternalTrackItem.ExternalTrackType.IN;
                    break;
            }

            externalTrackItem.Render();
        }
    }
}
