using railway_monitor.Components.GraphicItems;
using System.Windows;

namespace railway_monitor.Bases {
    public class Port {
        public event EventHandler<Port>? OnPortMerged;

        public List<GraphicItem> GraphicItems { get; }
        public Point Pos;

        public Port(TopologyItem parentItem, Point startPos) {
            TopologyItems = new List<TopologyItem>();
            AddItem(parentItem);
            Pos = startPos;
        }

        public void AddItem(TopologyItem item) {
            TopologyItems.Add(item);
            OnPortMerged += item.Reassign_OnPortMerged;
        }
        public void RemoveItem(TopologyItem item) {
            TopologyItems.Remove(item);
            OnPortMerged -= item.Reassign_OnPortMerged;
        }

        public void Merge(Port other) {
            // reassign port link
            other.OnPortMerged?.Invoke(this, other);
            // move items to new port's list
            foreach (TopologyItem item in other.TopologyItems) {
                this.AddItem(item);
            }
            // move items from old port's list
            foreach (TopologyItem item in this.TopologyItems) {
                other.RemoveItem(item);
            }

        }

        private void RenderTopologyItemsFlat() {
            foreach (TopologyItem item in TopologyItems) {
                item.Render();
            }
        }
        public void RenderTopologyGraphicItems() {
            List<Port> neighbourPorts = new List<Port>();
            foreach (TopologyItem item in TopologyItems) {
                item.Render();
                if (item is StraightRailTrackItem srt) {
                    // and neighbours to update them (for switches especially)
                    neighbourPorts.Add(srt.GetOtherPort(this));
                }
            }

            foreach (Port neighbourPort in neighbourPorts) {
                neighbourPort.RenderTopologyItemsFlat();
            }
        }

        public override string ToString() {
            return "<Port " + GetHashCode() + ">";
        }
    }
}
