using railway_monitor.Components.TopologyItems;
using System.Windows;

namespace railway_monitor.Bases {
    public class Port {
        public event EventHandler<Port>? OnPortMerged;

        public List<TopologyItem> TopologyItems { get; }
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

        #region Port types
        public static bool IsPortInput(Port port) {
            return port.TopologyItems.OfType<ExternalTrackItem>().Any(externalItem => externalItem.Type == ExternalTrackItem.ExternalTrackType.IN);
        }
        public static bool IsPortOutput(Port port) {
            return port.TopologyItems.OfType<ExternalTrackItem>().Any(externalItem => externalItem.Type == ExternalTrackItem.ExternalTrackType.OUT);
        }
        public static bool IsPortConnection(Port port) {
            return port.TopologyItems.OfType<StraightRailTrackItem>().Count() == port.TopologyItems.Count;
        }
        public static bool IsPortDeadend(Port port) {
            return port.TopologyItems.OfType<DeadendItem>().Any();
        }
        public static bool IsPortSwitch(Port port) {
            return port.TopologyItems.OfType<SwitchItem>().Any();
        }
        public static bool IsPortSignal(Port port) {
            return port.TopologyItems.OfType<SignalItem>().Any();
        }
        #endregion

        public override string ToString() {
            string typePrefix = "";
            if (IsPortInput(this)) {
                typePrefix = "Input ";
            }
            else if (IsPortOutput(this)) {
                typePrefix = "Output ";
            }
            else if (IsPortConnection(this)) {
                typePrefix = "Connection ";
            }
            else if (IsPortDeadend(this)) {
                typePrefix = "Deadend ";
            }
            else if (IsPortSwitch(this)) {
                typePrefix = "Switch ";
            }
            else if (IsPortSignal(this)) {
                typePrefix = "Signal ";
            }
            return "<" + typePrefix + "Port " + GetHashCode() + ">";
        }

        public int Id { get; set; } = -1;
    }
}
