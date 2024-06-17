using railway_monitor.Bases;
using railway_monitor.Components.GraphicItems;

namespace railway_monitor.Utils {
    public static class ConnectConditions {
        public static bool IsRailConnectable(Port connectionPort) {
            int switches = connectionPort.GraphicItems.OfType<SwitchItem>().Count();
            int srts = connectionPort.GraphicItems.OfType<StraightRailTrackItem>().Count();

            if (switches != 0 && srts >= 3) return false;
            return true;
        }
        private static bool OtherItemsPresent(Port connectionPort) {
            int srtItems = connectionPort.GraphicItems.OfType<StraightRailTrackItem>().Count();
            return connectionPort.GraphicItems.Count() - srtItems > 0;
        }
        public static bool IsSwitchConnectable(Port connectionPort) {
            return connectionPort.GraphicItems.OfType<StraightRailTrackItem>().Count() == 3 && !OtherItemsPresent(connectionPort);
        }
        public static bool IsSignalConnectable(Port connectionPort) {
            return connectionPort.GraphicItems.OfType<StraightRailTrackItem>().Count() >= 2 && !OtherItemsPresent(connectionPort);
        }
        public static bool IsExternalTrackConnectable(Port connectionPort) {
            return !OtherItemsPresent(connectionPort);
        }
        public static bool IsDeadendConnectable(Port connectionPort) {
            return connectionPort.GraphicItems.OfType<StraightRailTrackItem>().Count() == 1 && !OtherItemsPresent(connectionPort);
        }
        public static bool RailHasPlatform(StraightRailTrackItem srt) {
            return srt.PlatformType == StraightRailTrackItem.RailPlatformType.PASSENGER || srt.PlatformType == StraightRailTrackItem.RailPlatformType.CARGO;
        }
    }
}
