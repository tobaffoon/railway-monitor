using railway_monitor.Bases;
using railway_monitor.Components.GraphicItems;
using railway_monitor.MVVM.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace railway_monitor.Components.RailwayCanvas {
    public class RailwayCanvasViewModel : ViewModelBase {
        #region Highlight connection
        private StraightRailTrackItem? ConnectionPortTrack { get; set; }
        private HighlightPort HighlightPort = new HighlightPort();
        private double PortConnectionRadius = HighlightPort.ConnectRadius;
        
        public bool ConnectionErrorOccured {
            get => HighlightPort.ConnectionErrorOccured;
            set => HighlightPort.ConnectionErrorOccured = value;
        }
        #endregion

        #region Add platform
        public StraightRailTrackItem? ConnectionPlatformTrack { get; private set; }
        private double PlatformConnectionRadius = PlatformItem.ConnectRadius;
        #endregion

        public Port? DraggedPort;

        public ObservableCollection<GraphicItem> GraphicItems { get; }
        public TopologyItem? LatestTopologyItem { get; set; }
        public List<StraightRailTrackItem> Rails {
            get {
                return GraphicItems.OfType<StraightRailTrackItem>().ToList();
            }
        }

        private TopologyItem[] permanentItems;

        public RailwayCanvasViewModel() {
            permanentItems = [HighlightPort];
            GraphicItems = [];
            foreach (var item in permanentItems) {
                GraphicItems.Add(item);
            }
        }

        public void AddTopologyItem(TopologyItem item) {
            GraphicItems.Add(item);
            LatestTopologyItem = item;
        }
        public void AddTopologyItemBehind(TopologyItem item) {
            GraphicItems.Insert(permanentItems.Length, item);
            LatestTopologyItem = item;
        }
        public void DeleteTopologyItem(TopologyItem item) {
            GraphicItems.Remove(item);
            if (item == LatestTopologyItem) {
                LatestTopologyItem = null;
            }
        }
        public void DeleteStraightRailTrack(StraightRailTrackItem srt) {
            DeleteTopologyItem(srt);
            srt.PortStart.RemoveItem(srt);
            srt.PortEnd.RemoveItem(srt);
        }
        public void DeleteSwitch(SwitchItem swtch) {
            DeleteTopologyItem(swtch);
            swtch.Port.RemoveItem(swtch);
        }

        public void DeleteLatestTopologyItem() {
            if (LatestTopologyItem != null) {
                switch (LatestTopologyItem) {
                    case StraightRailTrackItem srt:
                        DeleteStraightRailTrack(srt);
                        break;
                    case SwitchItem swtch:
                        DeleteSwitch(swtch);
                        break;
                }
                DeleteTopologyItem(LatestTopologyItem);
            }
        }

        private bool RailDuplicates(StraightRailTrackItem srt) {
            // search among all SRTs on the same starting port excluding newly added one
            foreach (StraightRailTrackItem item in srt.PortStart.TopologyItems.OfType<StraightRailTrackItem>().Except<StraightRailTrackItem>([srt])) {
                if (item.Start == srt.Start && item.End == srt.End || item.Start == srt.End && item.End == srt.Start) {
                    return true;
                }
            }
            return false;
        }

        public void ResetLatestTopologyItem() {
            switch (LatestTopologyItem) {
                case StraightRailTrackItem srt:
                    if (RailDuplicates(srt)) {
                        DeleteStraightRailTrack(srt);
                    }
                    break;
            }
            LatestTopologyItem = null;
        }

        private HitTestResultBehavior PortRailHitTestResult(HitTestResult result) {
            if (result.VisualHit != LatestTopologyItem) {
                ConnectionPortTrack = result.VisualHit as StraightRailTrackItem;
                return HitTestResultBehavior.Stop;
            }
            else {
                return HitTestResultBehavior.Continue;
            }
        }

        private HitTestResultBehavior PlatformRailHitTestResult(HitTestResult result) {
            if (result.VisualHit != LatestTopologyItem) {
                ConnectionPlatformTrack = result.VisualHit as StraightRailTrackItem;
                return HitTestResultBehavior.Stop;
            }
            else {
                return HitTestResultBehavior.Continue;
            }
        }

        public Port? TryFindUnderlyingPort(Point mousePos) {
            // circle in which new srt tries to connect to an old srt
            EllipseGeometry expandedHitTestArea = new EllipseGeometry(mousePos, PortConnectionRadius, PortConnectionRadius);

            foreach (StraightRailTrackItem srt in GraphicItems.OfType<StraightRailTrackItem>()) {
                // try finding close srt by hittesting
                ConnectionPortTrack = null;
                VisualTreeHelper.HitTest(srt,
                    null,
                    new HitTestResultCallback(PortRailHitTestResult),
                    new GeometryHitTestParameters(expandedHitTestArea));
                if (ConnectionPortTrack == null) continue;

                // determine close enough vertex or make sure that there is no such vertex
                double distance1 = (ConnectionPortTrack.Start - mousePos).Length;
                double distance2 = (ConnectionPortTrack.End - mousePos).Length;
                if (distance1 < PortConnectionRadius || distance2 < PortConnectionRadius) {
                    HighlightPort.Visibility = Visibility.Visible;
                    if (distance2 < distance1) {
                        HighlightPort.Pos = ConnectionPortTrack.End;
                        return ConnectionPortTrack.PortEnd;
                    }
                    HighlightPort.Pos = ConnectionPortTrack.Start;
                    return ConnectionPortTrack.PortStart;
                }
            }

            // hide highlighter when no track is close enough
            HighlightPort.Visibility = Visibility.Collapsed;
            HighlightPort.ConnectionErrorOccured = false;
            return null;
        }

        public StraightRailTrackItem? TryFindRailForPlatform(Point mousePos) {
            // circle in which new srt tries to connect to an old srt
            EllipseGeometry expandedHitTestArea = new EllipseGeometry(mousePos, PlatformConnectionRadius, PlatformConnectionRadius);

            foreach (StraightRailTrackItem srt in GraphicItems.OfType<StraightRailTrackItem>()) {
                // try finding close srt by hittesting
                ConnectionPlatformTrack = null;
                VisualTreeHelper.HitTest(srt,
                    null,
                    new HitTestResultCallback(PlatformRailHitTestResult),
                    new GeometryHitTestParameters(expandedHitTestArea));
                if (ConnectionPlatformTrack == null) continue;

                return ConnectionPlatformTrack;
            }

            // hide highlighter when no track is close enough
            return null;
        }

        public void RenderDraggedPort() {
            if (DraggedPort == null) return;
            DraggedPort.RenderTopologyGraphicItems();
            HighlightPort.Pos = DraggedPort.Pos;
        }
    }
}
