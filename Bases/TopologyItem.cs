using System.Windows.Media;

namespace railway_monitor.Bases {
    public abstract class TopologyItem : GraphicItem {
        protected static readonly Brush brokenBrush = new SolidColorBrush(Color.FromArgb(200, 234, 67, 53));
        protected static readonly Pen brokenPen = new Pen(brokenBrush, 0.5);

        public abstract void Reassign_OnPortMerged(object? sender, Port oldPort);
    }
}
