using System.Windows;

namespace railway_monitor.Bases
{
    public class Port
    {
        public event EventHandler<Point>? OnPortMoved;

        public event EventHandler<Port> OnPortMerged;

        private List<GraphicItem> GraphicItems { get; }
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
            GraphicItems = new List<GraphicItem>();
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
            OnPortMerged?.Invoke(this, other);
            // move items from port's list
            foreach (GraphicItem item in other.GraphicItems) 
            {
                this.AddItem(item);
                other.RemoveItem(item);
            }
        }
    }
}
