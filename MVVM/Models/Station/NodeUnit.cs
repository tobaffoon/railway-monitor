using railway_monitor.Bases;
using System.Windows;

namespace railway_monitor.MVVM.Models.Station
{
    public class NodeUnit : PropertyNotifier
    {
        private Point _pos;
        public Point Pos
        {
            get => _pos;
            set => SetField(ref _pos, value);
        }
        private bool _broken;
        public bool Broken
        {
            get => _broken;
            set => SetField(ref _broken, value);
        }

        public NodeUnit(Point pos)
        {
            Pos = pos;
        }
    }
}
