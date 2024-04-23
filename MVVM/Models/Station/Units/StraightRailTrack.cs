using railway_monitor.Bases;
using railway_monitor.MVVM.Models.Station;

namespace railway_monitor.MVVM.Models.Station.Units
{
    public enum TerminalType
    {
        NONE,
        PASSENGER,
        CARGO,
        MAINTENANCE
    }

    public class StraightRailTrack : PropertyNotifier
    {
        public NodeUnit? startUnit;
        public NodeUnit? endUnit;
        public bool broken;

        private readonly int _id;
        private readonly int _len;
        private TerminalType _type;
        private bool _broken;
        public bool Broken
        {
            get => _broken;
            set => SetField(ref _broken, value);
        }

        public StraightRailTrack(NodeUnit startUnit, int len, TerminalType type, int id)
        {
            this.startUnit = startUnit;
            _len = len;
            broken = false;
            _type = type;
            _id = id;
        }
        public void Place(NodeUnit endUnit)
        {
            this.endUnit = endUnit;
        }
    }
}
