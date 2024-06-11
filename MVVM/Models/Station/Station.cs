using railway_monitor.MVVM.Models.Station.Units;

namespace railway_monitor.MVVM.Models.Station {
    /// <summary>
    /// Stores all the units (including SRTs)
    /// </summary>
    public class Station {
        private readonly HashSet<StraightRailTrack> railtracks;
        private readonly HashSet<NodeUnit> units;

        public Station() {
            railtracks = new HashSet<StraightRailTrack>();
            units = new HashSet<NodeUnit>();
        }

        public void addUnit(NodeUnit unit) {
            units.Add(unit);
        }

        public void deleteUnit(NodeUnit unit) {
            units.Remove(unit);
        }

        public void addStraightRailTrack(StraightRailTrack track) {
            railtracks.Add(track);
        }

        public void deleteStraightRailTrack(StraightRailTrack track) {
            railtracks.Remove(track);
        }
    }
}
