using railway_monitor.Bases;
using System.Windows;

namespace railway_monitor.MVVM.Models.Station
{
    public class NodeUnit : PropertyNotifier
    {
        private bool _broken;
        public bool Broken
        {
            get => _broken;
            set => SetField(ref _broken, value);
        }
    }
}
