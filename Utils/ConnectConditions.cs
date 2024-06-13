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

        public static bool IsSwitchConnectable(Port connectionPort) {
            return connectionPort.GraphicItems.OfType<StraightRailTrackItem>().Count() == 3 && connectionPort.GraphicItems.OfType<SwitchItem>().Count() == 0;
        }

        public static bool IsSignalConnectable(Port connectionPort) {
            return connectionPort.GraphicItems.OfType<StraightRailTrackItem>().Count() >= 2 && connectionPort.GraphicItems.OfType<SignalItem>().Count() == 0;
        }

        public static bool IsExternalTrackConnectable(Port connectionPort) {
            return connectionPort.GraphicItems.OfType<ExternalTrackItem>().Count() == 0;
        }
    }
}
