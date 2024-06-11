using railway_monitor.Components.GraphicItems;
using System.Windows;

namespace railway_monitor.Bases
{
    public class Port
    {
        public event EventHandler<Port>? OnPortMerged;

        public HashSet<GraphicItem> GraphicItems { get; }
        public Point Pos;

        public Port(GraphicItem parentItem, Point startPos)
        {
            GraphicItems = new HashSet<GraphicItem>();
            AddItem(parentItem);
            Pos = startPos;
        }

        public void AddItem(GraphicItem item)
        {
            GraphicItems.Add(item);
            OnPortMerged += item.Reassign_OnPortMerged;
        }
        public void RemoveItem(GraphicItem item)
        {
            GraphicItems.Remove(item);
            OnPortMerged -= item.Reassign_OnPortMerged;
        }

        public void Merge(Port other)
        {
            // reassign port link
            other.OnPortMerged?.Invoke(this, other);
            // move items to new port's list
            foreach (GraphicItem item in other.GraphicItems) 
            {
                this.AddItem(item);
            }
            // move items from old port's list
            foreach (GraphicItem item in this.GraphicItems) 
            {
                other.RemoveItem(item);
            }

        }

        private void RenderGraphicItemsFlat()
        {
            foreach (GraphicItem item in GraphicItems)
            {
                item.Render();
            }
        }
        public void RenderGraphicItems()
        {
            List<Port> neighbourPorts = new List<Port>();
            foreach (GraphicItem item in GraphicItems)
            {
                item.Render();
                if (item is StraightRailTrackItem srt)
                {
                    // and neighbours to update them (for switches especially)
                    neighbourPorts.Add(srt.GetOtherPort(this));
                }
            }

            foreach (Port neighbourPort in neighbourPorts)
            {
                neighbourPort.RenderGraphicItemsFlat();
            }
        }

        public override string ToString()
        {
            return "<Port " + GetHashCode() + ">";
        }
    }
}
