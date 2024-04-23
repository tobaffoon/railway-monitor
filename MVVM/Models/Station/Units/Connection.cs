using railway_monitor.MVVM.Models.Station;
using System.Windows;

namespace railway_monitor.MVVM.Models.Station.Units
{
    /// <summary>
    /// Connection between 2 SRTs. Doesn't do anything, just allows polyline tracks.
    /// </summary>
    public class Connection : NodeUnit
    {
        public Connection(Point pos) : base(pos)
        {
        }
    }
}
