using railway_monitor.Bases;
using railway_monitor.Components.GraphicItems;
using railway_monitor.MVVM.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace railway_monitor.Components.RailwayCanvas {
    public class RailwayCanvasViewModel : ViewModelBase {
        private StraightRailTrackItem? ConnectionTrack { get; set; }
        private HighlightConnection HighlightConnection = new HighlightConnection();
        private double ConnectionRadius {
            get => HighlightConnection.ConnectRadius;
        }
        public bool ConnectionErrorOccured {
            get => HighlightConnection.ConnectionErrorOccured;
            set => HighlightConnection.ConnectionErrorOccured = value;
        }

        public Port? DraggedPort;

        public ObservableCollection<GraphicItem> GraphicItems { get; }
        public GraphicItem? LatestGraphicItem { get; set; }
        public int Len { get { return GraphicItems.Count; } }

        public RailwayCanvasViewModel() {
            GraphicItems = [];
            GraphicItems.Add(HighlightConnection);
        }

        public void AddGraphicItem(GraphicItem item) {
            GraphicItems.Add(item);
            LatestGraphicItem = item;
        }
        public void AddGraphicItemBehind(GraphicItem item) {
            GraphicItems.Insert(1, item);
            LatestGraphicItem = item;
        }

        public void DeleteGraphicItem(GraphicItem item) {
            GraphicItems.Remove(item);
            if (item == LatestGraphicItem) {
                LatestGraphicItem = null;
            }
        }
        public void DeleteStraightRailTrack(StraightRailTrackItem srt) {
            DeleteGraphicItem(srt);
            srt.PortStart.RemoveItem(srt);
            srt.PortEnd.RemoveItem(srt);
        }
        public void DeleteSwitch(SwitchItem swtch) {
            DeleteGraphicItem(swtch);
            swtch.Port.RemoveItem(swtch);
        }

        public void DeleteLatestGraphicItem() {
            if (LatestGraphicItem != null) {
                switch (LatestGraphicItem) {
                    case StraightRailTrackItem srt:
                        DeleteStraightRailTrack(srt);
                        break;
                    case SwitchItem swtch:
                        DeleteSwitch(swtch);
                        break;
                }
                DeleteGraphicItem(LatestGraphicItem);
            }
        }

        private bool RailDuplicates(StraightRailTrackItem srt) {
            // search among all SRTs on the same starting port excluding newly added one
            foreach (StraightRailTrackItem item in srt.PortStart.GraphicItems.OfType<StraightRailTrackItem>().Except<StraightRailTrackItem>([srt])) {
                if (item.Start == srt.Start && item.End == srt.End || item.Start == srt.End && item.End == srt.Start) {
                    return true;
                }
            }
            return false;
        }

        public void ResetLatestGraphicItem() {
            switch (LatestGraphicItem) {
                case StraightRailTrackItem srt:
                    if (RailDuplicates(srt)) {
                        DeleteStraightRailTrack(srt);
                    }
                    break;
            }
            LatestGraphicItem = null;
        }

        private HitTestResultBehavior RailHitTestResult(HitTestResult result) {
            if (result.VisualHit != LatestGraphicItem) {
                ConnectionTrack = result.VisualHit as StraightRailTrackItem;
                return HitTestResultBehavior.Stop;
            }
            else {
                return HitTestResultBehavior.Continue;
            }
        }

        public Port? TryFindUnderlyingPort(Point mousePos) {
            // circle in which new srt tries to connect to an old srt
            EllipseGeometry expandedHitTestArea = new EllipseGeometry(mousePos, ConnectionRadius, ConnectionRadius);

            foreach (StraightRailTrackItem srt in GraphicItems.OfType<StraightRailTrackItem>()) {
                // try finding close srt by hittesting
                ConnectionTrack = null;
                VisualTreeHelper.HitTest(srt,
                    null,
                    new HitTestResultCallback(RailHitTestResult),
                    new GeometryHitTestParameters(expandedHitTestArea));
                if (ConnectionTrack == null) continue;

                // determine close enough vertex or make sure that there is no such vertex
                double distance1 = (ConnectionTrack.Start - mousePos).Length;
                double distance2 = (ConnectionTrack.End - mousePos).Length;
                if (distance1 < ConnectionRadius || distance2 < ConnectionRadius) {
                    HighlightConnection.Visibility = Visibility.Visible;
                    if (distance2 < distance1) {
                        HighlightConnection.Pos = ConnectionTrack.End;
                        return ConnectionTrack.PortEnd;
                    }
                    HighlightConnection.Pos = ConnectionTrack.Start;
                    return ConnectionTrack.PortStart;
                }
            }

            // hide highlighter when no track is close enough
            HighlightConnection.Visibility = Visibility.Collapsed;
            HighlightConnection.ConnectionErrorOccured = false;
            return null;
        }

        public void RenderDraggedPort() {
            if (DraggedPort == null) return;
            DraggedPort.RenderGraphicItems();
            HighlightConnection.Pos = DraggedPort.Pos;
        }
    }
}
