
using railway_monitor.Bases;
using railway_monitor.MVVM.Models.Station.Units;

namespace railway_monitor.MVVM.Models.Trains {
    public class Train : PropertyNotifier {
        private readonly TerminalType stationGoal;
        private readonly int length;

        public Train(TerminalType stationGoal, int length) {
            this.stationGoal = stationGoal;
            this.length = length;
        }
    }
}
