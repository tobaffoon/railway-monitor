using railway_monitor.Bases;
using railway_monitor.Components.TopologyItems;

namespace railway_monitor.Utils {
    public static class ConnectConditions {
        public static bool IsRailConnectable(Port connectionPort) {
            int switches = connectionPort.TopologyItems.OfType<SwitchItem>().Count();
            int srts = connectionPort.TopologyItems.OfType<StraightRailTrackItem>().Count();

            if (switches != 0 && srts >= 3) return false;
            return true;
        }
        public static bool IsSwitchConnectable(Port connectionPort) {
            IEnumerable<StraightRailTrackItem> connectedSrts = connectionPort.TopologyItems.OfType<StraightRailTrackItem>();
            return connectedSrts.Count() == 3 && connectedSrts.Count(srt => srt.MovementPortEnd == connectionPort) == 1 
                && connectedSrts.Count(srt => srt.MovementPortStart == connectionPort) == 2 && connectionPort.TopologyItems.OfType<SwitchItem>().Count() == 0;
        }
        public static bool IsSignalConnectable(Port connectionPort) {
            return connectionPort.TopologyItems.OfType<StraightRailTrackItem>().Count() == 2 && connectionPort.TopologyItems.OfType<SignalItem>().Count() == 0;
        }
        public static bool IsExternalTrackConnectable(Port connectionPort) {
            return connectionPort.TopologyItems.OfType<ExternalTrackItem>().Count() == 0;
        }
        public static bool IsDeadendConnectable(Port connectionPort) {
            return connectionPort.TopologyItems.OfType<StraightRailTrackItem>().Count() == 1 && connectionPort.TopologyItems.OfType<DeadendItem>().Count() == 0;
        }
        public static bool RailHasPlatform(StraightRailTrackItem srt) {
            return srt.PlatformType == StraightRailTrackItem.RailPlatformType.PASSENGER || srt.PlatformType == StraightRailTrackItem.RailPlatformType.CARGO;
        }
    }
}
