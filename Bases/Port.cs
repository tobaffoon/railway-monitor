using System.Windows;

namespace railway_monitor.Bases
{
    public class Port
    {
        public event EventHandler<Point>? OnPortMoved;

        public event EventHandler<Port> OnPortMerged;

        public HashSet<GraphicItem> GraphicItems { get; }
        private Point _pos;
        public Point Pos
        {
            get
            {
                return _pos;
            }
            set
            {
                _pos = value;
                OnPortMoved?.Invoke(this, value);
            }
        }

        public Port(GraphicItem parentItem, Point startPos)
        {
            GraphicItems = new HashSet<GraphicItem>();
            AddItem(parentItem);
            Pos = startPos;
        }

        public void AddItem(GraphicItem item)
        {
            GraphicItems.Add(item);
            OnPortMoved += item.Move_OnPortMoved;
            OnPortMerged += item.Reassign_OnPortMerged;
        }
        public void RemoveItem(GraphicItem item)
        {
            GraphicItems.Remove(item);
            OnPortMoved -= item.Move_OnPortMoved;
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
    }
}
